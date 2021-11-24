namespace Warcaby
{
    partial class main_window
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
            this.panel_menu = new System.Windows.Forms.Panel();
            this.button_start = new System.Windows.Forms.Button();
            this.label_player2 = new System.Windows.Forms.Label();
            this.label_player1 = new System.Windows.Forms.Label();
            this.textBox_black = new System.Windows.Forms.TextBox();
            this.textBox_white = new System.Windows.Forms.TextBox();
            this.panel_game = new System.Windows.Forms.Panel();
            this.screen_game = new System.Windows.Forms.PictureBox();
            this.panel_menu.SuspendLayout();
            this.panel_game.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.screen_game)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(482, 459);
            this.panel1.TabIndex = 0;
            // 
            // panel_menu
            // 
            this.panel_menu.Controls.Add(this.button_start);
            this.panel_menu.Controls.Add(this.label_player2);
            this.panel_menu.Controls.Add(this.label_player1);
            this.panel_menu.Controls.Add(this.textBox_black);
            this.panel_menu.Controls.Add(this.textBox_white);
            this.panel_menu.Location = new System.Drawing.Point(-1, -1);
            this.panel_menu.Name = "panel_menu";
            this.panel_menu.Size = new System.Drawing.Size(482, 459);
            this.panel_menu.TabIndex = 6;
            // 
            // button_start
            // 
            this.button_start.Location = new System.Drawing.Point(170, 122);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(75, 23);
            this.button_start.TabIndex = 4;
            this.button_start.Text = "START";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // label_player2
            // 
            this.label_player2.AutoSize = true;
            this.label_player2.Location = new System.Drawing.Point(92, 99);
            this.label_player2.Name = "label_player2";
            this.label_player2.Size = new System.Drawing.Size(72, 13);
            this.label_player2.TabIndex = 3;
            this.label_player2.Text = "Gracz czarny:";
            this.label_player2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_player1
            // 
            this.label_player1.AutoSize = true;
            this.label_player1.Location = new System.Drawing.Point(100, 73);
            this.label_player1.Name = "label_player1";
            this.label_player1.Size = new System.Drawing.Size(64, 13);
            this.label_player1.TabIndex = 2;
            this.label_player1.Text = "Gracz biały:";
            // 
            // textBox_black
            // 
            this.textBox_black.Location = new System.Drawing.Point(170, 96);
            this.textBox_black.Name = "textBox_black";
            this.textBox_black.Size = new System.Drawing.Size(135, 20);
            this.textBox_black.TabIndex = 1;
            // 
            // textBox_white
            // 
            this.textBox_white.Location = new System.Drawing.Point(170, 70);
            this.textBox_white.Name = "textBox_white";
            this.textBox_white.Size = new System.Drawing.Size(135, 20);
            this.textBox_white.TabIndex = 0;
            // 
            // panel_game
            // 
            this.panel_game.Controls.Add(this.screen_game);
            this.panel_game.Location = new System.Drawing.Point(0, 0);
            this.panel_game.Name = "panel_game";
            this.panel_game.Size = new System.Drawing.Size(482, 459);
            this.panel_game.TabIndex = 5;
            this.panel_game.Visible = false;
            // 
            // screen_game
            // 
            this.screen_game.Location = new System.Drawing.Point(0, 0);
            this.screen_game.Name = "screen_game";
            this.screen_game.Size = new System.Drawing.Size(482, 459);
            this.screen_game.TabIndex = 0;
            this.screen_game.TabStop = false;
            this.screen_game.Paint += new System.Windows.Forms.PaintEventHandler(this.screen_game_Paint);
            this.screen_game.MouseDown += new System.Windows.Forms.MouseEventHandler(this.screen_game_MouseDown);
            this.screen_game.MouseMove += new System.Windows.Forms.MouseEventHandler(this.screen_game_MouseMove);
            this.screen_game.MouseUp += new System.Windows.Forms.MouseEventHandler(this.screen_game_MouseUp);
            // 
            // main_window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 457);
            this.Controls.Add(this.panel_menu);
            this.Controls.Add(this.panel_game);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "main_window";
            this.Text = "Warcaby";
            this.panel_menu.ResumeLayout(false);
            this.panel_menu.PerformLayout();
            this.panel_game.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.screen_game)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.Label label_player2;
        private System.Windows.Forms.Label label_player1;
        private System.Windows.Forms.TextBox textBox_black;
        private System.Windows.Forms.TextBox textBox_white;
        public System.Windows.Forms.Panel panel_menu;
        public System.Windows.Forms.Panel panel_game;
        public System.Windows.Forms.PictureBox screen_game;




    }
}

