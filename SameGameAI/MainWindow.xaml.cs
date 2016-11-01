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
using System.Windows.Threading;
using System.IO;

namespace SameGameAI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //These variables are set to global so that they can be accessed by onTick()
        static List<City> cities = new List<City>();
        static TextBlock text;
        static List<Chromosome> population;
        static int count = 0;
        static int NUMBER_OF_CHROMOSOMES = 4;
        static int CROWD_SIZE = 500;
        static DispatcherTimer timer = new DispatcherTimer();
        static GameBoard OriginalBoard;
        static Generation MainGeneration;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void openFileClick(object sender, RoutedEventArgs e)
        {
            //Initialize
            DateTime start = DateTime.Now;
            if (OriginalBoard == null) OriginalBoard = new GameBoard(10, 10, 4, GameWindow);
            OriginalBoard.Update();
            OriginalBoard.Draw();
            MainGeneration = new Generation(OriginalBoard, 50, 10);
            double value = MainGeneration.Pool[0].FitnessScore;

            double previous = 0;
            int count = 0;
            while (count < 1000)
            {
                MainGeneration.NextGeneration();
                if (MainGeneration.Pool[0].FitnessScore == previous) count++;
                else
                {
                    previous = MainGeneration.Pool[0].FitnessScore;
                    count = 0;
                }
            }
            Chromosome WoC = Wisdom.WisdomOfCrowd(MainGeneration);
            double wocScore = WoC.FitnessScore;
            DateTime finish = DateTime.Now;
            resultsDisplay.Text = "Top Score GA: " + MainGeneration.Pool[0].FitnessScore.ToString() + "\nWoC Score: " + wocScore + "\nTime elapsed: " + (finish - start).ToString();
        }
    }
}
