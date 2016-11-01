using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SameGameAI
{
    class Generation
    {
        public List<Chromosome> Pool { get; set; }
        public GameBoard MainBoard { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public double Average { get; set; }
        public double StandardDeviation { get; set; }
        public int NumberOfFactors { get; set; }
        private Random random; //Random number generator used by algorithms and chromosome generation
        private double PM;
        private int optimal;

        public Generation(GameBoard gameBoard, int poolSize, int numberOfFactors)
        {
            //Set provided parameters
            MainBoard = gameBoard;
            NumberOfFactors = numberOfFactors;
            //Create first Generation of Chromosomes
            Pool = new List<Chromosome>();
            random = new Random();
            for (int i = 0; i < poolSize; i++)
            {
                Pool.Add(new Chromosome(NumberOfFactors, new GameBoard(MainBoard), random));
            }
            PM = random.Next(1, 11);
        }

        /// <summary>
        /// Create a child generation that replaces the parent generation
        /// </summary>
        public void NextGeneration()
        {
            int count = Pool.Count;
            //Store parents
            List<Chromosome> parents = Pool;
            //Clear the parents, so children can be added to the pool
            Pool = new List<Chromosome>();

            int survivingPopulation = parents.Count / 4;
            if (survivingPopulation % 2 == 1) survivingPopulation++;

            for (int j = 0; j < survivingPopulation; j++)
            {
                int parent1 = 0;
                int parent2 = 1;
                //Find the two highest scoring parents
                for (int i = 0; i < parents.Count; i++)
                {
                    if (parents[i].FitnessScore > parents[parent1].FitnessScore)
                    {
                        parent2 = parent1;
                        parent1 = i;
                    }
                    else if (parents[i].FitnessScore > parents[parent2].FitnessScore)
                    {
                        parent2 = i;
                    }
                }

                Pool.Add(parents[parent1]);
                Pool.Add(parents[parent2]);

                //Perform the crossover algorithms
                Pool.AddRange(Crossover(parents[parent1], parents[parent2], 1));
                Pool.AddRange(Crossover(parents[parent1], parents[parent2], 2));

                Chromosome remove1 = parents[parent1];
                Chromosome remove2 = parents[parent2];

                parents.Remove(remove1);
                parents.Remove(remove2);
            }

            int poolSize = Pool.Count;
            //Mutator algorithms ran 10% of the time
            for (int i = 1; i < poolSize; i++)
            {
                if (true)
                {
                    Pool.Add(Mutator(Pool[i], random.Next(1, 3)));
                }
            }

            Pool = Pool.OrderByDescending(p => p.FitnessScore).ToList();

            if (Pool.Count > count)
            {
                Pool.RemoveRange(count, Pool.Count - count);
            }
        }

        /// <summary>
        /// Performs a crossover algorithm determined by selector on specified Chromosomes
        /// </summary>
        /// <param name="parent1">The first parent Chromosome</param>
        /// <param name="parent2">The second parent Chromosome</param>
        /// <param name="selector">Determines which crossover algorithm to run</param>
        public List<Chromosome> Crossover(Chromosome parent1, Chromosome parent2, int selector)
        {
            List<Chromosome> children = new List<Chromosome>();

            children.Add(new Chromosome(parent1));
            children.Add(new Chromosome(parent2));

            if (selector == 1) //First Crossover - Insert randomized sections of parent into other parent
            {
                int startPosition1 = random.Next(0, parent1.FitnessFactors.Count);
                int startPosition2 = random.Next(0, parent2.FitnessFactors.Count);

                int length1 = random.Next(0, parent1.FitnessFactors.Count - startPosition1);
                int length2 = random.Next(0, parent1.FitnessFactors.Count - startPosition2);

                for (int i = 0; i < length1; i++)
                {
                    children[0].FitnessFactors[startPosition1 + i] = parent2.FitnessFactors[startPosition1 + i];
                }
                for (int i = 0; i < length2; i++)
                {
                    children[1].FitnessFactors[startPosition2 + i] = parent1.FitnessFactors[startPosition2 + i];
                }
            }
            else if (selector == 2) //Second crossover - Swap two random
            {
                for (int i = 0; i < 2; i++)
                {
                    double one = children[0].FitnessFactors[random.Next(0, children[0].FitnessFactors.Count)];
                    double two = children[1].FitnessFactors[random.Next(0, children[1].FitnessFactors.Count)];

                    children[0].FitnessFactors[random.Next(0, children[0].FitnessFactors.Count)] = two;
                    children[1].FitnessFactors[random.Next(0, children[1].FitnessFactors.Count)] = one;
                }
            }
            else
            {
                return children;
            }

            foreach (Chromosome child in children)
            {
                GameBoard board = new GameBoard(MainBoard);
                child.FitnessScore = board.Solve(child.FitnessFactors);
            }

            return children;
        }

        /// <summary>
        /// Performs a mutation algorithm determined by a selector on a specified Chromosome
        /// </summary>
        /// <param name="chrom">Chromosome to perform mutation on</param>
        /// <param name="selector">Determines which mutation algorithm to run</param>
        public Chromosome Mutator(Chromosome original, int selector)
        {
            Chromosome chrom = new Chromosome(original);
            if (selector == 1) //First mutator - Swaps start and random locations
            {
                int position = random.Next(1, chrom.FitnessFactors.Count - 1);
                double numb = chrom.FitnessFactors[position];
                chrom.FitnessFactors[position] = chrom.FitnessFactors[0];
                chrom.FitnessFactors[0] = numb;
            }
            else if (selector == 2) //Second mutator - Randomly re-generates 1 factor
            {
                int position = random.Next(0, chrom.FitnessFactors.Count);
                chrom.FitnessFactors[position] = random.NextDouble();
            }
            GameBoard board = new GameBoard(MainBoard);
            chrom.FitnessScore = board.Solve(chrom.FitnessFactors);
            return chrom;
        }

        public Chromosome getOptimalChromosome()
        {
            return Pool[optimal];
        }
    }
}
