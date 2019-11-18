using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace MBBSlib.AI
{
    public class Pathfinding
    {
        readonly float[,] map;
        readonly int maxX = 0;
        readonly int maxY = 0;
        float GetTileValue(Point p)
        {
            if(CheckPoint(p))
            return map[p.X, p.Y];
            return float.MaxValue;
        }
        float GetTileValue(Point p, int offX, int offY)
        {
            return GetTileValue(new Point(p.X + offX, p.Y + offY));
        }
        public bool IgnoreClipping { get; set; } = false;
        public Pathfinding(float[,] map)
        {
            this.map = map;
            maxX = map.GetUpperBound(0);
            maxY = map.GetUpperBound(1);
        }
        private float HeuristicCostEstimate(Point start, Point end)
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

            if(GetTileValue(point, 1, 0) != float.MaxValue && GetTileValue(point, 0, 1) != float.MaxValue)
                AddPoint(new Point(x + 1, y + 1), points);
            if (GetTileValue(point, 1, 0) != float.MaxValue && GetTileValue(point, 0, -1) != float.MaxValue)
                AddPoint(new Point(x + 1, y - 1), points);
            if (GetTileValue(point, -1, 0) != float.MaxValue && GetTileValue(point, 0, 1) != float.MaxValue)
                AddPoint(new Point(x - 1, y + 1), points);
            if (GetTileValue(point, -1, 0) != float.MaxValue && GetTileValue(point, 0, -1) != float.MaxValue)
                AddPoint(new Point(x - 1, y - 1), points);

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

            List<Point> discovered = new List<Point>
            {
                start
            };

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

            finalScores[start] = HeuristicCostEstimate(start, end);

            while(discovered.Count > 0)
            {
                //TODO optimalize
                Point current = discovered.OrderBy(n => finalScores[n]).Take(1).Single();
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
                    finalScores[point] = scores[point] + HeuristicCostEstimate(point, end);
                }
            }
            throw new Exception("Something went wrong");
        }

        public List<Point> GetPath(List<Point> starts, Point target)
        {
            List<Point> evaluated = new List<Point>();

            List<Point> discovered = new List<Point>();
            foreach(Point s in starts)
            {
                discovered.Add(s);

            }

            Dictionary<Point, Point> origins = new Dictionary<Point, Point>();
            Dictionary<Point, float> scores = new Dictionary<Point, float>();

            for (int x = 0; x < map.GetUpperBound(0); x++)
            {
                for (int y = 0; y < map.GetUpperBound(1); y++)
                {
                    scores.Add(new Point(x, y), float.PositiveInfinity);
                }
            }
            foreach(Point s in starts)
            {
                scores[s] = 0;
            }
            Dictionary<Point, float> finalScores = new Dictionary<Point, float>();
            for (int x = 0; x < map.GetUpperBound(0); x++)
            {
                for (int y = 0; y < map.GetUpperBound(1); y++)
                {
                    finalScores.Add(new Point(x, y), float.PositiveInfinity);
                }
            }

            foreach(Point s in starts)
            {
                finalScores[s] = HeuristicCostEstimate(s, target);
            }
            while (discovered.Count > 0)
            {
                //TODO optimalize
                Point current = discovered.OrderBy(n => finalScores[n]).Take(1).Single();
                if (current == target)
                    return ReconstructPath(origins, current);

                discovered.Remove(current);
                evaluated.Add(current);

                foreach (Point point in GetNeighbors(current))
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
                    finalScores[point] = scores[point] + HeuristicCostEstimate(point, target);
                }
            }
            return null;
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
            List<Point> path = new List<Point>
            {
                current
            };
            while (origins.ContainsKey(current))
            {
                current = origins[current];
                path.Add(current);
            }
            return path;
        }
    }
}
