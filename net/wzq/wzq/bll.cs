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
        private bool isPlaying = true;
        private readonly int hcount = 19, vcount = 19, startx = 21, starty = 25;
        private readonly float width = 27.83f, height = 26.83f;

        /// <summary>
        /// 是否托管给电脑走棋
        /// </summary>
        public bool IsAutoPlaying { get; set; } = false;
        public bool IsHumanGo { get; set; } = false;

        public bll()
        {
            game = new game(hcount, vcount);
            me = new me(game);
            ai = new ai(game);
            IsHumanGo = true;
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
            IsHumanGo = true;
            isPlaying = true;
            IsAutoPlaying = false;
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
            if (points.Length > 0)
                DrawChessCur(points[points.Length - 1], g);

            g.Dispose();
            return img;
        }

        public Image humanGo(int x, int y, Image img)
        {
            if (IsHumanGo)
            {
                (int px, int py) = getPoint(x, y);
                (int starth, int startv) = getPicturePosition(px, py);
                if (px >= 0 && py >= 0 && px < hcount && py < vcount)
                {
                    if (me.go(py, px))
                    {
                        Graphics g = Graphics.FromImage(img);
                        g.DrawImage(Properties.Resources.blackcur, new Rectangle(starth, startv, (int)width, (int)height));
                        IsHumanGo = false;
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
            int px, py;
            int value = IsHumanGo ? -1 : 1;

            Graphics g = Graphics.FromImage(img);
            if (game.gethistory().Length == 0)
            {
                px = 9;
                py = 9;
                ai.go(px, py, value);
            }
            else
            {
                (px, py) = ai.go(value);
            }

            (int starth, int startv) = getPicturePosition(py, px);
            Image imgChess = value == -1 ? Properties.Resources.blackcur : Properties.Resources.whitecur;
            g.DrawImage(imgChess, new Rectangle(starth, startv, (int)width, (int)height));
            ReDrawPreChess(g);
            IsHumanGo = !IsHumanGo;
            g.Dispose();
            return (px, py, img);
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

        private void DrawChessCur(point p, Graphics g)
        {
            Image imgchess = p.value == -1 ? Properties.Resources.blackcur : Properties.Resources.whitecur;
            (int starth, int startv) = getPicturePosition(p.y, p.x);
            g.DrawImage(imgchess, new Rectangle(starth, startv, (int)width, (int)height));
        }

        public Image regret()
        {
            IsHumanGo = true;
            IsAutoPlaying = false;
            game.reget();
            return refresh();
        }

        public bool iswin(int x, int y)
        {
            bool iswin = game.iswin(x, y);
            isPlaying = iswin ? false : true;
            return iswin;
        }
    }
}
