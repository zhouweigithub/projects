using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wzq_bll
{
    public class me
    {
        private game game;

        public me(game _game)
        {
            game = _game;
        }

        public bool go(int x, int y)
        {
            return game.go(x, y, -1);
        }
    }
}
