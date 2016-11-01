using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SameGameAI
{
    /// <summary>
    /// Class that stores x and y coordinates as a vector and allows various opperations for vectors
    /// </summary>
    class Vector : IEquatable<Vector>
    {
        public int x;
        public int y;
        public Vector(int px, int py) { x = px; y = py; }
        public Vector(){}

        public bool Equals(Vector other)
        {
            return this.x == other.x && this.y == other.y;
        }
    }
}
