using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SameGameAI
{
    //Class to store coordinates of each city
    class City
    {
        public Vector Vect { get; set; }
        City next;
        bool visited;
        public int cityNumb {get; set;}

        public City(int xParam, int yParam, int numb)
        {
            Vect = new Vector(xParam, yParam);
            visited = false;
            cityNumb = numb;
        }

        //Getters for the coordinates
        public double getx() { return Vect.x; }
        public double gety() { return Vect.y; }
        public void setNext(City city) { next = city; }
        public City getNext() { return next; }
        public void visit() { visited = true; }
        public bool hasBeenVisited() { return visited; }
    }
}
