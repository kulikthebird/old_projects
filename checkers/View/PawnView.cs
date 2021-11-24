using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Warcaby
{
    class PawnView
    {
        public static int counter = 1;                          // Licznik pionków
        public static Bitmap pawn_white = null;                 // Bitmapa zawierająca biały pionek
        public static Bitmap pawn_black = null;                 // Bitmapa zawierająca czarny pionek
        public static Bitmap pawn_white_queen = null;           // Bitmapa zawierająca białą damę
        public static Bitmap pawn_black_queen = null;           // Bitmapa zawierająca czarną damę
        public static PawnView[] pawns = new PawnView[25];      // Referencje do poszczególnych pionków
        public static int[,] board = new int[8, 8];             // Tablica z położeniem każdego pionka
        public static Point temp_location = new Point();        // Tymczasowe położenie pionka na ekranie
        public static Point mouse_start_pos = new Point();      // Pozycja startowa kursora podczas przeciągania

        public int coord_x;                                     // Współrzędna x na planszy
        public int coord_y;                                     // Współrzędna y na planszy
        public int color;                                       // Kolor oraz typ pionka, -1: czarny, 1: biały
        public Point screen_location = new Point();             // Położenie pionka na ekranie
        public int id;                                          // Numer ID pionka

        //
        //  Zmienia pozycję pionka wraz ze zmianą pozycji kursora podczas przeciągania pionka
        //

        public static void ChangePawnPos(int id, int dx, int dy)
        {

            Point temp_position = SeekPosition(dx, dy);

            Graphics graph_board = Graphics.FromImage(Main.BoardView.buffer);
            graph_board.DrawImage(Main.BoardView.buffer_move, new Rectangle(temp_location.X, temp_location.Y, 42, 41), new Rectangle(temp_location.X, temp_location.Y, 42, 41), GraphicsUnit.Pixel);
            temp_location = new Point(temp_location.X + dx - mouse_start_pos.X, temp_location.Y + dy - mouse_start_pos.Y);
            mouse_start_pos = new Point(dx, dy);
            if ((PawnView.pawns[id].color == 2) || (PawnView.pawns[id].color == -2))
                graph_board.DrawImage((PawnView.pawns[id].color == 2) ? PawnView.pawn_white_queen : PawnView.pawn_black_queen, temp_location.X, temp_location.Y, 42, 41);
            else
                graph_board.DrawImage((PawnView.pawns[id].color == 1)?PawnView.pawn_white:PawnView.pawn_black , temp_location.X, temp_location.Y, 42, 41);
            graph_board.Dispose();
            Program.Wnd.screen_game.Invalidate();
        }


        //
        //  Znajduje położenie na planszy względem położenia punktu
        //

        public static Point SeekPosition(int dx, int dy)
        {
            int x = -1, y = -1;

            int width = 50, height = 50;
            int centr_x = 0, centr_y = 0;

            // Sprawdzanie X

            if (dx >= 4 * width + centr_x)
            {
                if (dx >= 6 * width + centr_x)
                {
                    if (dx <= 7 * width + centr_x) x = 6;
                    else x = 7;
                }
                else
                {
                    if (dx <= 5 * width + centr_x) x = 4;
                    else x = 5;
                }
            }
            else
            {
                if (dx >= 2 * width + centr_x)
                {
                    if (dx <= 3 * width + centr_x) x = 2;
                    else x = 3;
                }
                else
                {
                    if (dx <= 1 * width + centr_x) x = 0;
                    else x = 1;
                }
            }

            // Sprawdzanie Y

            if (dy >= 4 * height + centr_y)
            {
                if (dy >= 6 * height + centr_y)
                {
                    if (dy <= 7 * height + centr_y) y = 6;
                    else y = 7;
                }
                else
                {
                    if (dy <= 5 * height + centr_y) y = 4;
                    else y = 5;
                }
            }
            else
            {
                if (dy >= 2 * height + centr_y)
                {
                    if (dy <= 3 * height + centr_y) y = 2;
                    else y = 3;
                }
                else
                {
                    if (dy <= 1 * height + centr_y) y = 0;
                    else y = 1;
                }
            }

            return new Point(x, y);

        }


        //
        //  Sprawdza, czy na danym polu na którym user chce rozpocząć przeciąganie, znajduje się pionek
        //  Jeśli tak, zapisuje odpowiednie położenie kursora, zmienia grafikę w tryb przeciągania
        //

        public static int CheckPawn(int x, int y)
        {
            // Sprawdzenie, czy na danym polu leży jakiś pionek

            if (board[x, y] == 0) return -1;
            else
            {
                int idx = board[x, y];
                Main.BoardView.buffer_move = new Bitmap(Main.BoardView.buffer);
                Graphics graph = Graphics.FromImage(Main.BoardView.buffer_move);

                graph.DrawImage(Main.BoardView.board, new Rectangle(pawns[idx].screen_location.X, pawns[idx].screen_location.Y, 42, 41), new Rectangle(pawns[idx].screen_location.X, pawns[idx].screen_location.Y, 42, 41), GraphicsUnit.Pixel);
                graph.Dispose();

                return idx;
            }
        }

        //
        //  Upuszcza pionek na dane pole
        //

        public static void DropPawn(int id, Move move)
        {
            int id_attack = 0;
            Point position;
            if(move.possible_move) position = new Point(move.x, move.y);
            else position = new Point(move.x0, move.y0);
            Point old_position = new Point(move.x0, move.y0);
            pawns[id].screen_location = new System.Drawing.Point(5 + position.X * 50, 5 + position.Y * 50);

            pawns[id].color = move.color;

            Graphics graph_board = Graphics.FromImage(Main.BoardView.buffer);
            
            
            graph_board.DrawImage(Main.BoardView.buffer_move,0,0);
            
            if(Math.Abs(pawns[id].color) == 2)
                graph_board.DrawImage((pawns[id].color == 2) ? pawn_white_queen : pawn_black_queen, pawns[id].screen_location.X, pawns[id].screen_location.Y);
            else
                graph_board.DrawImage((pawns[id].color == 1) ? pawn_white : pawn_black, pawns[id].screen_location.X, pawns[id].screen_location.Y);




            // Sprawdź, czy gracz zbił pionka przeciwnika
            // Jeśli tak, to usuń go z pola gry
            if (!move.next_attack && move.move_type == 2)
            {
                for (int i = 0; i < move.attacked_pawns; i++)
                {
                    id_attack = board[move.atk[i, 0], move.atk[i, 1]];
                    graph_board.DrawImage(Main.BoardView.board, new Rectangle(pawns[id_attack].screen_location.X, pawns[id_attack].screen_location.Y, 42, 41), new Rectangle(pawns[id_attack].screen_location.X, pawns[id_attack].screen_location.Y, 42, 41), GraphicsUnit.Pixel);
                    board[move.atk[i, 0], move.atk[i, 1]] = 0;
                    pawns[id_attack].Delete();
                }
            }


            graph_board.Dispose();
            
            board[old_position.X, old_position.Y] = 0;
            board[position.X, position.Y] = id;
            Program.Wnd.screen_game.Invalidate();
            PawnView.pawns[id].coord_x = position.X;
            PawnView.pawns[id].coord_y = position.Y;

        }

        //
        //  Tworzy nowy pionek na danych współrzędnych planszy oraz kolorze
        //

        public static void NewPawn(int x, int y, int type)
        {

            if (pawn_black == null || pawn_white == null)
            {
                pawn_black = new Bitmap(Image.FromFile("black.png"));
                pawn_white = new Bitmap(Image.FromFile("white.png"));
                pawn_black_queen = new Bitmap(Image.FromFile("black_queen.png"));
                pawn_white_queen = new Bitmap(Image.FromFile("white_queen.png"));
            }

            if (board[0, 0] != 0)
            {
                for (int yy = 0; yy < 8; yy++)
                    for (int xx = 0; xx < 8; xx++)
                        board[xx, yy] = 0;
            }

            

            pawns[counter] = new PawnView();
            pawns[counter].color = type;
            pawns[counter].id = counter;
            pawns[counter].screen_location = new System.Drawing.Point(5 + x * 50, 5 + y * 50);
            board[x, y] = counter;

            pawns[counter].coord_x = x;
            pawns[counter].coord_y = y;

            Graphics graph_board = Graphics.FromImage(Main.BoardView.buffer);
            graph_board.DrawImage((type == 1)?pawn_white:pawn_black, pawns[counter].screen_location.X, pawns[counter].screen_location.Y);
            graph_board.Dispose();

            counter++;
        }

        //
        //  Zmienia piona na królową
        //

        public void MakeQueen()
        {

        }


        //
        //  Kasuje pionek
        //

        public void Delete()
        {
            //img[id].Dispose();
        }

        //
        //  Sprowadza widok pionków do postaci czystej
        //

        public void Reset()
        {
            counter = 0;
            // nie wiem jeszcze co tu będzie, ale coś będzie
        }

    }
}
