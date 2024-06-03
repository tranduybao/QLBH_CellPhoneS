namespace DoAn_QLBHCellPhoneS.Extend
{
    partial class frmBaoCao
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBaoCao));
            this.panel1 = new System.Windows.Forms.Panel();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.cellPhoneS_QLBHDataSet = new DoAn_QLBHCellPhoneS.CellPhoneS_QLBHDataSet();
            this.cellPhoneSQLBHDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cellPhoneS_QLBHDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cellPhoneSQLBHDataSetBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.reportViewer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(2564, 1301);
            this.panel1.TabIndex = 1;
            // 
            // reportViewer1
            // 
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "DoAn_QLBHCellPhoneS.Extend.BaoCao.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(8, 8);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(2544, 1281);
            this.reportViewer1.TabIndex = 0;
            // 
            // cellPhoneS_QLBHDataSet
            // 
            this.cellPhoneS_QLBHDataSet.DataSetName = "CellPhoneS_QLBHDataSet";
            this.cellPhoneS_QLBHDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // cellPhoneSQLBHDataSetBindingSource
            // 
            this.cellPhoneSQLBHDataSetBindingSource.DataSource = this.cellPhoneS_QLBHDataSet;
            this.cellPhoneSQLBHDataSetBindingSource.Position = 0;
            // 
            // frmBaoCao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2564, 1301);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmBaoCao";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Báo Cáo";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmBaoCao_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cellPhoneS_QLBHDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cellPhoneSQLBHDataSetBindingSource)).EndInit();
            this.ResumeLayout(false);

        }


        #endregion
        private System.Windows.Forms.Panel panel1;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource cellPhoneSQLBHDataSetBindingSource;
        private CellPhoneS_QLBHDataSet cellPhoneS_QLBHDataSet;
    }
}