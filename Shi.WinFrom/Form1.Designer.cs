namespace Shi.WinFrom
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
            this.MenuMain = new System.Windows.Forms.MenuStrip();
            this.进制转换 = new System.Windows.Forms.ToolStripMenuItem();
            this.线程 = new System.Windows.Forms.ToolStripMenuItem();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.PanelScale = new System.Windows.Forms.Panel();
            this.MenuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuMain
            // 
            this.MenuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.进制转换,
            this.线程});
            this.MenuMain.Location = new System.Drawing.Point(0, 0);
            this.MenuMain.Name = "MenuMain";
            this.MenuMain.Size = new System.Drawing.Size(884, 25);
            this.MenuMain.TabIndex = 0;
            this.MenuMain.Text = "MenuMain";
            this.MenuMain.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.MenuMain_ItemClicked);
            // 
            // 进制转换
            // 
            this.进制转换.CheckOnClick = true;
            this.进制转换.Name = "进制转换";
            this.进制转换.Size = new System.Drawing.Size(68, 21);
            this.进制转换.Text = "进制转换";
            // 
            // 线程
            // 
            this.线程.CheckOnClick = true;
            this.线程.Name = "线程";
            this.线程.Size = new System.Drawing.Size(44, 21);
            this.线程.Text = "线程";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 48);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(303, 701);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // richTextBox2
            // 
            this.richTextBox2.Location = new System.Drawing.Point(335, 49);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(537, 700);
            this.richTextBox2.TabIndex = 2;
            this.richTextBox2.Text = "";
            // 
            // PanelScale
            // 
            this.PanelScale.Location = new System.Drawing.Point(0, 38);
            this.PanelScale.Name = "PanelScale";
            this.PanelScale.Size = new System.Drawing.Size(884, 723);
            this.PanelScale.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 761);
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.MenuMain);
            this.Controls.Add(this.PanelScale);
            this.MainMenuStrip = this.MenuMain;
            this.Name = "Form1";
            this.Text = "便捷工具";
            this.MenuMain.ResumeLayout(false);
            this.MenuMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MenuMain;
        private System.Windows.Forms.ToolStripMenuItem 线程;
        private System.Windows.Forms.ToolStripMenuItem 进制转换;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.Panel PanelScale;
    }
}

