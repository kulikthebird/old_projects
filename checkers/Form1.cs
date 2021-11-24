using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Warcaby
{

    public partial class main_window : Form
    {
        public main_window()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button_start_Click(object sender, EventArgs e)
        {
            Main.MouseController.StartButton(sender, e);
        }

        private void screen_game_MouseDown(object sender, MouseEventArgs e)
        {
            Main.MouseController.PawnMouseDown(sender, e);
        }

        private void screen_game_MouseMove(object sender, MouseEventArgs e)
        {
            Main.MouseController.PawnMouseMove(sender, e);
        }

        private void screen_game_MouseUp(object sender, MouseEventArgs e)
        {
            Main.MouseController.PawnMouseUp(sender, e);
        }

        private void screen_game_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(Main.BoardView.buffer, e.ClipRectangle, e.ClipRectangle, GraphicsUnit.Pixel);
        }
    }
}
