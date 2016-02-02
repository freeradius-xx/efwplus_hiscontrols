namespace TestControls
{
    partial class FrmPrescription
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.prescriptionControl1 = new Prescription.Controls.PrescriptionControl(this.components);
            this.SuspendLayout();
            // 
            // prescriptionControl1
            // 
            this.prescriptionControl1.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.prescriptionControl1.CausesValidation = false;
            this.prescriptionControl1.CurrPatListId = 0;
            this.prescriptionControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prescriptionControl1.HideColName = null;
            this.prescriptionControl1.IsShowFootText = true;
            this.prescriptionControl1.IsShowToolBar = true;
            this.prescriptionControl1.LicenseKey = null;
            this.prescriptionControl1.Location = new System.Drawing.Point(0, 0);
            this.prescriptionControl1.Name = "prescriptionControl1";
            this.prescriptionControl1.PrescriptionStyle = Prescription.Controls.PrescriptionStyle.全部;
            this.prescriptionControl1.PresDeptCode = 0;
            this.prescriptionControl1.PresDeptName = null;
            this.prescriptionControl1.PresDoctorId = 0;
            this.prescriptionControl1.PresDoctorName = null;
            this.prescriptionControl1.Size = new System.Drawing.Size(862, 497);
            this.prescriptionControl1.TabIndex = 0;
            this.prescriptionControl1.SinglePresPrint += new Prescription.Controls.SinglePrescriptionPrint(this.prescriptionControl1_SinglePresPrint);
            // 
            // FrmPrescription
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(862, 497);
            this.Controls.Add(this.prescriptionControl1);
            this.Name = "FrmPrescription";
            this.Text = "电子处方";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmPrescription_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Prescription.Controls.PrescriptionControl prescriptionControl1;

    }
}