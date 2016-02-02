using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tx4oleLib;
using DevComponents.DotNetBar;
using System.Runtime.InteropServices;
using BaseControls;

namespace EMR.Controls
{

    public struct FileFormat
    {
        public const int ANSI = 1;
        public const int TXANSI = 2;
        public const int TXANSIFormat = 3;
        public const int HTML = 4;
        public const int RTF = 5;
        public const int Unicode = 6;
        public const int TXUnicode = 7;
        public const int TXUnicodeFormat = 8;
        public const int Doc = 9;
        public const int XML = 10;
        public const int CSS = 11;
        public const int PDF = 12;
        public const int Docx = 13;
    }

    

    internal partial class EMREdit : UserControl
    {
        /// <summary>
        ///  该控件暴露的方法一般是"set_"开头的，
        /// </summary>

        #region 发送窗口消息函数
        private static int WM_EMR_CLIPBOARD = 5794;

        [DllImport("user32.dll", EntryPoint = "PostMessageA")]
        private static extern int PostMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        #endregion

        [Serializable]
        public class HYEMRDATA      //自定义剪贴板结构
        {
            //public int m_iPatientID;             //当前病人ID
            public Object m_pDataObject;     //原来数据对象
            public HYEMRDATA(IDataObject pDataObject)
            {
                //m_iPatientID = iPatientID;             //当前病人ID
                m_pDataObject = pDataObject.GetData(DataFormats.Rtf);
            }
            public bool CopyToClipboard()
            {
                if (m_pDataObject == null)
                    return false;

                Clipboard.Clear();
                DataFormats.Format format = DataFormats.GetFormat(typeof(HYEMRDATA).FullName);
                IDataObject dataObj = new DataObject();
                dataObj.SetData(format.Name, false, this);
                Clipboard.SetDataObject(dataObj, false);
                return true;
            }
            //恢复原有数据到剪贴板
            public bool RestoreToClipboard()
            {
                if (m_pDataObject == null)
                    return false;
                DataFormats.Format format = DataFormats.GetFormat(typeof(HYEMRDATA).FullName);
                IDataObject dataOld = new DataObject();
                dataOld.SetData(DataFormats.Rtf, m_pDataObject);
                Clipboard.Clear();
                Clipboard.SetDataObject(dataOld, false);
                return true;
            }
        }

