using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleInterpolation
{
    public class Triangle
    {
        public PPoint[] Vertices { get; } = new PPoint[3];
        public Edge[] Edges { get; } = new Edge[3];
        public PPoint Circumcenter { get; private set; }
        public double RadiusSquared;

        public Triangle TriangleWithSharedEdge(int i)
        {
            if (i > 2)
                return null;

            var neighbors = new HashSet<Triangle>();
            foreach (var vertex in Vertices.Where((vt, seq) => seq != i))
            {
                var trianglesWithSharedEdge = vertex.AdjacentTriangles.Where(o =>
                {
                    return o != this && SharesEdgeWith(o);
                });
                if (neighbors.Any())
                    neighbors.IntersectWith(trianglesWithSharedEdge);
                else
                    neighbors.UnionWith(trianglesWithSharedEdge);
            }

            if (neighbors.Any())
            {
                return neighbors.First();
            }
            else
                return null;
        }

        public List<Triangle> TrianglesWithSharedEdge
        {
            get
            {
                var neighbors = new HashSet<Triangle>();
                foreach (var vertex in Vertices)
                {
                    var trianglesWithSharedEdge = vertex.AdjacentTriangles.Where(o =>
                    {
                        return o != this && SharesEdgeWith(o);
                    });
                    neighbors.UnionWith(trianglesWithSharedEdge);
                }

                return neighbors.ToList();
            }
        }

        public Triangle(PPoint point1, PPoint point2, PPoint point3)
        {
            // In theory this shouldn't happen, but it was at one point so this at least makes sure we're getting a
            // relatively easily-recognised error message, and provides a handy breakpoint for debugging.
            if (point1 == point2 || point1 == point3 || point2 == point3)
            {
                throw new ArgumentException("Must be 3 distinct points");
            }

            if (!IsCountClockwise(point1, point2, point3))
            {
                Vertices[0] = point1;
                Vertices[1] = point3;
                Vertices[2] = point2;
                Edges[0] = new Edge(point3, point2);
                Edges[1] = new Edge(point2, point1);
                Edges[2] = new Edge(point1, point3);
            }
            else
            {
                Vertices[0] = point1;
                Vertices[1] = point2;
                Vertices[2] = point3;
                Edges[0] = new Edge(point2, point3);
                Edges[1] = new Edge(point3, point1);
                Edges[2] = new Edge(point1, point2);
            }

            //if (Vertices[0].X <= 1 && Vertices[0].X >= 0
            //    && Vertices[0].Y <= 1 && Vertices[0].Y >= 0
            //    && Vertices[1].X <= 1 && Vertices[1].X >= 0
            //    && Vertices[1].Y <= 1 && Vertices[1].Y >= 0
            //    && Vertices[2].X <= 1 && Vertices[2].X >= 0
            //    && Vertices[2].Y <= 1 && Vertices[2].Y >= 0)
            //{
            Vertices[0].AdjacentTriangles.Add(this);
            Vertices[1].AdjacentTriangles.Add(this);
            Vertices[2].AdjacentTriangles.Add(this);
            //}

            UpdateCircumcircle();
        }

        private void UpdateCircumcircle()
        {
            // https://codefound.wordpress.com/2013/02/21/how-to-compute-a-circumcircle/#more-58
            // https://en.wikipedia.org/wiki/Circumscribed_circle
            var p0 = Vertices[0];
            var p1 = Vertices[1];
            var p2 = Vertices[2];
            var dA = p0.X * p0.X + p0.Y * p0.Y;
            var dB = p1.X * p1.X + p1.Y * p1.Y;
            var dC = p2.X * p2.X + p2.Y * p2.Y;

            var aux1 = (dA * (p2.Y - p1.Y) + dB * (p0.Y - p2.Y) + dC * (p1.Y - p0.Y));
            var aux2 = -(dA * (p2.X - p1.X) + dB * (p0.X - p2.X) + dC * (p1.X - p0.X));
            var div = (2 * (p0.X * (p2.Y - p1.Y) + p1.X * (p0.Y - p2.Y) + p2.X * (p1.Y - p0.Y)));

            if (div == 0)
            {
                throw new DivideByZeroException();
            }

            var center = new PPoint(aux1 / div, aux2 / div, 0);
            Circumcenter = center;
            RadiusSquared = (center.X - p0.X) * (center.X - p0.X) + (center.Y - p0.Y) * (center.Y - p0.Y);
        }

        private bool IsCountClockwise(PPoint point1, PPoint point2, PPoint point3)
        {
            var result = (point2.X - point1.X) * (point3.Y - point1.Y) -
                (point3.X - point1.X) * (point2.Y - point1.Y);
            return result > 0;
        }

        public bool SharesEdgeWith(Triangle triangle)
        {
            var sharedVertices = Vertices.Where(o => triangle.Vertices.Contains(o)).Count();
            return sharedVertices == 2;
        }

        public bool IsPointInsideCircumcircle(PPoint point)
        {
            var d_squared = (point.X - Circumcenter.X) * (point.X - Circumcenter.X) +
                (point.Y - Circumcenter.Y) * (point.Y - Circumcenter.Y);
            return d_squared < RadiusSquared;
        }
    }
}
