using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using CustomDocument.Controls.Properties;

namespace CustomDocument.Controls
{
    [LicenseProvider(typeof(BaseControls.Licensing.ComponentLicenseProvider))]
    [Designer(typeof(UserControlDesigner))]
    public partial class CustomDocumentControl : UserControl
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
            BaseControls.Licensing.ComponentLicense lic = LicenseManager.Validate(typeof(CustomDocumentControl), this) as BaseControls.Licensing.ComponentLicense;
            if (lic != null)
            {
                _isDemo = lic.IsDemo;
                if (_isDemo)
                {
                    MessageBox.Show("正确设置[CustomDocumentControl]控件的许可证，才能使用！");
                }
            }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            CheckLicense();
        }
        #endregion

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Panel ContentArea
        {
            get
            {
                return pcbFormBack; // 在上面放一个Panel，名字叫 panel1
            }
        }

        private DisplayForm _PageDisplayForm=new DisplayForm("A5");

        private List<DisplayForm> _customDisplayForm;
        [Description("自定义页面格式")]
        public List<DisplayForm> PageDisplayForm
        {
            get
            {
                return _customDisplayForm;
            }
            set
            {
                _customDisplayForm = value;
            }
        }

        private PageType _pageType = PageType.A5;
        [Description("设置页面格式")]
        public PageType SelectPageType
        {
            get
            {
                return _pageType;
            }
            set
            {
                _pageType = value;
                switch (_pageType)
                {
                    case PageType.A5:
                        _PageDisplayForm = new DisplayForm("A5", 1480, 2100, 100, 100);
                        DrawMain();
                        break;
                    case PageType.A4:
                        _PageDisplayForm = new DisplayForm("A4", 2100, 2970, 100, 100);
                        DrawMain();
                        break;
                    case PageType.Custom:
                        DrawMain();
                        break;
                }
            }
        }

        private BackgroundImageType _backgroundImageType = BackgroundImageType.正常皮肤;
        [Description("设置背景图片")]
        public BackgroundImageType SelectBackgroundImageType
        {
            get
            {
                return _backgroundImageType;
            }
            set
            {
                _backgroundImageType = value;
                DrawMain();
            }
        }

    

        public CustomDocumentControl()
        {
            
            InitializeComponent();
            DrawMain();
            pcbFormBack.ControlAdded += new ControlEventHandler(pcbFormBack_ControlAdded);
            pcbFormBack.Parent = this;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();
        }

        void pcbFormBack_ControlAdded(object sender, ControlEventArgs e)
        {
            e.Control.Parent = pcbFormBack;
        }

        private void DrawMain()
        {
            if (_PageDisplayForm != null)
            {
               

                //生成背景
                Graphics Canvas = pcbFormBack.CreateGraphics();
                //转换为以像素表示的单位
                _PageDisplayForm.xPixPermm = Canvas.DpiX / 254.0f;
                _PageDisplayForm.yPixPermm = Canvas.DpiY / 254.0f;
                _PageDisplayForm.xScrDPIDiff = Canvas.DpiX / 96.0f;
                _PageDisplayForm.yScrDPIDiff = Canvas.DpiY / 96.0f;
                Canvas.Dispose();
                pcbFormBack.Width = Convert.ToInt32(_PageDisplayForm.PageWidth * _PageDisplayForm.xPixPermm);
                pcbFormBack.Height = Convert.ToInt32(_PageDisplayForm.PageHeight * _PageDisplayForm.yPixPermm);
                pcbFormBack.Visible = false;
                Bitmap bmpBack = new Bitmap((int)(_PageDisplayForm.PageWidth * _PageDisplayForm.xPixPermm),
                                            (int)(_PageDisplayForm.PageHeight * _PageDisplayForm.yPixPermm));
                Canvas = Graphics.FromImage(bmpBack);
                Canvas.PageUnit = GraphicsUnit.Pixel;
                Canvas.SmoothingMode = SmoothingMode.HighQuality;
                Canvas.PixelOffsetMode = PixelOffsetMode.HighQuality;
                Canvas.FillRectangle(Brushes.White, 0, 0, bmpBack.Width, bmpBack.Height);
                //画背景纹理图片
                DrawFormBackground(Canvas, _PageDisplayForm);

                //foreach (Control sobj in pcbFormBack.Controls)
                //    pcbFormBack.Controls.Remove(sobj);

                this.Size=new Size(pcbFormBack.Width + 20,pcbFormBack.Height + 20);

                this.SuspendLayout();

                //DrawFormContent(Canvas, _PageDisplayForm);

                Canvas.Dispose();
                this.ResumeLayout(false);
                this.PerformLayout();
                pcbFormBack.BackgroundImage = bmpBack;
                pcbFormBack.Left = Math.Max(8, (this.Width - 30 - pcbFormBack.Width) / 2);
                pcbFormBack.Visible = true;
            }
        }

        private void DrawFormBackground(Graphics Canvas, DisplayForm op_form)
        {
            //画背景纹理图片
            Image imgBack = Resources.灰色皮肤;

            switch (_backgroundImageType)
            {
                case BackgroundImageType.正常皮肤:
                    imgBack = Resources.正常皮肤;
                    break;
                case BackgroundImageType.灰色皮肤:
                    imgBack = Resources.灰色皮肤;
                    break;
                case BackgroundImageType.黄色皮肤:
                    imgBack = Resources.黄色皮肤;
                    break;
                case BackgroundImageType.红色皮肤:
                    imgBack = Resources.红色皮肤;
                    break;
                case BackgroundImageType.白色皮肤:
                    imgBack = Resources.白色皮肤;
                    break;
            }

            int iCol = (int)(op_form.PageWidth * op_form.xPixPermm) / imgBack.Width + 1;
            int iRow = (int)(op_form.PageHeight * op_form.yPixPermm) / imgBack.Height + 1;
            for (int i = 0; i < iRow; i++)
                for (int j = 0; j < iCol; j++)
                    Canvas.DrawImage(imgBack, j * imgBack.Width, i * imgBack.Height);
        }

        private void palView_Resize(object sender, EventArgs e)
        {
            pcbFormBack.Left = Math.Max(8, (this.Width - 30 - pcbFormBack.Width) / 2);
        }

        
    }

    public class UserControlDesigner : ControlDesigner
    {
        public override void Initialize(System.ComponentModel.IComponent Ic)
        {
            base.Initialize(Ic);
            CustomDocumentControl UC = (CustomDocumentControl)Ic;
            EnableDesignMode(UC.ContentArea, "ContentArea");
        }
    }

    public enum BackgroundImageType
    {
        正常皮肤, 红色皮肤, 黄色皮肤, 灰色皮肤,白色皮肤
    }

    public enum PageType
    {
        A4,A5,Custom
    }

    public class DisplayForm
    {
        public DisplayForm()
        {
        }

        public DisplayForm(string _formName)
        {
            DisplayFormName = _formName;
            PageWidth = 1480;
            PageHeight = 2100;
            TopMarign = 100;
            LeftMarign = 100;
        }

        public DisplayForm(string _formName, int _pagewidth, int _pageheight, int _topmarign, int _leftmarign)
        {
            DisplayFormName = _formName;
            PageWidth = _pagewidth;
            PageHeight = _pageheight;
            TopMarign = _topmarign;
            LeftMarign = _leftmarign;
        }

        /// <summary>
        /// 格式名称
        /// </summary>
        public string DisplayFormName { get; set; }
        /// <summary>
        /// 页面宽度, 单位 0.1mm
        /// </summary>
        public int PageWidth { get; set; }
        /// <summary>
        /// 页面高度, 单位 0.1mm
        /// </summary>
        public int PageHeight { get; set; }
        /// <summary>
        /// 上下边距, 单位 0.1mm
        /// </summary>
        public int TopMarign { get; set; }
        /// <summary>
        /// 左右边距, 单位 0.1mm
        /// </summary>
        public int LeftMarign { get; set; }
        /// <summary>
        /// 进纸方向，1--横向 0--纵向
        /// </summary>
        public int PageOrientate { get; set; }
        /// <summary>
        /// 纸张名称：如 A4, A5等
        /// </summary>
        public string PaperName { get; set; }
        /// <summary>
        /// X方向每0.1毫米单对应的象素单位
        /// </summary>
        public float xPixPermm { get; set; }
        /// <summary>
        /// Y方向每0.1毫米单对应的象素单位
        /// </summary>
        public float yPixPermm { get; set; }
        /// <summary>
        /// X方向屏幕DIP误差纠正
        /// </summary>
        public float xScrDPIDiff { get; set; }
        /// <summary>
        /// Y方向屏幕DIP误差纠正
        /// </summary>
        public float yScrDPIDiff { get; set; }

        /// <summary>
        /// 保留的控制选项，按位使用
        /// </summary>
        public int Options { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        public string Memo { get; set; }
    }
}
