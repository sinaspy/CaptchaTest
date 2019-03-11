namespace CaptchaTest
{
    partial class Form1
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
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.txtCaptchaLink = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.picOriginal = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.picModified = new System.Windows.Forms.PictureBox();
            this.btnTransformImage = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPredict = new System.Windows.Forms.TextBox();
            this.btnPredict = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.btnGetImage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picOriginal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picModified)).BeginInit();
            this.SuspendLayout();
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(137, 28);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(651, 20);
            this.txtUrl.TabIndex = 0;
            this.txtUrl.Text = "http://billing.nigc.ir/billing/gasAll.aspx";
            // 
            // txtCaptchaLink
            // 
            this.txtCaptchaLink.Location = new System.Drawing.Point(137, 67);
            this.txtCaptchaLink.Name = "txtCaptchaLink";
            this.txtCaptchaLink.ReadOnly = true;
            this.txtCaptchaLink.Size = new System.Drawing.Size(642, 20);
            this.txtCaptchaLink.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(137, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "URL:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(137, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Captcha Link:";
            // 
            // picOriginal
            // 
            this.picOriginal.Location = new System.Drawing.Point(137, 106);
            this.picOriginal.Name = "picOriginal";
            this.picOriginal.Size = new System.Drawing.Size(180, 50);
            this.picOriginal.TabIndex = 4;
            this.picOriginal.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(134, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Original:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(137, 159);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Modified:";
            // 
            // picModified
            // 
            this.picModified.Location = new System.Drawing.Point(137, 175);
            this.picModified.Name = "picModified";
            this.picModified.Size = new System.Drawing.Size(180, 50);
            this.picModified.TabIndex = 7;
            this.picModified.TabStop = false;
            // 
            // btnTransformImage
            // 
            this.btnTransformImage.Location = new System.Drawing.Point(12, 93);
            this.btnTransformImage.Name = "btnTransformImage";
            this.btnTransformImage.Size = new System.Drawing.Size(119, 132);
            this.btnTransformImage.TabIndex = 8;
            this.btnTransformImage.Text = "Transform";
            this.btnTransformImage.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(134, 228);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Prediction:";
            // 
            // txtPredict
            // 
            this.txtPredict.Location = new System.Drawing.Point(137, 244);
            this.txtPredict.Name = "txtPredict";
            this.txtPredict.ReadOnly = true;
            this.txtPredict.Size = new System.Drawing.Size(180, 20);
            this.txtPredict.TabIndex = 10;
            // 
            // btnPredict
            // 
            this.btnPredict.Location = new System.Drawing.Point(12, 231);
            this.btnPredict.Name = "btnPredict";
            this.btnPredict.Size = new System.Drawing.Size(119, 119);
            this.btnPredict.TabIndex = 11;
            this.btnPredict.Text = "Predict";
            this.btnPredict.UseVisualStyleBackColor = true;
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(323, 106);
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Size = new System.Drawing.Size(465, 257);
            this.txtLog.TabIndex = 12;
            this.txtLog.Text = "";
            // 
            // btnGetImage
            // 
            this.btnGetImage.Location = new System.Drawing.Point(12, 12);
            this.btnGetImage.Name = "btnGetImage";
            this.btnGetImage.Size = new System.Drawing.Size(119, 75);
            this.btnGetImage.TabIndex = 13;
            this.btnGetImage.Text = "Get Image";
            this.btnGetImage.UseVisualStyleBackColor = true;
            this.btnGetImage.Click += new System.EventHandler(this.btnGetImage_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnGetImage);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnPredict);
            this.Controls.Add(this.txtPredict);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnTransformImage);
            this.Controls.Add(this.picModified);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.picOriginal);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCaptchaLink);
            this.Controls.Add(this.txtUrl);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.picOriginal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picModified)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.TextBox txtCaptchaLink;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox picOriginal;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox picModified;
        private System.Windows.Forms.Button btnTransformImage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPredict;
        private System.Windows.Forms.Button btnPredict;
        private System.Windows.Forms.RichTextBox txtLog;
        private System.Windows.Forms.Button btnGetImage;
    }
}

