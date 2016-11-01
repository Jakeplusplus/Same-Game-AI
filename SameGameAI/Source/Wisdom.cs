using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SameGameAI
{
    class Wisdom
    {
        public static Chromosome WisdomOfCrowd(Generation gen) //WoC done by average of top 4th of the generation
        {
            Random random = new Random();
            Chromosome wisdomChild = new Chromosome(gen.Pool[0]);

            List<List<double>> wisdomFactor = new List<List<double>>();
            for (int i = 0; i < 10; i++)
            {
                wisdomFactor.Add(new List<double>());
            }

            for (int i = 0; i < wisdomChild.FitnessFactors.Count; i++) 
            {
                foreach (List<double> list in wisdomFactor) list.Clear();
                int topSegment = 0;
                int topCount = 0;
                for (int j = 0; j < gen.Pool.Count; j++)
                {
                    for (int k = 1; k <= wisdomFactor.Count; k++)
                    {
                        if (gen.Pool[j].FitnessFactors[i] >= (k - 1) * .1 && gen.Pool[j].FitnessFactors[i] < k * .1)
                        {
                            wisdomFactor[k - 1].Add(gen.Pool[j].FitnessFactors[i]);
                            if (wisdomFactor[k - 1].Count > topCount)
                            {
                                topSegment = k - 1;
                                topCount = wisdomFactor[k - 1].Count;
                            }
                        }
                    }
                }
                double avg = 0;
                foreach (double numb in wisdomFactor[topSegment])
                {
                    avg += numb;
                }
                avg /= wisdomFactor[topSegment].Count;
                wisdomChild.FitnessFactors[i] = avg;
            }

            wisdomChild.FitnessScore = gen.MainBoard.Solve(wisdomChild.FitnessFactors);
            return wisdomChild;
        }
    }
}
