using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Warcaby
{
    class _BoardView
    {

        public Bitmap board;                // Bitmapa z planszą
        public Bitmap buffer;               // Bitmapa zawierająca bufor wyjściowy na ekran
        public Bitmap buffer_move;          // Bitmapa w trybie przeciągania


        //
        //  Odkrywa panel z grą
        //

        public void ShowBoard()
        {
            Program.Wnd.panel_game.Visible = true;  
        }

        //
        //  Pobiera bitmapę z planszą, tworzy plansze.
        //
        public void LoadBoard()
        {
            if (board == null)
            {
                board = new Bitmap(Image.FromFile("board.png"));
            }

            buffer = new Bitmap(board);

            //PawnView.NewPawn(0, 7, 2);
            //PawnView.NewPawn(3, 4, -1);
            //PawnView.NewPawn(4, 1, -1);

            
            for (int i = 0; i < 4; i++)
                PawnView.NewPawn(1 + i * 2, 0, -1);
            for (int i = 0; i < 4; i++)
                PawnView.NewPawn(0 + i * 2, 1, -1);
            for (int i = 0; i < 4; i++)
                PawnView.NewPawn(1 + i * 2, 2, -1);

            for (int i = 0; i < 4; i++)
                PawnView.NewPawn(0 + i * 2, 7, 1);
            for (int i = 0; i < 4; i++)
                PawnView.NewPawn(1 + i * 2, 6, 1);
            for (int i = 0; i < 4; i++)
                PawnView.NewPawn(0 + i * 2, 5, 1); 
             
        }
    }
}
