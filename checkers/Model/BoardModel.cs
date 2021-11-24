using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warcaby
{
    class _BoardModel
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
        //

        public _BoardModel()
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
            //board[0, 7] = 7;
            //board[3, 4] = 1;
            //board[4, 1] = 1;
        }

        public bool CheckMove(Move move)
        {
            bool is_attacking = false;

            int enemy_color = (move.color < 0) ? 4 : 0;     // Kolor pionów przeciwnika
            int color = (move.color > 0) ? 4 : 0;           // Kolor pionów gracza
            int xa = 0, ya = 0;                             // Wspolrzedne atakowanego pionka

            //
            //
            // Sprawdzanie czy ruch jest możliwy
            //
            //
            
            // Kontrola pola docelowego (czy jest puste)
            if((board[move.x,move.y] & 1) != 0)
                return false;

            // Ruch po skosie
            if (Math.Abs(move.x - move.x0) != Math.Abs(move.y - move.y0))
                return false;

            // Sprawdzam, czy pionek nie kontynuuje swojego bicia
            if(move.next_attack)
            {
                if (move.x_atk != move.x0 || move.y_atk != move.y0)
                    return false;
            }


            // Sprawdzanie poprawności zasięgu ruchu
            if((board[move.x0, move.y0] & 2) == 0)
            {
                // Zwykły pionek
                int distance =  Math.Abs(move.x - move.x0) + Math.Abs(move.y - move.y0);
                if (distance == 2)
                {
                    // Zwykły ruch piona
                    is_attacking = false;
                    // Wykluczenie cofania się pionka
                    if((board[move.x0, move.y0] & 4) == 0)
                    {
                        // pionek czarny
                        if (move.y < move.y0)
                            return false;
                    }
                    else
                    {
                        // pionek biały
                        if (move.y > move.y0)
                            return false;
                    }
                }
                else if (distance == 4)
                {
                    // Atak pionka
                    
                    xa = (move.x + move.x0) / 2;
                    ya = (move.y + move.y0) / 2;
                    if ((board[xa, ya] & 1) == 0 || ((board[xa, ya] & 4) != enemy_color ) || (board[xa, ya] & 8) == 8)
                        return false;
                    is_attacking = true;

                }
                // Ruch niedozwolony dla pionka (za duży zasięg)
                else return false;
            }
            else
            {
                // Dama
                
                // Ustalenie kierunku ruchu damy
                // Sprawdzanie poprawności ruchu oraz czy dama atakuje przeciwnika
                int xx = move.x0, yy = move.y0; // Początkowe współrzędne damy
                int axis_x = 0, axis_y = 0;     // Kierunek poruszania się po osiach (ustalany później)
                if( move.x0 < move.x )
                {
                    if( move.y0 < move.y )
                    {
                        // Cwiartka I
                        axis_x = 1;
                        axis_y = 1;
                    }
                    else
                    {
                        // Cwiartka IV
                        axis_x = 1;
                        axis_y = -1;
                    }
                }
                else
                {
                    if (move.y0 < move.y)
                    {
                        // Cwiartka II
                        axis_x = -1;
                        axis_y = 1;
                    }
                    else
                    {
                        // Cwiartka III
                        axis_x = -1;
                        axis_y = -1;
                    }
                }

                xx += axis_x;   // Wykonanie pierwszego ruchu w kierunku docelowym
                yy += axis_y;   //

                while (true)
                {
                    if (!(xx <= 7 && xx >= 0 && yy <= 7 && yy >= 0))
                        return false;
                    if ((board[xx, yy] & 1) == 1 && (board[xx, yy] & 4) == enemy_color && (board[xx, yy] & 8) == 0)
                    {
                        if (is_attacking) return false;   // Nie można bić dwóch pionków, kolejne pola muszą być wolne
                        is_attacking = true;
                        xa = xx;
                        ya = yy;
                    }
                    else if ((board[xx, yy] & 1) == 0)
                    {
                        if (xx == move.x && yy == move.y) break;
                    }
                    // Niedozwolony ruch damą: zbicie większej ilości pionów jednym ruchem,
                    // próba zbicia dwóch pionów w odstępie jednego pola lub próba zbicia piona
                    // tego samego koloru
                    else return false;

                    xx += axis_x;
                    yy += axis_y;
                }

            }

            // Sprawdzanie bicia

            int max = 0;
            int temp = 0;
            int current_pawn = ((board[move.x0, move.y0] & 2) == 0)? AttackPossibilityP(move.x0, move.y0, enemy_color): AttackPossibilityQ(move.x0, move.y0, enemy_color);
            max = current_pawn;

            //
            if (current_pawn != 0 ^ is_attacking)
                return false;

            if (!move.next_attack)
            {
                for (int y = 0; y < 8; y++)
                    for (int x = 0; x < 8; x++)
                    {
                        if ((board[x, y] & 1) == 1 && (board[x, y] & 4) == color)
                            if ((board[x, y] & 2) == 0)
                                temp = AttackPossibilityP(x, y, enemy_color);
                            else temp = AttackPossibilityQ(x, y, enemy_color);
                        if (temp > max) max = temp;
                    }
                // Sprawdza, czy pionek wybrany do ruchu może zbić maksymalną ilość pionów
                if (max > current_pawn)
                    return false;
            }
            temp = 0;


            // Sprawdza czy atak, który wykonuje grasz jest najskuteczniejszy (wg zasady najkorzystniejszego bicia)
            if (is_attacking)
            {
                board[xa, ya] |= 8;
                int temporary_current_pawn = board[move.x0, move.y0];
                board[move.x0, move.y0] = 0;
                temp = (temporary_current_pawn & 2) == 0 ? AttackPossibilityP(move.x, move.y, enemy_color) + 1 : AttackPossibilityQ(move.x, move.y, enemy_color) + 1;
                board[xa, ya] ^= 8;
                board[move.x0, move.y0] = temporary_current_pawn;
                if (temp != max)
                    return false;
                if (temp > 1)
                {
                    move.next_attack = true;
                    move.x_atk = move.x;
                    move.y_atk = move.y;
                }
                else move.next_attack = false;
            }
            else move.next_attack = false;



            // Dodawanie damek
            if (((move.y == 0 && move.color > 0) || (move.y == 7 && move.color < 0)) && !move.next_attack)
            {
                if ((board[move.x0, move.y0] & 2) != 2) move.make_queen = true;
            }
            move.move_type = is_attacking?2:1;
            if(is_attacking) move.AttackPosition(xa, ya);
            return true;
        }


        // Funkcja sprawdzająca atak, którego wynikiem będzie największa ilość zbić pionków przeciwnika
        // Wersja dla pionka
        private int AttackPossibilityP(int x, int y, int enemy_color)
        {
            int max = 0, temp = 0;
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
                            temp = AttackPossibilityP(x + 2 * x1, y + 2 * y1, enemy_color) + 1;
                            if (temp > max) max = temp;
                            board[x + x1, y + y1] ^= 8;
                        }
            }
            return max;
        }

        
        // Funkcja ta sprawdza oblicza maksymalną możliwość bić pionka o danym kolorze na danym polu
        private int AttackPossibilityQ(int x, int y, int enemy_color)
        {
            int temp = 0, max = 0;
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

                                    temp = AttackPossibilityQ(x, y, enemy_color) + 1;
                                    if (temp > max) max = temp;
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
            return max;
        }


        public void MakeMove(Move move)
        {
            board[move.x, move.y] = board[move.x0, move.y0];
            board[move.x0, move.y0] = 0;
            if(move.move_type == 2)
            {
                board[move.atk[move.attacked_pawns - 1, 0], move.atk[move.attacked_pawns - 1, 1]] |= 8;
                if(!move.next_attack)
                {
                    for(int i = 0; i<move.attacked_pawns; i++)
                    {
                        board[move.atk[i, 0], move.atk[i, 1]] = 0;
                    }
                }
            }

            // Dodawanie damek
            if (move.make_queen)
            {
                board[move.x, move.y] |= 2;
                move.color = (board[move.x, move.y] & 4) == 4 ? 2 : (-2);
            }
        }


    }
}
