namespace Projekt1.Views
{
    partial class Menu
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
            this.easy = new System.Windows.Forms.Button();
            this.medium = new System.Windows.Forms.Button();
            this.hard = new System.Windows.Forms.Button();
            this.cbN = new System.Windows.Forms.ComboBox();
            this.cbK = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.PwP = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // easy
            // 
            this.easy.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.easy.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.easy.Location = new System.Drawing.Point(101, 191);
            this.easy.Name = "easy";
            this.easy.Size = new System.Drawing.Size(84, 29);
            this.easy.TabIndex = 0;
            this.easy.Text = "Łatwy";
            this.easy.UseVisualStyleBackColor = true;
            this.easy.Click += new System.EventHandler(this.easy_Click);
            // 
            // medium
            // 
            this.medium.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.medium.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.medium.Location = new System.Drawing.Point(101, 235);
            this.medium.Name = "medium";
            this.medium.Size = new System.Drawing.Size(84, 29);
            this.medium.TabIndex = 1;
            this.medium.Text = "Średni";
            this.medium.UseVisualStyleBackColor = true;
            this.medium.Click += new System.EventHandler(this.medium_Click);
            // 
            // hard
            // 
            this.hard.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hard.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.hard.Location = new System.Drawing.Point(101, 279);
            this.hard.Name = "hard";
            this.hard.Size = new System.Drawing.Size(84, 29);
            this.hard.TabIndex = 2;
            this.hard.Text = "Trudny";
            this.hard.UseVisualStyleBackColor = true;
            this.hard.Click += new System.EventHandler(this.hard_Click);
            // 
            // cbN
            // 
            this.cbN.FormattingEnabled = true;
            this.cbN.Location = new System.Drawing.Point(69, 132);
            this.cbN.Name = "cbN";
            this.cbN.Size = new System.Drawing.Size(64, 21);
            this.cbN.TabIndex = 4;
            this.cbN.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.n_KeyPress);
            // 
            // cbK
            // 
            this.cbK.FormattingEnabled = true;
            this.cbK.Location = new System.Drawing.Point(164, 132);
            this.cbK.Name = "cbK";
            this.cbK.Size = new System.Drawing.Size(64, 21);
            this.cbK.TabIndex = 5;
            this.cbK.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.k_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(141, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(12, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "x";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(92, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "Wybierz rozmiar gry";
            // 
            // PwP
            // 
            this.PwP.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PwP.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.PwP.Location = new System.Drawing.Point(101, 326);
            this.PwP.Name = "PwP";
            this.PwP.Size = new System.Drawing.Size(84, 29);
            this.PwP.TabIndex = 8;
            this.PwP.Text = "2 Graczy";
            this.PwP.UseVisualStyleBackColor = true;
            this.PwP.Click += new System.EventHandler(this.PwP_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Violet;
            this.label1.Location = new System.Drawing.Point(52, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(195, 31);
            this.label1.TabIndex = 9;
            this.label1.Text = "Kółko i krzyżyk";
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(301, 427);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PwP);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbK);
            this.Controls.Add(this.cbN);
            this.Controls.Add(this.hard);
            this.Controls.Add(this.medium);
            this.Controls.Add(this.easy);
            this.Name = "Menu";
            this.Text = "Menu";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button easy;
        private System.Windows.Forms.Button medium;
        private System.Windows.Forms.Button hard;
        private System.Windows.Forms.ComboBox cbN;
        private System.Windows.Forms.ComboBox cbK;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button PwP;
        private System.Windows.Forms.Label label1;
    }
}
