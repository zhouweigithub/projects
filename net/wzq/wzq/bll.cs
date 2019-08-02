using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wzq_bll;
using static wzq_bll.game;

namespace wzq
{
    public class bll
    {
        private game game;
        private me me;
        private ai ai;

        private readonly int hcount = 19, vcount = 19, startx = 21, starty = 25;
        private readonly float width = 27.83f, height = 26.83f;
        private bool isHumanGo = false;

        public bll()
        {
            game = new game(hcount, vcount);
            me = new me(game);
            ai = new ai(game);
            isHumanGo = true;
        }

        public (int x, int y) getPoint(int x, int y)
        {
            int xx = (int)((x - startx + width / 2) / width);
            int yy = (int)((y - starty + height / 2) / height);
            return (xx, yy);
        }

        public (int x, int y) getPicturePosition(int x, int y)
        {
            int xx = (int)(x * width + startx - width / 2);
            int yy = (int)(y * height + starty - height / 2);
            return (xx, yy);
        }

        public void reset()
        {
            game.reset();
            isHumanGo = true;
        }

        public Image refresh()
        {
            Image img = Properties.Resources.background;
            Graphics g = Graphics.FromImage(img);
            point[] points = game.gethistory();
            foreach (point item in points)
            {
                if (item.value != 0)
                {
                    DrawChess(item, g);
                }
            }
            g.Dispose();
            return img;
        }

        public Image humanGo(int x, int y, Image img)
        {
            if (isHumanGo)
            {
                (int px, int py) = getPoint(x, y);
                (int starth, int startv) = getPicturePosition(px, py);
                if (px >= 0 && py >= 0 && px < hcount && py < vcount)
                {
                    if (me.go(py, px))
                    {
                        Graphics g = Graphics.FromImage(img);
                        g.DrawImage(Properties.Resources.blackcur, new Rectangle(starth, startv, (int)width, (int)height));
                        isHumanGo = false;
                        ReDrawPreChess(g);
                        g.Dispose();
                    }
                }
                return img;
            }
            return null;
        }

        public (int px, int py, Image img) aiGo(Image img)
        {
            if (!isHumanGo)
            {
                Graphics g = Graphics.FromImage(img);
                (int px, int py) = ai.go();
                (int starth, int startv) = getPicturePosition(py, px);
                g.DrawImage(Properties.Resources.whitecur, new Rectangle(starth, startv, (int)width, (int)height));
                ReDrawPreChess(g);
                isHumanGo = true;
                g.Dispose();
                return (px, py, img);
            }
            return (-1, -1, null);
        }

        private void ReDrawPreChess(Graphics g)
        {
            point[] his = game.gethistory();
            if (his.Length < 2)
                return;

            point p = his[his.Length - 2];
            DrawChess(p, g);
        }

        private void DrawChess(point p, Graphics g)
        {
            Image imgchess = p.value == -1 ? Properties.Resources.black : Properties.Resources.white;
            (int starth, int startv) = getPicturePosition(p.y, p.x);
            g.DrawImage(imgchess, new Rectangle(starth, startv, (int)width, (int)height));
        }

        public Image regret()
        {
            game.reget();
            return refresh();
        }

        public bool iswin(int x, int y)
        {
            return game.iswin(x, y);
        }
    }
}