        #region 自定义属性
        private EventHandler eventValueChanged;
        private EventHandler eventCommandExecute;
        //private ElementSelect eselect;    //元素选择
        private DevComponents.DotNetBar.SliderItem _zoomslider;
        private Command _commandViewMode;
        private bool UpdateStateing = false;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Command CommandViewMode
        {
            get
            {
                return _commandViewMode;
            }
            set
            {
                _commandViewMode = value;
                if (_commandViewMode != null)
                {
                    eventCommandExecute = new EventHandler(CommandViewMode_Executed);
                    _commandViewMode.Executed += eventCommandExecute;
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CurrentInputFrontIsLF
        {
            get
            {
                if (this.txtEdit.SelStart <= 0 || txtEdit.SelLength > 0)
                    return false;
                txtEdit.SelStart = txtEdit.SelStart - 1;
                txtEdit.SelLength = 1;
                return txtEdit.SelText == "\n";
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DevComponents.DotNetBar.SliderItem ZoomSlider
        {
            get
            {
                return _zoomslider;
            }
            set
            {
                _zoomslider = value;
                if (_zoomslider != null)
                {
                    eventValueChanged = new EventHandler(ZoomSlider_ValueChanged);
                    _zoomslider.ValueChanged += eventValueChanged;
                    if (_zoomslider != null && Convert.ToInt16(ZoomSlider.Text.Substring(0, ZoomSlider.Text.Length - 1)) > 10)
                    {
                        txtEdit.ZoomFactor = Convert.ToInt16(ZoomSlider.Text.Substring(0, ZoomSlider.Text.Length - 1));
                    }
                    else
                    {
                        txtEdit.ZoomFactor = 100;
                    }
                }
            }
        }

        private Point _mousepoint;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Point MousePoint
        {
            get
            {
                return _mousepoint;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(true)]
        public override string Text
        {
            get
            {
                return txtEdit.CtlText;
            }
            set
            {
                txtEdit.CtlText = value;
                base.Text = value;
            }
        }

        private bool _emendstate = false;
        /// <summary>
        /// 是否在修定状态中
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool EmendState
        {
            get
            {
                return _emendstate;
            }
            set
            {
                _emendstate = value;
            }
        }
        
        private bool _FieldEditMode = false;
        /// <summary>
        /// 域任意修改模式
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool FieldEditMode
        {
            get
            {
                return _FieldEditMode;
            }
            set
            {
                _FieldEditMode = value;
                if (_FieldEditMode)
                    txtEdit.FieldSetInputPos(FieldInputPositionConstants.txInsideField);
                else
                    txtEdit.FieldSetInputPos(FieldInputPositionConstants.txOutsideField);
            }
        }

        private bool _documentChanged = false;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DocumentChanged
        {
            get
            {
                return _documentChanged;
            }
        }

        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                SelectTextFont = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Font SelectTextFont
        {
            get
            {
                FontStyle style = FontStyle.Regular;

                if (txtEdit.FontBold == 1)
                {
                    style = style | FontStyle.Bold;
                }
                if (txtEdit.FontItalic == 1)
                {
                    style = style | FontStyle.Italic;
                }
                if (txtEdit.FontUnderline == 1)
                {
                    style = style | FontStyle.Underline;
                }
                if (txtEdit.FontStrikethru == 1)
                {
                    style = style | FontStyle.Strikeout;
                }

                return new Font(txtEdit.FontName, txtEdit.FontSize, style);
            }
            set
            {
                txtEdit.FontName = value.Name;
                txtEdit.FontSize = (short)value.Size;
                txtEdit.FontBold = (short)(value.Bold ? 1 : 0);
                txtEdit.FontItalic = (short)(value.Italic ? 1 : 0);
                txtEdit.FontUnderline = (short)(value.Underline ? 1 : 0);
                txtEdit.FontStrikethru = (short)(value.Strikeout ? 1 : 0);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short Alignment
        {
            get
            {
                return txtEdit.Alignment;
            }
            set
            {
                txtEdit.Alignment = value;
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AllowDrag
        {
            get
            {
                return txtEdit.AllowDrag;
            }
            set
            {
                txtEdit.AllowDrag = value;
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AllowUndo
        {
            get
            {
                return txtEdit.AllowUndo;
            }
            set
            {
                txtEdit.AllowUndo = value;
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AutoExpand
        { get { return txtEdit.AutoExpand; } set { txtEdit.AutoExpand = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(true)]
        public override Color BackColor { get { return base.BackColor; } set { base.BackColor = value; txtEdit.BackColor = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short BackStyle { get { return txtEdit.BackStyle; } set { txtEdit.BackStyle = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short BaseLine { get { return txtEdit.BaseLine; } set { txtEdit.BaseLine = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short BorderStyle { get { return txtEdit.BorderStyle; } set { txtEdit.BorderStyle = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ButtonBarHandle { get { return txtEdit.ButtonBarHandle; } set { txtEdit.ButtonBarHandle = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanCopy { get { return txtEdit.CanCopy; } set { txtEdit.CanCopy = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanPaste { get { return txtEdit.CanPaste; } set { txtEdit.CanPaste = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short CanRedo { get { return txtEdit.CanRedo; } set { txtEdit.CanRedo = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short CanUndo { get { return txtEdit.CanUndo; } set { txtEdit.CanUndo = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ClipChildren { get { return txtEdit.ClipChildren; } set { txtEdit.ClipChildren = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ClipSiblings { get { return txtEdit.ClipSiblings; } set { txtEdit.ClipSiblings = value; } }
        /// <summary>
        /// 设置行间距
        ///Sets the distance, in twips, between the columns on a page for the whole document or for a document's section. The SectionCurrent property determines the part of the document. 
        ///This property can only be used for columns which are automatically calculated from the page width. 
        ///All columns have then the same distance determined through this property. 
        ///To set columns with different distances the ColumnWidthsAndDistances property must be used.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short ColumnDistance { get { return txtEdit.ColumnDistance; } set { txtEdit.ColumnDistance = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color ColumnLineColor { get { return txtEdit.ColumnLineColor; } set { txtEdit.ColumnLineColor = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short ColumnLineWidth { get { return txtEdit.ColumnLineWidth; } set { txtEdit.ColumnLineWidth = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short Columns { get { return txtEdit.Columns; } set { txtEdit.Columns = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object ColumnWidthsAndDistances { get { return txtEdit.ColumnWidthsAndDistances; } set { txtEdit.ColumnWidthsAndDistances = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ControlChars { get { return txtEdit.ControlChars; } set { txtEdit.ControlChars = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CtlAllowDrop { get { return txtEdit.CtlAllowDrop; } set { txtEdit.CtlAllowDrop = value; } }
        /// <summary>
        /// 0.表示可以修改 2.表示预览
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short CtlEditMode
        {
            get
            {
                return txtEdit.CtlEditMode;
            }
            set
            {
                txtEdit.CtlEditMode = value;
                if (txtEdit.CtlEditMode == 2)
                {
                    setEnableMenuItems(false);
                }
                else
                {
                    setEnableMenuItems(true);
                }
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string CtlText { get { return txtEdit.CtlText; } set { txtEdit.CtlText = value; } }
        /// <summary>
        /// Returns or sets an array of three values which specify the page, line and column number of the current text input position. 
        /// These values are the same as shown in Text Control's status bar.
        /// Syntax:   TXTextControl.CurrentInputPosition [= Array]
        ///Index   Description 
        ///0  Specifies the current page number. The first page has the number one.
        ///1  Specifies the current line number. The first line has the number one.
        ///2  Specifies the current column number. The first column has the number one.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object CurrentInputPosition { get { return txtEdit.CurrentInputPosition; } set { txtEdit.CurrentInputPosition = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentPages { get { return txtEdit.CurrentPages; } set { txtEdit.CurrentPages = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short DataFormat { get { return txtEdit.DataFormat; } set { txtEdit.DataFormat = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DataText { get { return txtEdit.DataText; } set { txtEdit.DataText = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short DataTextFormat { get { return txtEdit.DataTextFormat; } set { txtEdit.DataTextFormat = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object Device { get { return txtEdit.Device; } set { txtEdit.Device = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool EnableHyperlinks { get { return txtEdit.EnableHyperlinks; } set { txtEdit.EnableHyperlinks = value; } }
        /// <summary>
        /// 当前焦点是否在域字段内，如果返回0表示焦点不在域内
        /// Returns the field identifier of the field containing the input position. 
        /// Zero is returned when the input position is not inside a field.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short FieldAtInputPos { get { return txtEdit.FieldAtInputPos; } set { txtEdit.FieldAtInputPos = value; } }
        /// <summary>
        /// Specifies if the contents of a marked text field can be changed by the user. 
        /// The field identifier must have previously been determined with the FieldCurrent property.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool FieldChangeable { get { return txtEdit.FieldChangeable; } set { txtEdit.FieldChangeable = value; } }
        /// <summary>
        /// 返回当前焦点所在的域或对象上(如果为0表示焦点不再域或对象上)
        /// 或设置域或对象的标识
        /// Returns or sets the identifier of the current marked text field 
        /// for the Fieldxxx properties, methods and events.
        /// Syntax:   TXTextControl.FieldCurrent [= FieldId]
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short FieldCurrent { get { return txtEdit.FieldCurrent; } set { txtEdit.FieldCurrent = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool FieldDeleteable { get { return txtEdit.FieldDeleteable; } set { txtEdit.FieldDeleteable = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int FieldEnd { get { return txtEdit.FieldEnd; } set { txtEdit.FieldEnd = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool FieldLinkTargetMarkers { get { return txtEdit.FieldLinkTargetMarkers; } set { txtEdit.FieldLinkTargetMarkers = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int FieldPosX { get { return txtEdit.FieldPosX; } set { txtEdit.FieldPosX = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int FieldPosY { get { return txtEdit.FieldPosY; } set { txtEdit.FieldPosY = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int FieldStart { get { return txtEdit.FieldStart; } set { txtEdit.FieldStart = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string FieldText { get { return txtEdit.FieldText; } set { txtEdit.FieldText = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short FontBold { get { return txtEdit.FontBold; } set { txtEdit.FontBold = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short FontItalic { get { return txtEdit.FontItalic; } set { txtEdit.FontItalic = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string FontName { get { return txtEdit.FontName; } set { txtEdit.FontName = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short FontSettings { get { return txtEdit.FontSettings; } set { txtEdit.FontSettings = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short FontSize { get { return txtEdit.FontSize; } set { txtEdit.FontSize = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short FontStrikethru { get { return txtEdit.FontStrikethru; } set { txtEdit.FontStrikethru = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short FontUnderline { get { return txtEdit.FontUnderline; } set { txtEdit.FontUnderline = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short FontUnderlineStyle { get { return txtEdit.FontUnderlineStyle; } set { txtEdit.FontUnderlineStyle = value; } }
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Color ForeColor { get { return base.ForeColor; } set { base.ForeColor = value; txtEdit.ForeColor = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool FormatSelection { get { return txtEdit.FormatSelection; } set { txtEdit.FormatSelection = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short FrameDistance { get { return txtEdit.FrameDistance; } set { txtEdit.FrameDistance = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short FrameLineWidth { get { return txtEdit.FrameLineWidth; } set { txtEdit.FrameLineWidth = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short FrameStyle { get { return txtEdit.FrameStyle; } set { txtEdit.FrameStyle = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HeaderFooterConstants HeaderFooter { get { return txtEdit.HeaderFooter; } set { txtEdit.HeaderFooter = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short HeaderFooterAtInputPos { get { return txtEdit.HeaderFooterAtInputPos; } set { txtEdit.HeaderFooterAtInputPos = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HeaderFooterStyleConstants HeaderFooterStyle { get { return txtEdit.HeaderFooterStyle; } set { txtEdit.HeaderFooterStyle = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool HideSelection { get { return txtEdit.HideSelection; } set { txtEdit.HideSelection = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public int hWnd { get { return txtEdit.hWnd; } set { txtEdit.hWnd = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short ImageDisplayMode { get { return txtEdit.ImageDisplayMode; } set { txtEdit.ImageDisplayMode = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short ImageExportCompressionQuality { get { return txtEdit.ImageExportCompressionQuality; } set { txtEdit.ImageExportCompressionQuality = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ImageExportFilename { get { return txtEdit.ImageExportFilename; } set { txtEdit.ImageExportFilename = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ImageExportFilters { get { return txtEdit.ImageExportFilters; } set { txtEdit.ImageExportFilters = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short ImageExportFormat { get { return txtEdit.ImageExportFormat; } set { txtEdit.ImageExportFormat = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short ImageExportMaxResolution { get { return txtEdit.ImageExportMaxResolution; } set { txtEdit.ImageExportMaxResolution = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ImageFilename { get { return txtEdit.ImageFilename; } set { txtEdit.ImageFilename = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ImageFilters { get { return txtEdit.ImageFilters; } set { txtEdit.ImageFilters = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short ImageSaveMode { get { return txtEdit.ImageSaveMode; } set { txtEdit.ImageSaveMode = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short IndentB { get { return txtEdit.IndentB; } set { txtEdit.IndentB = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short IndentFL { get { return txtEdit.IndentFL; } set { txtEdit.IndentFL = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short IndentL { get { return txtEdit.IndentL; } set { txtEdit.IndentL = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short IndentR { get { return txtEdit.IndentR; } set { txtEdit.IndentR = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short IndentT { get { return txtEdit.IndentT; } set { txtEdit.IndentT = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool InsertionMode { get { return txtEdit.InsertionMode; } set { txtEdit.InsertionMode = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool KeepLinesTogether { get { return txtEdit.KeepLinesTogether; } set { txtEdit.KeepLinesTogether = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool KeepWithNext { get { return txtEdit.KeepWithNext; } set { txtEdit.KeepWithNext = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short Language { get { return txtEdit.Language; } set { txtEdit.Language = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short LineSpacing { get { return txtEdit.LineSpacing; } set { txtEdit.LineSpacing = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short LineSpacingT { get { return txtEdit.LineSpacingT; } set { txtEdit.LineSpacingT = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ListTypeConstants ListType { get { return txtEdit.ListType; } set { txtEdit.ListType = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool LockWindowUpdate { get { return txtEdit.LockWindowUpdate; } set { txtEdit.LockWindowUpdate = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short MousePointer { get { return txtEdit.MousePointer; } set { txtEdit.MousePointer = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int NextWindow { get { return txtEdit.NextWindow; } set { txtEdit.NextWindow = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short ObjectCurrent { get { return txtEdit.ObjectCurrent; } set { txtEdit.ObjectCurrent = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short ObjectHeight { get { return txtEdit.ObjectHeight; } set { txtEdit.ObjectHeight = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ObjectInsertionModeConstants ObjectInsertionMode { get { return txtEdit.ObjectInsertionMode; } set { txtEdit.ObjectInsertionMode = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object ObjectItem { get { return txtEdit.ObjectItem; } set { txtEdit.ObjectItem = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ObjectName { get { return txtEdit.ObjectName; } set { txtEdit.ObjectName = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ObjectPosX { get { return txtEdit.ObjectPosX; } set { txtEdit.ObjectPosX = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ObjectPosY { get { return txtEdit.ObjectPosY; } set { txtEdit.ObjectPosY = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short ObjectScaleX { get { return txtEdit.ObjectScaleX; } set { txtEdit.ObjectScaleX = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short ObjectScaleY { get { return txtEdit.ObjectScaleY; } set { txtEdit.ObjectScaleY = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short ObjectSizeMode { get { return txtEdit.ObjectSizeMode; } set { txtEdit.ObjectSizeMode = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short ObjectTextflow { get { return txtEdit.ObjectTextflow; } set { txtEdit.ObjectTextflow = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ObjectUserId { get { return txtEdit.ObjectUserId; } set { txtEdit.ObjectUserId = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short ObjectWidth { get { return txtEdit.ObjectWidth; } set { txtEdit.ObjectWidth = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool PageBreakBefore { get { return txtEdit.PageBreakBefore; } set { txtEdit.PageBreakBefore = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int PageHeight { get { return txtEdit.PageHeight; } set { txtEdit.PageHeight = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int PageMarginB { get { return txtEdit.PageMarginB; } set { txtEdit.PageMarginB = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int PageMarginL { get { return txtEdit.PageMarginL; } set { txtEdit.PageMarginL = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int PageMarginR { get { return txtEdit.PageMarginR; } set { txtEdit.PageMarginR = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int PageMarginT { get { return txtEdit.PageMarginT; } set { txtEdit.PageMarginT = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short PageOrientation { get { return txtEdit.PageOrientation; } set { txtEdit.PageOrientation = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PageViewStyles PageViewStyle { get { return txtEdit.PageViewStyle; } set { txtEdit.PageViewStyle = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int PageWidth { get { return txtEdit.PageWidth; } set { txtEdit.PageWidth = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool PrintColors { get { return txtEdit.PrintColors; } set { txtEdit.PrintColors = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int PrintDevice { get { return txtEdit.PrintDevice; } set { txtEdit.PrintDevice = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool PrintOffset { get { return txtEdit.PrintOffset; } set { txtEdit.PrintOffset = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short PrintZoom { get { return txtEdit.PrintZoom; } set { txtEdit.PrintZoom = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ResourceFile { get { return txtEdit.ResourceFile; } set { txtEdit.ResourceFile = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string RTFSelText { get { return txtEdit.RTFSelText; } set { txtEdit.RTFSelText = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int RulerHandle { get { return txtEdit.RulerHandle; } set { txtEdit.RulerHandle = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short ScrollBars { get { return txtEdit.ScrollBars; } set { txtEdit.ScrollBars = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ScrollPosX { get { return txtEdit.ScrollPosX; } set { txtEdit.ScrollPosX = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ScrollPosY { get { return txtEdit.ScrollPosY; } set { txtEdit.ScrollPosY = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short SectionAtInputPos { get { return txtEdit.SectionAtInputPos; } set { txtEdit.SectionAtInputPos = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SectionBreakKindConstants SectionBreakKind { get { return txtEdit.SectionBreakKind; } set { txtEdit.SectionBreakKind = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short SectionCount { get { return txtEdit.SectionCount; } set { txtEdit.SectionCount = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short SectionCurrent { get { return txtEdit.SectionCurrent; } set { txtEdit.SectionCurrent = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelLength { get { return txtEdit.SelLength; } set { txtEdit.SelLength = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelStart { get { return txtEdit.SelStart; } set { txtEdit.SelStart = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelText { get { return txtEdit.SelText; } set { txtEdit.SelText = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short SizeMode { get { return txtEdit.SizeMode; } set { txtEdit.SizeMode = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int StatusBarHandle { get { return txtEdit.StatusBarHandle; } set { txtEdit.StatusBarHandle = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string StyleCurrent { get { return txtEdit.StyleCurrent; } set { txtEdit.StyleCurrent = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object Styles { get { return txtEdit.Styles; } set { txtEdit.Styles = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short TabCurrent { get { return txtEdit.TabCurrent; } set { txtEdit.TabCurrent = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool TabKey { get { return txtEdit.TabKey; } set { txtEdit.TabKey = value; } }
        /// <summary>
        /// 返回当前光标所在的表的标识，如果光标不在表单元格内或同时选中表格的多个单元格时返回0
        /// Returns the table identifier of the table containing the input position. Zero is returned when the input position 
        /// is not inside a table or when more than one table cell is selected.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short TableAtInputPos { get { return txtEdit.TableAtInputPos; } set { txtEdit.TableAtInputPos = value; } }
        /// <summary>
        /// 该属性指示选中的单元格是否可以修改
        /// This property provides information about whether the attributes of all the selected table cells can be altered. 
        /// It returns False when the selection is not completely within a single table. 
        /// Otherwise it returns True.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool TableCanChangeAttr { get { return txtEdit.TableCanChangeAttr; } set { txtEdit.TableCanChangeAttr = value; } }
        /// <summary>
        /// 该属性指示表的列是否可以被删除，如果当前选中了单元格文本或光标在表格外则返回false
        ///This method deletes the table column with the current input position. 
        //A table column can only be deleted if the current input position is in a table and
        //if no text is selected. 
        //The TableCanDeleteColumn property informs whether a column can be deleted.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool TableCanDeleteColumn { get { return txtEdit.TableCanDeleteColumn; } set { txtEdit.TableCanDeleteColumn = value; } }
        /// <summary>
        /// 该属性指示是否可以删除表格边框
        /// This property provides information about whether table lines can be deleted. 
        /// It returns False when no table line is selected or 
        /// when the current input position is outside a table. Otherwise it returns True.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool TableCanDeleteLines { get { return txtEdit.TableCanDeleteLines; } set { txtEdit.TableCanDeleteLines = value; } }
        /// <summary>
        /// 该属性指示当前位置是否可以插入表格
        /// This property provides information about whether a table can be inserted. 
        /// It returns False when a selection exists or the current input position is inside a table. 
        /// Otherwise it returns True.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool TableCanInsert { get { return txtEdit.TableCanInsert; } set { txtEdit.TableCanInsert = value; } }
        /// <summary>
        /// This property provides information about whether a table column can be inserted. 
        /// It returns True if a table column can be inserted. 
        /// It returns False if text is selected or if the current input position is outside a table.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool TableCanInsertColumn { get { return txtEdit.TableCanInsertColumn; } set { txtEdit.TableCanInsertColumn = value; } }
        /// <summary>
        /// This property provides information about whether table lines can be inserted. 
        /// It returns True if table lines can be inserted. 
        /// It returns False if text is selected or if the current input position is outside a table.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool TableCanInsertLines { get { return txtEdit.TableCanInsertLines; } set { txtEdit.TableCanInsertLines = value; } }
        /// <summary>
        /// Checks whether a table can be split. 
        /// A table can only be split, 
        /// if it contains the current input position and no further text is selected.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool TableCanSplit { get { return txtEdit.TableCanSplit; } set { txtEdit.TableCanSplit = value; } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short ObjectInsert(int objectType, string fileName, int textPos, short alignment, int posX, int posY, short scaleX, short scaleY, short textflow, short distanceL, short distanceT, short distanceR, short distanceB)
        {
            return txtEdit.ObjectInsert(objectType, fileName,textPos,alignment,posX,posY,scaleX,scaleY, textflow,distanceL,distanceT,distanceR,distanceB);
        }

        /// <summary>
        /// This method inserts a new text frame, which is then handled as a single character in the text.
        ///Parameter   Description 
        ///TextPos  Specifies a text position where the text frame should be inserted. If TextPos is -1, the text frame is inserted at the current input position.
        ///Width  Specifies the width of the text frame in twips.
        ///Height  Specifies the height of the text frame in twips.
        ///Return Value:   The method returns the text frame's identifier, if a new text frame could be inserted. Otherwise, it returns zero. The text frame's identifier can also be obtained with the ObjectCurrent property. 
        /// </summary>
        /// <param name="txtPos"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short TextFrameInsertAsChar(int txtPos, short width, short height)
        {
            return txtEdit.TextFrameInsertAsChar(txtPos, width, height);
        }
        /// <summary>
        ///  设置域字段数据类型，详情请参考开发文档中的属性TXTextControl.FieldType
        /// This property sets or returns the type of a marked text field. 
        /// The chapter Technical Articles - Marked Text Fields - Special Types of Marked Text Fields describes all the types and the data belonging to these types. Type-related data must be set with the FieldTypeData property.
        /// </summary>
        /// <param name="fieldId"></param>
        /// <param name="fieldTypeConstants"></param>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public void SetFieldType(short fieldId,FieldTypeConstants fieldTypeConstants)
        {
            txtEdit.set_FieldType(fieldId, fieldTypeConstants);
        }

        /// <summary>
        ///  设置域字段数据类型，详情请参考开发文档中的属性TXTextControl.FieldTypeData
        /// This property sets or returns the data that belongs to a marked text field of a special type. 
        /// The chapter Technical Articles - Marked Text Fields - Special Types of Marked Text Fields informs about all the types and the data belonging to these types.
        /// </summary>
        /// <param name="fieldId"></param>
        /// <param name="fieldTypeConstants"></param>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public void SetFieldTypeData(short fieldId, Object paramObject)
        {
            txtEdit.set_FieldTypeData(fieldId, paramObject);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public void FindReplace(short typeofdialog)
        {
            txtEdit.FindReplace(typeofdialog);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Find(string findWhat)
        {
            return txtEdit.Find(findWhat);
        }
        
        //public virtual short ObjectInsert(int objectType, string fileName, int textPos, short alignment, int posX, int posY, short scaleX, short scaleY, short textflow, short distanceL, short distanceT, short distanceR, short distanceB, object kindOfObject);
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public bool TableCellAttribute { get { return txtEdit.TableCellAttribute; } set { txtEdit.TableCellAttribute = value; } }
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public bool TableCellLength { get { return txtEdit.TableCellLength; } set { txtEdit.TableCellLength = value; } }
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public bool TableCellStart { get { return txtEdit.TableCellStart; } set { txtEdit.TableCellStart = value; } }
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public bool TableCellText { get { return txtEdit.TableCellText; } set { txtEdit.TableCellText = value; } }
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public bool TableColumns { get { return txtEdit.TableColumns; } set { txtEdit.TableColumns = value; } }

        /// <summary>
        /// Description:    This method inserts a new table in the text.
        ///Syntax:          TXTextControl.TableInsert Rows, Columns, TextPos [, TableId]
        ///Parameter        Description 
        ///Rows             Specifies the number of rows.
        ///Columns          Specifies the number of columns.
        ///TextPos          Specifies the text position where the new table is to be inserted. 
        ///                 It is inserted at the current input position when this parameter is set to -1.
        ///TableId          Optional. Specifies a table identifier. This identifier can be used to access or to alter the table's text and attributes. It must be in the range of 10 to 32,767.
        ///Return Value:   
        ///Parameter        Description 
        ///0                An error has occurred or the table could not be inserted. Tables cannot be inserted inside existing tables or when a section of text has been selected.
        ///-1               The new table has been inserted at the top or at the bottom of an existing table and has been combined with this table.
        ///otherwise  The table's identifier. This is the same value as specified with the TableId parameter or an internal identifier selected by Text Control when the optional TableId parameter has been omitted.
        /// </summary>
        /// <param name="Rows"></param>
        /// <param name="Columns"></param>
        /// <param name="TextPos"></param>
        /// <param name="TableId"></param>
        /// <returns></returns>
        public short TableInsert(short Rows, short Columns, int TextPos, int TableId)
        {
            if (TableId > 32767 || TableId < 10)
                return txtEdit.TableInsert(Rows, Columns, TextPos);
            else
                return txtEdit.TableInsert(Rows, Columns, TextPos,TableId);
        }

        /// <summary>
        /// Description:    This method inserts a table column left or right of the column with the current input position. A table column can only be inserted if the current input position is in a table and if no text is selected. The TableCanInsertColumn property informs whether a column can be inserted.
        ///Syntax:          TXTextControl.TableInsertColumn Position
        ///Parameter        Description 
        ///Position         Specifies where to insert the table column. Possible values are the following:
        ///Constant         Description 
        ///txTableInsertInFront(1)  The column is inserted left of the column with the current input position.
        ///txTableInsertAfter(2)    The column is inserted right of the column with the current input position.
        ///Return Value:    The method returns True if the table column could successfully be inserted. The method returns False if an error has occurred or if the current input position is not within a table.
        /// </summary>
        /// <param name="InsertConstants"></param>
        /// <returns></returns>
        public bool TableInsertColumn(short InsertConstants)
        {
            if(InsertConstants < 1 || InsertConstants > 2)
                return false;

            if (InsertConstants == 1)
                return txtEdit.TableInsertColumn(TableInsertConstants.txTableInsertInFront);
            else
                return txtEdit.TableInsertColumn(TableInsertConstants.txTableInsertAfter);
        }

        /// <summary>
        /// Description:    This member function inserts table lines above or below the table line with the current input position. Table lines can only be inserted if the current input position is in a table and if no text is selected. The TableCanInsertLines property informs whether table lines can be inserted.
        /// Syntax:         TXTextControl.TableInsertLines Position, Lines
        /// Parameter       Description 
        /// Position        Specifies where to insert the table lines. Possible values are the following:
        /// Constant        Description 
        ///txTableInsertInFront(1)  Table lines are inserted above the line with the current input position.
        ///txTableInsertAfter(2)    Table lines are inserted below the line with the current input position.
        ///Lines  Specifies the number of lines to insert.
        ///Return Value:   The method returns True if the table lines could successfully be inserted. The method returns False if an error has occurred or if the current input position is not within a table.
        /// </summary>
        /// <param name="InsertConstants"></param>
        /// <param name="Lines"></param>
        /// <returns></returns>
        public bool TableInsertLines(short InsertConstants,short Lines)
        {
            if (InsertConstants < 1 || InsertConstants > 2)
                return false;

            if (InsertConstants == 1)
                return txtEdit.TableInsertLines(TableInsertConstants.txTableInsertInFront, Lines);
            else
                return txtEdit.TableInsertLines(TableInsertConstants.txTableInsertAfter, Lines);
        }

        /// <summary>
        /// 设置表属性，在关闭框时返回true/false指示表格属性是否已经改变
        /// This method invokes the built-in dialog box for setting table attributes and, 
        /// after the user has closed the dialog box, specifies whether he has changed something.
        /// </summary>
        /// <returns></returns>
        public bool TableAttrDialog()
        {
            return txtEdit.TableAttrDialog(); 
        }

        /// <summary>
        /// 设置Object对象属性
        /// This method invokes the built-in dialog box for setting attributes of images, text frames and OLE objects. 
        /// The dialog box offers different options depending on the kind of object, currently selected.
        /// This method invokes the built-in dialog box for setting table attributes and, 
        /// after the user has closed the dialog box, specifies whether he has changed something.
        /// </summary>
        /// <returns>The method returns True when the user has changed one or more attibutes. 
        /// The method returns False when the formatting remains unchanged
        /// </returns>
        public bool ObjectAttrDialog()
        {
            return txtEdit.ObjectAttrDialog();
        }

        /// <summary>
        /// 设置字体样式
        /// Invokes the Text Control's built-in font dialog box and, 
        /// after the user has closed the dialog box, specifies whether he has changed something.
        /// </summary>
        /// <returns>The method returns True when the user has changed one or more attibutes. 
        /// The method returns False when the formatting remains unchanged.
        ///</returns>
        public bool FontDialog()
        {
            return txtEdit.FontDialog();
        }

        /// <summary>
        /// 设置段落样式
        /// Invokes the Text Control's built-in paragraph attributes dialog box and,
        /// after the user has closed the dialog box, specifies whether he has changed something.
        /// </summary>
        /// <returns>The method returns True when the user has changed one or more attibutes. 
        /// The method returns False when the formatting remains unchanged
        ///</returns>
        public bool ParagraphDialog()
        {
            return txtEdit.ParagraphDialog();
        }

        /// <summary>
        /// 设置序列样式
        /// Invokes the Text Control's built-in dialog box for setting attributes of bulleted and numbered lists.
        /// </summary>
        /// <returns>The method returns True when the user has changed one or more attributes. 
        /// The method returns False when nothing has been changed.
        ///</returns>
        public bool ListAttrDialog()
        {
            return txtEdit.ListAttrDialog();
        }

        /// <summary>
        ///Description:     This method deletes the table column with the current input position. A table column can only be deleted if the current input position is in a table and if no text is selected. The TableCanDeleteColumn property informs whether a column can be deleted.
        ///Syntax:          TXTextControl.TableDeleteColumn
        ///Return Value:    The method returns True if the table column could successfully be deleted. The method returns False if an error has occurred or if the current input position is not within a table.
        /// </summary>
        /// <returns></returns>
        public bool TableDeleteColumn()
        {
            return txtEdit.TableDeleteColumn();
        }

        /// <summary>
        ///Description:    This method deletes the currently selected table lines or the table line at the current input position.
        ///Syntax:         TXTextControl.TableDeleteLines
        ///Return Value:   The method returns True if table lines have been deleted. Otherwise it returns False.
        /// </summary>
        /// <returns></returns>
        public bool TableDeleteLines()
        {
            return txtEdit.TableDeleteLines();
        }

        /// <summary>
        /// Splits a table below or above the current input position. If the table does not contain the current input position, it cannot be split. 
        /// The TableCanSplit property can be used to determine whether a table can be split or not. 
        /// If a table is split above the first row, a new line is inserted above the table and if a table is split below the last row, a new line is inserted below the table. 
        /// This is useful to insert text above or below a nested table that immediately starts and/or ends at the beginning or the end of the cell in which it is nested.
        /// </summary>
        /// <param name="InsertConstants">
        /// txTableInsertInFront (1)  The table is split below the current input position.
        /// txTableInsertAfter (2)    The table is split in front of the current input position.
        /// </param>
        /// <returns></returns>
        public bool TableSplit(short InsertConstants)
        {
            if (InsertConstants < 1 || InsertConstants > 2)
                return false;

            if (InsertConstants == 1)
                return txtEdit.TableSplit(TableInsertConstants.txTableInsertInFront);
            else
                return txtEdit.TableSplit(TableInsertConstants.txTableInsertAfter);
        }
        /// <summary>
        ///If the current text input position is a position at the beginning or at the end of a marked text field with a doubled input position, 
        ///this method can be used to define whether the position is inside or outside the field. The input position's character position is not changed.
        ///Syntax:              TXTextControl.FieldSetInputPos FieldInputPosition
        ///Parameter            Description 
        ///FieldInputPosition   Specifies the position. It can be one of the following Constants:
        ///Value                Description  
        ///txInsideField        The current input position is inside the field.
        ///txOutsideField       The current input position is outside the field.
        ///txInsideNextField    The specified position is inside the next field. This value is only possible, if there are two following fields without any character between the fields. In this case txInsideField is in the first field, txOutsideField is between the fields and txInsideNextField is in the second field.
        ///Return Value:        The return value is non-zero if the input position has been changed. It is zero if the current input position is not at the beginning or the at the end of a field or if the field has no doubled input position.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool FieldSetInputPos(FieldInputPositionConstants PositionConstants)
        {
            return txtEdit.FieldSetInputPos(PositionConstants);
        }
        
        /// <summary>
        /// Returns the number of the current input column in a table. 
        /// It is zero 
        /// when the input position is not inside a table or when more than one table cell is selected.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short TableColAtInputPos { get { return txtEdit.TableColAtInputPos; } set { txtEdit.TableColAtInputPos = value; } }
        /// <summary>
        /// This property determines whether or not grid lines in tables are visible
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool TableGridLines { get { return txtEdit.TableGridLines; } set { txtEdit.TableGridLines = value; } }
        /// <summary>
        /// Returns the number of the current input row in a table. 
        /// It is zero when the input position is not inside a table or 
        /// when more than one table cell is selected.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short TableRowAtInputPos { get { return txtEdit.TableRowAtInputPos; } set { txtEdit.TableRowAtInputPos = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TabPos { get { return txtEdit.TabPos; } set { txtEdit.TabPos = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short TabType { get { return txtEdit.TabType; } set { txtEdit.TabType = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color TextBkColor { get { return txtEdit.TextBkColor; } set { txtEdit.TextBkColor = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color TextFrameBackColor { get { return txtEdit.TextFrameBackColor; } set { txtEdit.TextFrameBackColor = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short TextFrameBorderWidth { get { return txtEdit.TextFrameBorderWidth; } set { txtEdit.TextFrameBorderWidth = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool TextFrameMarkerLines { get { return txtEdit.TextFrameMarkerLines; } set { txtEdit.TextFrameMarkerLines = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int VerticalRulerHandle { get { return txtEdit.VerticalRulerHandle; } set { txtEdit.VerticalRulerHandle = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ViewImagePath { get { return txtEdit.ViewImagePath; } set { txtEdit.ViewImagePath = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short ViewMode
        {
            get
            {
                return txtEdit.ViewMode;
            }
            set
            {
                if (!this.IsDisposed)
                {
                    txtEdit.ViewMode = value;
                }
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short ViewSection { get { return txtEdit.ViewSection; } set { txtEdit.ViewSection = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string VTSpellDictionary { get { return txtEdit.VTSpellDictionary; } set { txtEdit.VTSpellDictionary = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short WidowOrphanLines { get { return txtEdit.WidowOrphanLines; } set { txtEdit.WidowOrphanLines = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short WordWrapMode { get { return txtEdit.WordWrapMode; } set { txtEdit.WordWrapMode = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short XMLEditMode { get { return txtEdit.XMLEditMode; } set { txtEdit.XMLEditMode = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public short ZoomFactor
        {
            get
            {
                return txtEdit.ZoomFactor;
            }
            set
            {
                if (!this.IsDisposed)
                {
                    if (value >= 10 && value <= 550)
                    {
                        txtEdit.ZoomFactor = value;
                    }
                }
            }
        }

        private bool _disabledpaste = false;        //禁止粘贴不同病人的病历内容
        public bool DisabledPaste { get { return _disabledpaste; } set { _disabledpaste = value; } }
        #endregion

        #region 常用功能

        public short FieldNext(short fieldId, short fieldGroup)
        {
            return txtEdit.FieldNext(fieldId, fieldGroup);
        }

        public bool HeaderFooterSelect(HeaderFooterConstants headerFooter)
        {
            return txtEdit.HeaderFooterSelect(headerFooter);
        }

        public bool HeaderFooterActivate(HeaderFooterConstants headerFooter)
        {
            return txtEdit.HeaderFooterActivate(headerFooter);
        }

        public short TableInsert(short rows, short columns, int textPos)
        {
            return txtEdit.TableInsert(rows, columns, textPos);
        }

        public bool TableSplit(TableInsertConstants position)
        {
            return txtEdit.TableSplit(position);
        }

        public void TableCellAttribute(short tableId, short row, short column, TableCellAttributeConstants attribute, object param)
        {
            txtEdit.set_TableCellAttribute(tableId, row, column, attribute, param);
        }

        public void Destroy()
        {
            _zoomslider.ValueChanged -= eventValueChanged;
            this.Dispose();
            GC.SuppressFinalize(txtEdit);

        }

        public int LoadFile(string fileName)
        {
            return txtEdit.Load(fileName);
        }

        public int LoadFile(string fileName, int offest, object format, object curSelection)
        {
            return txtEdit.Load(fileName, offest, format, curSelection);
        }
        /// <summary>
        /// Specifies whether only the field or the field including its text is deleted. If this parameter is True, the marked text field including its text is deleted.
        /// If this parameter is False, the marked text field is deleted, but its text contents are preserved
        /// </summary>
        /// <param name="deleteTotal"></param>
        /// <returns></returns>
        public bool FieldDelete(bool deleteTotal)
        {
            if (txtEdit.FieldCurrent > 0)
                return txtEdit.FieldDelete(deleteTotal);
            return false;
        }

        public long Save(string filename, int format, bool CurSelection)
        {
            return txtEdit.Save(filename, 0, format, CurSelection);
        }

        public byte[] SaveToByteArray(bool CurSelection)
        {
            Object obj = txtEdit.SaveToMemory(FileFormat.TXANSIFormat, CurSelection);
            return (byte[])obj;
        }

        public bool LoadFromMemory(object dataBuffer)
        {
            return txtEdit.LoadFromMemory(dataBuffer);
        }

        public bool LoadFromMemory(object dataBuffer, int format, bool curSelection)
        {
            try
            {
                return txtEdit.LoadFromMemory(dataBuffer, format, curSelection);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void SaveAttribute()
        {
            txtEdit.set_LoadSaveAttribute(LoadSaveAttributeConstants.txDocWidth, txtEdit.PageWidth);
            txtEdit.set_LoadSaveAttribute(LoadSaveAttributeConstants.txDocHeight, txtEdit.PageHeight);
            txtEdit.set_LoadSaveAttribute(LoadSaveAttributeConstants.txDocLeftMargin, txtEdit.PageMarginL);
            txtEdit.set_LoadSaveAttribute(LoadSaveAttributeConstants.txDocTopMargin, txtEdit.PageMarginT);
            txtEdit.set_LoadSaveAttribute(LoadSaveAttributeConstants.txDocRightMargin, txtEdit.PageMarginR);
            txtEdit.set_LoadSaveAttribute(LoadSaveAttributeConstants.txDocBottomMargin, txtEdit.PageMarginB);
        }

        public void SaveAttribute(LoadSaveAttributeConstants attribute, object param0)
        {
            txtEdit.set_LoadSaveAttribute(attribute, param0);
        }

        public object LoadAttribute(LoadSaveAttributeConstants attribute)
        {
            return txtEdit.get_LoadSaveAttribute(attribute);
        }

        public void LoadAttribute()
        {
            if (Convert.ToInt32(txtEdit.get_LoadSaveAttribute(LoadSaveAttributeConstants.txDocWidth)) > 0)
            {
                txtEdit.PageWidth = Convert.ToInt32(txtEdit.get_LoadSaveAttribute(LoadSaveAttributeConstants.txDocWidth));
            }
            if (Convert.ToInt32(txtEdit.get_LoadSaveAttribute(LoadSaveAttributeConstants.txDocHeight)) > 0)
            {
                txtEdit.PageHeight = Convert.ToInt32(txtEdit.get_LoadSaveAttribute(LoadSaveAttributeConstants.txDocHeight));
            }
            if (Convert.ToInt32(txtEdit.get_LoadSaveAttribute(LoadSaveAttributeConstants.txDocLeftMargin)) > 0)
            {
                txtEdit.PageMarginL = Convert.ToInt32(txtEdit.get_LoadSaveAttribute(LoadSaveAttributeConstants.txDocLeftMargin));
            }
            if (Convert.ToInt32(txtEdit.get_LoadSaveAttribute(LoadSaveAttributeConstants.txDocTopMargin)) > 0)
            {
                txtEdit.PageMarginT = Convert.ToInt32(txtEdit.get_LoadSaveAttribute(LoadSaveAttributeConstants.txDocTopMargin));
            }
            if (Convert.ToInt32(txtEdit.get_LoadSaveAttribute(LoadSaveAttributeConstants.txDocRightMargin)) > 0)
            {
                txtEdit.PageMarginR = Convert.ToInt32(txtEdit.get_LoadSaveAttribute(LoadSaveAttributeConstants.txDocRightMargin));
            }
            if (Convert.ToInt32(txtEdit.get_LoadSaveAttribute(LoadSaveAttributeConstants.txDocBottomMargin)) > 0)
            {
                txtEdit.PageMarginB = Convert.ToInt32(txtEdit.get_LoadSaveAttribute(LoadSaveAttributeConstants.txDocBottomMargin));
            }
        }

        public void FieldEditAttr(short fieldId, short param)
        {
            txtEdit.set_FieldEditAttr(fieldId, param);
        }

        public string GetFieldData(short fieldId)
        {
            //Modify Rao 2012-05-29
            object obj = txtEdit.get_FieldData(fieldId);
            if (obj == null)
                return "";
            else
                return obj.ToString();
        }

        /// <summary>
        /// 设置该元素的身份标识符
        /// </summary>
        /// <param name="fieldId"></param>
        /// <param name="param"></param>
        public void SetFieldData(short fieldId, object param)
        {
            txtEdit.set_FieldData(fieldId, param);
        }
        /// <summary>
        /// Description:   Inserts a new marked text field at the current caret position.
        ///Syntax:         TXTextControl.FieldInsert FieldText
        ///Parameter       Description 
        ///FieldText       Specifies the text contents for the new marked text field.
        ///Return Value:   The method returns True if a field could be inserted, otherwise it returns False.
        ///Remarks:        Selected text can be converted to a marked text field by using an empty string as FieldText. Inserting a marked text field changes 
        ///                the value of the FieldCurrent property to the identifier of the newly created field.
        /// </summary>
        /// <param name="fieldText"></param>
        /// <returns></returns>
        public bool FieldInsert(string fieldText)
        {
            return txtEdit.FieldInsert(fieldText);
        }

        private void CommandFont_Executed(object sender, EventArgs e)
        {
            if (!UpdateStateing)
            {
                //设置选中文字或段落的字体
                txtEdit.FontName = (sender as ComboBoxItem).SelectedItem.ToString();
                //设置按钮按钮状态
                CommandExecuted();
                //return;
                TransCheckStyle(0);
            }
        }

       /// <summary>
        /// 替换特殊勾选字符
       /// </summary>
        /// <param name="fieldId">0.表示替换所有； >0表示替换当前</param>
        public void TransCheckStyle(short fieldId)
        {
            /*
            String oldFontName = FontName;
            if (txtEdit.CtlEditMode == 0)
            {
                #region //替换病历内容中的域
                try
                {
                    txtEdit.PosChange -= new EventHandler(txtEdit_PosChange);
                    txtEdit.PosChange -= new EventHandler(txtEdit_PosChange);
                    this.CommandFont.Executed -= new System.EventHandler(this.CommandFont_Executed);
                    this.CommandFont.Executed -= new System.EventHandler(this.CommandFont_Executed);
                       
                    if (fieldId == 0)
                    {
                        short n = 0;
                        txtEdit.HeaderFooterActivate(HeaderFooterConstants.txMainText);
                        n = txtEdit.FieldNext(0, 0);

                        int iSelStart = SelStart;
                        int iSelLength = SelLength;
                        //SelLength = 0;
                        //String stemp = "";
                        while (n > 0)
                        {
                            string strReturn = GetFieldData(n);
                            if (strReturn.Split('|').Length > 1 && strReturn.Split('|')[1] == "11")
                            {
                                if (FieldText == null || FieldText == "")
                                {
                                    n = FieldNext(n, 0);
                                    continue;
                                }
                                //下面该语句不能少，否则会出现只能替换部分的问题
                                FieldCurrent = n;
                                int iStartLengthIndex = FieldStart; 
                                //下面两个语句赋值都会引发“FontName”的变化
                                SelStart = iStartLengthIndex - 1;
                                SelLength = 1;
                                //stemp += iStartLengthIndex.ToString() + "#";

                                if (FieldText != "□")
                                    txtEdit.FontName = "Wingdings 2";
                                else
                                    txtEdit.FontName = "宋体";
                            }

                            n = FieldNext(n, 0);
                        }
                        SelStart = iSelStart;
                        SelLength = iSelLength;
                        //FontName = oldFontName;
                        //txtEdit.FontName = oldFontName;
                    }
                    else
                    {
                        string strReturn = GetFieldData(FieldCurrent);
                        if (!(strReturn.Split('|').Length > 1 && strReturn.Split('|')[1] == "11"))
                            return;

                        txtEdit.HeaderFooterActivate(HeaderFooterConstants.txMainText);
                        int iSelStart = SelStart;
                        int iSelLength = SelLength;
                        int iStartLengthIndex = FieldStart; 
                        //下面两个语句赋值都会引发“FontName”的变化
                        SelStart = iStartLengthIndex - 1;
                        SelLength = 1;
                        //stemp += iStartLengthIndex.ToString() + "#";
                        if (FieldText != "□")
                            txtEdit.FontName = "Wingdings 2";
                        else
                            txtEdit.FontName = "宋体";
                        SelStart = iSelStart;
                        SelLength = iSelLength;
                        //FontName = oldFontName;
                        //txtEdit.FontName = oldFontName;
                    }
                    //MessageBox.Show(stemp);
                    txtEdit.PosChange += new EventHandler(txtEdit_PosChange);
                    this.CommandFont.Executed += new System.EventHandler(this.CommandFont_Executed);
                }
                catch (Exception ex)
                {
                }
                #endregion
            }
             * */
        }

        private void CommandFormatBrush_Executed(object sender, EventArgs e)
        {
            if (!UpdateStateing)
            {
                if (bOpenFormat == false)
                {
                    if (txtEdit.SelText != null && txtEdit.SelText.Length > 0)
                    {
                        CommandFormatBrush.Checked = true;
                        txtEdit.FormatSelection = true;
                        bOpenFormat = true;
                        this.txtEdit.FormatSelection = true;
                        FormatFontBold = this.txtEdit.FontBold;
                        FormatFontItalic = this.txtEdit.FontItalic;
                        FormatFontSize = this.txtEdit.FontSize;
                        FormatColor = this.txtEdit.ForeColor;
                        FormatLineSpacing = this.txtEdit.LineSpacing;
                    }
                }
                else
                {
                    bOpenFormat = false;
                    CommandFormatBrush.Checked = false;
                }
            }
        }

        private void CommandCut_Executed(object sender, EventArgs e)
        {

            txtEdit.Clip(1);
            if (_disabledpaste)
            {
                HYEMRDATA emrData = new HYEMRDATA(Clipboard.GetDataObject());
                emrData.CopyToClipboard();
            }
            CommandExecuted();

        }

        private void CommandCopy_Executed(object sender, EventArgs e)
        {

            txtEdit.Clip(2);
            if (_disabledpaste )
            {
                HYEMRDATA emrData = new HYEMRDATA(Clipboard.GetDataObject());
                emrData.CopyToClipboard();
            }
            CommandExecuted();

        }

        private void CommandPaste_Executed(object sender, EventArgs e)
        {
            IDataObject dataObj = Clipboard.GetDataObject();
            if (dataObj != null)
            {
                if (_disabledpaste && dataObj.GetDataPresent(typeof(HYEMRDATA).FullName))
                {
                    HYEMRDATA emrData = (HYEMRDATA)dataObj.GetData(typeof(HYEMRDATA).FullName, true);
                    if (emrData != null )
                    {
                        emrData.RestoreToClipboard();
                        txtEdit.Clip(3);
                        //重新添加到剪贴板，以便下次粘贴使用
                        emrData.CopyToClipboard();
                    }
                    else
                    {
                        MessageBox.Show("不能将不同病人的病历内容插入到本病历中，请使用强制粘贴功能！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                    txtEdit.Clip(3);
            }
            CommandExecuted();
        }

        private void CommandDirectPaste_Executed(object sender, EventArgs e)
        {
            IDataObject dataObj = Clipboard.GetDataObject();
            if (dataObj != null)
            {
                if (_disabledpaste && dataObj.GetDataPresent(typeof(HYEMRDATA).FullName))
                {
                    HYEMRDATA emrData = (HYEMRDATA)dataObj.GetData(typeof(HYEMRDATA).FullName, true);
                    if (emrData != null)
                    {
                        emrData.RestoreToClipboard();
                        txtEdit.Clip(3);
                        //重新添加到剪贴板，以便下次粘贴使用
                        emrData.CopyToClipboard();
                    }
                }
                else
                    txtEdit.Clip(3);
            }
            CommandExecuted();
        }

        private void CommandFontBold_Executed(object sender, EventArgs e)
        {
            if (!UpdateStateing)
            {
                if (txtEdit.FontBold == 0)
                {
                    txtEdit.FontBold = 1;
                }
                else
                {
                    txtEdit.FontBold = 0;
                }

                CommandExecuted();
            }
        }

        private void CommandFontItalic_Executed(object sender, EventArgs e)
        {
            if (!UpdateStateing)
            {
                if (txtEdit.FontItalic == 0)
                {
                    txtEdit.FontItalic = 1;
                }
                else
                {
                    txtEdit.FontItalic = 0;
                }

                CommandExecuted();
            }
        }

        private void CommandFontUnderline_Executed(object sender, EventArgs e)
        {
            if (!UpdateStateing)
            {
                if (txtEdit.FontUnderline == 0)
                {
                    txtEdit.FontUnderline = 1;
                }
                else
                {
                    txtEdit.FontUnderline = 0;
                }

                CommandExecuted();
            }
        }

        private void CommandFontStrike_Executed(object sender, EventArgs e)
        {
            if (!UpdateStateing)
            {
                if (txtEdit.FontStrikethru == 0)
                {
                    txtEdit.FontStrikethru = 1;
                }
                else
                {
                    txtEdit.FontStrikethru = 0;
                }

                CommandExecuted();
            }
        }

        private void CommandFontSize_Executed(object sender, EventArgs e)
        {
            if (!UpdateStateing)
            {
                txtEdit.FontSize = Convert.ToInt16((sender as ComboBoxItem).SelectedItem.ToString());
                txtEdit.Focus();
                CommandExecuted();
            }
        }

        private void CommandAlignLeft_Executed(object sender, EventArgs e)
        {
            if (!UpdateStateing)
            {
                txtEdit.Alignment = 0;
                CommandExecuted();
            }
        }

        private void CommandAlignCenter_Executed(object sender, EventArgs e)
        {
            if (!UpdateStateing)
            {
                txtEdit.Alignment = 2;
                CommandExecuted();
            }
        }

        private void CommandAlignRight_Executed(object sender, EventArgs e)
        {
            if (!UpdateStateing)
            {
                txtEdit.Alignment = 1;
                CommandExecuted();
            }
        }

        private void CommandAlignJustify_Executed(object sender, EventArgs e)
        {
            if (!UpdateStateing)
            {
                txtEdit.Alignment = 3;
                CommandExecuted();
            }

        }

        private void CommandUndo_Executed(object sender, EventArgs e)
        {

            txtEdit.Undo();
            CommandExecuted();

        }

        private void CommandRedo_Executed(object sender, EventArgs e)
        {

            txtEdit.Redo();
            CommandExecuted();

        }

        private void CommandListNumbered_Executed(object sender, EventArgs e)
        {
            if (!UpdateStateing)
            {
                txtEdit.set_NumberingAttribute(NumberingConstants.txNumberingPos, 0);
                txtEdit.set_NumberingAttribute(NumberingConstants.txNumberingTextPos, 360);
                if (txtEdit.ListType != ListTypeConstants.txListNumbered)
                {
                    txtEdit.ListType = ListTypeConstants.txListNumbered;
                }
                else
                {
                    txtEdit.ListType = ListTypeConstants.txListNone;
                }
                CommandExecuted();
            }
        }

        private void CommandListBulleted_Executed(object sender, EventArgs e)
        {
            if (!UpdateStateing)
            {
                txtEdit.set_BulletAttribute(BulletConstants.txBulletPos, 0);
                txtEdit.set_BulletAttribute(BulletConstants.txBulletTextPos, 360);
                if (txtEdit.ListType != ListTypeConstants.txListBulleted)
                {
                    txtEdit.ListType = ListTypeConstants.txListBulleted;
                }
                else
                {
                    txtEdit.ListType = ListTypeConstants.txListNone;
                }
                CommandExecuted();
            }
        }

        private void CommondBackTab_Executed(object sender, EventArgs e)
        {

        }

        private void CommandTextColor_Executed(object sender, EventArgs e)
        {
            if (!UpdateStateing)
            {
                if (sender is ColorPickerDropDownEr)
                {
                    ColorPickerDropDownEr ColorPicker = sender as ColorPickerDropDownEr;
                    txtEdit.ForeColor = ColorPicker.SelectedColor;
                }
            }
        }

        private void CommandLineSpacing_Executed(object sender, EventArgs e)
        {
            if (!UpdateStateing)
            {
                if (sender is ButtonItem)
                {
                    if (Tools.IsNumberic((sender as ButtonItem).CommandParameter))
                    {
                        txtEdit.LineSpacing = (short)(Convert.ToDouble((sender as ButtonItem).CommandParameter) * 100);
                    }
                }
                CommandExecuted();
            }
        }

        private void CommandSubScript_Executed(object sender, EventArgs e)
        {
            if (txtEdit.BaseLine < 0)
            {
                if (txtEdit.BaseLine == (short)(-(txtEdit.FontSize * 4 / 3 + 1) * 3))
                {
                    txtEdit.FontSize = (short)(txtEdit.FontSize * 4 / 3 + 1);
                }
                else
                {
                    txtEdit.FontSize = (short)(txtEdit.FontSize * 4 / 3);
                }
                txtEdit.BaseLine = 0;

            }
            else
            {
                if (txtEdit.BaseLine == 0)
                {
                    txtEdit.BaseLine = (short)(-txtEdit.FontSize * 3);
                    txtEdit.FontSize = (short)(txtEdit.FontSize / 4 * 3);
                }
                else
                {
                    CommandSuperScript.Execute();
                    CommandSubScript.Execute();
                }
            }
        }

        private void CommandSuperScript_Executed(object sender, EventArgs e)
        {

            if (txtEdit.BaseLine > 0)
            {
                if (txtEdit.BaseLine == (short)((txtEdit.FontSize * 4 / 3 + 1) * 9))
                {
                    txtEdit.FontSize = (short)(txtEdit.FontSize * 4 / 3 + 1);
                }
                else
                {
                    txtEdit.FontSize = (short)(txtEdit.FontSize * 4 / 3);
                }
                txtEdit.BaseLine = 0;
            }
            else
            {
                if (txtEdit.BaseLine == 0)
                {
                    txtEdit.BaseLine = (short)(txtEdit.FontSize * 9);
                    txtEdit.FontSize = (short)(txtEdit.FontSize / 4 * 3);
                }
                else
                {
                    CommandSubScript.Execute();
                    CommandSuperScript.Execute();
                }
            }

        }

        private void CommandSelectAll_Executed(object sender, EventArgs e)
        {
            txtEdit.SelStart = 0;
            txtEdit.SelLength = txtEdit.CtlText.Length;
        }

        private void CommandDelete_Executed(object sender, EventArgs e)
        {
            txtEdit.Clip(4);
            CommandExecuted();
        }

        private void CommandFind_Executed(object sender, EventArgs e)
        {

        }

        private void CommandReplace_Executed(object sender, EventArgs e)
        {

        }

        private void CommandOutdent_Executed(object sender, EventArgs e)
        {
            if (txtEdit.IndentFL > 240)
            {
                txtEdit.IndentFL -= 240;
            }
            else
            {
                if (txtEdit.IndentFL != 0)
                {
                    txtEdit.IndentFL = 0;
                }
                else
                {
                    txtEdit.IndentL = 0;
                }
            }
        }

        private void CommandIndent_Executed(object sender, EventArgs e)
        {
            if ((txtEdit.IndentFL + 240) <= (txtEdit.PageWidth - (txtEdit.PageMarginL + txtEdit.PageMarginR)))
            {
                txtEdit.IndentFL += 240;
            }
            else
            {
                txtEdit.IndentFL = (short)(txtEdit.PageWidth - txtEdit.PageMarginL - txtEdit.PageMarginR);
            }
        }

        public void setEnableMenuItems(bool able)
        {
            //CommandFormatBrush.Enabled = able;
            CommandPaste.Enabled = able;
            CommandDirectPaste.Enabled = able;
            CommandSelectAll.Enabled = able;
            CommandFind.Enabled = able;
            CommandFindNext.Enabled = able;
            CommandReplace.Enabled = able;

            CommandAlignLeft.Enabled = able;
            CommandAlignRight.Enabled = able;
            CommandAlignCenter.Enabled = able;
            CommandAlignJustify.Enabled = able;

            CommandFontBold.Enabled = able;
            CommandFontItalic.Enabled = able;
            CommandFontUnderline.Enabled = able;
            CommandFontStrike.Enabled = able;


            CommandTextColor.Enabled = able;
            CommandFontSize.Enabled = able;
            CommandFont.Enabled = able;

            CommandListBulleted.Enabled = able;
            CommandListNumbered.Enabled = able;
            CommandOutdent.Enabled = able;
            CommandIndent.Enabled = able;

            CommandLineSpacing.Enabled = able;
            CommandSubScript.Enabled = able;
            CommandSuperScript.Enabled = able;

            UpdateSelectionCommandsState();
        }

        public bool bOpenFormat = false;
        //public string strFormatFontName = "";
        public short FormatFontSize = 12;
        public short FormatFontBold = 0;
        public short FormatFontItalic = 0;
        public short FormatLineSpacing = 150;
        public Color FormatColor = Color.Black;
        public void FormatSelTxt()
        {
            //暂停格式刷
            return;

            if (bOpenFormat == false)
                return;
            if (txtEdit.SelText == null || txtEdit.SelText.Length <= 0)
                return;

            this.txtEdit.FormatSelection = true;
            this.txtEdit.FontBold = FormatFontBold;
            this.txtEdit.FontItalic = FormatFontItalic;
            this.txtEdit.FontSize = FormatFontSize;
            //this.txtEdit.ForeColor = FormatColor;
            this.txtEdit.LineSpacing = FormatLineSpacing;
            bOpenFormat = false;
            CommandFormatBrush.Checked = false;
        }

        /// <summary>
        /// 统一设置签名字体，并返回签名行的字数
        /// </summary>
        /// <returns>返回签名行的汉字数，除了签名图片</returns>
        public float SetSingFont()
        {
            FontName = "宋体";
            FontSize = 12;
            FontBold = 0;
            LineSpacing = 150;
            TransCheckStyle(0);
            return (txtEdit.PageWidth - txtEdit.PageMarginL - txtEdit.PageMarginR) / 240f;
        }

        /// <summary>
        /// 删除所有的值为空的字段，在做模板的时候，防止为空的字段在以后的应用中出现不可预料的内容
        /// </summary>
        public void DeleteAllEmptyField()
        {
            short FieldID = txtEdit.FieldNext(0, 0);
            while (FieldID > 0)
            {
                txtEdit.FieldCurrent = FieldID;
                string strReturn = txtEdit.FieldText;
                if (strReturn == null || strReturn == string.Empty)
                {
                    //重新开始搜索
                    txtEdit.FieldDelete(true);
                    FieldID = txtEdit.FieldNext(0, 0);
                }
                else
                    FieldID = txtEdit.FieldNext(FieldID, 0);
            }
        }

        public int ContextEndAddLF()
        {
            int iLFCount = 0;
            for (int i = txtEdit.CtlText.Length; i > 0; i--)
            {
                if (txtEdit.CtlText[i - 1] == '\n' || txtEdit.CtlText[i - 1] == ' ' || txtEdit.CtlText[i - 1] == '　')
                    iLFCount++;
                else
                    break;
            }
            if (iLFCount > 1)
            {
                //将病历后面的多个" "或“\n”替换成一个“ ”
                txtEdit.SelStart = txtEdit.CtlText.Length - iLFCount;
                txtEdit.SelLength = iLFCount;
                txtEdit.LoadFromMemory("\n", 1, true);
                //if (txtEdit.CtlText.EndsWith("\n") == false)
                //    txtEdit.LoadFromMemory("\n", 1, true);
                return 1;
            }
            else if (iLFCount == 0)
                txtEdit.LoadFromMemory("\n", 1, true);
            return 0;
        }

        #endregion

        #region 自定义事件
        public delegate void CMouseDownEventHandler(object sender, AxTx4oleLib._DTX4OLEEvents_MouseDownEvent e);
        public delegate void CMouseUpEventtHandler(object sender, AxTx4oleLib._DTX4OLEEvents_MouseUpEvent e);
        public delegate void CFieldDblClicked(object sender, AxTx4oleLib._DTX4OLEEvents_FieldDblClickedEvent e);
        public delegate void CFieldClicked(object sender, AxTx4oleLib._DTX4OLEEvents_FieldClickedEvent e);
        public delegate void CObjectDblClicked(object sender, AxTx4oleLib._DTX4OLEEvents_ObjectDblClickedEvent e);
        public delegate void CSelectTextChangeHander(object sender, EventArgs e);


        public event CMouseDownEventHandler MouseDownEvent;
        public event CMouseUpEventtHandler MouseUpEvent;
        public event EventHandler MouseDblClick;
        public event CFieldDblClicked FieldDblClicked;
        public event CFieldClicked FieldClicked;
        public event CObjectDblClicked ObjectDblClicked;

        public event CSelectTextChangeHander SelectTextChangeEvent;


        private void CommandViewMode_Executed(object sender, EventArgs e)
        {
            if (!this.IsDisposed)
            {
                ICommandSource source = sender as ICommandSource;
                switch (Convert.ToString(source.CommandParameter))
                {
                    case "PageLayout":
                        this.ViewMode = 2;
                        break;
                    case "FullScreen":
                        this.ViewMode = 1;
                        break;
                    case "WebLayout":
                        this.ViewMode = 0;
                        break;
                    case "Outline":
                        this.ViewMode = 3;
                        break;
                    case "Draft":
                        this.ViewMode = 4;
                        break;
                }
            }
        }

        private void ZoomSlider_ValueChanged(object sender, EventArgs e)
        {
            if (ZoomSlider.Value >= 100)
            {
                ZoomSlider.Step = 2;
                ZoomFactor = Convert.ToInt16((ZoomSlider.Value - 100) * 5 + 100);
                ZoomSlider.Text = Convert.ToInt16((ZoomSlider.Value - 100) * 5 + 100) + "%";
            }
            else
            {
                if (ZoomSlider.Value % 10 != 0)
                {
                    ZoomSlider.Value -= ZoomSlider.Value % 10;
                }
                ZoomSlider.Step = 10;
                ZoomFactor = Convert.ToInt16(ZoomSlider.Value);
                ZoomSlider.Text = Convert.ToString(ZoomSlider.Value) + "%";
            }

        }

        private void txtEdit_FieldClicked(object sender, AxTx4oleLib._DTX4OLEEvents_FieldClickedEvent e)
        {
            if (FieldClicked != null)
            {
                FieldClicked(sender, e);
            }
            //this.FieldCurrent = this.FieldAtInputPos;
            //if (this.FieldCurrent == 0) return;
        }

        /// <summary>
        /// 元素事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtEdit_FieldDblClicked(object sender, AxTx4oleLib._DTX4OLEEvents_FieldDblClickedEvent e)
        {
            if (FieldDblClicked != null)
            {
                FieldDblClicked(sender, e);
            }
        }

        protected override void OnContextMenuChanged(EventArgs e)
        {
            base.OnContextMenuChanged(e);
        }

        private void txtEdit_DblClick(object sender, EventArgs e)
        {
            if (MouseDblClick != null)
            {
                MouseDblClick(sender, e);
            }
        }

        private void txtEdit_MouseDownEvent(object sender, AxTx4oleLib._DTX4OLEEvents_MouseDownEvent e)
        {
            if (MouseDownEvent != null)
            {
                MouseDownEvent(sender, e);
            }

            CommandFormatBrush.Enabled = CtlEditMode == 0;
        }

        private void txtEdit_MouseUpEvent(object sender, AxTx4oleLib._DTX4OLEEvents_MouseUpEvent e)
        {
            if (MouseUpEvent != null)
            {
                MouseUpEvent(sender, e);
            }

            //Update,RAO,2013-03-06,QS-Font
            //return;
            //if (txtEdit.CtlEditMode == 0)
            //{
            //    if (FontName != "宋体")
            //        FontName = "宋体";
            //}
            //if (this.txtEdit.SelLength > 0)
            //    FormatSelTxt();
        }

        private void txtEdit_MouseMoveEvent(object sender, AxTx4oleLib._DTX4OLEEvents_MouseMoveEvent e)
        {
            _mousepoint = new Point(e.x, e.y);
        }

        private void txtEdit_ObjectDblClicked(object sender, AxTx4oleLib._DTX4OLEEvents_ObjectDblClickedEvent e)
        {
            if (ObjectDblClicked != null)
            {
                ObjectDblClicked(sender, e);
            }
        }

        private void txtEdit_KeyStateChange(object sender, EventArgs e)
        {
            if (txtEdit.FieldAtInputPos > 0 && txtEdit.FieldText.Length <= 0)
                txtEdit.FieldDelete(false);
        }

        private void txtEdit_KeyDownEvent(object sender, AxTx4oleLib._DTX4OLEEvents_KeyDownEvent e)
        {
            if (_disabledpaste && (e.shift & 2) == 2)
            {
                //监控键盘复制、剪切和粘贴等功能
                switch (e.keyCode)
                {
                    case 67:    //复制
                        PostMessage(this.Handle, WM_EMR_CLIPBOARD, e.keyCode, 1);
                        break;
                    case 88:    //剪切
                        PostMessage(this.Handle, WM_EMR_CLIPBOARD, e.keyCode, 0);
                        break;
                    case 86:    //粘贴
                        PostMessage(this.Handle, WM_EMR_CLIPBOARD, e.keyCode, 2);
                        break;
                    case 90:    //取消
                        txtEdit.Undo();
                        break;
                    case 89:    //重复
                        txtEdit.Redo();
                        break;
                }
            }

            txtEdit.FieldSetInputPos(FieldInputPositionConstants.txInsideField);
            if (txtEdit.FieldAtInputPos > 0)
            {
                if (e.keyCode == 8 && txtEdit.FieldText.Length <= 1 && txtEdit.FieldAtInputPos == 1)
                {
                    txtEdit.FieldDelete(false);
                }
                else
                {
                    if (_FieldEditMode)
                        txtEdit.FieldSetInputPos(FieldInputPositionConstants.txInsideField);
                    else
                        txtEdit.FieldSetInputPos(FieldInputPositionConstants.txOutsideField);
                    txtEdit.ForeColor = txtEdit.FieldAtInputPos > 0 ? Color.Red : Color.Black;
                }
            }
            bOpenFormat = false;
            CommandFormatBrush.Enabled = false;
            CommandFormatBrush.Checked = false;
        }

        [Browsable(false)] // 这是设定事件被隐藏的标签
        public new event System.EventHandler MouseDown = null;
        [Browsable(false)]
        public new event EventHandler MouseUp = null;
        [Browsable(false)]
        public new event MouseEventHandler MouseDoubleClick = null;
        #endregion

        #region 构造函数、私有成员

        public EMREdit()
        {
            InitializeComponent();

            //控件显示模式
            txtEdit.ViewMode = 2;

            //只修改选中的字体
            txtEdit.FormatSelection = true;
            //txtEdit.ImageSaveMode = 1;
            //显示上标尺
            txtEdit.RulerHandle = TRulerH.hWnd;
            txtEdit.VerticalRulerHandle = TRulerV.hWnd;
            txtEdit.StatusBarHandle = TStatusBar.hWnd;
            txtEdit.ButtonBarHandle = TButtonBar.hWnd;
            //页眉、页脚
            txtEdit.HeaderFooterStyle = HeaderFooterStyleConstants.txMouseClick;
            txtEdit.HeaderFooter = (HeaderFooterConstants)5;

            //纸张尺寸
            txtEdit.PageWidth = 12000;
            txtEdit.PageHeight = 17000;
            txtEdit.PageMarginL = 650;
            txtEdit.PageMarginR = 650;
            txtEdit.PageMarginT = 585;
            txtEdit.PageMarginB = 585;
            txtEdit.LineSpacing = 150;

            txtEdit.PrintColors = false;

            //更新操作状态
            UpdateSelectionCommandsState();

            this.Dock = DockStyle.Fill;

            //txtEdit.ControlChars = false;

            txtEdit.FieldDeleted += new AxTx4oleLib._DTX4OLEEvents_FieldDeletedEventHandler(txtEdit_FieldDeleted);
            txtEdit.KeyDownEvent += new AxTx4oleLib._DTX4OLEEvents_KeyDownEventHandler(txtEdit_KeyDownEvent);
            txtEdit.KeyStateChange += new EventHandler(txtEdit_KeyStateChange);
            txtEdit.FieldSetCursor += new AxTx4oleLib._DTX4OLEEvents_FieldSetCursorEventHandler(txtEdit_FieldSetCursor);
        }

        void txtEdit_FieldSetCursor(object sender, AxTx4oleLib._DTX4OLEEvents_FieldSetCursorEvent e)
        {
            e.mousePointer = 12;//鼠标移到域为手形光标 
        }

        /// <summary>
        /// 删除域后发生将字体颜色恢复为黑色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtEdit_FieldDeleted(object sender, AxTx4oleLib._DTX4OLEEvents_FieldDeletedEvent e)
        {
            if(txtEdit.ForeColor != Color.Black)
                txtEdit.ForeColor = Color.Black;
        }

        protected override void DefWndProc(ref Message m)
        {
            if (m.Msg == WM_EMR_CLIPBOARD)
            {
                switch (m.LParam.ToInt32())
                {
                    case 0:     //剪切
                        txtEdit.Undo();
                        this.CommandCut_Executed(this, null);
                        break;
                    case 1:     //复制
                        this.CommandCopy_Executed(this, null);
                        break;
                    case 2:     //粘贴
                        this.CommandPaste_Executed(this, null);
                        break;
                }
            }
            else
                base.DefWndProc(ref m);
        }

        private void CommandExecuted()
        {
            _documentChanged = true;
            UpdateSelectionCommandsState();
        }

        private void UpdateStatusBar()
        {

        }

        private void UpdateSelectionCommandsState()
        {
            UpdateStateing = true;
            UpdateStatusBar();

            CommandCut.Enabled = txtEdit.CanCopy;
            CommandCopy.Enabled = txtEdit.CanCopy;
            CommandPaste.Enabled = true;
            CommandDirectPaste.Enabled = true;
            //CommandFormatBrush.Enabled = txtEdit.SelLength > 0;

            CommandFontBold.Checked = txtEdit.FontBold == 0 ? false : true;
            CommandFontItalic.Checked = txtEdit.FontItalic == 0 ? false : true;
            CommandFontUnderline.Checked = txtEdit.FontUnderline == 0 ? false : true;
            CommandFontStrike.Checked = txtEdit.FontStrikethru == 0 ? false : true;

            //if (txtEdit.FontName != string.Empty)
            //{
            //    CommandFont.Text = txtEdit.FontName;
            //}
            //else
            //{
            //    //Update,RAO,2013-03-06,QS-Font
            //    //CommandFont.Text = "";

            //    FontName = "宋体";
            //    CommandFont.Text = txtEdit.FontName;
            //}

            if (txtEdit.FontSize != 0)
            {
                try
                {
                    if (txtEdit.BaseLine != 0)
                    {
                        if (Math.Abs(txtEdit.BaseLine) == (short)((txtEdit.FontSize * 2 + 1) * (txtEdit.BaseLine > 0 ? 9 : 3)))
                            CommandFontSize.Text = Convert.ToString((short)(txtEdit.FontSize * 2 + 1));
                        else
                            CommandFontSize.Text = Convert.ToString((short)(txtEdit.FontSize * 2));
                    }
                    else
                        CommandFontSize.Text = Convert.ToString(txtEdit.FontSize);
                }
                catch
                {

                }
            }
            else
                CommandFontSize.Text = "";
            CommandFont.Text = txtEdit.FontName;
            CommandSuperScript.Checked = txtEdit.BaseLine > 0;
            CommandSubScript.Checked = txtEdit.BaseLine < 0;

            CommandAlignLeft.Checked = (txtEdit.Alignment == 0);
            CommandAlignRight.Checked = (txtEdit.Alignment == 1);
            CommandAlignCenter.Checked = (txtEdit.Alignment == 2);
            CommandAlignJustify.Checked = (txtEdit.Alignment == 3);

            CommandListBulleted.Checked = (txtEdit.ListType == ListTypeConstants.txListBulleted);
            CommandListNumbered.Checked = (txtEdit.ListType == ListTypeConstants.txListNumbered);

            //CommandLineSpacing.Checked = ((int)(txtEdit.LineSpacing / 150) == Convert.ToInt32(CommandLineSpacing.Text));

            UpdateStateing = false;
        }

        private void txtEdit_PosChange(object sender, EventArgs e)
        {
            UpdateSelectionCommandsState();

            if (SelectTextChangeEvent != null)
                SelectTextChangeEvent(sender, e);
        }

        #endregion
    }
}
