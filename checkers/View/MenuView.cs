using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warcaby
{
    class _MenuView
    {
        public void HideMenuPanel()
        {
            // Ukrywa panel zawierający menu.

            Program.Wnd.panel_menu.Visible = false;
        }
    }
}
