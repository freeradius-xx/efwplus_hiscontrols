using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EMR.Controls.Action;
using System.IO;
using System.Drawing.Printing;
using System.Reflection;
using Tx4oleLib;
using DevComponents.DotNetBar;
using System.Drawing.Imaging;
using System.Xml;

namespace EMR.Controls
{
    [LicenseProvider(typeof(BaseControls.Licensing.ComponentLicenseProvider))]
    public partial class EmrRecord : UserControl, IEmrRecord
    {
        #region 许可证
        [Browsable(false)]
        public static string DeveloperKey { get; set; }
        private bool _isDemo = true;
        //private static string _licKey = "";
        [Category("许可证"), Description("设置许可证你才能使用此控件")]
        public string LicenseKey
        {
            get { return DeveloperKey; }
            set
            {
                DeveloperKey = value;
                //CheckLicense();
                this.Invalidate();
            }
        }
        private void CheckLicense()
        {
            BaseControls.Licensing.ComponentLicense lic = LicenseManager.Validate(typeof(EmrRecord), this) as BaseControls.Licensing.ComponentLicense;
            if (lic != null)
            {
                _isDemo = lic.IsDemo;
                //Btnbar.Enabled = _isDemo ? false : true;
                if (_isDemo)
                {
                    MessageBox.Show("正确设置[EmrRecord]控件的许可证，才能使用！");
                }
            }
        }
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            CheckLicense();
        }
        #endregion

        public emrController controller;//控制器
        //private IEmrDataSource emrDataSource;
        public EmrRecord()
        {
            InitializeComponent();
            emrMedicalRecord.FieldDblClicked += new EMREdit.CFieldDblClicked(emrMedicalRecord_FieldDblClicked);
            emrMedicalRecord.FieldClicked += new EMREdit.CFieldClicked(emrMedicalRecord_FieldClicked);
            controller = new emrController(this);
            Btnbar.Font = new Font("微软雅黑", 10.5F);
            InstallBtnFunctonEvent();
            InstallToolsEvent();
            btnState();

            //CheckLicense();//检查许可证
        }

       
        /// <summary>
        /// 文件方式存储，调用此方法
        /// </summary>
        public void InitLoad()
        {
            controller.InitLoad();
        }
        /// <summary>
        /// 数据库方式存储，调用此方法
        /// </summary>
        public void InitLoad(IEmrDbHelper _emrDbHelper)
        {
            controller.InitLoad(_emrDbHelper);
        }

        /// <summary>
        /// 模板功能调用此方法
        /// </summary>
        /// <param name="_keyData"></param>
        public void InitLoad(IEmrTemplateDbHelper _emrTemplateDbHelper, EmrBindKeyData _keyData)
        {
            controller.InitLoad(_emrTemplateDbHelper, _keyData);
            if (_EmrTemplateTree != null)
            {
                _EmrTemplateTree.controller = controller;
                controller.InitTemplate(_EmrTemplateTree);
            }
        }

        /// <summary>
        /// 病历书写调用此方法
        /// </summary>
        /// <param name="_keyData"></param>
        public void InitLoad(IEmrWriteDbHelper _emrWriteDbHelper, EmrBindKeyData _keyData, bool IsPreview)
        {
            controller.InitLoad(_emrWriteDbHelper, _keyData, IsPreview);
            if (_EmrWriteTree != null)
            {
                _EmrWriteTree.controller = controller;
                controller.InitWrite(_EmrWriteTree);
            }
        }


        #region 属性
        private bool _isShowOpenBtn = true;
        //[Browsable(false)]
        public bool IsShowFileBtn
        {
            get { return _isShowOpenBtn; }
            set
            {
                _isShowOpenBtn = value;
                this.biFile.Visible = _isShowOpenBtn;
            }
        }

        private EmrTemplateTree _EmrTemplateTree;
        public EmrTemplateTree EmrTemplateTree
        {
            get
            {
                return _EmrTemplateTree;
            }
            set
            {
                _EmrTemplateTree = value;
            }
        }
        private EmrWriteTree _EmrWriteTree;
        public EmrWriteTree EmrWriteTree
        {
            get
            {
                return _EmrWriteTree;
            }
            set
            {
                _EmrWriteTree = value;
            }
        }
        #endregion

        #region 事件
        public event EventHandler EmrTextChanged;
        public event EventHandler OpenedEmrData;
        public event EventHandler CloseedEmrData;
        #endregion

        void emrMedicalRecord_FieldDblClicked(object sender, AxTx4oleLib._DTX4OLEEvents_FieldDblClickedEvent e)
        {
            if (emrMedicalRecord.FieldCurrent > 0 && emrOperStyle == EmrOperStyle.修改)
            {
                //string[] strArray = emrMedicalRecord.GetFieldData(emrMedicalRecord.FieldCurrent).Split('|');
                EmrEditFieldData _efielddata = new EmrEditFieldData(emrMedicalRecord.GetFieldData(emrMedicalRecord.FieldCurrent), emrMedicalRecord.FieldText);

                Point location = new Point();
                location = emrMedicalRecord.txtEdit.PointToScreen(location);
                location.X += (emrMedicalRecord.MousePoint.X + 10);
                location.Y += (emrMedicalRecord.MousePoint.Y + 10);
                EmrToolTipManage.ShowEmrToolTip(location, _efielddata.dtype.ToString(), Convert.ToString((int)_efielddata.inputType), _efielddata.elId, _efielddata.Value, delegate(string text, string value)
                {
                    if (emrMedicalRecord.FieldCurrent == 0)
                        return;
                    emrMedicalRecord.FieldText = text;
                    emrMedicalRecord.SetFieldData(emrMedicalRecord.FieldCurrent, _efielddata.dtype.ToString() + "|" + Convert.ToString((int)_efielddata.inputType) + "|" + _efielddata.elId + "|" + value);
                });
            }
        }
        void emrMedicalRecord_FieldClicked(object sender, AxTx4oleLib._DTX4OLEEvents_FieldClickedEvent e)
        {
            if (emrMedicalRecord.FieldCurrent > 0 && emrOperStyle == EmrOperStyle.修改)
            {
                //string[] strArray = emrMedicalRecord.GetFieldData(emrMedicalRecord.FieldCurrent).Split('|');
                EmrEditFieldData _efielddata = new EmrEditFieldData(emrMedicalRecord.GetFieldData(emrMedicalRecord.FieldCurrent), emrMedicalRecord.FieldText);

                if (_efielddata.inputType == InputType.CheckBox && (_efielddata.elId == "SysTrueOrFalse"))//复选框
                {
                    if (_efielddata.Value == "-1")
                    {
                        emrMedicalRecord.FontBold = 1;
                        emrMedicalRecord.FontName = "Wingdings 2";
                        emrMedicalRecord.FieldText = "R";
                        emrMedicalRecord.SetFieldData(emrMedicalRecord.FieldCurrent, _efielddata.dtype.ToString() + "|" + Convert.ToString((int)_efielddata.inputType) + "|" + _efielddata.elId + "|" + "1");
                    }
                    else if (_efielddata.Value == "1")
                    {
                        emrMedicalRecord.FontBold = 1;
                        emrMedicalRecord.FontName = "Wingdings 2";
                        emrMedicalRecord.FieldText = "T";
                        emrMedicalRecord.SetFieldData(emrMedicalRecord.FieldCurrent, _efielddata.dtype.ToString() + "|" + Convert.ToString((int)_efielddata.inputType) + "|" + _efielddata.elId + "|" + "0");
                    }
                    else if (_efielddata.Value == "0")
                    {
                        emrMedicalRecord.FontName = "Wingdings";
                        emrMedicalRecord.FieldText = "o";
                        emrMedicalRecord.SetFieldData(emrMedicalRecord.FieldCurrent, _efielddata.dtype.ToString() + "|" + Convert.ToString((int)_efielddata.inputType) + "|" + _efielddata.elId + "|" + "-1");
                    }
                }
            }
        }

