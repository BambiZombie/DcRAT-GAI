
namespace Server.Forms
{
    partial class FormRegValueEditWord
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRegValueEditWord));
            this.cancelButton = new System.Windows.Forms.Button();
            this.baseBox = new System.Windows.Forms.GroupBox();
            this.radioDecimal = new System.Windows.Forms.RadioButton();
            this.radioHexa = new System.Windows.Forms.RadioButton();
            this.okButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.valueNameTxtBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.valueDataTxtBox = new Server.Helper.WordTextBox();
            this.baseBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(316, 158);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(88, 27);
            this.cancelButton.TabIndex = 12;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // baseBox
            // 
            this.baseBox.Controls.Add(this.radioDecimal);
            this.baseBox.Controls.Add(this.radioHexa);
            this.baseBox.Location = new System.Drawing.Point(222, 70);
            this.baseBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.baseBox.Name = "baseBox";
            this.baseBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.baseBox.Size = new System.Drawing.Size(182, 74);
            this.baseBox.TabIndex = 14;
            this.baseBox.TabStop = false;
            this.baseBox.Text = "Base";
            // 
            // radioDecimal
            // 
            this.radioDecimal.AutoSize = true;
            this.radioDecimal.Location = new System.Drawing.Point(16, 47);
            this.radioDecimal.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioDecimal.Name = "radioDecimal";
            this.radioDecimal.Size = new System.Drawing.Size(74, 18);
            this.radioDecimal.TabIndex = 4;
            this.radioDecimal.Text = "Decimal";
            this.radioDecimal.UseVisualStyleBackColor = true;
            // 
            // radioHexa
            // 
            this.radioHexa.AutoSize = true;
            this.radioHexa.Checked = true;
            this.radioHexa.Location = new System.Drawing.Point(16, 20);
            this.radioHexa.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioHexa.Name = "radioHexa";
            this.radioHexa.Size = new System.Drawing.Size(102, 18);
            this.radioHexa.TabIndex = 3;
            this.radioHexa.TabStop = true;
            this.radioHexa.Text = "Hexadecimal";
            this.radioHexa.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(222, 158);
            this.okButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(88, 27);
            this.okButton.TabIndex = 11;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 70);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 14);
            this.label2.TabIndex = 15;
            this.label2.Text = "Value data:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // valueNameTxtBox
            // 
            this.valueNameTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.valueNameTxtBox.Location = new System.Drawing.Point(14, 40);
            this.valueNameTxtBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.valueNameTxtBox.Name = "valueNameTxtBox";
            this.valueNameTxtBox.ReadOnly = true;
            this.valueNameTxtBox.Size = new System.Drawing.Size(389, 22);
            this.valueNameTxtBox.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 14);
            this.label1.TabIndex = 16;
            this.label1.Text = "Value name:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // valueDataTxtBox
            // 
            this.valueDataTxtBox.IsHexNumber = false;
            this.valueDataTxtBox.Location = new System.Drawing.Point(15, 90);
            this.valueDataTxtBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.valueDataTxtBox.MaxLength = 8;
            this.valueDataTxtBox.Name = "valueDataTxtBox";
            this.valueDataTxtBox.Size = new System.Drawing.Size(199, 22);
            this.valueDataTxtBox.TabIndex = 17;
            this.valueDataTxtBox.Type = Server.Helper.WordTextBox.WordType.DWORD;
            // 
            // FormRegValueEditWord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 200);
            this.Controls.Add(this.valueDataTxtBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.baseBox);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.valueNameTxtBox);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormRegValueEditWord";
            this.Text = "FormRegValueEditWord";
            this.baseBox.ResumeLayout(false);
            this.baseBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.GroupBox baseBox;
        private System.Windows.Forms.RadioButton radioDecimal;
        private System.Windows.Forms.RadioButton radioHexa;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox valueNameTxtBox;
        private System.Windows.Forms.Label label1;
        private Server.Helper.WordTextBox valueDataTxtBox;
    }
}