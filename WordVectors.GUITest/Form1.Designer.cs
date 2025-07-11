namespace WordVectors.GUITest
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
            button1 = new Button();
            textBox1 = new TextBox();
            richTextBox2 = new RichTextBox();
            label1 = new Label();
            openFileDialog1 = new OpenFileDialog();
            button2 = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Font = new Font("Sahel", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button1.Location = new Point(259, 17);
            button1.Name = "button1";
            button1.Size = new Size(88, 47);
            button1.TabIndex = 0;
            button1.Text = "بررسی";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Sahel", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox1.Location = new Point(353, 17);
            textBox1.Name = "textBox1";
            textBox1.RightToLeft = RightToLeft.Yes;
            textBox1.Size = new Size(255, 47);
            textBox1.TabIndex = 1;
            // 
            // richTextBox2
            // 
            richTextBox2.Dock = DockStyle.Bottom;
            richTextBox2.Font = new Font("Sahel", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            richTextBox2.Location = new Point(0, 70);
            richTextBox2.Name = "richTextBox2";
            richTextBox2.ReadOnly = true;
            richTextBox2.RightToLeft = RightToLeft.Yes;
            richTextBox2.Size = new Size(800, 487);
            richTextBox2.TabIndex = 3;
            richTextBox2.Text = "";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Sahel", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(614, 20);
            label1.Name = "label1";
            label1.RightToLeft = RightToLeft.Yes;
            label1.Size = new Size(155, 39);
            label1.TabIndex = 4;
            label1.Text = "کلمه مورد نظر:";
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            openFileDialog1.FileOk += openFileDialog1_FileOk;
            // 
            // button2
            // 
            button2.Font = new Font("Sahel", 13.8F);
            button2.Location = new Point(12, 16);
            button2.Name = "button2";
            button2.Size = new Size(159, 47);
            button2.TabIndex = 5;
            button2.Text = "انتخاب مدل";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 557);
            Controls.Add(button2);
            Controls.Add(label1);
            Controls.Add(richTextBox2);
            Controls.Add(textBox1);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private TextBox textBox1;
        private RichTextBox richTextBox2;
        private Label label1;
        private OpenFileDialog openFileDialog1;
        private Button button2;
    }
}
