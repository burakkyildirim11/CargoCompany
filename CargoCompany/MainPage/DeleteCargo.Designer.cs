
namespace CargoCompany.MainPage
{
    partial class DeleteCargo
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDeleteCargo = new System.Windows.Forms.Button();
            this.lblDeleteCargo = new System.Windows.Forms.Label();
            this.comboBox = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel1.Controls.Add(this.btnDeleteCargo);
            this.panel1.Controls.Add(this.lblDeleteCargo);
            this.panel1.Controls.Add(this.comboBox);
            this.panel1.Location = new System.Drawing.Point(1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(335, 451);
            this.panel1.TabIndex = 0;
            // 
            // btnDeleteCargo
            // 
            this.btnDeleteCargo.BackColor = System.Drawing.Color.Orange;
            this.btnDeleteCargo.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteCargo.Location = new System.Drawing.Point(132, 160);
            this.btnDeleteCargo.Name = "btnDeleteCargo";
            this.btnDeleteCargo.Size = new System.Drawing.Size(78, 29);
            this.btnDeleteCargo.TabIndex = 3;
            this.btnDeleteCargo.Text = "Delete";
            this.btnDeleteCargo.UseVisualStyleBackColor = false;
            this.btnDeleteCargo.Click += new System.EventHandler(this.btnDeleteCargo_Click);
            // 
            // lblDeleteCargo
            // 
            this.lblDeleteCargo.Font = new System.Drawing.Font("Microsoft JhengHei", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblDeleteCargo.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblDeleteCargo.Location = new System.Drawing.Point(42, 63);
            this.lblDeleteCargo.Name = "lblDeleteCargo";
            this.lblDeleteCargo.Size = new System.Drawing.Size(258, 31);
            this.lblDeleteCargo.TabIndex = 2;
            this.lblDeleteCargo.Text = "Silmek istediğiniz kargoyu seçiniz :";
            // 
            // comboBox
            // 
            this.comboBox.FormattingEnabled = true;
            this.comboBox.Location = new System.Drawing.Point(87, 114);
            this.comboBox.Name = "comboBox";
            this.comboBox.Size = new System.Drawing.Size(169, 21);
            this.comboBox.TabIndex = 1;
            // 
            // DeleteCargo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(337, 450);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DeleteCargo";
            this.Text = "Delete Cargo";
            this.Load += new System.EventHandler(this.DeleteCargo_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox comboBox;
        private System.Windows.Forms.Button btnDeleteCargo;
        private System.Windows.Forms.Label lblDeleteCargo;
    }
}