        #region 按钮
        private void InstallBtnFunctonEvent()
        {
            biFile.Click += new EventHandler(biFile_Click);
            biOpen.Click += new EventHandler(biOpen_Click);
            biAdd.Click += new EventHandler(biAdd_Click);
            biModify.Click += new EventHandler(biModify_Click);
            biDelete.Click += new EventHandler(biDelete_Click);
            biSave.Click += new EventHandler(biSave_Click);
            biCancel.Click += new EventHandler(biCancel_Click);

            biSignName.Click += new EventHandler(biSignName_Click);
            biBack.Click += new EventHandler(biBack_Click);
            biBackReason.Click += new EventHandler(biBackReason_Click);
            biWriteBack.Click += new EventHandler(biWriteBack_Click);
            biUpdateTitle.Click += new EventHandler(biUpdateTitle_Click);
            biExport.Click += new EventHandler(biExport_Click);
            biImport.Click += new EventHandler(biImport_Click);
            biTemplate.Click += new EventHandler(biTemplate_Click);
            biSaveAs.Click += new EventHandler(biSaveAs_Click);
            biClinicData.Click += new EventHandler(biClinicData_Click);
            biAssistant.Click += new EventHandler(biAssistant_Click);
            btnSignature.Click += new EventHandler(btnSignature_Click);
            biEmrPrint.Click += new EventHandler(biPrint_Click);

            biRefresh.Click += new EventHandler(biRefresh_Click);
            biMergeView.Click += new EventHandler(biMergeView_Click);
            biPrint.Click += new EventHandler(biPrint_Click);
            biSaveAsDoc.Click += new EventHandler(biSaveAsDoc_Click);

            biHeader.Click += new EventHandler(biHeader_Click);
            biFooter.Click += new EventHandler(biFooter_Click);
            biPageSetup.Click += new EventHandler(biPageSetup_Click);

            biInsertDomain.Click += new EventHandler(biInsertDomain_Click);
            biEditDomain.Click += new EventHandler(biEditDomain_Click);
            biDeleteDomain.Click += new EventHandler(biDeleteDomain_Click);
        }

        

        #region 文件操作
        void biFile_Click(object sender, EventArgs e)
        {
            biFile.Expanded = true;
        }

