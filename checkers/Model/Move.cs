using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warcaby
{
    class Move
    {
        public int x0, y0;
        public int x, y;
        public int x_atk, y_atk;            // wspolrzedne pionka, ktory wykonuje wiecej niż jedno bicie.
        public int[,] atk = new int[12, 2]; // wspolrzedne pionkow do skasowania
        public int attacked_pawns = 0;      // ilosc zaatakowanych pionkow
        public int color = 0;               // 0 - pusty, 1 - biały pion, 2 - biała dama, -1,-2 analogicznie do czarnego
        public int move_type = 0;           // 0 - brak typu, 1 - zwykły ruch, 2 - atak
        public bool make_queen = false;     // Czy ruch powoduje powstanie damy
        public bool next_attack = false;    // Czy możliwy jest kolejny atak
        public bool possible_move = true;   // Czy ruch jest poprawny
        
          
        public Move()
        {

        }
        public Move(int x0, int y0, int x, int y, int color)
        {
            this.x0 = x0;
            this.y0 = y0;
            this.x = x;
            this.y = y;
            this.color = color;
        }

        public void ChangeDst(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void ChangeSrc(int x0, int y0)
        {
            this.x0 = x0;
            this.y0 = y0;
        }

        public void ChangeClr(int color)
        {
            this.color = color;
        }

        public void AttackPosition(int x, int y)
        {
            atk[attacked_pawns, 0] = x;
            atk[attacked_pawns, 1] = y;
            attacked_pawns++;
        }

    }
}
