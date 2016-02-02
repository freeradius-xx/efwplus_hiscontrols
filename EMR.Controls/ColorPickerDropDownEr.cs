using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.Reflection;

namespace EMR.Controls
{
    public class ColorPickerDropDownEr : DevComponents.DotNetBar.ColorPickerDropDown
    {

        protected override void OnItemAdded(DevComponents.DotNetBar.BaseItem item)
        {
            switch (item.Text)
            {
                case "Theme Colors":
                    item.Text = "主题颜色";
                    break;
                case "Standard Colors":
                    item.Text = "基本颜色";
                    break;
                case "&More Colors...":
                    item.Text = "更多颜色...";
                    break;
            }
        }

        protected void itemClick(object sender, EventArgs e)
        {
            this.DisplayMoreColorsDialog();
        }

        public new void DisplayMoreColorsDialog()
        {
            ColorDialog dlgColor = new ColorDialog();
            if (!this.SelectedColor.IsEmpty)
            {
                dlgColor.Color = this.SelectedColor;
            }
            //this.SetSourceControl(dlgColor);
            CancelObjectValueEventArgs e = new CancelObjectValueEventArgs(dlgColor);
            this.OnBeforeColorDialog(e);
            if (e.Cancel)
            {
                dlgColor.Dispose();
            }
            else
            {
                DialogResult dlgResult;
                if (this.OwnerWindow != null)
                {
                    dlgResult = dlgColor.ShowDialog(this.OwnerWindow);
                }
                else
                {
                    dlgResult = dlgColor.ShowDialog();
                }
                if ((dlgResult == DialogResult.OK) && !dlgColor.Color.IsEmpty)
                {
                    this.SelectedColor = dlgColor.Color;
                    this.OnSelectedColorChanged(new EventArgs());
                    this.RaiseClick();
                    this.UpdateSelectedColorImage();

                }
                dlgColor.Dispose();
            }
        }
    }
}
