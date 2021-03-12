namespace SnakeUI
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
            this.rowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.ColPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.Start_btn = new System.Windows.Forms.Button();
            this.Bot_Game_btn = new System.Windows.Forms.Button();
            this.Buttons_layout = new System.Windows.Forms.Panel();
            this.LengthL = new System.Windows.Forms.Label();
            this.Buttons_layout.SuspendLayout();
            this.SuspendLayout();
            // 
            // rowPanel
            // 
            this.rowPanel.AutoSize = true;
            this.rowPanel.BackColor = System.Drawing.Color.Transparent;
            this.rowPanel.Location = new System.Drawing.Point(153, 81);
            this.rowPanel.Name = "rowPanel";
            this.rowPanel.Size = new System.Drawing.Size(0, 0);
            this.rowPanel.TabIndex = 0;
            // 
            // ColPanel
            // 
            this.ColPanel.AutoSize = true;
            this.ColPanel.BackColor = System.Drawing.Color.Transparent;
            this.ColPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.ColPanel.Location = new System.Drawing.Point(24, 12);
            this.ColPanel.Name = "ColPanel";
            this.ColPanel.Size = new System.Drawing.Size(27, 159);
            this.ColPanel.TabIndex = 1;
            // 
            // Start_btn
            // 
            this.Start_btn.AutoSize = true;
            this.Start_btn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Start_btn.BackColor = System.Drawing.Color.Green;
            this.Start_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Start_btn.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Start_btn.Location = new System.Drawing.Point(65, 3);
            this.Start_btn.Name = "Start_btn";
            this.Start_btn.Size = new System.Drawing.Size(132, 27);
            this.Start_btn.TabIndex = 2;
            this.Start_btn.Text = "Start a new game";
            this.Start_btn.UseVisualStyleBackColor = false;
            // 
            // Bot_Game_btn
            // 
            this.Bot_Game_btn.AutoSize = true;
            this.Bot_Game_btn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Bot_Game_btn.BackColor = System.Drawing.Color.Blue;
            this.Bot_Game_btn.Enabled = false;
            this.Bot_Game_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Bot_Game_btn.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Bot_Game_btn.ForeColor = System.Drawing.Color.White;
            this.Bot_Game_btn.Location = new System.Drawing.Point(97, 36);
            this.Bot_Game_btn.Name = "Bot_Game_btn";
            this.Bot_Game_btn.Size = new System.Drawing.Size(82, 27);
            this.Bot_Game_btn.TabIndex = 3;
            this.Bot_Game_btn.Text = "Bot Game";
            this.Bot_Game_btn.UseVisualStyleBackColor = false;
            this.Bot_Game_btn.Visible = false;
            this.Bot_Game_btn.Click += new System.EventHandler(this.Bot_Game_btn_Click);
            // 
            // Buttons_layout
            // 
            this.Buttons_layout.Controls.Add(this.LengthL);
            this.Buttons_layout.Controls.Add(this.Bot_Game_btn);
            this.Buttons_layout.Controls.Add(this.Start_btn);
            this.Buttons_layout.Location = new System.Drawing.Point(532, 79);
            this.Buttons_layout.Name = "Buttons_layout";
            this.Buttons_layout.Size = new System.Drawing.Size(245, 125);
            this.Buttons_layout.TabIndex = 4;
            // 
            // LengthL
            // 
            this.LengthL.AutoSize = true;
            this.LengthL.Font = new System.Drawing.Font("Pristina", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LengthL.ForeColor = System.Drawing.Color.DarkOrange;
            this.LengthL.Location = new System.Drawing.Point(116, 66);
            this.LengthL.Name = "LengthL";
            this.LengthL.Size = new System.Drawing.Size(71, 27);
            this.LengthL.TabIndex = 5;
            this.LengthL.Text = "Length: ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Buttons_layout);
            this.Controls.Add(this.rowPanel);
            this.Controls.Add(this.ColPanel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.Buttons_layout.ResumeLayout(false);
            this.Buttons_layout.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel rowPanel;
        private System.Windows.Forms.FlowLayoutPanel ColPanel;
        private System.Windows.Forms.Button Start_btn;
        private System.Windows.Forms.Button Bot_Game_btn;
        private System.Windows.Forms.Panel Buttons_layout;
        private System.Windows.Forms.Label LengthL;
    }
}

