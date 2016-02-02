namespace EMR.Controls
{
    partial class EMREdit
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        { 
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EMREdit));
            this.TRulerH = new AxTx4oleLib.AxTXRuler();
            this.TStatusBar = new AxTx4oleLib.AxTXStatusBar();
            this.TButtonBar = new AxTx4oleLib.AxTXButtonBar();
            this.txtEdit = new AxTx4oleLib.AxTXTextControl();
            this.CommandFont = new DevComponents.DotNetBar.Command();
            this.CommandUndo = new DevComponents.DotNetBar.Command();
            this.CommandCut = new DevComponents.DotNetBar.Command();
            this.CommandCopy = new DevComponents.DotNetBar.Command();
            this.CommandPaste = new DevComponents.DotNetBar.Command();
            this.CommandDelete = new DevComponents.DotNetBar.Command();
            this.CommandSelectAll = new DevComponents.DotNetBar.Command();
            this.CommandFindNext = new DevComponents.DotNetBar.Command();
            this.CommandFontBold = new DevComponents.DotNetBar.Command();
            this.CommandFontItalic = new DevComponents.DotNetBar.Command();
            this.CommandFontUnderline = new DevComponents.DotNetBar.Command();
            this.CommandFontStrike = new DevComponents.DotNetBar.Command();
            this.CommandAlignLeft = new DevComponents.DotNetBar.Command();
            this.CommandAlignCenter = new DevComponents.DotNetBar.Command();
            this.CommandAlignRight = new DevComponents.DotNetBar.Command();
            this.CommandFind = new DevComponents.DotNetBar.Command();
            this.CommandFontSize = new DevComponents.DotNetBar.Command();
            this.CommandStatus = new DevComponents.DotNetBar.Command();
            this.CommandTextColor = new DevComponents.DotNetBar.Command();
            this.CommandReplace = new DevComponents.DotNetBar.Command();
            this.CommandListBulleted = new DevComponents.DotNetBar.Command();
            this.CommandListNumbered = new DevComponents.DotNetBar.Command();
            this.CommandAlignJustify = new DevComponents.DotNetBar.Command();
            this.CommandRedo = new DevComponents.DotNetBar.Command();
            this.CommandFormatBrush = new DevComponents.DotNetBar.Command();
            this.CommandLineSpacing = new DevComponents.DotNetBar.Command();
            this.CommandSubScript = new DevComponents.DotNetBar.Command();
            this.CommandSuperScript = new DevComponents.DotNetBar.Command();
            this.CommandIndent = new DevComponents.DotNetBar.Command();
            this.CommandOutdent = new DevComponents.DotNetBar.Command();
            this.CommandDirectPaste = new DevComponents.DotNetBar.Command();
            this.TRulerV = new AxTx4oleLib.AxTXRuler();
            ((System.ComponentModel.ISupportInitialize)(this.TRulerH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TStatusBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TButtonBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TRulerV)).BeginInit();
            this.SuspendLayout();
            // 
            // TRulerH
            // 
            this.TRulerH.Dock = System.Windows.Forms.DockStyle.Top;
            this.TRulerH.Location = new System.Drawing.Point(0, 0);
            this.TRulerH.Name = "TRulerH";
            this.TRulerH.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("TRulerH.OcxState")));
            this.TRulerH.Size = new System.Drawing.Size(732, 25);
            this.TRulerH.TabIndex = 3;
            // 
            // TStatusBar
            // 
            this.TStatusBar.Location = new System.Drawing.Point(27, 338);
            this.TStatusBar.Name = "TStatusBar";
            this.TStatusBar.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("TStatusBar.OcxState")));
            this.TStatusBar.Size = new System.Drawing.Size(100, 18);
            this.TStatusBar.TabIndex = 0;
            // 
            // TButtonBar
            // 
            this.TButtonBar.Location = new System.Drawing.Point(12, 46);
            this.TButtonBar.Name = "TButtonBar";
            this.TButtonBar.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("TButtonBar.OcxState")));
            this.TButtonBar.Size = new System.Drawing.Size(100, 28);
            this.TButtonBar.TabIndex = 1;
            // 
            // txtEdit
            // 
            this.txtEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEdit.Location = new System.Drawing.Point(25, 25);
            this.txtEdit.Name = "txtEdit";
            this.txtEdit.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("txtEdit.OcxState")));
            this.txtEdit.Size = new System.Drawing.Size(707, 498);
            this.txtEdit.TabIndex = 5;
            this.txtEdit.DblClick += new System.EventHandler(this.txtEdit_DblClick);
            this.txtEdit.MouseDownEvent += new AxTx4oleLib._DTX4OLEEvents_MouseDownEventHandler(this.txtEdit_MouseDownEvent);
            this.txtEdit.ObjectDblClicked += new AxTx4oleLib._DTX4OLEEvents_ObjectDblClickedEventHandler(this.txtEdit_ObjectDblClicked);
            this.txtEdit.FieldClicked += new AxTx4oleLib._DTX4OLEEvents_FieldClickedEventHandler(this.txtEdit_FieldClicked);
            this.txtEdit.PosChange += new System.EventHandler(this.txtEdit_PosChange);
            this.txtEdit.MouseUpEvent += new AxTx4oleLib._DTX4OLEEvents_MouseUpEventHandler(this.txtEdit_MouseUpEvent);
            this.txtEdit.MouseMoveEvent += new AxTx4oleLib._DTX4OLEEvents_MouseMoveEventHandler(this.txtEdit_MouseMoveEvent);
            this.txtEdit.FieldDblClicked += new AxTx4oleLib._DTX4OLEEvents_FieldDblClickedEventHandler(this.txtEdit_FieldDblClicked);

            // 
            // CommandCopy
            // 
            this.CommandFormatBrush.Executed += new System.EventHandler(this.CommandFormatBrush_Executed);
            // 
            // CommandFont
            // 
            this.CommandFont.Executed += new System.EventHandler(this.CommandFont_Executed);
            // 
            // CommandUndo
            // 
            this.CommandUndo.Executed += new System.EventHandler(this.CommandUndo_Executed);
            // 
            // CommandCut
            // 
            this.CommandCut.Executed += new System.EventHandler(this.CommandCut_Executed);
            // 
            // CommandCopy
            // 
            this.CommandCopy.Executed += new System.EventHandler(this.CommandCopy_Executed);
            // 
            // CommandPaste
            // 
            this.CommandPaste.Executed += new System.EventHandler(this.CommandPaste_Executed);
            // 
            // CommandDelete
            // 
            this.CommandDelete.Executed += new System.EventHandler(this.CommandDelete_Executed);
            // 
            // CommandSelectAll
            // 
            this.CommandSelectAll.Executed += new System.EventHandler(this.CommandSelectAll_Executed);
            // 
            // CommandFontBold
            // 
            this.CommandFontBold.Executed += new System.EventHandler(this.CommandFontBold_Executed);
            // 
            // CommandFontItalic
            // 
            this.CommandFontItalic.Executed += new System.EventHandler(this.CommandFontItalic_Executed);
            // 
            // CommandFontUnderline
            // 
            this.CommandFontUnderline.Executed += new System.EventHandler(this.CommandFontUnderline_Executed);
            // 
            // CommandFontStrike
            // 
            this.CommandFontStrike.Executed += new System.EventHandler(this.CommandFontStrike_Executed);
            // 
            // CommandAlignLeft
            // 
            this.CommandAlignLeft.Executed += new System.EventHandler(this.CommandAlignLeft_Executed);
            // 
            // CommandAlignCenter
            // 
            this.CommandAlignCenter.Executed += new System.EventHandler(this.CommandAlignCenter_Executed);
            // 
            // CommandAlignRight
            // 
            this.CommandAlignRight.Executed += new System.EventHandler(this.CommandAlignRight_Executed);
            // 
            // CommandFind
            // 
            this.CommandFind.Executed += new System.EventHandler(this.CommandFind_Executed);
            // 
            // CommandFontSize
            // 
            this.CommandFontSize.Executed += new System.EventHandler(this.CommandFontSize_Executed);
            // 
            // CommandTextColor
            // 
            this.CommandTextColor.Executed += new System.EventHandler(this.CommandTextColor_Executed);
            // 
            // CommandReplace
            // 
            this.CommandReplace.Executed += new System.EventHandler(this.CommandReplace_Executed);
            // 
            // CommandListBulleted
            // 
            this.CommandListBulleted.Executed += new System.EventHandler(this.CommandListBulleted_Executed);
            // 
            // CommandListNumbered
            // 
            this.CommandListNumbered.Executed += new System.EventHandler(this.CommandListNumbered_Executed);
            // 
            // CommandAlignJustify
            // 
            this.CommandAlignJustify.Executed += new System.EventHandler(this.CommandAlignJustify_Executed);
            // 
            // CommandRedo
            // 
            this.CommandRedo.Executed += new System.EventHandler(this.CommandRedo_Executed);
            // 
            // CommandLineSpacing
            // 
            this.CommandLineSpacing.Executed += new System.EventHandler(this.CommandLineSpacing_Executed);
            // 
            // CommandSubScript
            // 
            this.CommandSubScript.Executed += new System.EventHandler(this.CommandSubScript_Executed);
            // 
            // CommandSuperScript
            // 
            this.CommandSuperScript.Executed += new System.EventHandler(this.CommandSuperScript_Executed);
            // 
            // CommandIndent
            // 
            this.CommandIndent.Executed += new System.EventHandler(this.CommandIndent_Executed);
            // 
            // CommandOutdent
            // 
            this.CommandOutdent.Executed += new System.EventHandler(this.CommandOutdent_Executed);
            // 
            // TRulerV
            // 
            this.TRulerV.Dock = System.Windows.Forms.DockStyle.Left;
            this.TRulerV.Location = new System.Drawing.Point(0, 25);
            this.TRulerV.Name = "TRulerV";
            this.TRulerV.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("TRulerV.OcxState")));
            this.TRulerV.Size = new System.Drawing.Size(25, 498);
            this.TRulerV.TabIndex = 6;
            // 
            // EMREdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtEdit);
            this.Controls.Add(this.TRulerV);
            this.Controls.Add(this.TRulerH);
            this.Controls.Add(this.TButtonBar);
            this.Controls.Add(this.TStatusBar);
            this.Name = "EMREdit";
            this.Size = new System.Drawing.Size(732, 523);
            ((System.ComponentModel.ISupportInitialize)(this.TRulerH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TStatusBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TButtonBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TRulerV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public AxTx4oleLib.AxTXRuler TRulerH;
        public AxTx4oleLib.AxTXStatusBar TStatusBar;
        public AxTx4oleLib.AxTXButtonBar TButtonBar;
        public DevComponents.DotNetBar.Command CommandFont;
        public DevComponents.DotNetBar.Command CommandUndo;
        public DevComponents.DotNetBar.Command CommandCut;
        public DevComponents.DotNetBar.Command CommandCopy;
        public DevComponents.DotNetBar.Command CommandPaste;
        public DevComponents.DotNetBar.Command CommandDelete;
        public DevComponents.DotNetBar.Command CommandFontBold;
        public DevComponents.DotNetBar.Command CommandFontItalic;
        public DevComponents.DotNetBar.Command CommandFontUnderline;
        public DevComponents.DotNetBar.Command CommandFontStrike;
        public DevComponents.DotNetBar.Command CommandAlignLeft;
        public DevComponents.DotNetBar.Command CommandAlignCenter;
        public DevComponents.DotNetBar.Command CommandAlignRight;
        public DevComponents.DotNetBar.Command CommandFind;
        public DevComponents.DotNetBar.Command CommandFontSize;
        public DevComponents.DotNetBar.Command CommandStatus;
        public DevComponents.DotNetBar.Command CommandTextColor;
        public DevComponents.DotNetBar.Command CommandListNumbered;
        public DevComponents.DotNetBar.Command CommandAlignJustify;
        public DevComponents.DotNetBar.Command CommandRedo;
        public DevComponents.DotNetBar.Command CommandSelectAll;
        public DevComponents.DotNetBar.Command CommandFindNext;
        public DevComponents.DotNetBar.Command CommandReplace;
        public DevComponents.DotNetBar.Command CommandListBulleted;
        public DevComponents.DotNetBar.Command CommandFormatBrush;
        public AxTx4oleLib.AxTXTextControl txtEdit;
        public DevComponents.DotNetBar.Command CommandLineSpacing;
        public DevComponents.DotNetBar.Command CommandSubScript;
        public DevComponents.DotNetBar.Command CommandSuperScript;
        public DevComponents.DotNetBar.Command CommandIndent;
        public DevComponents.DotNetBar.Command CommandOutdent;
        public DevComponents.DotNetBar.Command CommandDirectPaste;
        public AxTx4oleLib.AxTXRuler TRulerV;
    }
}
