using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Warcaby
{
    class _GameController
    {
        Player black, white;
        int player = 1;
        Move part_move = null;

        public void StartGame()
        {
            Main.BoardView.LoadBoard();

        }

        public void ChangePawnPos()
        {

        }

        public bool CheckPlayer(int color)
        {
            if (Math.Sign(color) != player)
                return false;
            return true;
        }

        public void PawnDown(int id)
        {
            Point new_position = PawnView.SeekPosition(PawnView.temp_location.X + 20, PawnView.temp_location.Y + 20);
            Point old_position = new Point(PawnView.pawns[id].coord_x, PawnView.pawns[id].coord_y);


            if(part_move == null) 
                part_move = new Move(old_position.X, old_position.Y, new_position.X, new_position.Y, PawnView.pawns[id].color);
            else
            {
                part_move.ChangeSrc(old_position.X, old_position.Y);
                part_move.ChangeDst(new_position.X, new_position.Y);
                part_move.ChangeClr(PawnView.pawns[id].color);
            }

            if(Main.BoardModel.CheckMove(part_move))
            {
                Main.BoardModel.MakeMove(part_move);
                PawnView.DropPawn(id, part_move);
                if (!part_move.next_attack)
                {
                    player *= -1;



                    // Ruch komputera
                    //while (true)
                    //{
                        CPU_Move();
                        player *= -1;
                        //Main.AIModel.my_color = 4;
                        //CPU_Move();
                        //player *= -1;
                        //Main.AIModel.my_color = 0;
                    //}
                    // <--


                    //player *= -1;
                }

                
            }
            else
            {
                part_move.possible_move = false;
                PawnView.temp_location = PawnView.pawns[id].screen_location;
                PawnView.DropPawn(id, part_move);
                part_move.possible_move = true;
            }


            
        }


        public void CPU_Move()
        {
            int id;
            part_move = null;

            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                    Main.AIModel.board[x, y] = Main.BoardModel.board[x, y];

            do
            {
                part_move = Main.AIModel.MyTurn(part_move);
                if (!Main.BoardModel.CheckMove(part_move))
                    part_move.possible_move = false;
                else part_move.possible_move = true;

                Main.BoardModel.MakeMove(part_move);
                id = PawnView.CheckPawn(part_move.x0, part_move.y0);
                PawnView.DropPawn(id, part_move);
            }
            while (part_move.next_attack);
            part_move = null;
        }

    }
}
