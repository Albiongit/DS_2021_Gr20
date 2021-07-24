
namespace Serveri
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.CertifikataBtn = new System.Windows.Forms.Button();
            this.Box = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(42, 202);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(354, 29);
            this.button1.TabIndex = 0;
            this.button1.Text = "Fillo Degjimin";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // CertifikataBtn
            // 
            this.CertifikataBtn.Location = new System.Drawing.Point(514, 409);
            this.CertifikataBtn.Name = "CertifikataBtn";
            this.CertifikataBtn.Size = new System.Drawing.Size(176, 29);
            this.CertifikataBtn.TabIndex = 2;
            this.CertifikataBtn.Text = "Certifikata";
            this.CertifikataBtn.UseVisualStyleBackColor = true;
            this.CertifikataBtn.Click += new System.EventHandler(this.CertifikataBtn_Click);
            // 
            // Box
            // 
            this.Box.FormattingEnabled = true;
            this.Box.ItemHeight = 20;
            this.Box.Location = new System.Drawing.Point(420, 30);
            this.Box.Name = "Box";
            this.Box.Size = new System.Drawing.Size(349, 364);
            this.Box.TabIndex = 3;
            this.Box.SelectedIndexChanged += new System.EventHandler(this.CertifikataBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Box);
            this.Controls.Add(this.CertifikataBtn);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button CertifikataBtn;
        private System.Windows.Forms.ListBox Box;
    }
}

