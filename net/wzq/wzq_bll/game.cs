using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wzq_bll
{
    public class game
    {
        private int width;
        private int height;
        private int[,] chess;
        private int[,] chess_copy;
        private int[,] score_human;
        private int[,] score_computer;
        private List<point> history = new List<point>();
        //computer vlaue=1 human=-1

        public game(int width, int height)
        {
            this.width = width;
            this.height = height;
            init(width, height);
        }

        public int gethumanscore(int x, int y)
        {
            return score_human[x, y];
        }

        private void init(int width, int height)
        {
            chess = new int[width, height];
            chess_copy = new int[width, height];
            score_human = new int[width, height];
            score_computer = new int[width, height];
        }

        public point[] gethistory()
        {
            point[] result = new point[history.Count];
            history.CopyTo(result);
            return result;
        }

        public void reset()
        {
            init(width, height);
            history.Clear();
        }

        public bool go(int x, int y, int value)
        {
            if (chess[x, y] == 0)
            {
                chess[x, y] = value;
                history.Add(new point(x, y, value));
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool iswin(int x, int y)
        {
            int value = chess[x, y];
            return getMaxContinueCount(x, y, value) >= 5;
        }

        public void reget()
        {
            if (history.Count == 0)
                return;

            point p = history[history.Count - 1];
            chess[p.x, p.y] = 0;
            history.RemoveAt(history.Count - 1);

            p = history[history.Count - 1];
            chess[p.x, p.y] = 0;
            history.RemoveAt(history.Count - 1);
        }

        //public point getBestPosition(int value)
        //{

        //}

        private void copychess()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    chess_copy[i, j] = chess[i, j];
                }
            }
        }

        public void refreshscore()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (chess[i, j] == 0)
                    {
                        score_human[i, j] = getscore(i, j, -1);
                        score_computer[i, j] = getscore(i, j, 1);
                    }
                    else
                    {
                        score_human[i, j] = 0;
                        score_computer[i, j] = 0;
                    }
                }
            }
            Console.WriteLine("human");
            Console.WriteLine(getchessstring(score_human));
            Console.WriteLine("computer");
            Console.WriteLine(getchessstring(score_computer));

        }

        private int getscore(int x, int y, int value)
        {
            int left = 0, right = 0, top = 0, bottom = 0, lefttop = 0, righttop = 0, leftbottom = 0, rightbottom = 0;
            chesstype t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16;
            (t1, t2, left) = getContinueCount(x, y, direct.left, value);
            (t3, t4, right) = getContinueCount(x, y, direct.right, value);
            (t5, t6, top) = getContinueCount(x, y, direct.top, value);
            (t7, t8, bottom) = getContinueCount(x, y, direct.bottom, value);
            (t9, t10, lefttop) = getContinueCount(x, y, direct.lefttop, value);
            (t11, t12, leftbottom) = getContinueCount(x, y, direct.leftbottom, value);
            (t13, t14, righttop) = getContinueCount(x, y, direct.righttop, value);
            (t15, t16, rightbottom) = getContinueCount(x, y, direct.rightbottom, value);

            int hor, ver, lefttilt, righttilt;
            hor = left + right + 1;
            ver = top + bottom + 1;
            lefttilt = lefttop + rightbottom + 1;
            righttilt = righttop + leftbottom + 1;

            int horscort = getscort(hor, t1, t2, t3, t4);
            int verscort = getscort(ver, t5, t6, t7, t8);
            int lefttiltscort = getscort(lefttilt, t9, t10, t11, t12);
            int righttiltscort = getscort(righttilt, t13, t14, t15, t16);

            return horscort + verscort + lefttiltscort + righttiltscort;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count">连子数量</param>
        /// <param name="p1">连子外左或上最近第1个</param>
        /// <param name="p2">连子外左或上最近第2个</param>
        /// <param name="a1">连子外右或下最近第1个</param>
        /// <param name="a2">连子外右或下最近第2个</param>
        /// <returns></returns>
        private int getscort(int count, chesstype p1, chesstype p2, chesstype a1, chesstype a2)
        {
            if (count >= 5)
                return 100000;   //满五

            if (count == 4 && p1 == chesstype.empty && a1 == chesstype.empty)
                return 10000;    //活四

            if (count == 4 && (p1 == chesstype.empty || a1 == chesstype.empty))
                return 2000;    //冲四

            if (count == 3 && p1 == chesstype.empty && a1 == chesstype.empty)
                return 2000;    //活三

            if (count == 3 && (p1 == chesstype.empty || a1 == chesstype.empty))
                return 400;    //冲三

            if (count == 2 && p1 == chesstype.empty && a1 == chesstype.empty)
                return 200;    //活二

            if (count == 2 && (p1 == chesstype.empty || a1 == chesstype.empty))
                return 50;    //冲二

            if (count == 1 && p1 == chesstype.empty && a1 == chesstype.empty)
                return 10;    //活一

            if (count == 1 && (p1 == chesstype.empty || a1 == chesstype.empty))
                return 2;    //冲一

            return 0;
        }

        private (chesstype t1, chesstype t2, int count) getContinueCount(int x, int y, direct d, int value)
        {
            int count = 0;
            chesstype t1 = chesstype.empty;
            chesstype t2 = chesstype.empty;
            int q1, q2;
            switch (d)
            {
                case direct.left:
                    while (x != 0 && chess[x - 1, y] == value)
                    {
                        x--;
                        count++;
                    }
                    q1 = x - 1;
                    if (q1 >= 0)
                        t1 = chess[q1, y] == 0 ? chesstype.empty : chess[q1, y] == value ? chesstype.self : chesstype.enemy;
                    else
                        t1 = chesstype.border;

                    q2 = q1 - 1;
                    if (q2 >= 0)
                        t2 = chess[q2, y] == 0 ? chesstype.empty : chess[q2, y] == value ? chesstype.self : chesstype.enemy;
                    else
                        t2 = chesstype.border;
                    break;
                case direct.right:
                    while (x != width - 1 && chess[x + 1, y] == value)
                    {
                        x++;
                        count++;
                    }
                    q1 = x + 1;
                    if (q1 < width)
                        t1 = chess[q1, y] == 0 ? chesstype.empty : chess[q1, y] == value ? chesstype.self : chesstype.enemy;
                    else
                        t1 = chesstype.border;

                    q2 = q1 + 1;
                    if (q2 < width)
                        t2 = chess[q2, y] == 0 ? chesstype.empty : chess[q2, y] == value ? chesstype.self : chesstype.enemy;
                    else
                        t2 = chesstype.border;
                    break;
                case direct.top:
                    while (y != 0 && chess[x, y - 1] == value)
                    {
                        y--;
                        count++;
                    }
                    q1 = y - 1;
                    if (q1 >= 0)
                        t1 = chess[x, q1] == 0 ? chesstype.empty : chess[x, q1] == value ? chesstype.self : chesstype.enemy;
                    else
                        t1 = chesstype.border;

                    q2 = q1 - 1;
                    if (q2 >= 0)
                        t2 = chess[x, q2] == 0 ? chesstype.empty : chess[x, q2] == value ? chesstype.self : chesstype.enemy;
                    else
                        t2 = chesstype.border;
                    break;
                case direct.bottom:
                    while (y != height - 1 && chess[x, y + 1] == value)
                    {
                        y++;
                        count++;
                    }
                    q1 = y + 1;
                    if (q1 < height)
                        t1 = chess[x, q1] == 0 ? chesstype.empty : chess[x, q1] == value ? chesstype.self : chesstype.enemy;
                    else
                        t1 = chesstype.border;

                    q2 = q1 + 1;
                    if (q2 < height)
                        t2 = chess[x, q2] == 0 ? chesstype.empty : chess[x, q2] == value ? chesstype.self : chesstype.enemy;
                    else
                        t2 = chesstype.border;
                    break;
                case direct.lefttop:
                    while (x != 0 && y != 0 && chess[x - 1, y - 1] == value)
                    {
                        x--;
                        y--;
                        count++;
                    }
                    q1 = x - 1;
                    q2 = y - 1;
                    if (q1 >= 0 && q2 >= 0)
                        t1 = chess[q1, q2] == 0 ? chesstype.empty : chess[q1, q2] == value ? chesstype.self : chesstype.enemy;
                    else
                        t1 = chesstype.border;

                    q1 = q1 - 1;
                    q2 = q2 - 1;
                    if (q1 >= 0 && q2 >= 0)
                        t2 = chess[q1, q2] == 0 ? chesstype.empty : chess[q1, q2] == value ? chesstype.self : chesstype.enemy;
                    else
                        t2 = chesstype.border;
                    break;
                case direct.righttop:
                    while (x != width - 1 && y != 0 && chess[x + 1, y - 1] == value)
                    {
                        x++;
                        y--;
                        count++;
                    }
                    q1 = x + 1;
                    q2 = y - 1;
                    if (q1 < width && q2 >= 0)
                        t1 = chess[q1, q2] == 0 ? chesstype.empty : chess[q1, q2] == value ? chesstype.self : chesstype.enemy;
                    else
                        t1 = chesstype.border;

                    q1 = q1 + 1;
                    q2 = q2 - 1;
                    if (q1 < width && q2 >= 0)
                        t2 = chess[q1, q2] == 0 ? chesstype.empty : chess[q1, q2] == value ? chesstype.self : chesstype.enemy;
                    else
                        t2 = chesstype.border;
                    break;
                case direct.leftbottom:
                    while (x != 0 && y != height - 1 && chess[x - 1, y + 1] == value)
                    {
                        x--;
                        y++;
                        count++;
                    }
                    q1 = x - 1;
                    q2 = y + 1;
                    if (q1 >= 0 && q2 < height)
                        t1 = chess[q1, q2] == 0 ? chesstype.empty : chess[q1, q2] == value ? chesstype.self : chesstype.enemy;
                    else
                        t1 = chesstype.border;

                    q1 = q1 - 1;
                    q2 = q2 + 1;
                    if (q1 >= 0 && q2 < height)
                        t2 = chess[q1, q2] == 0 ? chesstype.empty : chess[q1, q2] == value ? chesstype.self : chesstype.enemy;
                    else
                        t2 = chesstype.border;
                    break;
                case direct.rightbottom:
                    while (x != width - 1 && y != height - 1 && chess[x + 1, y + 1] == value)
                    {
                        x++;
                        y++;
                        count++;
                    }
                    q1 = x + 1;
                    q2 = y + 1;
                    if (q1 < width && q2 < height)
                        t1 = chess[q1, q2] == 0 ? chesstype.empty : chess[q1, q2] == value ? chesstype.self : chesstype.enemy;
                    else
                        t1 = chesstype.border;

                    q1 = q1 + 1;
                    q2 = q2 + 1;
                    if (q1 < width && q2 < height)
                        t2 = chess[q1, q2] == 0 ? chesstype.empty : chess[q1, q2] == value ? chesstype.self : chesstype.enemy;
                    else
                        t2 = chesstype.border;
                    break;
                default:
                    break;
            }

            return (t1, t2, count);
        }

        private int getMaxContinueCount(int x, int y, int value)
        {
            int hor, ver, lefttilt, righttilt;
            int left = 0, right = 0, top = 0, bottom = 0, lefttop = 0, righttop = 0, leftbottom = 0, rightbottom = 0;
            getContinueCount(x, y, direct.left, value, ref left);
            getContinueCount(x, y, direct.right, value, ref right);
            getContinueCount(x, y, direct.top, value, ref top);
            getContinueCount(x, y, direct.bottom, value, ref bottom);
            getContinueCount(x, y, direct.lefttop, value, ref lefttop);
            getContinueCount(x, y, direct.righttop, value, ref righttop);
            getContinueCount(x, y, direct.leftbottom, value, ref leftbottom);
            getContinueCount(x, y, direct.rightbottom, value, ref rightbottom);

            hor = left + right + 1;
            ver = top + bottom + 1;
            lefttilt = lefttop + rightbottom + 1;
            righttilt = righttop + leftbottom + 1;

            return Math.Max(Math.Max(hor, ver), Math.Max(lefttilt, righttilt));
        }

        private void getContinueCount(int x, int y, direct d, int value, ref int count)
        {
            switch (d)
            {
                case direct.left:
                    if (x == 0)
                        break;
                    if (chess[x - 1, y] == value)
                    {
                        count++;
                        getContinueCount(x - 1, y, d, value, ref count);
                    }
                    break;
                case direct.right:
                    if (x == width - 1)
                        break;
                    if (chess[x + 1, y] == value)
                    {
                        count++;
                        getContinueCount(x + 1, y, d, value, ref count);
                    }
                    break;
                case direct.top:
                    if (y == 0)
                        break;
                    if (chess[x, y - 1] == value)
                    {
                        count++;
                        getContinueCount(x, y - 1, d, value, ref count);
                    }
                    break;
                case direct.bottom:
                    if (y == height - 1)
                        break;
                    if (chess[x, y + 1] == value)
                    {
                        count++;
                        getContinueCount(x, y + 1, d, value, ref count);
                    }
                    break;
                case direct.lefttop:
                    if (x == 0 || y == 0)
                        break;
                    if (chess[x - 1, y - 1] == value)
                    {
                        count++;
                        getContinueCount(x - 1, y - 1, d, value, ref count);
                    }
                    break;
                case direct.righttop:
                    if (x == width - 1 || y == 0)
                        break;
                    if (chess[x + 1, y - 1] == value)
                    {
                        count++;
                        getContinueCount(x + 1, y - 1, d, value, ref count);
                    }
                    break;
                case direct.leftbottom:
                    if (x == 0 || y == height - 1)
                        break;
                    if (chess[x - 1, y + 1] == value)
                    {
                        count++;
                        getContinueCount(x - 1, y + 1, d, value, ref count);
                    }
                    break;
                case direct.rightbottom:
                    if (x == width - 1 || y == height - 1)
                        break;
                    if (chess[x + 1, y + 1] == value)
                    {
                        count++;
                        getContinueCount(x + 1, y + 1, d, value, ref count);
                    }
                    break;
                default:
                    break;
            }
        }

        public List<scoreinfo> getMaxScoreInfos(int flag)
        {
            int maxscore = getMaxScore(flag);
            var array = flag == 0 ? score_human : score_computer;
            List<scoreinfo> result = new List<scoreinfo>();
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (array[i, j] == maxscore)
                    {
                        result.Add(new scoreinfo(i, j, maxscore));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 获取得分最大的位置信息
        /// </summary>
        /// <param name="flag">0玩家 1电脑</param>
        /// <returns></returns>
        private int getMaxScore(int flag)
        {
            int max = 0;
            var array = flag == 0 ? score_human : score_computer;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (array[i, j] > max)
                    {
                        max = array[i, j];
                    }
                }
            }

            return max;
        }

        private string getchessstring(int[,] array)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    sb.AppendFormat("[{0},{1}] {2}\t", i, j, array[i, j]);
                }
                sb.Append("\n");
            }
            return sb.ToString();
        }

        public class point
        {
            public int x;
            public int y;
            public int value;

            public point(int _x, int _y, int _value)
            {
                x = _x;
                y = _y;
                value = _value;
            }
        }

        private enum direct
        {
            left,
            right,
            top,
            bottom,
            lefttop,
            righttop,
            leftbottom,
            rightbottom,
        }

        public enum chesstype
        {
            empty,
            self,
            enemy,
            border,
        }


    }
    public class scoreinfo
    {
        public int score;
        public int x;
        public int y;

        public scoreinfo(int _x, int _y, int _score)
        {
            x = _x;
            y = _y;
            score = _score;
        }

    }
}
