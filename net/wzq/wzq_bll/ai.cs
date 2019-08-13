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

        public (int x, int y) go(int value)
        {
            int x = 0, y = 0;

            game.refreshscore();

            List<scoreinfo> playerscores = game.getMaxScoreInfos(-1);
            List<scoreinfo> computerscores = game.getMaxScoreInfos(1);

            List<scoreinfo> jingong = value == -1 ? playerscores : computerscores;  //当前走棋方
            List<scoreinfo> fangshou = value == -1 ? computerscores : playerscores;  //当前走棋的反方

            if ((fangshou[0].score - jingong[0].score <= 2000 || fangshou[0].score < 3000 || (jingong[0].score > 8000 && fangshou[0].score < 100000))
                && game.gethistory().Length > 5)
            {   //进攻
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
            }
            else
            {   //防守
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

            }
            game.go(x, y, value);
            return (x, y);
        }

        public void go(int x, int y, int value)
        {
            game.go(x, y, value);
        }

    }
}
