using System.Collections.Generic;

namespace SimpleInterpolation
{
    public class PPoint
    {
        public double X { get; set; }
        public double Y { get; set; }
        public int DisplayX { get; set; }
        public int DisplayY { get; set; }
        public double Dx { get; set; }
        public double Dy { get; set; }
        public double VAL { get; set; }

        /// <summary>
        /// Used only for generating a unique ID for each instance of this class that gets generated
        /// </summary>
        private static int _counter;

        /// <summary>
        /// Used for identifying an instance of a class; can be useful in troubleshooting when geometry goes weird
        /// (e.g. when trying to identify when Triangle objects are being created with the same Point object twice)
        /// </summary>
        private readonly int _instanceId = _counter++;

        public HashSet<Triangle> AdjacentTriangles { get; } = new HashSet<Triangle>();

        public HashSet<PPoint> AdjacentPoints { get; } = new HashSet<PPoint>();

        public PPoint(double x, double y, double val)
        {
            X = x;
            Y = y;
            VAL = val;
            AdjacentPoints.Clear();
            AdjacentTriangles.Clear();
        }

        public override string ToString()
        {
            // Simple way of seeing what's going on in the debugger when investigating weirdness
            return $"{nameof(PPoint)} {_instanceId} {X:0.##}@{Y:0.##}";
        }
    }
}