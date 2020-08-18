using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleInterpolation
{
    public partial class Test : Form
    {
        private List<PPoint> _points = new List<PPoint>();
        IEnumerable<Triangle> triangulation;
        List<RateColor> rcs;

        public int InterpolatePower { get; set; }

        private Graphics g = null;
        private int _offset = 100;
        private int _diameter = 1;
        private int _left = 0;
        private int _top = 1;

        public Test()
        {
            InitializeComponent();
        }

        private void pnlCanvas_Paint(object sender, PaintEventArgs e)
        {
            SetColorMap();
            g = pnlCanvas.CreateGraphics();
            g.Clear(Color.White);
            Brush brush = new SolidBrush(Color.LightGray);
            Pen pen = new Pen(Color.Red);
            _diameter = pnlCanvas.Width > pnlCanvas.Height ? pnlCanvas.Height : pnlCanvas.Width;
            _left = _offset - (_diameter - pnlCanvas.Width) / 2;
            _top = _offset - (_diameter - pnlCanvas.Height) / 2;
            _diameter -= _offset * 2;
            if (_diameter < 1)
                return;

            Rectangle rect = new Rectangle(_left, _top, _diameter, _diameter);
            g.DrawEllipse(pen, rect);

            if (_points == null || !_points.Any())
                return;

            double dMax = _points.Max(pt => pt.VAL);
            double dMin = _points.Min(pt => pt.VAL);
            double dRan = dMax - dMin;
            dRan = dRan == 0 ? 1 : dRan;

            foreach (var point in _points)
            {
                //int pointX = (int)((point.X / 300 + 0.5d) * _diameter) + _left;
                //int pointY = (int)((-point.Y / 300 + 0.5d) * _diameter) + _top;
                int pointX = (int)Math.Round((point.X + 0.5d) * _diameter) + _left;
                int pointY = (int)Math.Round((-point.Y + 0.5d) * _diameter) + _top;
                point.DisplayX = pointX;
                point.DisplayY = pointY;
                var rate = (point.VAL - dMin) / dRan * 100;
                Color color = GetColor(rate);
                g.FillEllipse(new SolidBrush(color), pointX - 2, pointY - 2, 4, 4);
            }

            int iDelta;
            Int32.TryParse(txtRectangleDelta.Text, out iDelta);

            if (chkInterpolation.Checked)
            {
                int iPow;
                Int32.TryParse(txtPowerOfDiatance.Text, out iPow);

                // Brush[,] arrBrush = new Brush[_diameter / iDelta + 1, _diameter / iDelta + 1];
                double[,] arrValue = new double[_diameter / iDelta + 1, _diameter / iDelta + 1];

                Parallel.For(0, _diameter / iDelta, x0 =>
                {
                    int x = x0 * iDelta;

                    for (int y = 0; y < _diameter; y += iDelta)
                    {
                        int qX = x > _diameter / 2 ? x - _diameter / 2 : _diameter / 2 - x;
                        int qY = y > _diameter / 2 ? y - _diameter / 2 : _diameter / 2 - y;

                        if (qX * qX + qY * qY > _diameter * _diameter / 4)
                        {
                            arrValue[x / iDelta, y / iDelta] = Double.NaN;
                            continue;
                        }

                        //double pX = x * 300d / _diameter - 150;
                        //double pY = 150 - y * 300d / _diameter;
                        double pX = x * 1d / _diameter - 0.5;
                        double pY = 0.5 - y * 1d / _diameter;

                        var qInterpolation0 = from pt in _points
                                              let distX = pt.X - pX
                                              let distY = pt.Y - pY
                                              let dist = distX * distX + distY * distY
                                              select new
                                              {
                                                  DIST = Math.Pow(Math.Sqrt(dist), iPow),
                                                  VAL = (double)pt.VAL
                                              };

                        double dInterpolation1;
                        if (qInterpolation0.Any(pt => pt.DIST == 0))
                            dInterpolation1 = qInterpolation0.Where(pt => pt.DIST == 0).Sum(pt => pt.VAL);
                        else
                            dInterpolation1 = qInterpolation0.Sum(p => p.VAL / p.DIST) / qInterpolation0.Sum(p => 1 / p.DIST);

                        arrValue[x / iDelta, y / iDelta] = dInterpolation1;
                    }
                });

                var dt = ArraytoDatatable(arrValue);

                var qSerial = Enumerable.Range(0, _diameter / iDelta).SelectMany(row =>
                    Enumerable.Range(0, _diameter / iDelta).Select(col => arrValue[row, col]).Where(d => !Double.IsNaN(d))
                );

                dMax = qSerial.Max();
                dMin = qSerial.Min();
                dRan = dMax - dMin;

                for (int x = 0; x < _diameter; x += iDelta)
                {
                    for (int y = 0; y < _diameter; y += iDelta)
                    {
                        if (Double.IsNaN(arrValue[x / iDelta, y / iDelta]))
                            continue;

                        var rate = (arrValue[x / iDelta, y / iDelta] - dMin) / dRan * 100;
                        Color colorInterpolation = GetColor(rate);
                        Brush brushInterpolation = new SolidBrush(colorInterpolation);

                        g.FillRectangle(brushInterpolation, x + _left, y + _top, iDelta, iDelta);
                    }
                }
            }

            if (chkVoronoiDiagram.Checked && !chkInterpolation.Checked)
            {
                Brush[,] arrBrush = new Brush[_diameter / iDelta + 1, _diameter / iDelta + 1];

                Parallel.For(0, _diameter / iDelta, x0 =>
                {
                    int x = x0 * iDelta;

                    for (int y = 0; y < _diameter; y += iDelta)
                    {
                        int qX = x > _diameter / 2 ? x - _diameter / 2 : _diameter / 2 - x;
                        int qY = y > _diameter / 2 ? y - _diameter / 2 : _diameter / 2 - y;

                        if (qX * qX + qY * qY > _diameter * _diameter / 4)
                            continue;

                        var qInterpolation0 = from pt in _points
                                              let distX = pt.DisplayX - x - _left
                                              let distY = pt.DisplayY - y - _top
                                              let dist = distX * distX + distY * distY
                                              select new
                                              {
                                                  DIST = Math.Sqrt(dist),
                                                  VAL = (double)pt.VAL
                                              };

                        double dInterpolation1 = qInterpolation0.OrderBy(pt => pt.DIST).First().VAL;

                        var rate = (dInterpolation1 - dMin) / dRan * 100;
                        Color colorInterpolation = GetColor(rate);
                        Brush brushInterpolation = new SolidBrush(colorInterpolation);
                        arrBrush[x / iDelta, y / iDelta] = brushInterpolation;
                    }
                });

                // ts = DateTime.Now - dtStart;

                for (int x = 0; x < _diameter; x += iDelta)
                {
                    for (int y = 0; y < _diameter; y += iDelta)
                    {
                        if (arrBrush[x / iDelta, y / iDelta] == null)
                            continue;

                        g.FillRectangle(arrBrush[x / iDelta, y / iDelta], x + _left, y + _top, iDelta, iDelta);
                    }
                }
            }

            if (chkCloughTocher2D.Checked && triangulation != null && triangulation.Any())
            {
                // Brush[,] arrBrush = new Brush[_diameter / iDelta + 1, _diameter / iDelta + 1];
                double[,] arrValue = new double[_diameter / iDelta + 1, _diameter / iDelta + 1];
                //Parallel.For(0, _diameter / iDelta, x0 =>
                for (int x0 = 0; x0 < _diameter / iDelta; x0 += iDelta)
                {
                    int x = x0 * iDelta;

                    for (int y = 0; y < _diameter; y += iDelta)
                    {
                        int qX = x > _diameter / 2 ? x - _diameter / 2 : _diameter / 2 - x;
                        int qY = y > _diameter / 2 ? y - _diameter / 2 : _diameter / 2 - y;

                        if (qX * qX + qY * qY > _diameter * _diameter / 4)
                        {
                            arrValue[x / iDelta, y / iDelta] = Double.NaN;
                            continue;
                        }
                        //double pX = x * 300d / _diameter - 150;
                        //double pY = 150 - y * 300d / _diameter;
                        double pX = x * 1d / _diameter - 0.5;
                        double pY = 0.5 - y * 1d / _diameter;

                        PPoint point = new PPoint(pX, pY, 0);

                        Triangle triangle = GetTriangle(point);

                        if (triangle == null)
                        {
                            arrValue[x / iDelta, y / iDelta] = Double.NaN;
                            continue;
                        }

                        if (x == 95 && y == 148)
                            ;

                        var w = CloughTocher2dSingle(triangle, point);
                        arrValue[x / iDelta, y / iDelta] = w;
                    }
                }
                //});

                var dt =ArraytoDatatable(arrValue);

                var qSerial = Enumerable.Range(0, _diameter / iDelta).SelectMany(row =>
                    Enumerable.Range(0, _diameter / iDelta).Select(col => arrValue[row, col]).Where(d => !Double.IsNaN(d))
                );

                dMax = qSerial.Max();
                dMin = qSerial.Min();
                dRan = dMax - dMin;

                for (int x = 0; x < _diameter; x += iDelta)
                {
                    for (int y = 0; y < _diameter; y += iDelta)
                    {
                        if (Double.IsNaN(arrValue[x / iDelta, y / iDelta]))
                            continue;

                        var rate = (arrValue[x / iDelta, y / iDelta] - dMin) / dRan * 100;

                        Color colorInterpolation = GetColor(rate);
                        Brush brushInterpolation = new SolidBrush(colorInterpolation);

                        g.FillRectangle(brushInterpolation, x + _left, y + _top, iDelta, iDelta);
                    }
                }
            }

            if (chkDelaunayTriangle.Checked)
            {
                if (triangulation == null)
                    return;

                var edges = new List<Edge>();
                foreach (var triangle in triangulation)
                {
                    edges.Add(new Edge(triangle.Vertices[0], triangle.Vertices[1]));
                    edges.Add(new Edge(triangle.Vertices[1], triangle.Vertices[2]));
                    edges.Add(new Edge(triangle.Vertices[2], triangle.Vertices[0]));
                }

                Pen penBlack = new Pen(Color.Black);
                foreach (var edge in edges)
                {
                    g.DrawLine(penBlack, edge.Point1.DisplayX, edge.Point1.DisplayY, edge.Point2.DisplayX, edge.Point2.DisplayY);
                }
            }

            if (chkVoronoiLine.Checked)
            {
                if (triangulation == null)
                    return;

                Voronoi voronoi = new Voronoi();
                var vornoiEdges = voronoi.GenerateEdgesFromDelaunay(triangulation);

                Pen penViolet = new Pen(Color.DarkViolet);
                foreach (var edge in vornoiEdges)
                {
                    //edge.Point1.DisplayX = (int)((edge.Point1.X / 300 + 0.5d)* _diameter) + _left;
                    //edge.Point1.DisplayY = (int)((-edge.Point1.Y / 300 + 0.5d)* _diameter) + _top;
                    //edge.Point2.DisplayX = (int)((edge.Point2.X / 300 + 0.5d)* _diameter) + _left;
                    //edge.Point2.DisplayY = (int)((-edge.Point2.Y / 300+ 0.5d)* _diameter) + _top;
                    edge.Point1.DisplayX = (int)((edge.Point1.X  + 0.5d) * _diameter) + _left;
                    edge.Point1.DisplayY = (int)((-edge.Point1.Y  + 0.5d) * _diameter) + _top;
                    edge.Point2.DisplayX = (int)((edge.Point2.X  + 0.5d) * _diameter) + _left;
                    edge.Point2.DisplayY = (int)((-edge.Point2.Y  + 0.5d) * _diameter) + _top;
                    g.DrawLine(penViolet, edge.Point1.DisplayX, edge.Point1.DisplayY, edge.Point2.DisplayX, edge.Point2.DisplayY);
                }
            }
        }

        private Triangle GetTriangle(PPoint point)
        {
            foreach (Triangle triangle in triangulation)
            {
                var coord = BarycentricCoordinates(triangle, point);
                if (!coord.Any(cr => cr < 0))
                    return triangle;
            }

                return null;
        }

        //private Color GetColor(int val)
        //{
        //    if (val > 255)
        //        val = 255;
        //    if (val < 0)
        //        val = 0;

        //    int R = val * 2 - 255;
        //    R = R > 0 ? R : 0;
        //    int G = 255 - (int)Math.Abs((127.5 - val)) * 2;
        //    int B = 255 - val * 2;
        //    B = B > 0 ? B : 0;
        //    return Color.FromArgb(R, G, B);
        //}

        private void btnMakePoints_Click(object sender, EventArgs e)
        {
            _points.Clear();
            //int iCount;

            //if (!Int32.TryParse(txtPointsCount.Text, out iCount))
            //    return;
            //Random random = new Random(DateTime.Now.Millisecond);

            //for (int i = 0; i < iCount; i++)
            //{
            //    double X = random.NextDouble();
            //    double ratioY = X > 0.5 ? Math.Sqrt(0.25 - (X - 0.5) * (X - 0.5)) * 2 : Math.Sqrt(0.25 - (0.5 - X) * (0.5 - X)) * 2;
            //    double Y = random.NextDouble();
            //    Y = (Y - 0.5) * ratioY + 0.5;
            //    PPoint pPoint = new PPoint(X, Y, random.Next(255));
            //    //pPoint.X = ;
            //    //pPoint.Y = ;
            //    //pPoint.VAL = ;
            //    _points.Add(pPoint);
            //}

            double[][] xy = new double[][]
            {
               new double[]{ -117.848923, -8.603366 , 2495.85              },
               new double[]{ 8.441828, -127.453359  , 2488.99              },
               new double[]{ 8.421086, -8.581343     , 2472.69          },
               new double[]{ 8.400363, 110.290673   , 2504.82             },
               new double[]{ 109.437111, -8.563725  , 2499.2               },
               new double[]{ 58.913554, 80.581478   , 2513.02             },
               new double[]{ -67.356465, 80.559455  , 2499.88              },
               new double[]{ -67.330549, -68.030585 , 2509.72           },
               new double[]{ 58.93947, -68.008542    , 2501.78          },
               new double[]{ 58.929103, -8.572534    , 2499.56          },
               new double[]{ 8.420729, 50.854665     , 2493.73          },
               new double[]{ -67.340916, -8.594557   , 2505.14          },
               new double[]{ 8.431462, -68.017351    , 2501.39          },
            };

            double[] z = new double[]
            {
                2495.85,
                2488.99,
                2472.69,
                2504.82,
                2499.2,
                2513.02,
                2499.88,
                2509.72,
                2501.78,
                2499.56,
                2493.73,
                2505.14,
                2501.39
            };

            foreach (var co in xy)
            {
                double X = co[0] / 300;
                double Y = co[1] / 300;
                double Z = co[2];
                PPoint pPoint = new PPoint(X, Y, Z);
                _points.Add(pPoint);
            }

            pnlCanvas.Invalidate();
        }

        private void chkInterpolation_CheckedChanged(object sender, EventArgs e)
        {
            pnlCanvas.Invalidate();
        }

        private void btnDelaunai_Click(object sender, EventArgs e)
        {
            // chkVoronoiLine.Checked = true;

            var points = new List<PPoint>();

            DelaunayTriangulator delaunay = new DelaunayTriangulator(1, 1);

            foreach (var point in _points)
            {
                points.Add(point);

                //if (points.Count > 2)
                //    DelaySystem(1000);
            }
            foreach (var pt in points)
            {
                pt.AdjacentPoints.Clear();
                pt.AdjacentTriangles.Clear();
            }
            triangulation = delaunay.BowyerWatson(points);

            foreach (var triangle in triangulation)
            {
                foreach (var vertex in triangle.Vertices)
                {
                    vertex.AdjacentPoints.Clear();
                }
            }

            foreach (var triangle in triangulation)
            {
                foreach(var vertex in triangle.Vertices)
                {
                    foreach (var vertexOther in triangle.Vertices)
                    {
                        if (vertex == vertexOther)
                            continue;

                        vertex.AdjacentPoints.Add(vertexOther);
                    }
                }
            }

            int maxiter = EstimateGradientsGlobal(points);

            pnlCanvas.Invalidate();
        }

        private void PreEstimateGradients2dGlobal(List<PPoint> points, IEnumerable<Triangle> triangles)
        {
            int iCountPointInTriangles = triangles.Select(tri => tri.Vertices).Distinct().Count();
            int iCountPoints = points.Count;

            if (iCountPointInTriangles != iCountPoints)
                throw new Exception("Points count is wrong number");
        }

        private int EstimateGradientsGlobal(List<PPoint> points)
        {
            for (int i = 0; i < 400; i++)
            {
                double err = 0;
                foreach (var point in points)
                {
                    var Q = new double[] { 0, 0, 0, 0 };
                    var s = new double[] { 0, 0 };
                    var r = new double[2];

                    // walk over neighbours of given point
                    foreach (var point2 in point.AdjacentPoints.Distinct())
                    {
                        // edge
                        double ex = point2.X - point.X;
                        double ey = point2.Y - point.Y;
                        double L = Math.Sqrt(ex * ex + ey * ey);
                        double L3 = L * L * L;

                        // data at vertices
                        var f1 = point.VAL;
                        var f2 = point2.VAL;

                        // scaled gradient projections on the edge
                        var df2 = -ex * point2.Dx - ey * point2.Dy;

                        Q[0] += 4 * ex * ex / L3;
                        Q[1] += 4 * ex * ey / L3;
                        Q[3] += 4 * ey * ey / L3;

                        s[0] += (6 * (f1 - f2) - 2 * df2) * ex / L3;
                        s[1] += (6 * (f1 - f2) - 2 * df2) * ey / L3;
                    }
                    Q[2] = Q[1];

                    var det = Q[0] * Q[3] - Q[1] * Q[2];
                    r[0] = (Q[3] * s[0] - Q[1] * s[1]) / det;
                    r[1] = (-Q[2] * s[0] + Q[0] * s[1]) / det;

                    var change = Math.Max(Math.Abs(point.Dx + r[0]), Math.Abs(point.Dy + r[1]));
                    point.Dx = -r[0];
                    point.Dy = -r[1];

                    var divisor = new List<double> { 1, Math.Abs(r[0]), Math.Abs(r[1]) }.Max();
                    change /= divisor;

                    err = Math.Max(err, change);
                }

                if (err < 1E-10)
                    return i + 1;
            }

            return 0;
        }

        private double CloughTocher2dSingle(Triangle triangle, PPoint point)
        {
            var pt0 = triangle.Vertices[0];
            var pt1 = triangle.Vertices[1];
            var pt2 = triangle.Vertices[2];
 
            var f1 = pt0.VAL;
            var f2 = pt1.VAL;
            var f3 = pt2.VAL;

            var df0x = pt0.Dx;
            var df0y = pt0.Dy;
            var df1x = pt1.Dx;
            var df1y = pt1.Dy;
            var df2x = pt2.Dx;
            var df2y = pt2.Dy;

            var e12x = pt1.X - pt0.X;
            var e12y = pt1.Y - pt0.Y;
            var e23x = pt2.X - pt1.X;
            var e23y = pt2.Y - pt1.Y;
            var e31x = pt0.X - pt2.X;
            var e31y = pt0.Y - pt2.Y;

            //var e14x = (e12x - e31x) / 3;
            //var e14y = (e12y - e31y) / 3;
            //var e24x = (-e12x + e23x) / 3;
            //var e24y = (-e12y + e23y) / 3;
            //var e34x = (e31x - e23x) / 3;
            //var e34y = (e31y - e23y) / 3;

            var df12 = +(df0x * e12x + df0y * e12y);
            var df21 = -(df1x * e12x + df1y * e12y);
            var df23 = +(df1x * e23x + df1y * e23y);
            var df32 = -(df2x * e23x + df2y * e23y);
            var df31 = +(df2x * e31x + df2y * e31y);
            var df13 = -(df0x * e31x + df0y * e31y);

            var c3000 = f1;
            var c2100 = (df12 + 3 * c3000) / 3;
            var c2010 = (df13 + 3 * c3000) / 3;
            var c0300 = f2;
            var c1200 = (df21 + 3 * c0300) / 3;
            var c0210 = (df23 + 3 * c0300) / 3;
            var c0030 = f3;
            var c1020 = (df31 + 3 * c0030) / 3;
            var c0120 = (df32 + 3 * c0030) / 3;

            var c2001 = (c2100 + c2010 + c3000) / 3;
            var c0201 = (c1200 + c0300 + c0210) / 3;
            var c0021 = (c1020 + c0120 + c0030) / 3;

            #region 
            // Now, we need to impose the condition that the gradient of the spline
            // to some direction `w` is a linear function along the edge.
            //
            // As long as two neighbouring triangles agree on the choice of the
            // direction `w`, this ensures global C1 differentiability.
            // Otherwise, the choice of the direction is arbitrary (except that
            // it should not point along the edge, of course).
            //
            // In [CT]_, it is suggested to pick `w` as the normal of the edge.
            // This choice is given by the formulas
            //
            // w_12 = E_24 + g[0] * E_23
            // w_23 = E_34 + g[1] * E_31
            // w_31 = E_14 + g[2] * E_12
            //
            // g[0] = -(e24x*e23x + e24y*e23y) / (e23x**2 + e23y**2)
            // g[1] = -(e34x*e31x + e34y*e31y) / (e31x**2 + e31y**2)
            // g[2] = -(e14x*e12x + e14y*e12y) / (e12x**2 + e12y**2)
            //
            // However, this choice gives an interpolant that is *not*
            // invariant under affine transforms. This has some bad
            // consequences: for a very narrow triangle, the spline can
            // develops huge oscillations. For instance, with the input data
            //
            //     [(0, 0), (0, 1), (eps, eps)],   eps = 0.01
            // F  = [0, 0, 1]
            // dF = [(0,0), (0,0), (0,0)]
            //
            // one observes that as eps -> 0, the absolute maximum value of the
            // interpolant approaches infinity.
            //
            // So below, we aim to pick affine invariant `g[k]`.
            // We choose
            //
            // w = V_4' - V_4
            //
            // where V_4 is the centroid of the current triangle, and V_4' the
            // centroid of the neighbour. Since this quantity transforms similarly
            // as the gradient under affine transforms, the resulting interpolant
            // is affine-invariant. Moreover, two neighbouring triangles clearly
            // always agree on the choice of `w` (sign is unimportant), and so
            // this choice also makes the interpolant C1.
            //
            // The drawback here is a performance penalty, since we need to
            // peek into neighbouring triangles.
            #endregion

            double[] g = new double[3];

            for (int k = 0; k < 3; k++)
            {
                var itri = triangle.TriangleWithSharedEdge(k);
                if (itri == null)
                {
                    //# No neighbour.
                    //# Compute derivative to the centroid direction (e_12 + e_13)/2.
                    g[k] = -1.0d / 2;
                    continue;
                }

                // Centroid of the neighbour, in our local barycentric coordinates
                List<IEnumerable<double>> y = new List<IEnumerable<double>>();
                var x0 = itri.Vertices.Sum(vt => vt.X) / 3;
                var y0 = itri.Vertices.Sum(vt => vt.Y) / 3;
                PPoint pt = new PPoint(x0, y0, 0);

                var c =BarycentricCoordinates(triangle, pt);

                // Rewrite V_4'-V_4 = const*[(V_4-V_2) + g_i*(V_3 - V_2)]

                // Now, observe that the results can be written *in terms of
                // barycentric coordinates*. Barycentric coordinates stay
                // invariant under affine transformations, so we can directly
                // conclude that the choice below is affine-invariant.

                

                if (k == 0)
                    g[k] = (2 * c[2] + c[1] - 1) / (2 - 3 * c[2] - 3 * c[1]);
                else if (k == 1)
                    g[k] = (2 * c[0] + c[2] - 1) / (2 - 3 * c[0] - 3 * c[2]);
                else if (k == 2)
                    g[k] = (2 * c[1] + c[0] - 1) / (2 - 3 * c[1] - 3 * c[0]);
            }
            var c0111 = (g[0] * (-c0300 + 3 * c0210 - 3 * c0120 + c0030)
                     + (-c0300 + 2 * c0210 - c0120 + c0021 + c0201)) / 2;
            var c1011 = (g[1] * (-c0030 + 3 * c1020 - 3 * c2010 + c3000)
                     + (-c0030 + 2 * c1020 - c2010 + c2001 + c0021)) / 2;
            var c1101 = (g[2] * (-c3000 + 3 * c2100 - 3 * c1200 + c0300)
                     + (-c3000 + 2 * c2100 - c1200 + c2001 + c0201)) / 2;

            var c1002 = (c1101 + c1011 + c2001) / 3;
            var c0102 = (c1101 + c0111 + c0201) / 3;
            var c0012 = (c1011 + c0111 + c0021) / 3;

            var c0003 = (c1002 + c0102 + c0012) / 3;

            //# extended barycentric coordinates
            var b = BarycentricCoordinates(triangle, point);
            var minval = b.Min();

            var b1 = b[0] - minval;
            var b2 = b[1] - minval;
            var b3 = b[2] - minval;
            var b4 = 3 * minval;

            //    # evaluate the polynomial -- the stupid and ugly way to do it,
            //# one of the 4 coordinates is in fact zero
            var w = (b1 * b1 * b1 * c3000 + 3 * b1 * b1 * b2 * c2100 + 3 * b1 * b1 * b3 * c2010
                  + 3 * b1 * b1 * b4 * c2001 + 3 * b1 * b2 * b2 * c1200 + 6 * b1 * b2 * b4 * c1101
                  + 3 * b1 * b3 * b3 * c1020 + 6 * b1 * b3 * b4 * c1011 + 3 * b1 * b4 * b4 * c1002
                       + b2 * b2 * b2 * c0300 + 3 * b2 * b2 * b3 * c0210 + 3 * b2 * b2 * b4 * c0201
                  + 3 * b2 * b3 * b3 * c0120 + 6 * b2 * b3 * b4 * c0111 + 3 * b2 * b4 * b4 * c0102
                       + b3 * b3 * b3 * c0030 + 3 * b3 * b3 * b4 * c0021 + 3 * b3 * b4 * b4 * c0012
                       + b4 * b4 * b4 * c0003);

            if (w > 2513 || w < 2471)
                ;

            return w;
        }

        private void DelaySystem(int ms)
        {
            DateTime dtAfter = DateTime.Now;
            TimeSpan dtDuration = new TimeSpan(0, 0, 0, 0, ms);
            DateTime dtThis = dtAfter.Add(dtDuration);
            while (dtThis >= dtAfter)
            {
                System.Windows.Forms.Application.DoEvents();
                dtAfter = DateTime.Now;
            }
        }

        private double[] BarycentricCoordinates(Triangle tri, PPoint point)
        {
            // int ndim = 2;
            double[] c = new double[3];
            // Compute barycentric coordinates.
            double x1 = tri.Vertices[0].X;
            double y1 = tri.Vertices[0].Y;
            double x2 = tri.Vertices[1].X;
            double y2 = tri.Vertices[1].Y;
            double x3 = tri.Vertices[2].X;
            double y3 = tri.Vertices[2].Y;

            double x = point.X;
            double y = point.Y;

            // https://en.wikipedia.org/wiki/Barycentric_coordinate_system
            double T = (y2 - y3) * (x1 - x3) + (x3 - x2) * (y1 - y3);
            c[0] = (y2 - y3) * (x - x3) + (x3 - x2) * (y - y3);
            c[0] = c[0] / T;
            c[1] = (y3 - y1) * (x - x3) + (x1 - x3) * (y - y3);
            c[1] = c[1] / T;
            c[2] = 1 - c[0] - c[1];

            return c;
        }

        public DataTable ArraytoDatatable(double[,] numbers)
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < numbers.GetLength(1); i++)
            {
                dt.Columns.Add("Column" + (i + 1));
            }

            for (var i = 0; i < numbers.GetLength(0); ++i)
            {
                DataRow row = dt.NewRow();
                for (var j = 0; j < numbers.GetLength(1); ++j)
                {
                    row[j] = numbers[i, j];
                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        private void SetColorMap()
        {
            rcs = new List<RateColor>();

            RateColor rc0 = new RateColor();
            rc0.MIN = 0;
            rc0.MAX = 12;
            rc0.COLOR = Color.FromArgb(0, 0, 192);
            rcs.Add(rc0);

            RateColor rc1 = new RateColor();
            rc1.MIN = 12;
            rc1.MAX = 25;
            rc1.COLOR = Color.FromArgb(0, 64, 255); ;// Color.DarkBlue;
            rcs.Add(rc1);

            RateColor rc2 = new RateColor();
            rc2.MIN = 25;
            rc2.MAX = 37;
            rc2.COLOR = Color.FromArgb(0, 192, 255);// Color.LightBlue;
            rcs.Add(rc2);

            RateColor rc3 = new RateColor();
            rc3.MIN = 37;
            rc3.MAX = 50;
            rc3.COLOR = Color.FromArgb(64, 255, 192);
            rcs.Add(rc3);

            RateColor rc4 = new RateColor();
            rc4.MIN = 50;
            rc4.MAX = 62;
            rc4.COLOR = Color.FromArgb(192, 255, 64);
            rcs.Add(rc4);

            RateColor rc5 = new RateColor();
            rc5.MIN = 62;
            rc5.MAX = 75;
            rc5.COLOR = Color.FromArgb(255, 192, 0);
            rcs.Add(rc5);

            RateColor rc6 = new RateColor();
            rc6.MIN = 75;
            rc6.MAX = 87;
            rc6.COLOR = Color.FromArgb(255, 64, 0);
            rcs.Add(rc6);

            RateColor rc7 = new RateColor();
            rc7.MIN = 87;
            rc7.MAX = 100;
            rc7.COLOR = Color.FromArgb(192, 0, 0);
            rcs.Add(rc7);
        }

        private Color GetColor(double rate)
        {


            var qColor = from rc in rcs
                         where rc.MAX >= rate && rc.MIN <= rate
                         select rc;

            if (qColor == null || !qColor.Any())
                return Color.Black;

            var color = qColor.First();

            var midVal = (color.MAX + color.MIN) / 2;

            if (rate == midVal)
                return color.COLOR;
            else if ((rate > midVal && color.MAX != 100) || color.MIN == 0)
            {
                var cUp = rcs.Where(c => c.MIN == color.MAX).First();
                var upMidVal = (cUp.MAX + cUp.MIN) / 2;
                var a = upMidVal - rate;
                var b = rate - midVal;
                var R = (color.COLOR.R * a + b * cUp.COLOR.R) / (a + b);
                var G = (color.COLOR.G * a + b * cUp.COLOR.G) / (a + b);
                var B = (color.COLOR.B * a + b * cUp.COLOR.B) / (a + b);

                R = R > 255 ? 255 : R < 0 ? 0 : R;
                G = G > 255 ? 255 : G < 0 ? 0 : G;
                B = B > 255 ? 255 : B < 0 ? 0 : B;

                Color col = Color.FromArgb((byte)R, (byte)G, (byte)B);
                return col;
            }
            else if ((rate < midVal && color.MIN != 0) || color.MAX == 100)
            {
                var cDn = rcs.Where(c => c.MAX == color.MIN).First();
                var downMidVal = (cDn.MAX + cDn.MIN) / 2;
                var a = rate - downMidVal;
                var b = midVal - rate;
                var R = (color.COLOR.R * a + b * cDn.COLOR.R) / (a + b);
                var G = (color.COLOR.G * a + b * cDn.COLOR.G) / (a + b);
                var B = (color.COLOR.B * a + b * cDn.COLOR.B) / (a + b);

                R = R > 255 ? 255 : R < 0 ? 0 : R;
                G = G > 255 ? 255 : G < 0 ? 0 : G;
                B = B > 255 ? 255 : B < 0 ? 0 : B;

                Color col = Color.FromArgb((byte)R, (byte)G, (byte)B);
                return col;
            }
            else
                return Color.Silver;
        }
    }

    public class RateColor
    {
        public float MIN { get; set; }
        public float MAX { get; set; }
        public Color COLOR { get; set; }
    }
}
