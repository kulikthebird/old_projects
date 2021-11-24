using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Warcaby
{
    class _MouseController
    {

        int pawn_id = -1;
        bool mouse_state;

        public void StartButton(object sender, EventArgs e)
        {
            // Rozpoczęcie partii

            Main.MenuView.HideMenuPanel();
            Main.BoardView.ShowBoard();
            Main.GameController.StartGame();
        }

        public void PawnMouseDown(object sender, MouseEventArgs e)
        {
            PawnView.mouse_start_pos = new Point(e.X, e.Y);
            Point position = PawnView.SeekPosition(e.X, e.Y);

            if(PawnView.board[position.X, position.Y] == 0)
                return;
            if(!Main.GameController.CheckPlayer(PawnView.pawns[(PawnView.board[position.X, position.Y])].color))
                return;

            pawn_id = PawnView.CheckPawn(position.X, position.Y);
            if (pawn_id == -1)
            {
                mouse_state = false;
                return;
            }
            else mouse_state = true;
            PawnView.temp_location = new Point(PawnView.pawns[pawn_id].screen_location.X, PawnView.pawns[pawn_id].screen_location.Y);
        }
        public void PawnMouseUp(object sender, MouseEventArgs e)
        {
            if(mouse_state)
            {
                Main.GameController.PawnDown(pawn_id);
            }
            mouse_state = false;
            pawn_id = -1;
        }

        public void PawnMouseMove(object sender, MouseEventArgs e)
        {
            if(mouse_state)
            {
                PawnView.ChangePawnPos(pawn_id, e.X, e.Y);
            }
        }
    }
}
