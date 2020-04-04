using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wzq_bll
{
    public class ai
    {
        private game game;

        public ai(game _game)
        {
            game = _game;
        }

        /// <summary>
        /// 处理进攻状态时才进来，第一个棋子走选定的可能位置
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private (int x, int y) think(int value)
        {
            List<scoreinfo> highs = game.getHighScores(value, 5);
            List<scoreinfo> failed = new List<scoreinfo>(); //走这些地方最终会输掉

            for (int i = 0; i < highs.Count; i++)
            {
                game gametmp = new game(game);
                scoreinfo point = highs[i];

                //模拟走5步
                for (int k = 0; k < 10; k++)
                {
                    List<scoreinfo> playerscores = gametmp.getMaxScoreInfos(-1);
                    List<scoreinfo> computerscores = gametmp.getMaxScoreInfos(1);

                    List<scoreinfo> jingong = value == -1 ? playerscores : computerscores;  //当前走棋方
                    List<scoreinfo> fangshou = value == -1 ? computerscores : playerscores;  //当前走棋的反方

                    int xx, yy;
                    if (CanJinGong(gametmp, jingong, fangshou))
                    {   //进攻
                        if (k == 0)
                        {
                            xx = point.x;
                            yy = point.y;
                            go(point.x, point.y, value, gametmp);    //自己走棋
                        }
                        else
                        {
                            (xx, yy) = Jingong(value, gametmp, jingong);
                            go(xx, yy, value, gametmp);
                        }
                    }
                    else
                    {   //防守
                        (xx, yy) = Fangshou(-value, gametmp, fangshou);
                        go(xx, yy, value, gametmp);
                    }
                    if (gametmp.iswin(xx, yy))
                    {
                        Console.WriteLine($"find win point x:{point.x} y:{point.y} value:{value} i:{i} step:{k}");
                        return (point.x, point.y);  //自己赢了
                    }

                    (int tx, int ty) = go(-value, gametmp);    //对方走棋
                    if (gametmp.iswin(tx, ty))
                    {
                        failed.Add(point);
                        break;  //对方赢了
                    }
                }
            }

            //排除掉可能导致输掉的位置，然后选第一个，就是得分最高那个
            var result = highs.Except(failed).FirstOrDefault();
            if (result != null)
                return (result.x, result.y);
            else
                return (highs[0].x, highs[0].y);
        }

        private bool CanJinGong(game gametmp, int value)
        {
            List<scoreinfo> playerscores = gametmp.getMaxScoreInfos(-1);
            List<scoreinfo> computerscores = gametmp.getMaxScoreInfos(1);

            List<scoreinfo> jingong = value == -1 ? playerscores : computerscores;  //当前走棋方
            List<scoreinfo> fangshou = value == -1 ? computerscores : playerscores;  //当前走棋的反方

            return CanJinGong(gametmp, jingong, fangshou);
        }

        private static bool CanJinGong(game game, List<scoreinfo> jingong, List<scoreinfo> fangshou)
        {
            return (fangshou[0].score - jingong[0].score <= 2000 || fangshou[0].score < 3000 || (jingong[0].score > 8000 && fangshou[0].score < 80000))
                && game.gethistory().Length > 5;
        }

        public (int x, int y) go(int value)
        {
            int x = -1, y = -1;
            if (CanJinGong(game, value))
            {   //进攻
                (x, y) = think(value);
            }
            else
            {   //防守
                (x, y) = think(-value);
            }

            if (x != -1 && y != -1)
                go(x, y, value);
            else
                (x, y) = go(value, game);

            return (x, y);
        }

        private (int x, int y) go(int value, game game)
        {
            int x = 0, y = 0;
            List<scoreinfo> playerscores = game.getMaxScoreInfos(-1);
            List<scoreinfo> computerscores = game.getMaxScoreInfos(1);

            List<scoreinfo> jingong = value == -1 ? playerscores : computerscores;  //当前走棋方
            List<scoreinfo> fangshou = value == -1 ? computerscores : playerscores;  //当前走棋的反方

            if (CanJinGong(game, jingong, fangshou))
            {   //进攻
                (x, y) = Jingong(value, game, jingong);
            }
            else
            {   //防守
                (x, y) = Fangshou(value, game, fangshou);
            }
            game.go(x, y, value);
            return (x, y);
        }

        private (int x, int y) Jingong(int value, game game, List<scoreinfo> jingong)
        {
            int x = -1, y = -1;
            if (jingong.Count == 1)
            {
                x = jingong[0].x;
                y = jingong[0].y;
            }
            else
            {   //多个分数一样的点，就选择对方这些点中值最高的点
                int maxscore = 0;
                foreach (scoreinfo item in jingong)
                {
                    int score = value == -1 ? game.getcomputerscore(item.x, item.y) : game.gethumanscore(item.x, item.y);
                    if (score > maxscore)
                    {
                        maxscore = score;
                        x = item.x;
                        y = item.y;
                    }
                }
            }
            return (x, y);
        }

        private (int x, int y) Fangshou(int value, game game, List<scoreinfo> fangshou)
        {
            int x = -1, y = -1;
            if (fangshou.Count == 1)
            {
                x = fangshou[0].x;
                y = fangshou[0].y;
            }
            else
            {   //多个分数一样的点，就选择对方这些点中值最高的点
                int maxscore = 0;
                foreach (scoreinfo item in fangshou)
                {
                    int score = value == -1 ? game.getcomputerscore(item.x, item.y) : game.gethumanscore(item.x, item.y);
                    if (score > maxscore)
                    {
                        maxscore = score;
                        x = item.x;
                        y = item.y;
                    }
                }
            }
            return (x, y);
        }

        public void go(int x, int y, int value)
        {
            game.go(x, y, value);
        }

        public void go(int x, int y, int value, game game)
        {
            game.go(x, y, value);
        }

    }
}
