using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace EMR.Controls
{
    public partial class Signature : Office2007Form
    {
        //记录直线或者曲线的对象
        private System.Drawing.Drawing2D.GraphicsPath mousePath = new System.Drawing.Drawing2D.GraphicsPath();
        //画笔透明度
        private int myAlpha = 100;
        //画笔颜色对象
        private Color myUserColor = new Color();
        //画笔宽度
        private int myPenWidth = 5;
        //签名的图片对象
        public Bitmap SavedBitmap;
        public bool retOk = false;

        public Signature()
        {
            InitializeComponent();
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                try
                {
                    mousePath.AddLine(e.X, e.Y, e.X, e.Y);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            pictureBox.Invalidate();
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                mousePath.StartFigure();
            }
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                myUserColor = System.Drawing.Color.Black;
                myAlpha = 255;
                Pen CurrentPen = new Pen(Color.FromArgb(myAlpha, myUserColor), myPenWidth);
                e.Graphics.DrawPath(CurrentPen, mousePath);
            }
            catch { }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            pictureBox.CreateGraphics().Clear(Color.White);
            mousePath.Reset();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SavedBitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
            pictureBox.DrawToBitmap(SavedBitmap, new Rectangle(0, 0, pictureBox.Width, pictureBox.Height));
            //this.DialogResult = System.Windows.Forms.DialogResult.OK;
            //this.Dispose();
            retOk = true;
            this.Close();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            myPenWidth = Convert.ToInt32(numericUpDown1.Value);
        }

        private void pictureBox_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Cross;
        }

        private void pictureBox_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }
    }
}
