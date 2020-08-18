using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleInterpolation
{
    public class Edge
    {
        public PPoint Point1 { get; }
        public PPoint Point2 { get; }

        public HashSet<Triangle> AdjacentTriangles { get; } = new HashSet<Triangle>();

        public Edge(PPoint point1, PPoint point2)
        {
            Point1 = point1;
            Point2 = point2;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != GetType()) return false;
            var edge = obj as Edge;

            var samePoints = Point1 == edge.Point1 && Point2 == edge.Point2;
            var samePointsReversed = Point1 == edge.Point2 && Point2 == edge.Point1;
            return samePoints || samePointsReversed;
        }

        public override int GetHashCode()
        {
            int hCode = (int)Point1.X ^ (int)Point1.Y ^ (int)Point2.X ^ (int)Point2.Y;
            return hCode.GetHashCode();
        }
    }
}
