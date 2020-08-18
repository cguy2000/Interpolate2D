using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleInterpolation
{
    public class DelaunayTriangulator
    {
        private double MaxX { get; set; }
        private double MaxY { get; set; }
        private IEnumerable<Triangle> border;

        public IEnumerable<PPoint> GeneratePoints(int amount, double maxX, double maxY)
        {
            MaxX = maxX;
            MaxY = maxY;

            // TODO make more beautiful
            var point0 = new PPoint(0, 0, 0);
            var point1 = new PPoint(0, MaxY, 0);
            var point2 = new PPoint(MaxX, MaxY, 0);
            var point3 = new PPoint(MaxX, 0, 0);
            var points = new List<PPoint>() { point0, point1, point2, point3 };


            var random = new Random();
            for (int i = 0; i < amount - 4; i++)
            {
                var pointX = (float)(random.NextDouble() * MaxX);
                var pointY = (float)(random.NextDouble() * MaxY);
                points.Add(new PPoint(pointX, pointY, 0));
            }

            return points;
        }

        public DelaunayTriangulator(double maxX, double maxY)
        {
            MaxX = maxX;
            MaxY = maxY;
        }

        public IEnumerable<Triangle> BowyerWatson(List<PPoint> points)
        {
            if (points.Count() < 3)
                return null;
            //var tri1 = new Triangle(points[0], points[1], points[2]);
            //var tri2 = new Triangle(points[0], points[2], points[3]);
            //border = new List<Triangle>() { tri1, tri2 };

            var supraTriangle = GenerateSupraTriangle();
            var triangulation = new HashSet<Triangle>(new List<Triangle>() { supraTriangle });

            foreach (var point in points)
            {
                var badTriangles = FindBadTriangles(point, triangulation);
                var polygon = FindHoleBoundaries(badTriangles);

                foreach (var triangle in badTriangles)
                {
                    foreach (var vertex in triangle.Vertices)
                    {
                        vertex.AdjacentTriangles.Remove(triangle);
                    }
                }
                triangulation.RemoveWhere(o => badTriangles.Contains(o));

                foreach (var edge in polygon.Where(possibleEdge => possibleEdge.Point1 != point && possibleEdge.Point2 != point))
                {
                    var triangle = new Triangle(point, edge.Point1, edge.Point2);
                    triangulation.Add(triangle);
                }
            }

            // if (false)
            triangulation.RemoveWhere(o => o.Vertices.Any(v => supraTriangle.Vertices.Contains(v)));
            return triangulation;
        }

        private List<Edge> FindHoleBoundaries(ISet<Triangle> badTriangles)
        {
            var edges = new List<Edge>();
            foreach (var triangle in badTriangles)
            {
                edges.Add(new Edge(triangle.Vertices[0], triangle.Vertices[1]));
                edges.Add(new Edge(triangle.Vertices[1], triangle.Vertices[2]));
                edges.Add(new Edge(triangle.Vertices[2], triangle.Vertices[0]));
            }
            var grouped = edges.GroupBy(o => o);
            var boundaryEdges = edges.GroupBy(o => o).Where(o => o.Count() == 1).Select(o => o.First());
            return boundaryEdges.ToList();
        }

        private Triangle GenerateSupraTriangle()
        {
            //   1  -> maxX
            //  / \
            // 2---3
            // |
            // v maxY
            var margin = 500;
            var point1 = new PPoint(0.5f * MaxX, -2 * MaxX - margin, 0);
            var point2 = new PPoint(-2 * MaxY - margin, 2 * MaxY + margin, 0);
            var point3 = new PPoint(2 * MaxX + MaxY + margin, 2 * MaxY + margin, 0);
            return new Triangle(point1, point2, point3);
        }

        private ISet<Triangle> FindBadTriangles(PPoint point, HashSet<Triangle> triangles)
        {
            var badTriangles = triangles.Where(o => o.IsPointInsideCircumcircle(point));
            return new HashSet<Triangle>(badTriangles);
        }
    }
}
