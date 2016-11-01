using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SameGameAI
{
    class GameMove
    {
        public List<Vector> Tiles { get; set; }
        public double FitnessScore { get; set; }
        public int Score { get; set; }

        public GameMove(int startX, int startY, List<int> determinedValues, List<List<int>> board, List<double> fitnessFactors)
        {
            FitnessScore = 0;
            if (startX >= 0 && startY >= 0)
            {
                Tiles = new List<Vector>();
                int tileType = board[startX][startY];

                //Find all tiles in move
                Queue<Vector> check = new Queue<Vector>();
                check.Enqueue(new Vector(startX, startY));
                while (check.Count > 0)
                {
                    Vector checking = check.Dequeue();
                    if (!Tiles.Contains(checking)) Tiles.Add(checking);

                    //Check cardinal directions
                    if (checking.x - 1 >= 0 && board[checking.x - 1][checking.y] == tileType)
                    {
                        Vector toAdd = new Vector(checking.x - 1, checking.y);
                        if (!Tiles.Contains(toAdd)) check.Enqueue(toAdd);
                    }
                    if (checking.x + 1 < board.Count && board[checking.x + 1][checking.y] == tileType)
                    {
                        Vector toAdd = new Vector(checking.x + 1, checking.y);
                        if (!Tiles.Contains(toAdd)) check.Enqueue(toAdd);
                    }
                    if (checking.y - 1 >= 0 && board[checking.x][checking.y - 1] == tileType)
                    {
                        Vector toAdd = new Vector(checking.x, checking.y - 1);
                        if (!Tiles.Contains(toAdd)) check.Enqueue(toAdd);
                    }
                    if (checking.y + 1 < board[0].Count && board[checking.x][checking.y + 1] == tileType)
                    {
                        Vector toAdd = new Vector(checking.x, checking.y + 1);
                        if (!Tiles.Contains(toAdd)) check.Enqueue(toAdd);
                    }
                }

                //Calculate Fitness
                double totalFitness = 0;

                //Check if large (> 5)
                int sign = 0;
                if (fitnessFactors[5] < 0.5) sign = -1;
                else sign = 1;
                totalFitness += fitnessFactors[5] * sign * (Tiles.Count / Convert.ToDouble(board.Count * board[0].Count)) * 100;
                //if (Tiles.Count > 5) totalFitness += fitnessFactors[5];
                //else totalFitness += 1 - fitnessFactors[5];
                
                //Should you stick to walls?
                if (determinedValues[0] == 1)
                {
                    //Find which wall - left or right
                    int tileX = board.Count + 1;
                    foreach (Vector tile in Tiles)
                    {
                        if (tile.x < tileX) tileX = tile.x;
                    }
                    if (tileX < board.Count / 2) totalFitness += fitnessFactors[6];
                    else totalFitness += 1 - fitnessFactors[6];
                }

                //Should you stick to floor or ceiling?
                if (determinedValues[1] == 1)
                {
                    //Find floor or ceiling
                    int tileY = board[0].Count + 1;
                    foreach (Vector tile in Tiles)
                    {
                        if (tile.y < tileY) tileY = tile.y;
                    }
                    if (tileY < board[0].Count / 2) totalFitness += fitnessFactors[7];
                    else totalFitness += 1 - fitnessFactors[7];
                }

                //Remaining color
                if (determinedValues[2] == 1)
                {
                    List<int> numberOfColor = new List<int>();
                    for (int i = 0; i <= determinedValues[5]; i++) numberOfColor.Add(0);
                    for (int i = 0; i < board.Count; i++)
                    {
                        for (int j = 0; j < board[i].Count; j++)
                        {
                            numberOfColor[board[i][j] + 1]++;
                        }
                    }
                    double remainingAverage = 0;
                    for (int i = 0; i < determinedValues[5]; i++)
                    {
                        remainingAverage += numberOfColor[i];
                    }
                    bool largerThanAverage = true;
                    if (numberOfColor[tileType] < remainingAverage) largerThanAverage = false;
                    if (largerThanAverage) totalFitness += fitnessFactors[8];
                    else totalFitness += 1 - fitnessFactors[8];
                }

                //Weight a color?
                if (determinedValues[3] == 1)
                {
                    int whichTile = determinedValues[4];
                    if (tileType == whichTile + 1) totalFitness += fitnessFactors[9];
                    else totalFitness += 1 - fitnessFactors[9];
                }
                FitnessScore = totalFitness;

                Score = (Tiles.Count - 2) * (Tiles.Count - 2);
            }
            else FitnessScore = -1;
        }
    }
}
