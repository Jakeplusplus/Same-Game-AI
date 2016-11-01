using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SameGameAI
{
    class Chromosome
    {
        public List<double> FitnessFactors{ get; set; }
        public double FitnessScore { get; set; }


        public Chromosome(int numberOfFactors, GameBoard gameBoard, Random random)
        {
            FitnessFactors = new List<double>();
            for (int i = 0; i < numberOfFactors; i++)
            {
                FitnessFactors.Add(random.NextDouble());
            }

            //Solve game and get score
            FitnessScore = gameBoard.Solve(FitnessFactors);
        }

        public Chromosome(Chromosome original)
        {
            FitnessFactors = new List<double>(original.FitnessFactors);
            FitnessScore = original.FitnessScore;
        }
    }
}
