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

        public (int x, int y) go()
        {
            int x = 0, y = 0;
            game.refreshscore();
            List<scoreinfo> playerscores = game.getMaxScoreInfos(0);
            List<scoreinfo> computerscores = game.getMaxScoreInfos(1);
            if ((computerscores[0].score + 800 >= playerscores[0].score || playerscores[0].score < 600 || (computerscores[0].score > 8000 && playerscores[0].score < 100000))
                && game.gethistory().Length > 5)
            {   //进攻
                if (computerscores.Count == 1)
                {
                    x = computerscores[0].x;
                    y = computerscores[0].y;
                }
                else
                {   //多个分数一样的点，就选择对方这些点中值最高的点
                    int maxscore = 0;
                    foreach (scoreinfo item in computerscores)
                    {
                        int score = game.gethumanscore(item.x, item.y);
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
                if (playerscores.Count == 1)
                {
                    x = playerscores[0].x;
                    y = playerscores[0].y;
                }
                else
                {   //多个分数一样的点，就选择对方这些点中值最高的点
                    int maxscore = 0;
                    foreach (scoreinfo item in playerscores)
                    {
                        int score = game.gethumanscore(item.x, item.y);
                        if (score > maxscore)
                        {
                            maxscore = score;
                            x = item.x;
                            y = item.y;
                        }
                    }
                }

            }
            game.go(x, y, 1);
            return (x, y);
        }
    }
}