        void biOpen_Click(object sender, EventArgs e)
        {
            if (emrDatastorageType == EmrDatastorageType.文件存储)
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    currFileInfo = new FileInfo(openFileDialog.FileName);
                    controller.OpenFileToEmr();
                }
            }
            else if (emrDatastorageType == EmrDatastorageType.数据库存储)
            {
                StorageList fsl = new StorageList(controller);
                fsl.ShowDialog();
                if (fsl.isOk)
                {
                    CurrBindKeyData = new EmrBindKeyData(fsl.EmrDataID);
                    controller.OpenDatabaseToEmr();
                }
            }
        }
        void biCancel_Click(object sender, EventArgs e)
        {
            emrOperStyle = EmrOperStyle.默认;
            if (emrDatastorageType == EmrDatastorageType.文件存储)
            {
                controller.OpenFileToEmr();
            }
            else if (emrDatastorageType == EmrDatastorageType.数据库存储)
            {
                controller.OpenDatabaseToEmr();
            }
            btnState();
            btnSignState();
            btnModifyState();
        }

        void biSave_Click(object sender, EventArgs e)
        {
            if (emrDatastorageType == EmrDatastorageType.文件存储)
            {
                if (currFileInfo == null)
                {
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        currFileInfo = new FileInfo(saveFileDialog.FileName);
                        controller.SaveFileToLocal();
                        emrOperStyle = EmrOperStyle.默认;
                    }
                }
                else
                {
                    controller.SaveFileToLocal();
                    emrOperStyle = EmrOperStyle.默认;
                }
            }
            else if (emrDatastorageType == EmrDatastorageType.数据库存储)
            {
                controller.SaveDatabase();
                emrOperStyle = EmrOperStyle.默认;
            }
            btnState();
            btnSignState();
            btnModifyState();
            controller.OpenDatabaseToEmr();
        }

        void biDelete_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void biModify_Click(object sender, EventArgs e)
        {
            if ((emrDatastorageType == EmrDatastorageType.文件存储 && currFileInfo != null) || (emrDatastorageType == EmrDatastorageType.数据库存储 && CurrBindKeyData != null && CurrBindKeyData.EmrDataId != -1))
            {
                emrOperStyle = EmrOperStyle.修改;
                btnState();
                controller.OpenDatabaseToEmr();
            }
        }

        void biAdd_Click(object sender, EventArgs e)
        {
            if (emrDatastorageType == EmrDatastorageType.文件存储)
            {
                currFileInfo = null;
            }
            else if (emrDatastorageType == EmrDatastorageType.数据库存储)
            {
                if (_currBindKeyData == null)
                {
                    CurrBindKeyData = new EmrBindKeyData();
                }
                CurrBindKeyData.EmrDataId = 0;
            }
            emrMedicalRecord.HeaderFooterActivate(HeaderFooterConstants.txMainText);
            emrMedicalRecord.Text = "";
            emrMedicalRecord.HeaderFooter = (HeaderFooterConstants)5;
            emrMedicalRecord.HeaderFooterStyle = HeaderFooterStyleConstants.txNoDblClk;
            emrOperStyle = EmrOperStyle.修改;
            btnState();
        }
        #endregion

        #region 病历操作
        void biClinicData_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void biSaveAs_Click(object sender, EventArgs e)
        {
            if (emrControlType == EmrControlType.病历编辑 && CurrBindKeyData.EmrDataId != -1)
            {
                SaveAsTemplate sat = new SaveAsTemplate(CurrBindKeyData.EmrTitle);
                sat.ShowDialog();
                if (sat.isOk)
                {
                    controller.SaveAsTemplateData(sat.TemplateText, sat.Level);
                    MessageBoxEx.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        //调用模板
        void biTemplate_Click(object sender, EventArgs e)
        {
            if (emrControlType == EmrControlType.病历编辑 && CurrBindKeyData.EmrDataId != -1)
            {
                SelectEmrTemplate set = new SelectEmrTemplate(controller);
                set.ShowDialog();
                if (set.isOk)
                {
                    controller.OpenDatabaseToEmr(set.EmrDataID);
                    emrOperStyle = EmrOperStyle.修改;
                    btnState();
                }
            }
        }

        void biImport_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void biExport_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void biRefresh_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void biUpdateTitle_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void biBackReason_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void biBack_Click(object sender, EventArgs e)
        {
            if (emrControlType == EmrControlType.病历编辑 && CurrBindKeyData.EmrDataId != -1)
            {
                controller.SignBack();
                btnSignState();
                btnModifyState();
                //controller.GetWriteRecordTreeData();
                controller.OpenDatabaseToEmr();
            }
        }

        void biSignName_Click(object sender, EventArgs e)
        {
            if (emrControlType == EmrControlType.病历编辑 && CurrBindKeyData.EmrDataId != -1)
            {
                Signature dlg = new Signature();
                dlg.ShowDialog();
                if (dlg.retOk)
                {
                    Bitmap imgSign = dlg.SavedBitmap;
                    controller.SignName(imgSign);
                    btnSignState();
                    btnModifyState();
                    controller.OpenDatabaseToEmr();
                }
            }
        }

        WriteHelper dlgWriteHelper;
        //bool ckbuttonHelper = false;
        void biAssistant_Click(object sender, EventArgs e)
        {
            if (biAssistant.Checked)
            {
                if (dlgWriteHelper==null)
                    dlgWriteHelper = new WriteHelper(controller);

                dlgWriteHelper.Show(this);
                //ckbuttonHelper = true;
            }
            else
            {
                if (dlgWriteHelper != null)
                {
                    dlgWriteHelper.Hide();
                    //ckbuttonHelper = false;
                }
            }
        }

        void btnSignature_Click(object sender, EventArgs e)
        {
            Signature dlg = new Signature();
            dlg.ShowDialog();
            if (dlg.retOk)
            {
                Bitmap imgSign = dlg.SavedBitmap;
                imgSign.Save(Application.StartupPath + "\\Sign.png", ImageFormat.Png);
                imgSign.Dispose();

                float fCharsRow = emrMedicalRecord.SetSingFont();
                emrMedicalRecord.CtlEditMode = 0;
                emrMedicalRecord.SelStart = emrMedicalRecord.CtlText.Length;
                if (emrMedicalRecord.Text.EndsWith("\n") == false)
                    emrMedicalRecord.LoadFromMemory("\n", 1, true);

                int[] iPos = new int[3];
                iPos = (int[])emrMedicalRecord.txtEdit.CurrentInputPosition;
                String sSingTitle = CurrBindKeyData.UserLevelName + "：" + CurrBindKeyData.UserName + "    " + "签名：";
                String sContext = new String('　', (int)(fCharsRow + 0.5f - 4 - sSingTitle.Length)) + sSingTitle;
                emrMedicalRecord.LoadFromMemory(sContext, 1, true);

                emrMedicalRecord.SetSingFont();
                emrMedicalRecord.txtEdit.FieldInsert("签名");
                emrMedicalRecord.txtEdit.FieldChangeable = false;
                //int[] iPos = new int[3];
                iPos = (int[])emrMedicalRecord.txtEdit.CurrentInputPosition;
                int x = emrMedicalRecord.txtEdit.FieldPosX + emrMedicalRecord.txtEdit.FieldText.Length;
                int y = emrMedicalRecord.txtEdit.FieldPosY;
                if (iPos[0] < 3)
                    y = y - (emrMedicalRecord.txtEdit.PageHeight + emrMedicalRecord.txtEdit.PageMarginT + emrMedicalRecord.txtEdit.PageMarginB) * (iPos[0] - 1);
                else
                    y = y - (emrMedicalRecord.txtEdit.PageHeight + emrMedicalRecord.txtEdit.PageMarginT + emrMedicalRecord.txtEdit.PageMarginB) * (iPos[0] - 1) + (iPos[0] - 2) * emrMedicalRecord.txtEdit.PageMarginT;

                emrMedicalRecord.txtEdit.FieldDelete(true);
                iPos = (int[])emrMedicalRecord.txtEdit.CurrentInputPosition;
                emrMedicalRecord.txtEdit.SelLength = 0;
                short iShowHeight = 12;
                emrMedicalRecord.txtEdit.ImageInsertFixed(Application.StartupPath + "\\Sign.png", iPos[0], x, y + iPos[0] * 20, iShowHeight, iShowHeight, 3, 0, 0, 0, 0);

                if (iPos[0] <= 1)
                    emrMedicalRecord.txtEdit.ObjectPosY = y + 20;
                emrMedicalRecord.txtEdit.ObjectPosX = x;
                //恢复最后插入文字的位置
                emrMedicalRecord.txtEdit.CurrentInputPosition = iPos;

                File.Delete(Application.StartupPath + "\\Sign.png");

                if (emrMedicalRecord.Text.EndsWith("\n") == false)
                    emrMedicalRecord.LoadFromMemory("\n", 1, true);

                emrMedicalRecord.CtlEditMode = 2;
            }
        }
        #endregion

        #region 打印
        void biWriteBack_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private int iPages = 1; //当前打印页码
        private FieldInfo m_Position;
        private MethodInfo m_SetPositionMethod;
        private PrintPreviewDialog Preview;
        void biPrint_Click(object sender, EventArgs e)
        {
            if (CurrBindKeyData.EmrDataId == -1) return;

            Preview = new PrintPreviewDialog();
            //显示打印对话框
            iPages = 1;
            PrintDialog dlgPrint = new PrintDialog();
            PrintDocument printer = new PrintDocument();
            dlgPrint.Document = printer;
            dlgPrint.AllowCurrentPage = true;
            dlgPrint.AllowSelection = true;
            dlgPrint.AllowSomePages = true;
            dlgPrint.PrinterSettings.FromPage = 1;
            dlgPrint.PrinterSettings.ToPage = emrMedicalRecord.txtEdit.CurrentPages;
            dlgPrint.PrinterSettings.MinimumPage = 1;
            dlgPrint.PrinterSettings.MaximumPage = emrMedicalRecord.txtEdit.CurrentPages;
            //printer.PrinterSettings.PrinterName = Common.GetDefaultPrinter();
            if (dlgPrint.ShowDialog() == DialogResult.OK)
            {
                iPages = printer.PrinterSettings.FromPage;
                printer.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
                Preview.Document = printer;
                Preview.PrintPreviewControl.Zoom = 1;
                Type type = typeof(System.Windows.Forms.PrintPreviewControl);
                m_Position = type.GetField("position", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.ExactBinding);
                m_SetPositionMethod = type.GetMethod("SetPositionNoInvalidate", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.ExactBinding);
                Preview.PrintPreviewControl.MouseWheel += new MouseEventHandler(Preview_MouseWheel);
                Preview.PrintPreviewControl.Top = 0;
                Preview.PrintPreviewControl.Left = 0;
                Preview.PrintPreviewControl.Width = Screen.PrimaryScreen.WorkingArea.Width;
                Preview.PrintPreviewControl.Height = Screen.PrimaryScreen.WorkingArea.Height;
                Preview.ShowIcon = false;
                Preview.Width = Screen.PrimaryScreen.WorkingArea.Width;
                Preview.Height = Screen.PrimaryScreen.WorkingArea.Height;
                Preview.Left = 0;
                Preview.Top = 0;
                Preview.Shown += new EventHandler(Preview_Shown);
                Preview.ShowDialog();
            }
            else
                iPages = 1;
        }

        void biMergeView_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void biSaveAsDoc_Click(object sender, EventArgs e)
        {
            if (saveAsFileDialog.ShowDialog() == DialogResult.OK)
            {
                int format=saveAsFileDialog.FilterIndex==1?9:12;
                emrMedicalRecord.Save(saveAsFileDialog.FileName, format, false);
            }
        }

        //一下是打印的实现
        private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            IntPtr hdc = ev.Graphics.GetHdc();
            emrMedicalRecord.txtEdit.PrintDevice = hdc.ToInt32();
            emrMedicalRecord.txtEdit.PrintPage((short)iPages);
            ev.Graphics.ReleaseHdc(hdc);
            iPages++;
            if (ev.PageSettings.PrinterSettings.PrintRange == System.Drawing.Printing.PrintRange.AllPages)
            {
                ev.HasMorePages = iPages <= emrMedicalRecord.txtEdit.CurrentPages;
                if (ev.HasMorePages == false)
                    iPages = 1;
            }
            else
            {
                ev.HasMorePages = iPages <= ev.PageSettings.PrinterSettings.ToPage;
                if (ev.HasMorePages == false)
                    iPages = ev.PageSettings.PrinterSettings.FromPage;
            }
        }
        private void Preview_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!SystemInformation.MouseWheelPresent)
                return;
            int scrollAmount;
            float amount = Math.Abs(e.Delta) / SystemInformation.MouseWheelScrollDelta;
            amount *= SystemInformation.MouseWheelScrollLines;
            amount *= 12;//Row height      
            amount *= (float)Preview.PrintPreviewControl.Zoom;//Zoom Rate   
            scrollAmount = e.Delta < 0 ? (int)amount : -(int)amount;
            Point curPos = (Point)(m_Position.GetValue(Preview.PrintPreviewControl));
            m_SetPositionMethod.Invoke(Preview.PrintPreviewControl, new object[] { new Point(curPos.X + 0, curPos.Y + scrollAmount) });
        }
        private void Preview_Shown(object sender, EventArgs e)
        {
            Form frm = (Form)(sender);
            frm.Width = Screen.PrimaryScreen.WorkingArea.Width;
            frm.Height = Screen.PrimaryScreen.WorkingArea.Height;
            frm.Left = 0;
            frm.Top = 0;
        }
        #endregion

        #region 页眉页脚
        void biPageSetup_Click(object sender, EventArgs e)
        {
            PageSettings ps = new PageSettings();
            //ps.Margins = new Margins(emrText.PageMarginL / 15, emrText.PageMarginR / 15, emrText.PageMarginT / 15, emrText.PageMarginB / 15);
            PageSetupDialog dlgPageSetup = new PageSetupDialog();
            dlgPageSetup.PageSettings = ps;
            if (dlgPageSetup.ShowDialog(this) == DialogResult.OK)
            {
                emrMedicalRecord.PageWidth = Convert.ToInt32(dlgPageSetup.PageSettings.PaperSize.Width * 15);
                emrMedicalRecord.PageHeight = Convert.ToInt32(dlgPageSetup.PageSettings.PaperSize.Height * 15);
                emrMedicalRecord.PageMarginL = Convert.ToInt32(dlgPageSetup.PageSettings.Margins.Left * 15);
                emrMedicalRecord.PageMarginR = Convert.ToInt32(dlgPageSetup.PageSettings.Margins.Right * 15);
                emrMedicalRecord.PageMarginT = Convert.ToInt32(dlgPageSetup.PageSettings.Margins.Top * 15);
                emrMedicalRecord.PageMarginB = Convert.ToInt32(dlgPageSetup.PageSettings.Margins.Bottom * 15);
            }
        }

        void biHeader_Click(object sender, EventArgs e)
        {
            if (biHeader.Checked)
            {
                if (biFooter.Checked)
                {
                    biFooter.Checked = false;
                    biFooter_Click(null, null);
                }
                emrMedicalRecord.HeaderFooter = emrMedicalRecord.HeaderFooter | HeaderFooterConstants.txHeader;
                emrMedicalRecord.HeaderFooterSelect(HeaderFooterConstants.txHeader);
                emrMedicalRecord.HeaderFooterSelect(0);
                emrMedicalRecord.HeaderFooterActivate(HeaderFooterConstants.txHeader);
            }
            else
            {
                emrMedicalRecord.HeaderFooterSelect(HeaderFooterConstants.txMainText);
                emrMedicalRecord.HeaderFooterSelect(0);
                emrMedicalRecord.HeaderFooterActivate(HeaderFooterConstants.txMainText);
            }
        }
        void biFooter_Click(object sender, EventArgs e)
        {
            if (biFooter.Checked)
            {
                if (biHeader.Checked)
                {
                    biHeader.Checked = false;
                    biHeader_Click(null, null);
                }
                emrMedicalRecord.HeaderFooter = emrMedicalRecord.HeaderFooter | HeaderFooterConstants.txFooter;
                emrMedicalRecord.HeaderFooterSelect(HeaderFooterConstants.txFooter);
                emrMedicalRecord.HeaderFooterSelect(0);
                emrMedicalRecord.HeaderFooterActivate(HeaderFooterConstants.txFooter);
            }
            else
            {
                emrMedicalRecord.HeaderFooterSelect(HeaderFooterConstants.txMainText);
                emrMedicalRecord.HeaderFooterSelect(0);
                emrMedicalRecord.HeaderFooterActivate(HeaderFooterConstants.txMainText);
            }
        }
        #endregion

        #region 域
        void biDeleteDomain_Click(object sender, EventArgs e)
        {
            emrMedicalRecord.FieldCurrent = emrMedicalRecord.FieldAtInputPos;
            emrMedicalRecord.FieldDelete(true);
        }

        void biEditDomain_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void biInsertDomain_Click(object sender, EventArgs e)
        {
            WriteDomain domain = new WriteDomain();
            domain.ShowDialog(this);
            if (domain.DialogResult == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(domain.domainId) && domain.domainType == (int)InputType.None)
                {
                    EmrInsertText(domain.domainText);
                }
                else
                    EmrInsertDomain(domain.domainText, domain.domainValue, "0", domain.domainId.ToString(), domain.domainType.ToString());
            }
        }
        #endregion
        #endregion

        #region 工具栏
        private void InstallToolsEvent()
        {
            //emrMedicalRecord.CommandViewMode = FrmMain.frmMain.APPCommandViewMode;
            //emrMedicalRecord.ZoomSlider = FrmMain.frmMain.zoomSlider;
            //emrMedicalRecord.HeaderFooter = HeaderFooterConstants.txMainText;
            //emrMedicalRecord.HeaderFooterStyle = HeaderFooterStyleConstants.txNoDblClk;

            buttonCopy.Command = emrMedicalRecord.CommandCopy;
            buttonCut.Command = emrMedicalRecord.CommandCut;
            buttonPaste.Command = emrMedicalRecord.CommandPaste;
            buttonFormatBrush.Command = emrMedicalRecord.CommandFormatBrush;
            buttonUnDo.Command = emrMedicalRecord.CommandUndo;
            buttonReDo.Command = emrMedicalRecord.CommandRedo;

            //buttonFind.Command = emrMedicalRecord.CommandFind;
            //buttonReplace.Command = emrMedicalRecord.CommandReplace;
            //buttonSelectAll.Command = emrMedicalRecord.CommandSelectAll;

            buttonAlignJustify.Command = emrMedicalRecord.CommandAlignJustify;
            buttonAlignCenter.Command = emrMedicalRecord.CommandAlignCenter;
            buttonAlignLeft.Command = emrMedicalRecord.CommandAlignLeft;
            buttonAlignRight.Command = emrMedicalRecord.CommandAlignRight;

            comboFont.Command = emrMedicalRecord.CommandFont;
            buttonFontBold.Command = emrMedicalRecord.CommandFontBold;
            buttonFontItalic.Command = emrMedicalRecord.CommandFontItalic;
            comboFontSize.Command = emrMedicalRecord.CommandFontSize;
            buttonFontStrike.Command = emrMedicalRecord.CommandFontStrike;
            buttonFontUnderline.Command = emrMedicalRecord.CommandFontUnderline;
            buttonTextColor.Command = emrMedicalRecord.CommandTextColor;

            buttonListBulleted.Command = emrMedicalRecord.CommandListBulleted;
            buttonListNumbered.Command = emrMedicalRecord.CommandListNumbered;
            buttonIndentL.Command = emrMedicalRecord.CommandOutdent;
            buttonIndentR.Command = emrMedicalRecord.CommandIndent;

            buttonLineSpacing.Command = emrMedicalRecord.CommandLineSpacing;
            buttonLineSpacing_1.Command = emrMedicalRecord.CommandLineSpacing;
            buttonLineSpacing_2.Command = emrMedicalRecord.CommandLineSpacing;
            buttonLineSpacing_3.Command = emrMedicalRecord.CommandLineSpacing;
            buttonLineSpacing_4.Command = emrMedicalRecord.CommandLineSpacing;
            buttonLineSpacing_5.Command = emrMedicalRecord.CommandLineSpacing;
            buttonLineSpacing_6.Command = emrMedicalRecord.CommandLineSpacing;

            buttonSubscript.Command = emrMedicalRecord.CommandSubScript;
            buttonSuperscript.Command = emrMedicalRecord.CommandSuperScript;
            buttonStanrdFont.Click += new EventHandler(buttonStanrdFont_Click);

            buttonTable.Click += new EventHandler(buttonTable_Click);
            buttonDrawLine.Click += new EventHandler(buttonDrawLine_Click);
            emrMedicalRecord.setEnableMenuItems(true);
        }

        void buttonDrawLine_Click(object sender, EventArgs e)
        {
            System.IO.StringWriter myStr = new System.IO.StringWriter();
            myStr.Write("{\\rtf1\\ansi\\ansicpg936\\uc1\\deff1{\\fonttbl\r\n{\\f0\\fnil\\fcharset134\\fprq2 \\'cb\\'ce\\'cc\\'e5;}\r\n{\\f1\\fswiss\\fcharset0\\fprq0 Calibri;}\r\n{\\f2\\fnil\\fcharset134\\fprq0 \\'cb\\'ce\\'cc\\'e5;}\r\n{\\f3\\froman\\fcharset2\\fprq2 Symbol;}}\r\n{\\colortbl;\\red0\\green0\\blue0;\\red255\\green255\\blue255;}\r\n{\\stylesheet{\\s0\\itap0\\nowidctlpar\\qj\\f1\\fs21 Normal;}{\\*\\cs10\\additive Default Paragraph Font;}{\\s16\\itap0\\nowidctlpar\\f0\\fs24 [Normal];}}\r\n{\\*\\generator TX_RTF32 15.1.531.501;}\r\n\\deftab1134\\paperw12240\\paperh15840\\margl1800\\margt1440\\margr1800\\margb1440\\widowctrl\\formshade\\sectd\r\n\\headery720\\footery720\\pgwsxn12240\\pghsxn15840\\marglsxn1800\\margtsxn1440\\margrsxn1800\\margbsxn1440\\pard\\itap0\\nowidctlpar\\qj\\brdrb\\brdrs\\brdrw10\\brdrbtw\\brdrs\\brdrw10\\tx420\\tx840\\tx1260\\tx1680\\tx2100\\tx2520\\tx2940\\tx3360\\tx3780\\tx4200\\tx4620\\tx5040\\tx5460\\tx5880\\plain\\f2\\fs21 \\par\\pard\\s16\\itap0\\nowidctlpar\\sl360\\slmult1\\par }");
            //myStr.Write(@"{\rtf1\ansi\ansicpg936{\fonttbl{\f0 \'c1\'a5\'ca\'e9;}{\f1 \'cb\'ce\'cc\'e5;}}{\colortbl ;\red0\green0\blue255 ;}\qc\f0\fs30 \'d5\'e2\'ca\'c7\'b5\'da\'d2\'bb\'b6\'ce\'ce\'c4\'b1\'be\'20\cf1 \'c1\'a5\'ca\'e9\'20\cf0\f1 \'be\'d3\'d6\'d0\'b6\'d4\'c6\'eb\'20ABC12345\par\pard\f1\fs20\cf1 \'d5\'e2\'ca\'c7\'b5\'da\'b6\'fe\'b6\'ce\'ce\'c4\'b1\'be\'20\'cb\'ce\'cc\'e5\'20\'d7\'f3\'b6\'d4\'c6\'eb\'20ABC12345}");
            myStr.Close();

            IDataObject data = new DataObject();
            data.SetData(DataFormats.Rtf, myStr.ToString());
            Clipboard.Clear();
            Clipboard.SetDataObject(data, true);
            emrMedicalRecord.txtEdit.Clip(3);
            Clipboard.Clear();
        }

        void buttonTable_Click(object sender, EventArgs e)
        {
            FrmTableColRow frmTableColRow = new FrmTableColRow();
            if (frmTableColRow.ShowDialog() == DialogResult.OK)
            {
                emrMedicalRecord.TableInsert((short)frmTableColRow.RowCount, (short)frmTableColRow.ColumnCount, -1);
            }
        }

        private void buttonStanrdFont_Click(object sender, EventArgs e)
        {
            if (emrMedicalRecord.SelText != null && emrMedicalRecord.SelText.Length > 0 && emrMedicalRecord.CtlEditMode == 0)
                emrMedicalRecord.SetSingFont();
        }
        #endregion

        #region IEmrRecord 成员
        private EmrControlType _emrControlType = EmrControlType.病历模板;
        [Browsable(false)]
        public EmrControlType emrControlType
        {
            get
            {
                return _emrControlType;
            }
            set
            {
                _emrControlType = value;
                //btnState();
            }
        }
        private EmrOperStyle _emrOperStyle = EmrOperStyle.默认;
        [Browsable(false)]
        public EmrOperStyle emrOperStyle
        {
            get
            {
                return _emrOperStyle;
            }
            set
            {
                _emrOperStyle = value;
                //btnState();
            }
        }

        private EmrDatastorageType _emrDatastorageType = EmrDatastorageType.文件存储;
        [Browsable(false)]
        public EmrDatastorageType emrDatastorageType
        {
            get
            {
                return _emrDatastorageType;
            }
            set
            {
                _emrDatastorageType = value;
                //btnState();
            }
        }

        public void btnState()
        {
            biFile.Visible = _isShowOpenBtn;
            switch (_emrControlType)
            {
                case EmrControlType.病历模板:
                    IcEditControl.Visible = true;
                    IcEmrControl.Visible = false;
                    IcViewControl.Visible = false;
                    IcPageControl.Visible = true;
                    IcDomainControl.Visible = true;

                    ToolBar.Visible = true;
                    break;
                case EmrControlType.病历预览:
                    IcEditControl.Visible = false;
                    IcEmrControl.Visible = false;
                    IcViewControl.Visible = false;
                    IcPageControl.Visible = false;
                    IcDomainControl.Visible = false;

                    ToolBar.Visible = false;
                    break;
                case EmrControlType.病历编辑:
                    IcEditControl.Visible = true;
                    IcEmrControl.Visible = true;
                    IcViewControl.Visible = true;
                    IcPageControl.Visible = false;
                    IcDomainControl.Visible = true;

                    ToolBar.Visible = true;

                    break;
            }

            if (_emrControlType == EmrControlType.病历模板 || _emrControlType == EmrControlType.病历编辑)
            {
                switch (_emrDatastorageType)
                {
                    case EmrDatastorageType.文件存储:
                        biOpen.Visible = true;

                        biDelete.Enabled = false;
                        biSignName.Enabled = false;
                        biBack.Enabled = false;
                        biBackReason.Enabled = false;
                        biWriteBack.Enabled = false;
                        biUpdateTitle.Enabled = false;
                        biExport.Enabled = false;
                        biImport.Enabled = false;
                        biSaveAs.Enabled = false;
                        biClinicData.Enabled = false;
                        biRefresh.Enabled = false;
                        biMergeView.Enabled = false;

                        break;
                    case EmrDatastorageType.数据库存储:
                        biOpen.Visible = true;

                        biDelete.Enabled = true;
                        biSignName.Enabled = true;
                        biBack.Enabled = true;
                        biBackReason.Enabled = true;
                        biWriteBack.Enabled = true;
                        biUpdateTitle.Enabled = true;
                        biExport.Enabled = true;
                        biImport.Enabled = true;
                        biSaveAs.Enabled = true;
                        biClinicData.Enabled = true;
                        biRefresh.Enabled = true;
                        biMergeView.Enabled = true;
                        break;
                }

                switch (_emrOperStyle)
                {
                    case EmrOperStyle.默认:
                        if (biAssistant.Checked)
                        {
                            biAssistant.Checked = false;
                            biAssistant_Click(null, null);
                        }
                        if (biHeader.Checked)
                        {
                            biHeader.Checked = false;
                            biHeader_Click(null, null);
                        }
                        if (biFooter.Checked)
                        {
                            biFooter.Checked = false;
                            biFooter_Click(null, null);
                        }

                        biOpen.Enabled = true;
                        biAdd.Enabled = true;
                        biModify.Enabled = true;
                        biDelete.Enabled = true;
                        biSave.Enabled = false;
                        biCancel.Enabled = false;

                        biSignName.Enabled = true;
                        biBack.Enabled = true;

                        biHeader.Enabled = false;
                        biFooter.Enabled = false;
                        biInsertDomain.Enabled = false;
                        biEditDomain.Enabled = false;
                        biDeleteDomain.Enabled = false;

                        ToolBar.Enabled = false;
                        //rightMenuStrip.Visible = false;
                        //emrMedicalRecord.Text = "";
                        emrMedicalRecord.SetSingFont();
                        emrMedicalRecord.CtlEditMode = 2; 
                        
                        break;
                    case EmrOperStyle.修改:
                        biOpen.Enabled = false;
                        biAdd.Enabled = false;
                        biModify.Enabled = false;
                        biDelete.Enabled = false;
                        biSave.Enabled = true;
                        biCancel.Enabled = true;

                        biSignName.Enabled = false;
                        biBack.Enabled = false;

                        biHeader.Enabled = true;
                        biFooter.Enabled = true;
                        biInsertDomain.Enabled = true;
                        biEditDomain.Enabled = true;
                        biDeleteDomain.Enabled = true;

                        ToolBar.Enabled = true;
                        //rightMenuStrip.Visible = true;
                        emrMedicalRecord.SetSingFont();
                        emrMedicalRecord.CtlEditMode = 0;
                        break;
                }
            }
            Btnbar.Refresh();
        }

        private void loadDataSourceValue(HeaderFooterConstants activate)
        {
            EmrTextControl.HeaderFooterActivate(activate);
            short fid = 0;
            string[] strdata = null;
            DataSourceManage.SetAllValue(delegate()
            {
                fid = EmrTextControl.FieldNext(fid, 0);
                if (fid <= 0) return null;
                object fdata = EmrTextControl.get_FieldData(fid);
                if (fdata == null) return "";
                strdata = fdata.ToString().Split('|');
                if (strdata.Length != 4) return "";
                if (strdata[0] == "0")
                {
                    //第几页
                    if (strdata[2] == "Gpage")
                    {
                        EmrTextControl.set_FieldType(fid, FieldTypeConstants.txFieldPageNumber);
                        return "";
                    }
                    //共多少页
                    if (strdata[2] == "Gpcount")
                    {
                        EmrTextControl.FieldCurrent = fid;
                        EmrTextControl.FieldText = EmrTextControl.CurrentPages.ToString();
                        return "";
                    }
                }
                return strdata[2];
            }, delegate(string text, string value)
            {
                if (!(text == "" && value == ""))
                {
                    EmrTextControl.FieldCurrent = fid;
                    EmrTextControl.FieldText = text;
                    EmrTextControl.set_FieldData(fid, strdata[0] + "|" + strdata[1] + "|" + strdata[2] + "|" + value);
                }
            });
        }

        public void ClearEmrText()
        {
            emrMedicalRecord.HeaderFooterActivate(HeaderFooterConstants.txMainText);
            emrMedicalRecord.Text = "";
            emrMedicalRecord.HeaderFooter = (HeaderFooterConstants)5;
            emrMedicalRecord.HeaderFooterStyle = HeaderFooterStyleConstants.txNoDblClk;
        }

        public void LoadEmrText(byte[] wordByte)
        {
            if (CloseedEmrData != null)
                CloseedEmrData(this, new EventArgs());

            emrMedicalRecord.HeaderFooterActivate(HeaderFooterConstants.txMainText);
            emrMedicalRecord.Text = "";
            if (wordByte == null || wordByte.Length <= 0)
            {
                return;
            }
            //1.加载病历内容
            emrMedicalRecord.SetSingFont();
            emrMedicalRecord.LoadFromMemory(wordByte, 3, true);
            if (emrMedicalRecord.Text.EndsWith("\n") == false)
                emrMedicalRecord.LoadFromMemory("\n", 1, true);//加改行目的是防止最后列表位置改变

            emrMedicalRecord.HeaderFooter = (HeaderFooterConstants)5;
            emrMedicalRecord.HeaderFooterStyle = HeaderFooterStyleConstants.txNoDblClk;

            //2.病历编辑下获取数据源
            if (_emrControlType == EmrControlType.病历编辑 || _emrControlType == EmrControlType.病历预览)
            {
                loadDataSourceValue(HeaderFooterConstants.txHeader);
                loadDataSourceValue(HeaderFooterConstants.txFooter);
                loadDataSourceValue(HeaderFooterConstants.txMainText);
            }

            //3.病历模板下清空数据源
            if (_emrControlType == EmrControlType.病历模板)
            {
                short fid = 0;
                fid = EmrTextControl.FieldNext(fid, 0);
                while (fid > 0)
                {
                    EmrTextControl.FieldCurrent = fid;
                    EmrEditFieldData _efielddata = new EmrEditFieldData(EmrTextControl.get_FieldData(fid).ToString(), EmrTextControl.FieldText);
                    if (_efielddata.dtype == 2)
                    {
                        XmlNodeList xnlist = KnowledgeManage.DataSource_xd.DocumentElement.SelectNodes("DataClass/Value[@id='" + _efielddata.elId + "']");
                        if (xnlist != null)
                        {
                            _efielddata.Text = "{" + xnlist[0].Attributes["name"].Value + "}";//获取数据源XML
                            _efielddata.Value = "";
                        }
                        else
                        {
                            _efielddata.Text = "";//获取数据源XML
                            _efielddata.Value = "";
                        }
                        EmrTextControl.FieldText = _efielddata.Text;
                        EmrTextControl.set_FieldData(fid, _efielddata.dtype.ToString() + "|" + Convert.ToString((int)_efielddata.inputType) + "|" + _efielddata.elId + "|" + _efielddata.Value);
                    }
                    //下一个
                    fid = EmrTextControl.FieldNext(fid, 0);
                }
            }

            //4.显示签名
            if (emrOperStyle == EmrOperStyle.默认)
                LoadEmrSignName();

            if (OpenedEmrData != null)
                OpenedEmrData(this, new EventArgs());
        }

        public void LoadEmrSignName()
        {
            emrMedicalRecord.CtlEditMode = 0;//病历控件进入编辑状态，追加签名内容
            Bitmap imgSign;
            string levelname;
            string doctorname;
            if (CurrBindKeyData.FirstSignature == 1)//一级签名
            {
                controller.GetSignData(1, out imgSign, out levelname, out doctorname);
                addSignName(imgSign, levelname, doctorname);
            }
            if (CurrBindKeyData.SecondSignature == 1)//二级签名
            {
                controller.GetSignData(2, out imgSign, out levelname, out doctorname);
                addSignName(imgSign, levelname, doctorname);
            }
            if (CurrBindKeyData.ThreeSignature == 1)//三级签名
            {
                controller.GetSignData(3, out imgSign, out levelname, out doctorname);
                addSignName(imgSign, levelname, doctorname);
            }
            emrMedicalRecord.CtlEditMode = 2;//病历控件进入只读状态
        }

        private void addSignName(Bitmap imgSign, string levelname, string doctorname)
        {
            if (imgSign != null)
            {
                imgSign.Save(Application.StartupPath + "\\Sign.png", ImageFormat.Png);
                imgSign.Dispose();
            }

            float fCharsRow = emrMedicalRecord.SetSingFont();
            emrMedicalRecord.SelStart = emrMedicalRecord.CtlText.Length;
            if (emrMedicalRecord.Text.EndsWith("\n") == false)
                emrMedicalRecord.LoadFromMemory("\n", 1, true);

            int[] iPos = new int[3];
            iPos = (int[])emrMedicalRecord.txtEdit.CurrentInputPosition;
            String sSingTitle = levelname + "：" + doctorname + "    " + "签名：";
            String sContext = new String('　', (int)(fCharsRow + 0.5f - 4 - sSingTitle.Length)) + sSingTitle;
            emrMedicalRecord.LoadFromMemory(sContext, 1, true);

            if (imgSign != null)
            {
                emrMedicalRecord.SetSingFont();
                emrMedicalRecord.txtEdit.FieldInsert("签名");
                emrMedicalRecord.txtEdit.FieldChangeable = false;
                //int[] iPos = new int[3];
                iPos = (int[])emrMedicalRecord.txtEdit.CurrentInputPosition;
                int x = emrMedicalRecord.txtEdit.FieldPosX + emrMedicalRecord.txtEdit.FieldText.Length;
                int y = emrMedicalRecord.txtEdit.FieldPosY;
                if (iPos[0] < 3)
                    y = y - (emrMedicalRecord.txtEdit.PageHeight + emrMedicalRecord.txtEdit.PageMarginT + emrMedicalRecord.txtEdit.PageMarginB) * (iPos[0] - 1);
                else
                    y = y - (emrMedicalRecord.txtEdit.PageHeight + emrMedicalRecord.txtEdit.PageMarginT + emrMedicalRecord.txtEdit.PageMarginB) * (iPos[0] - 1) + (iPos[0] - 2) * emrMedicalRecord.txtEdit.PageMarginT;

                emrMedicalRecord.txtEdit.FieldDelete(true);
                iPos = (int[])emrMedicalRecord.txtEdit.CurrentInputPosition;
                emrMedicalRecord.txtEdit.SelLength = 0;
                short iShowHeight = 12;
                emrMedicalRecord.txtEdit.ImageInsertFixed(Application.StartupPath + "\\Sign.png", iPos[0], x, y + iPos[0] * 20, iShowHeight, iShowHeight, 3, 0, 0, 0, 0);

                if (iPos[0] <= 1)
                    emrMedicalRecord.txtEdit.ObjectPosY = y + 20;
                emrMedicalRecord.txtEdit.ObjectPosX = x;
                //恢复最后插入文字的位置
                emrMedicalRecord.txtEdit.CurrentInputPosition = iPos;

                File.Delete(Application.StartupPath + "\\Sign.png");
            }
            if (emrMedicalRecord.Text.EndsWith("\n") == false)
                emrMedicalRecord.LoadFromMemory("\n", 1, true);
        }

        public byte[] GetEmrText()
        {
            return emrMedicalRecord.SaveToByteArray(false);
        }

        //插入普通文本
        public void EmrInsertText(string text)
        {
            emrMedicalRecord.SelText = text;
            emrMedicalRecord.Focus();

            if (EmrTextChanged != null)
                EmrTextChanged(this, new EventArgs());
        }
        //插入域，数据动态
        public void EmrInsertDomain(string text, string value, string dtype, string elId, string inputType)
        {
            //emrMedicalRecord.ForeColor = Color.Red;
            if (emrMedicalRecord.FieldAtInputPos > 0)
            {
                MessageBoxEx.Show("控件不支持在元素内插入元素!");
                //emrMedicalRecord.ForeColor = Color.Black;
                return;
            }

            EmrEditFieldData _efielddata = new EmrEditFieldData(text, value, dtype, elId, inputType);

            string fielddata = "";
            string fieldtext = "";
            string _fontname = emrMedicalRecord.FontName;
            short _fontbold = emrMedicalRecord.FontBold;
            string fontname = emrMedicalRecord.FontName;
            short fontbold = emrMedicalRecord.FontBold;

            _efielddata.getdata(emrControlType, out fielddata, out fieldtext, ref fontname, ref fontbold);

            emrMedicalRecord.FontBold = fontbold;
            emrMedicalRecord.FontName = fontname;
            if (_fontname != fontname)
                emrMedicalRecord.SelText = " ";

            emrMedicalRecord.FieldInsert(fieldtext);
            short id = emrMedicalRecord.FieldCurrent;
            //emrMedicalRecord.TransCheckStyle(1);
            emrMedicalRecord.FieldChangeable = false;
            //如：1|1|PatientNum|00012345  数据源|List|住院号|00012345   ??
            emrMedicalRecord.SetFieldData(id, fielddata);
            emrMedicalRecord.FieldEditAttr(id, 16);

            emrMedicalRecord.FontBold = _fontbold;
            emrMedicalRecord.FontName = _fontname;

            emrMedicalRecord.SelText = " ";//追加空文本
            emrMedicalRecord.Focus();

            if (EmrTextChanged != null)
                EmrTextChanged(this, new EventArgs());

            #region old code
            /*
            string fieldtext = string.IsNullOrEmpty(text) ? "{空}" : text;
            string fieldvalue = string.IsNullOrEmpty(value) ? "<none>" : value;
            //获取数据源值
            if (dtype == "2" && _emrControlType==EmrControlType.病历编辑)
            {
                DataSourceManage.SetValue(elId, delegate(string _text, string _value)
                {
                    if (_text != "" || _value != "")
                    {
                        fieldtext = _text;
                        fieldvalue = _value;
                    }
                });
            }
            //获取复选框
            if (dtype == "0" && (elId == "SysTrueOrFalse"))
            {
                string _fontname = emrMedicalRecord.FontName;
                short _fontbold = emrMedicalRecord.FontBold;
                if (fieldtext == "o")
                {
                    emrMedicalRecord.FontName = "Wingdings";
                }
                else if (fieldtext == "R" ||fieldtext == "T")
                {
                    emrMedicalRecord.FontBold = 1;
                    emrMedicalRecord.FontName = "Wingdings 2";
                }
                emrMedicalRecord.FieldInsert(fieldtext);
                short id = emrMedicalRecord.FieldCurrent;
                //emrMedicalRecord.TransCheckStyle(1);
                emrMedicalRecord.FieldChangeable = false;
                //如：1|1|PatientNum|00012345  数据源|List|住院号|00012345   ??
                emrMedicalRecord.SetFieldData(id, dtype + "|" + inputType + "|" + elId + "|" + fieldvalue);
                emrMedicalRecord.FieldEditAttr(id, 16);

                emrMedicalRecord.FontBold = _fontbold;
                emrMedicalRecord.FontName = _fontname;
            }
            else
            {
                emrMedicalRecord.FieldInsert(fieldtext);
                short id = emrMedicalRecord.FieldCurrent;
                //emrMedicalRecord.TransCheckStyle(1);
                emrMedicalRecord.FieldChangeable = false;
                //如：1|1|PatientNum|00012345  数据源|List|住院号|00012345   ??
                emrMedicalRecord.SetFieldData(id, dtype + "|" + inputType + "|" + elId + "|" + fieldvalue);
                emrMedicalRecord.FieldEditAttr(id, 16);
                //emrMedicalRecord.ForeColor = Color.Black;
            }
            emrMedicalRecord.SelText = " ";//追加空文本
            emrMedicalRecord.Focus();

            if (EmrTextChanged != null)
                EmrTextChanged(this, new EventArgs());
            */
            #endregion
        }
        //插入段标题
        public void EmrInsertDomain(string text, string elId, bool isParagraph)
        {
            if (emrMedicalRecord.FieldAtInputPos > 0)
            {
                MessageBoxEx.Show("控件不支持在元素内插入元素！");
                return;
            }

            if (isParagraph && ((int[])EmrTextControl.CurrentInputPosition)[2] > 0)
                //emrMedicalRecord.LoadFromMemory("\r\n", 1, true);
                EmrTextControl.SelText = "\v";//换行
            if (isParagraph) emrMedicalRecord.FontBold = 1;
            //if (isParagraph) EmrTextControl.IndentL += 240 * 2;//右缩进??

            //emrMedicalRecord.ForeColor = Color.Red;
            emrMedicalRecord.FieldInsert(text);//第一步：插入文本
            short id = emrMedicalRecord.FieldCurrent;
            emrMedicalRecord.FieldChangeable = false;
            emrMedicalRecord.SetFieldData(id, "0|0|" + elId + "|<none>");
            emrMedicalRecord.FieldEditAttr(id, 16);
            //emrMedicalRecord.ForeColor = Color.Black;
            if (isParagraph) emrMedicalRecord.FontBold = 0;
            emrMedicalRecord.SelText = " ";//追加空文本
            emrMedicalRecord.Focus();

            if (EmrTextChanged != null)
                EmrTextChanged(this, new EventArgs());
        }
        [Browsable(false)]
        public AxTx4oleLib.AxTXTextControl EmrTextControl
        {
            get { return emrMedicalRecord.txtEdit; }
        }

        [Browsable(false)]
        public FileInfo currFileInfo
        {
            get;
            set;
        }
        private EmrBindKeyData _currBindKeyData = null;
        [Browsable(false)]
        public EmrBindKeyData CurrBindKeyData
        {
            get
            {
                return _currBindKeyData;
            }
            set
            {
                _currBindKeyData = value;
            }
        }

        public void btnSignState()
        {
            if (emrControlType == EmrControlType.病历编辑)
            {
                #region 根据签名状态，显示签名按钮
                bool _sign, _back;
                controller.GetSignNameState(out _sign, out _back);
                biSignName.Enabled = _sign;
                biBack.Enabled = _back;
                #endregion
            }
        }

        public void btnModifyState()
        {
            if (emrControlType == EmrControlType.病历编辑)
            {
                #region 根据签名状态，显示签名按钮
                bool _modify;
                controller.GetModifyState(out _modify);
                biModify.Enabled = _modify;
                #endregion

                //签名后的病历不能再调用模板
                if (CurrBindKeyData.FirstSignature == 1)
                    biTemplate.Enabled = false;
                else
                    biTemplate.Enabled = true;
            }
        }

        #endregion

        #region 右键菜单
        private void 插入域ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            biInsertDomain.RaiseClick();
        }

        private void 删除域ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            biDeleteDomain.RaiseClick();
        }

        private void 插入表格ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonTable.RaiseClick();
        }

        private void 删除表格ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //EmrTextControl.set_TableCellAttribute(EmrTextControl.TabCurrent, 0, 0, TableCellAttributeConstants.txTableCellBackColor, Color.Red);
            //EmrTextControl.TableAttrDialog ();
        }


        private void 插入行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EmrTextControl.TableInsertLines(TableInsertConstants.txTableInsertAfter, 1);
        }

        private void 删除行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EmrTextControl.TableDeleteLines();
        }

        private void 插入列ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EmrTextControl.TableInsertColumn(TableInsertConstants.txTableInsertAfter);
        }

        private void 删除列ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EmrTextControl.TableDeleteColumn();
        }

        private void 剪切ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonCut.RaiseClick();
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonCopy.RaiseClick();
        }

        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonPaste.RaiseClick();
        }

        private void rightMenuStrip_Opened(object sender, EventArgs e)
        {
            bool bIsTableObject = EmrTextControl.TableAtInputPos != 0;
            if (bIsTableObject)
            {
                插入表格ToolStripMenuItem.Enabled = false;
                删除表格ToolStripMenuItem.Enabled = true;
                插入行ToolStripMenuItem.Enabled = true;
                删除行ToolStripMenuItem.Enabled = true;
                插入列ToolStripMenuItem.Enabled = true;
                删除列ToolStripMenuItem.Enabled = true;

            }
            else {
                插入表格ToolStripMenuItem.Enabled = true;
                删除表格ToolStripMenuItem.Enabled = false;
                插入行ToolStripMenuItem.Enabled = false;
                删除行ToolStripMenuItem.Enabled = false;
                插入列ToolStripMenuItem.Enabled = false;
                删除列ToolStripMenuItem.Enabled = false;
            }

            剪切ToolStripMenuItem.Enabled = buttonCut.Enabled;
            复制ToolStripMenuItem.Enabled = buttonCopy.Enabled;
            粘贴ToolStripMenuItem.Enabled = buttonPaste.Enabled;

            if (emrOperStyle == EmrOperStyle.默认)
            {
                rightMenuStrip.Enabled = false;
            }
            else
            {
                rightMenuStrip.Enabled = true;
            }
        }

        #endregion

      
        
    }
}
