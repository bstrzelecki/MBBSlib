using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace MBBSlib.AI
{
    public class Pathfinding
    {
        float[,] map;
        int maxX = 0;
        int maxY = 0;
        public Pathfinding(float[,] map)
        {
            this.map = map;
            maxX = map.GetUpperBound(0);
            maxY = map.GetUpperBound(1);
        }
        float heuristic_cost_estimate(Point start, Point end)
        {

            return (float)Math.Sqrt(
            Math.Pow(start.X -end.X, 2) +
            Math.Pow(start.Y -end.Y, 2)
        );


        }
        private List<Point> GetNeighbors(Point point) {
            List<Point> points = new List<Point>();
            int x = point.X;
            int y = point.Y;
            AddPoint(new Point(x + 1, y), points);
            AddPoint(new Point(x - 1, y), points);
            AddPoint(new Point(x, y + 1), points);
            AddPoint(new Point(x, y - 1), points);

            AddPoint(new Point(x + 1, y + 1), points);
            AddPoint(new Point(x + 1, y - 1), points);
            AddPoint(new Point(x - 1, y + 1), points);
            AddPoint(new Point(x - 1, y - 1), points);
            //TODO add more connections

            return points;
        }
        private void AddPoint(Point p, List<Point> pp)
        {
            if (CheckPoint(p))
                pp.Add(p);
        }
        private bool CheckPoint(Point p)
        {
            if (p.X >= 0 && p.X < maxX + 1 && p.Y >= 0 && p.Y < maxY + 1) return true;
            return false;
        }
        public List<Point> GetPath(Point start, Point end)
        {
            List<Point> evaluated = new List<Point>();

            List<Point> discovered = new List<Point>();
            discovered.Add(start);

            Dictionary<Point, Point> origins = new Dictionary<Point, Point>();
            Dictionary<Point, float> scores = new Dictionary<Point, float>();
            for (int x = 0; x < map.GetUpperBound(0); x++)
            {
                for (int y = 0; y < map.GetUpperBound(1); y++)
                {
                    scores.Add(new Point(x, y), float.PositiveInfinity);
                }
            }
            scores[start] = 0;

            Dictionary<Point, float> finalScores = new Dictionary<Point, float>();
            for(int x = 0; x < map.GetUpperBound(0); x++)
            {
                for(int y = 0; y < map.GetUpperBound(1); y++)
                {
                    finalScores.Add(new Point(x, y), float.PositiveInfinity);
                }
            }

            finalScores[start] = heuristic_cost_estimate(start, end);

            while(discovered.Count > 0)
            {
                //TODO optimalize
                Point current = discovered.OrderBy(n => finalScores[n]).First();

                if (current == end)
                    return ReconstructPath(origins, current);

                discovered.Remove(current);
                evaluated.Add(current);

                foreach(Point point in GetNeighbors(current))
                {
                    if (ContainsPoint(evaluated, point)) continue;

                    float tScore = scores[current] + (Distance(current, point) * map[point.X, point.Y]);

                    if (!discovered.Contains(point))
                    {
                        discovered.Add(point);
                    }
                    else if (tScore >= scores[point])
                    {
                        continue;
                    }
                    if (!ContainsPoint(origins, point))
                        origins.Add(point, current);
                    else
                        origins[point] = current;
                    scores[point] = tScore;
                    finalScores[point] = scores[point] + heuristic_cost_estimate(point, end);
                }
            }
            throw new Exception("Something went wrong");
        }

        private static bool ContainsPoint(List<Point> evaluated, Point point)
        {
            foreach(Point p in evaluated)
            {
                if (p == point) return true;
            }
            return false;
        }
        private static bool ContainsPoint(Dictionary<Point, Point> evaluated, Point point)
        {
            foreach (Point p in evaluated.Keys)
            {
                if (p == point) return true;
            }
            return false;
        }

        private float Distance(Point a, Point b)
        {
            var ac = b.X - a.X;
            var bc = b.Y - a.Y;

            return (float)Math.Sqrt(ac * ac + bc * bc);
        }

        private List<Point> ReconstructPath(Dictionary<Point, Point> origins, Point current)
        {
            List<Point> path = new List<Point>();
            path.Add(current);
            while (origins.ContainsKey(current))
            {
                current = origins[current];
                path.Add(current);
            }
            return path;
        }
    }
}
