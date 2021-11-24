using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warcaby
{
    static class Main
    {
        private static int game_state = 0;         // Stan gry. 0 - menu gry, 1 - plansza
        
        //
        //  Obiekty Controller'a
        //

        public static _GameController GameController = null;
        public static _KeyboardController KeyboardController = null;
        public static _MenuController MenuController = null;
        public static _MouseController MouseController = null;

        //
        //  Obiekty Model'u
        //

        public static _AIModel AIModel = null;
        public static _AIModel AIModel2 = null;
        public static _BoardModel BoardModel = null;
      
        //
        //  Obiekty View
        //

        public static _BoardView BoardView = null;
        public static _MenuView MenuView = null;

        public static void start()
        {
            GameController = new _GameController();
            KeyboardController = new _KeyboardController();
            MenuController = new _MenuController();
            MouseController = new _MouseController();

            AIModel = new _AIModel();
            AIModel2 = new _AIModel();
            BoardModel = new _BoardModel();

            BoardView = new _BoardView();
            MenuView = new _MenuView();

        }
    }
}
