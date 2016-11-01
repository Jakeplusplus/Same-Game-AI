using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SameGameAI
{
    class GameBoard
    {
        public int NumberOfColors { get; set; }
        public int NumberOfColumns { get; set; }
        public int NumberOfRows { get; set; }
        public double TileSize { get; set; }
        public List<List<int>> Board { get; set; }
        public Canvas GameCanvas { get; set; }
        public Random Rand { get; set; }
        private List<SolidColorBrush> colors;


        public GameBoard(GameBoard original)
        {
            this.NumberOfColors = original.NumberOfColors;
            this.NumberOfColumns = original.NumberOfColumns;
            this.NumberOfRows = original.NumberOfRows;
            this.TileSize = original.TileSize;
            this.GameCanvas = original.GameCanvas;
            this.Rand = original.Rand;

            this.Board = new List<List<int>>();
            for (int i = 0; i < original.Board.Count; i++)
            {
                this.Board.Add(new List<int>(original.Board[i]));
            }

            colors = new List<SolidColorBrush>();
            colors.Add(new SolidColorBrush(Colors.Black));
            colors.Add(new SolidColorBrush(Colors.Blue));
            colors.Add(new SolidColorBrush(Colors.Red));
            colors.Add(new SolidColorBrush(Colors.Yellow));
            colors.Add(new SolidColorBrush(Colors.Green));
            colors.Add(new SolidColorBrush(Colors.Purple));
            colors.Add(new SolidColorBrush(Colors.Turquoise));
            colors.Add(new SolidColorBrush(Colors.Orange));
            colors.Add(new SolidColorBrush(Colors.Pink));
            colors.Add(new SolidColorBrush(Colors.White));
            colors.Add(new SolidColorBrush(Colors.Gray));
        }

        public GameBoard(int columns, int rows, int tiles, Canvas canvas)
        {
            //Set items to value passed in by parameters
            NumberOfColumns = columns;
            NumberOfRows = rows;
            NumberOfColors = tiles;
            GameCanvas = canvas;

            //Determine and set size of tiles
            if (NumberOfColumns >= NumberOfRows) TileSize = GameCanvas.ActualWidth / NumberOfColumns;
            else TileSize = GameCanvas.ActualHeight / NumberOfRows;

            //Initialize other components
            Rand = new Random();
            colors = new List<SolidColorBrush>();
            colors.Add(new SolidColorBrush(Colors.Black));
            colors.Add(new SolidColorBrush(Colors.Blue));
            colors.Add(new SolidColorBrush(Colors.Red));
            colors.Add(new SolidColorBrush(Colors.Yellow));
            colors.Add(new SolidColorBrush(Colors.Green));
            colors.Add(new SolidColorBrush(Colors.Purple));
            colors.Add(new SolidColorBrush(Colors.Turquoise));
            colors.Add(new SolidColorBrush(Colors.Orange));
            colors.Add(new SolidColorBrush(Colors.Pink));
            colors.Add(new SolidColorBrush(Colors.White));
            colors.Add(new SolidColorBrush(Colors.Gray));

            //Initialize game board
            Board = new List<List<int>>();
            for (int i = 0; i < NumberOfColumns; i++)
            {
                Board.Add(new List<int>());
                for (int j = 0; j < NumberOfRows; j++)
                {
                    Board[i].Add(Rand.Next(0, NumberOfColors));
                }
            }
        }

        public void Draw()
        {
            GameCanvas.Children.Clear();
            for (int i = 0; i < NumberOfColumns; i++)
            {
                for (int j = 0; j < NumberOfRows; j++)
                {
                    Drawing.DrawTile((TileSize * i) + 1, (TileSize * j) + 1, TileSize, colors[Board[i][j] + 1], GameCanvas);
                }
            }
        }

        public void Update()
        {
            //Check for and perform "gravity" on tiles
            for (int i = 0; i < NumberOfColumns; i++)
            {
                for (int j = 0; j < NumberOfRows; j++)
                {
                    if (Board[i][j] == -1)
                    {
                        for (int k = 1; k < NumberOfRows - j; k++)
                        {
                            if (Board[i][j + k] > -1)
                            {
                                Board[i][j] = Board[i][j + k];
                                Board[i][j + k] = -1;
                                k = NumberOfRows;
                            }
                        }
                    }
                }
            }

            //Check for and perform column shifts
            for (int i = 0; i < NumberOfColumns; i++)
            {
                if (Board[i][0] == -1)
                {
                    for (int j = 0; j < NumberOfColumns - i; j++)
                    {
                        if (Board[i + j][0] > -1)
                        {
                            List<int> temp = Board[i];
                            Board[i] = Board[i + j];
                            Board[i + j] = temp;
                            j = NumberOfColumns;
                        }
                    }
                }
            }
        }

        public GameMove FindMove(List<double> fitnessFactors)
        {
            List<GameMove> possibleMoves = new List<GameMove>();

            List<int> determinedValues = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                if (fitnessFactors[i] < 0.5) determinedValues.Add(1);
                else determinedValues.Add(0);
            }
            //determinedValues.Add(Rand.Next(1, NumberOfColors + 1));
            determinedValues.Add(1);
            determinedValues.Add(NumberOfColors);

            //Check cardinal directions
            for (int i = 0; i < NumberOfColumns; i++)
            {
                for (int j = 0; j < NumberOfRows; j++)
                {
                    Vector current = new Vector(i, j);
                    bool inPossibleMoves = false;
                    foreach (GameMove move in possibleMoves)
                    {
                        if (move.Tiles.Contains(current)) inPossibleMoves = true;
                    }

                    int tileType = Board[i][j];
                    if (!inPossibleMoves && tileType != -1)
                    {
                        if (current.x - 1 >= 0 && Board[current.x - 1][current.y] == tileType) { possibleMoves.Add(new GameMove(i, j, determinedValues, Board, fitnessFactors)); continue; }
                        if (current.x + 1 < Board.Count && Board[current.x + 1][current.y] == tileType) { possibleMoves.Add(new GameMove(i, j, determinedValues, Board, fitnessFactors)); continue; }
                        if (current.y - 1 >= 0 && Board[current.x][current.y - 1] == tileType) { possibleMoves.Add(new GameMove(i, j, determinedValues, Board, fitnessFactors)); continue; }
                        if (current.y + 1 < Board[0].Count && Board[current.x][current.y + 1] == tileType) { possibleMoves.Add(new GameMove(i, j, determinedValues, Board, fitnessFactors)); continue; }
                    }
                }
            }

            GameMove optimalMove = new GameMove(-1, -1, determinedValues, Board, fitnessFactors);
            if (possibleMoves.Count > 0)
            {
                optimalMove = possibleMoves[0];
                foreach (GameMove move in possibleMoves)
                {
                    if (move.FitnessScore > optimalMove.FitnessScore) optimalMove = move;
                }
            }

            return optimalMove;
        }

        public double Solve(List<double> fitnessFactors)
        {
            double score = 0;

            bool solving = true;
            while (solving)
            {
                GameMove move = FindMove(fitnessFactors);
                if (move.FitnessScore == -1) solving = false;
                else
                {
                    score += move.Score;
                    foreach (Vector tile in move.Tiles)
                    {
                        Board[tile.x][tile.y] = -1;
                    }
                }
                Update();
            }

            return score;
        }
    }
}
