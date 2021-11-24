using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warcaby
{
    class _AIModel
    {
        public int[,] board = new int[8, 8];
        // OFLAGOWANIE:
        //
        // 0 - 0 puste pole, 1 jest pionek (damka)
        // 1 - 0 pionek, 1 damka
        // 2 - 0 czarny, 1 biały
        // 3 - 0 pozornie jest, pozornie zbity (potrzebne do symulacji)
        // 4 - ...
        //

        public int my_color = 0;

        //

        public _AIModel()
        {
            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                    board[x, y] = 0;

            ResetBoard();

        }

        public void ResetBoard()
        {
            for (int i = 0; i < 4; i++)
                board[1 + i * 2, 0] = 1;
            for (int i = 0; i < 4; i++)
                board[0 + i * 2, 1] = 1;
            for (int i = 0; i < 4; i++)
                board[1 + i * 2, 2] = 1;

            for (int i = 0; i < 4; i++)
                board[0 + i * 2, 5] = 5;
            for (int i = 0; i < 4; i++)
                board[1 + i * 2, 6] = 5;
            for (int i = 0; i < 4; i++)
                board[0 + i * 2, 7] = 5;
        }

        // Funkcja sprawdzająca atak, którego wynikiem będzie największa ilość zbić pionków przeciwnika
        // Wersja dla pionka
        private _move AttackPossibilityP(int x, int y, int enemy_color)
        {
            _move max = new _move();
            _move temp;
            int x1, y1;

            for (int i = 0; i < 4; i++)
            {
                x1 = (i % 2 == 0) ? (-1) : (1);
                y1 = (i < 2) ? (-1) : (1);

                if (x + 2 * x1 >= 0 && x + 2 * x1 <= 7 && y + 2 * y1 >= 0 && y + 2 * y1 <= 7)
                    if ((board[x + 2 * x1, y + 2 * y1] & 1) == 0)
                        if ((board[x + x1, y + y1] & 1) == 1 && (board[x + x1, y + y1] & 4) == enemy_color && (board[x + x1, y + y1] & 8) == 0)
                        {
                            board[x + x1, y + y1] |= 8;
                            temp = AttackPossibilityP(x + 2 * x1, y + 2 * y1, enemy_color);
                            
                            if (temp.value > max.value)
                            {
                                max.value = temp.value + 1;
                                max.x = x + 2 * x1;
                                max.y = y + 2 * y1;
                                max.x0 = x;
                                max.y0 = y;
                                max.x_atk = x + x1;
                                max.y_atk = y + y1;
                                if(temp.value > 0) max.next_move[0] = temp;
                                max.move_count = 1;
                            } else if(temp.value == max.value)
                            {
                                max.value = temp.value + 1;
                                max.x = x + 2 * x1;
                                max.y = y + 2 * y1;
                                max.x0 = x;
                                max.y0 = y;
                                max.x_atk = x + x1;
                                max.y_atk = y + y1;
                                if (temp.value > 0) max.next_move[max.move_count++] = temp;
                            }


                            board[x + x1, y + y1] ^= 8;
                        }
            }
            return max;
        }


        // Funkcja ta sprawdza oblicza maksymalną możliwość bić pionka o danym kolorze na danym polu
        private _move AttackPossibilityQ(int x, int y, int enemy_color)
        {
            _move temp = new _move();
            _move result = new _move();
            int x1, y1, xx, yy, x2, y2;
            bool attacked = false;

            xx = x;
            yy = y;

            for (int i = 0; i < 4; i++)
            {
                x1 = (i % 2 == 0) ? (-1) : (1);
                y1 = (i < 2) ? (-1) : (1);
                attacked = false;

                x += x1;
                y += y1;
                while (x >= 0 && y >= 0 && x <= 7 && y <= 7)
                {
                    // Wykryto pionek należący do atakującego gracza
                    if ((board[x, y] & 4) != enemy_color && (board[x, y] & 1) == 1)
                        break;
                    // Wykryto pionek zbity w poprzednim pozornym ruchu
                    if ((board[x, y] & 8) == 8)
                        break;
                    if ((board[x, y] & 4) == enemy_color && (board[x, y] & 1) == 1)
                    {
                        // Wykryto pionek przeciwnika

                        // Sprawdzam, czy to nie jest kolejny pionek do zbicia
                        if (attacked)
                            break;
                        attacked = true;
                        x2 = x;
                        y2 = y;
                        board[x2, y2] |= 8;
                        x += x1;
                        y += y1;
                        if (x >= 0 && y >= 0 && x <= 7 && y <= 7)
                        {
                            // Jest przynajmniej jedno pole za tym pionkiem
                            while (x >= 0 && y >= 0 && x <= 7 && y <= 7)
                            {
                                if ((board[x, y] & 1) == 0)
                                {
                                    // Jest to wolne pole, więc można spokojnie zaatakować

                                    temp = AttackPossibilityQ(x, y, enemy_color);
                                    if (temp.value > result.value)
                                    {
                                        result.value = temp.value + 1;
                                        result.x = x;
                                        result.y = y;
                                        result.x0 = xx;
                                        result.y0 = yy;
                                        result.x_atk = x2;
                                        result.y_atk = y2;
                                        if (temp.value > 0) result.next_move[0] = temp;
                                        result.move_count = 1;
                                    } else if(temp.value == result.value)
                                    {
                                        result.value = temp.value + 1;
                                        result.x = x;
                                        result.y = y;
                                        result.x0 = xx;
                                        result.y0 = yy;
                                        result.x_atk = x2;
                                        result.y_atk = y2;
                                        if (temp.value > 0) result.next_move[result.move_count++] = temp;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                                x += x1;
                                y += y1;
                            }
                        }
                        board[x2, y2] ^= 8;
                    }
                    x += x1;
                    y += y1;
                }
                x = xx;
                y = yy;
            }
            return result;
        }

        bool is_nextmove = false;
        _move next_move;

        public Move MyTurn(Move old_move)
        {
            Move my_move;
            if (old_move != null) my_move = old_move;
            else my_move = new Move();

            if(is_nextmove)
            {
                my_move.x0 = next_move.x0;
                my_move.y0 = next_move.y0;
                my_move.x = next_move.x;
                my_move.y = next_move.y;
                my_move.color = ((board[my_move.x0, my_move.y0] & 2) == 0) ? (-1):(-2);
                if (next_move.move_count == 0)
                {
                    is_nextmove = false;
                }
                else next_move = next_move.next_move[0];
            }
            else
            {

                _move result = Brain(0, my_color);

                if (result.move_count != 0)
                {
                    next_move = result.next_move[0];
                    is_nextmove = true;
                }
                my_move.x0 = result.x0;
                my_move.y0 = result.y0;
                my_move.x = result.x;
                my_move.y = result.y;
                my_move.color = ((board[my_move.x0, my_move.y0] & 2) == 0) ? (-1) : (-2);
            }
            


            return my_move;
        }



        private _move MakeAttack(_move move, int color, int counter)
        {
            _move result = move;
            _move temp = new _move();
            int enemy_pawn_type, my_pawn_type;
            for(int i=0; i<move.move_count; i++)
            {
                enemy_pawn_type = board[move.x_atk, move.y_atk];
                board[move.x_atk, move.y_atk] = 0;
                my_pawn_type = board[move.x0, move.y0];
                board[move.x0, move.y0] = 0;
                board[move.x, move.y] = my_pawn_type;

                if (move.next_move[i].move_count == 0)
                {
                    temp = Brain(counter+1, color ^ 4);
                    if (temp.value >= result.value) result.value = temp.value + 1;
                }
                else
                {
                    temp = MakeAttack(move.next_move[i], color, counter);
                    if (temp.value >= result.value)
                        result.next_move[0] = temp;
                }

                board[move.x_atk, move.y_atk] = enemy_pawn_type;
                board[move.x0, move.y0] = my_pawn_type;
                board[move.x, move.y] = 0;

            }
            return result;
        }


        // Metoda rekurencyjna obliczająca wartość punktową dla danego pionka oraz zwracająca
        // teoretycznie najlepszy jego ruch

        private _move Brain(int counter, int color)
        {

            _move result = new _move();
            _move[] atk_result = new _move[5];
            _move temp = new _move();
            int atk_possibilities = 0;
            int this_pawn;
            int x1 = 0, y1 = 0;

            if (counter > 7)
                return result;

            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                {
                    if ((board[x, y] & 1) == 1 && (board[x, y] & 4) == color)
                    {
                        if ((board[x, y] & 2) == 0)
                            temp = AttackPossibilityP(x, y, (color ^ 4));
                        else temp = AttackPossibilityQ(x, y, (color ^ 4));
                    }
                    if (temp.value == 0) continue;
                    if (temp.value > result.value)
                    {
                        atk_result[0] = temp;
                        atk_possibilities = 1;
                    }
                    else if(temp.value == result.value)
                    {
                        atk_result[atk_possibilities] = temp;
                        atk_possibilities++;
                    }
                }


            if (atk_possibilities != 0)
            {
                for (int i = 0; i < atk_possibilities; i++)
                {
                    temp = MakeAttack(atk_result[i], color, counter);
                    if (temp.value >= result.value)
                        result = atk_result[i];
                }
                if (color != my_color)
                    result.value *= -1;
                result.value += temp.value;
            }
            else
            {
                // Wykonaj zwykły ruch

                temp = new _move();
                bool first_move = true;

                for (int y = 0; y < 8; y++)
                    for (int x = 0; x < 8; x++)
                    {
                        if((board[x, y] & 1) == 1 && (board[x, y] & 4) == color)
                        {
                            if ((board[x, y] & 2) == 0)
                            {
                                // Pionek
                                if (color == 0) y1 = 1;
                                else y1 = -1;
                                if (x - 1 >= 0 && y + y1 >= 0 && y + y1 <= 7)
                                {
                                    if (board[x - 1, y + y1] == 0)
                                    {
                                        // Wykonaj ruch

                                        this_pawn = board[x, y];
                                        board[x - 1, y + y1] = this_pawn;
                                        if ((y + y1 == 7 && y1 == 1) || (y + y1 == 0 && y1 == -1))
                                            board[x - 1, y + y1] |= 2;
                                        board[x, y] = 0;

                                        temp = Brain(counter + 1, color ^ 4);

                                        if ((color == my_color && temp.value >= result.value) || (color != my_color && temp.value <= result.value) || first_move)
                                        {
                                            first_move = false;
                                            result.value = temp.value;
                                            result.x = x - 1;
                                            result.y = y + y1;
                                            result.x0 = x;
                                            result.y0 = y;
                                        }

                                        board[x, y] = this_pawn;
                                        board[x - 1, y + y1] = 0;
                                    }
                                }
                                if (x + 1 <= 7 && y + y1 >= 0 && y + y1 <= 7)
                                {
                                    if (board[x + 1, y + y1] == 0)
                                    {
                                        // Wykonaj ruch
                                        this_pawn = board[x, y];
                                        board[x + 1, y + y1] = this_pawn;
                                        if ((y + y1 == 7 && y1 == 1) || (y + y1 == 0 && y1 == -1))
                                            board[x + 1, y + y1] |= 2;
                                        board[x, y] = 0;

                                        temp = Brain(counter + 1, color ^ 4);

                                        if ((color == my_color && temp.value >= result.value) || (color != my_color && temp.value <= result.value) || first_move)
                                        {
                                            first_move = false;
                                            result.value = temp.value;
                                            result.x = x + 1;
                                            result.y = y + y1;
                                            result.x0 = x;
                                            result.y0 = y;
                                        }

                                        board[x, y] = this_pawn;
                                        board[x + 1, y + y1] = 0;
                                    }
                                }
                            }
                            else
                            {
                                // Dama

                                this_pawn = board[x, y];

                                int xx = x;
                                int yy = y;

                                for (int i = 0; i < 4; i++)
                                {
                                    x1 = (i % 2 == 0) ? (-1) : (1);
                                    y1 = (i < 2) ? (-1) : (1);

                                    xx += x1;
                                    yy += y1;

                                    while (xx >= 0 && yy >= 0 && xx <= 7 && yy <= 7)
                                    {
                                        // Wyjdź z pętli, gdy napotkasz na pionek
                                        if (board[xx, yy] != 0)
                                            break;
                                        
                                        board[xx, yy] = this_pawn;
                                        board[x, y] = 0;

                                        temp = Brain(counter + 1, color ^ 4);

                                        if ((color == my_color && temp.value >= result.value) || (color != my_color && temp.value <= result.value) || first_move)
                                        {
                                            first_move = false;
                                            result.value = temp.value;
                                            result.x = xx;
                                            result.y = yy;
                                            result.x0 = x;
                                            result.y0 = y;
                                        }

                                        board[x, y] = this_pawn;
                                        board[xx, yy] = 0;

                                        xx += x1;
                                        yy += y1;

                                    }

                                    xx = x;
                                    yy = y;
                                }
                            }
                        }
                    }

            }

            return result;
        }

 

    }

    class _move
    {
        public int value = 0;
        public int x;
        public int y;
        public int x0;
        public int y0;
        public int x_atk = -1;
        public int y_atk = -1;
        public _move[] next_move = new _move[5];
        public int move_count = 0;
    }
}