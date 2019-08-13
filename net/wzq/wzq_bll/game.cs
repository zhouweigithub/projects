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

        public int getcomputerscore(int x, int y)
        {
            return score_computer[x, y];
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
            do
            {
                chess[p.x, p.y] = 0;
                history.RemoveAt(history.Count - 1);
                if (history.Count > 0)
                    p = history[history.Count - 1];
                else
                    p = null;
            } while (p != null && p.value != 1);
        }

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

            //Console.WriteLine("human");
            //Console.WriteLine(getchessstring(score_human));
            //Console.WriteLine("computer");
            //Console.WriteLine(getchessstring(score_computer));
        }

        private int getscore(int x, int y, int value)
        {
            bool hasEmptyChess1, hasEmptyChess2, hasEmptyChess3, hasEmptyChess4,
                hasEmptyChess5, hasEmptyChess6, hasEmptyChess7, hasEmptyChess8;
            int left = 0, right = 0, top = 0, bottom = 0, lefttop = 0, righttop = 0, leftbottom = 0, rightbottom = 0;
            chesstype t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16;
            (t1, t2, left, hasEmptyChess1) = getContinueCount(x, y, direct.left, value);
            (t3, t4, right, hasEmptyChess2) = getContinueCount(x, y, direct.right, value);
            (t5, t6, top, hasEmptyChess3) = getContinueCount(x, y, direct.top, value);
            (t7, t8, bottom, hasEmptyChess4) = getContinueCount(x, y, direct.bottom, value);
            (t9, t10, lefttop, hasEmptyChess5) = getContinueCount(x, y, direct.lefttop, value);
            (t11, t12, leftbottom, hasEmptyChess6) = getContinueCount(x, y, direct.leftbottom, value);
            (t13, t14, righttop, hasEmptyChess7) = getContinueCount(x, y, direct.righttop, value);
            (t15, t16, rightbottom, hasEmptyChess8) = getContinueCount(x, y, direct.rightbottom, value);

            int hor, ver, lefttilt, righttilt;
            hor = left + right + (hasEmptyChess1 && hasEmptyChess2 ? 0 : 1);
            ver = top + bottom + (hasEmptyChess3 && hasEmptyChess4 ? 0 : 1);
            lefttilt = lefttop + rightbottom + (hasEmptyChess5 && hasEmptyChess8 ? 0 : 1);
            righttilt = righttop + leftbottom + (hasEmptyChess6 && hasEmptyChess7 ? 0 : 1);

            //水平方面是否靠近棋子较多那一侧
            bool isNearMoreHor = GetIsNearMore(left, right, hasEmptyChess1, hasEmptyChess2);
            bool isNearMoreVer = GetIsNearMore(top, bottom, hasEmptyChess3, hasEmptyChess4);
            bool isNearMoreLefttilt = GetIsNearMore(lefttop, rightbottom, hasEmptyChess5, hasEmptyChess8);
            bool isNearMoreRighttilt = GetIsNearMore(righttop, leftbottom, hasEmptyChess6, hasEmptyChess7);

            int horscort = getscore(hor, isNearMoreHor, t1, t2, t3, t4, hasEmptyChess1 || hasEmptyChess2);
            int verscort = getscore(ver, isNearMoreVer, t5, t6, t7, t8, hasEmptyChess3 || hasEmptyChess4);
            int lefttiltscort = getscore(lefttilt, isNearMoreLefttilt, t9, t10, t15, t16, hasEmptyChess5 || hasEmptyChess8);
            int righttiltscort = getscore(righttilt, isNearMoreRighttilt, t11, t12, t13, t14, hasEmptyChess6 || hasEmptyChess7);

            return horscort + verscort + lefttiltscort + righttiltscort;
        }

        private bool GetIsNearMore(int count1, int count2, bool hasEmptyChess1, bool hasEmptyChess2)
        {
            if (count1 >= count2)
            {
                return !hasEmptyChess1;
            }
            else if (count2 >= count1)
            {
                return !hasEmptyChess2;
            }
            else
                return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count">连子数量</param>
        /// <param name="isMore">是否更靠近棋子更多那一侧</param>
        /// <param name="p1">连子外左或上最近第1个</param>
        /// <param name="p2">连子外左或上最近第2个</param>
        /// <param name="a1">连子外右或下最近第1个</param>
        /// <param name="a2">连子外右或下最近第2个</param>
        /// <returns></returns>
        private int getscore(int count, bool isMore, chesstype p1, chesstype p2, chesstype a1, chesstype a2, bool hasEmptyChess)
        {
            int result = 0;
            if (count >= 5)
            {
                result = 100000;   //满五
            }
            if (count == 4 && p1 == chesstype.empty && a1 == chesstype.empty)
            {
                result = 10000;    //活四
            }
            else if (count == 4 && (p1 == chesstype.empty || a1 == chesstype.empty))
            {
                result = 2200;    //冲四
            }
            else if (count == 3 && p1 == chesstype.empty && a1 == chesstype.empty)
            {
                result = 2000;    //活三
            }
            else if (count == 3 && (p1 == chesstype.empty || a1 == chesstype.empty) && p2 != chesstype.enemy && a2 != chesstype.enemy)
            {
                result = 400;    //冲三
            }
            else if (count == 2 && p1 == chesstype.empty && a1 == chesstype.empty && (p2 == chesstype.empty || a2 == chesstype.empty))
            {
                result = 200;    //活二
            }
            else if (count == 2 && (p1 == chesstype.empty || a1 == chesstype.empty))
            {
                result = 50;    //冲二
            }
            else if (count == 1 && p1 == chesstype.empty && a1 == chesstype.empty)
            {
                result = 10;    //活一
            }
            else if (count == 1 && (p1 == chesstype.empty || a1 == chesstype.empty))
            {
                result = 2;    //冲一
            }

            if (hasEmptyChess)
            {
                result -= (int)(result * 0.2);
                if (!isMore)    //如果含有空位，并且不是个数较多那一侧，则减少一定权重
                    result -= 1000;
            }

            return result;
        }

        private (chesstype t1, chesstype t2, int count, bool hasEmptyChess) getContinueCount(int x, int y, direct d, int value)
        {
            int count = 0;
            chesstype t1 = chesstype.empty;
            chesstype t2 = chesstype.empty;
            int q1, q2;
            int emptyCount = 0; //搜索到的空位置的数量
            bool hasEmptyChess = false; //搜索到的棋子中是否有空位置（允许出现一个空位置）
            switch (d)
            {
                case direct.left:
                    //搜索到边界或者空位置数量达到2个或者搜索到对方的棋子就停止搜索
                    while (x != 0 && emptyCount < 2 && chess[x - 1, y] != -value)
                    {
                        if (chess[x - 1, y] == 0)
                        {
                            emptyCount++;   //空位置数量+1
                            if (emptyCount < 2)
                                x--;    //只有空位置数量小于2个才继续向前搜索
                        }
                        else if (chess[x - 1, y] == value)
                        {   //自己的棋就继续搜索
                            x--;
                            count++;
                            if (emptyCount > 0)
                                hasEmptyChess = true;
                        }
                    }
                    q1 = chess[x, y] == 0 && emptyCount >= 1 ? x : x - 1;  //如果前一个位置是空位置，就将前一个位置算成第一个
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
                    while (x != width - 1 && emptyCount < 2 && chess[x + 1, y] != -value)
                    {
                        if (chess[x + 1, y] == 0)
                        {
                            emptyCount++;   //空位置数量+1
                            if (emptyCount < 2)
                                x++;    //只有空位置数量小于2个才继续向前搜索
                        }
                        else if (chess[x + 1, y] == value)
                        {   //自己的棋就继续搜索
                            x++;
                            count++;
                            if (emptyCount > 0)
                                hasEmptyChess = true;
                        }
                    }
                    q1 = chess[x, y] == 0 && emptyCount >= 1 ? x : x + 1;  //如果前一个位置是空位置，就将前一个位置算成第一个
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
                    while (y != 0 && emptyCount < 2 && chess[x, y - 1] != -value)
                    {
                        if (chess[x, y - 1] == 0)
                        {
                            emptyCount++;   //空位置数量+1
                            if (emptyCount < 2)
                                y--;    //只有空位置数量小于2个才继续向前搜索
                        }
                        else if (chess[x, y - 1] == value)
                        {   //自己的棋就继续搜索
                            y--;
                            count++;
                            if (emptyCount > 0)
                                hasEmptyChess = true;
                        }
                    }
                    q1 = chess[x, y] == 0 && emptyCount >= 1 ? y : y - 1;  //如果前一个位置是空位置，就将前一个位置算成第一个
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
                    while (y != height - 1 && emptyCount < 2 && chess[x, y + 1] != -value)
                    {
                        if (chess[x, y + 1] == 0)
                        {
                            emptyCount++;   //空位置数量+1
                            if (emptyCount < 2)
                                y++;    //只有空位置数量小于2个才继续向前搜索
                        }
                        else if (chess[x, y + 1] == value)
                        {   //自己的棋就继续搜索
                            y++;
                            count++;
                            if (emptyCount > 0)
                                hasEmptyChess = true;
                        }
                    }
                    q1 = chess[x, y] == 0 && emptyCount >= 1 ? y : y + 1;  //如果前一个位置是空位置，就将前一个位置算成第一个
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
                    while (x != 0 && y != 0 && emptyCount < 2 && chess[x - 1, y - 1] != -value)
                    {
                        if (chess[x - 1, y - 1] == 0)
                        {
                            emptyCount++;   //空位置数量+1
                            if (emptyCount < 2)
                            {
                                x--;
                                y--;    //只有空位置数量小于2个才继续向前搜索
                            }
                        }
                        else if (chess[x - 1, y - 1] == value)
                        {   //自己的棋就继续搜索
                            x--;
                            y--;
                            count++;
                            if (emptyCount > 0)
                                hasEmptyChess = true;
                        }
                    }

                    //如果前一个位置是空位置，就将前一个位置算成第一个
                    if (chess[x, y] == 0 && emptyCount >= 1)
                    {
                        q1 = x;
                        q2 = y;
                    }
                    else
                    {
                        q1 = x - 1;
                        q2 = y - 1;
                    }
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
                    while (x != width - 1 && y != 0 && emptyCount < 2 && chess[x + 1, y - 1] != -value)
                    {
                        if (chess[x + 1, y - 1] == 0)
                        {
                            emptyCount++;   //空位置数量+1
                            if (emptyCount < 2)
                            {
                                x++;
                                y--;    //只有空位置数量小于2个才继续向前搜索
                            }
                        }
                        else if (chess[x + 1, y - 1] == value)
                        {   //自己的棋就继续搜索
                            x++;
                            y--;
                            count++;
                            if (emptyCount > 0)
                                hasEmptyChess = true;
                        }
                    }

                    //如果前一个位置是空位置，就将前一个位置算成第一个
                    if (chess[x, y] == 0 && emptyCount >= 1)
                    {
                        q1 = x;
                        q2 = y;
                    }
                    else
                    {
                        q1 = x + 1;
                        q2 = y - 1;
                    }
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
                    while (x != 0 && y != height - 1 && emptyCount < 2 && chess[x - 1, y + 1] != -value)
                    {
                        if (chess[x - 1, y + 1] == 0)
                        {
                            emptyCount++;   //空位置数量+1
                            if (emptyCount < 2)
                            {
                                x--;
                                y++;    //只有空位置数量小于2个才继续向前搜索
                            }
                        }
                        else if (chess[x - 1, y + 1] == value)
                        {   //自己的棋就继续搜索
                            x--;
                            y++;
                            count++;
                            if (emptyCount > 0)
                                hasEmptyChess = true;
                        }
                    }

                    //如果前一个位置是空位置，就将前一个位置算成第一个
                    if (chess[x, y] == 0 && emptyCount >= 1)
                    {
                        q1 = x;
                        q2 = y;
                    }
                    else
                    {
                        q1 = x - 1;
                        q2 = y + 1;
                    }
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
                    while (x != width - 1 && y != height - 1 && emptyCount < 2 && chess[x + 1, y + 1] != -value)
                    {
                        if (chess[x + 1, y + 1] == 0)
                        {
                            emptyCount++;   //空位置数量+1
                            if (emptyCount < 2)
                            {
                                x++;
                                y++;    //只有空位置数量小于2个才继续向前搜索
                            }
                        }
                        else if (chess[x + 1, y + 1] == value)
                        {   //自己的棋就继续搜索
                            x++;
                            y++;
                            count++;
                            if (emptyCount > 0)
                                hasEmptyChess = true;
                        }
                    }

                    //如果前一个位置是空位置，就将前一个位置算成第一个
                    if (chess[x, y] == 0 && emptyCount >= 1)
                    {
                        q1 = x;
                        q2 = y;
                    }
                    else
                    {
                        q1 = x + 1;
                        q2 = y + 1;
                    }
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

            return (t1, t2, count, hasEmptyChess);
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

        public List<scoreinfo> getMaxScoreInfos(int value)
        {
            int maxscore = getMaxScore(value);
            var array = value == -1 ? score_human : score_computer;
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
            var array = flag == -1 ? score_human : score_computer;
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

        private enum chesstype
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
