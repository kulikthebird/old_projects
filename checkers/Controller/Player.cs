using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warcaby
{
    class Player
    {
        String name = "";
        int color;

        public Player(String n, int c)
        {
            name = n;
            color = c;
        }

        public String GetName()
        {
            return String.Copy(name);
        }

    }
}
