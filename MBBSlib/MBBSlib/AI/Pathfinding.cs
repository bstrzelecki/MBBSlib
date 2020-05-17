using System;
using System.Collections.Generic;
using System.Linq;

namespace MBBSlib.AI
{
    public class Pathfinding
    {
        readonly float[,] _map;
        readonly int _maxX = 0;
        readonly int _maxY = 0;
        float GetTileValue(Point p) => CheckPoint(p) ? _map[p.X, p.Y] : float.MaxValue;
        float GetTileValue(Point p, int offX, int offY) => GetTileValue(new Point(p.X + offX, p.Y + offY));
        /// <summary>
        /// If true will ignore choke points
        /// </summary>
        public bool IgnoreClipping { get; set; } = false;

        /// <summary>
        /// Crerates pathfinding instance
        /// </summary>
        /// <param name="map">weight map</param>
        public Pathfinding(float[,] map)
        {
            this._map = map;
            _maxX = map.GetUpperBound(0);
            _maxY = map.GetUpperBound(1);
        }
        private float HeuristicCostEstimate(Point start, Point end) => (float)System.Math.Sqrt(
            System.Math.Pow(start.X - end.X, 2) +
            System.Math.Pow(start.Y - end.Y, 2)
        );
        private List<Point> GetNeighbors(Point point)
        {
            var points = new List<Point>();
            int x = point.X;
            int y = point.Y;
            AddPoint(new Point(x + 1, y), points);
            AddPoint(new Point(x - 1, y), points);
            AddPoint(new Point(x, y + 1), points);
            AddPoint(new Point(x, y - 1), points);

            if(GetTileValue(point, 1, 0) != float.MaxValue && GetTileValue(point, 0, 1) != float.MaxValue)
                AddPoint(new Point(x + 1, y + 1), points);
            if(GetTileValue(point, 1, 0) != float.MaxValue && GetTileValue(point, 0, -1) != float.MaxValue)
                AddPoint(new Point(x + 1, y - 1), points);
            if(GetTileValue(point, -1, 0) != float.MaxValue && GetTileValue(point, 0, 1) != float.MaxValue)
                AddPoint(new Point(x - 1, y + 1), points);
            if(GetTileValue(point, -1, 0) != float.MaxValue && GetTileValue(point, 0, -1) != float.MaxValue)
                AddPoint(new Point(x - 1, y - 1), points);

            return points;
        }
        private void AddPoint(Point p, List<Point> pp)
        {
            if(CheckPoint(p))
                pp.Add(p);
        }
        private bool CheckPoint(Point p) => p.X >= 0 && p.X < _maxX + 1 && p.Y >= 0 && p.Y < _maxY + 1;
        /// <summary>
        /// Finds shortest bath from point A to point B
        /// </summary>
        /// <param name="start">Entry point</param>
        /// <param name="end">Destination</param>
        /// <returns></returns>
        public List<Point> GetPath(Point start, Point end)
        {
            var evaluated = new List<Point>();

            var discovered = new List<Point>
            {
                start
            };

            var origins = new Dictionary<Point, Point>();
            var scores = new Dictionary<Point, float>();
            for(int x = 0; x < _map.GetUpperBound(0); x++)
            {
                for(int y = 0; y < _map.GetUpperBound(1); y++)
                {
                    scores.Add(new Point(x, y), float.PositiveInfinity);
                }
            }
            scores[start] = 0;

            var finalScores = new Dictionary<Point, float>();
            for(int x = 0; x < _map.GetUpperBound(0); x++)
            {
                for(int y = 0; y < _map.GetUpperBound(1); y++)
                {
                    finalScores.Add(new Point(x, y), float.PositiveInfinity);
                }
            }

            finalScores[start] = HeuristicCostEstimate(start, end);

            while(discovered.Count > 0)
            {
                //TODO optimalize
                Point current = discovered.OrderBy(n => finalScores[n]).Take(1).Single();
                if(current == end)
                    return ReconstructPath(origins, current);

                discovered.Remove(current);
                evaluated.Add(current);

                foreach(Point point in GetNeighbors(current))
                {
                    if(ContainsPoint(evaluated, point)) continue;

                    float tScore = scores[current] + (Distance(current, point) * _map[point.X, point.Y]);

                    if(!discovered.Contains(point))
                    {
                        discovered.Add(point);
                    }
                    else if(tScore >= scores[point])
                    {
                        continue;
                    }
                    if(!ContainsPoint(origins, point))
                        origins.Add(point, current);
                    else
                        origins[point] = current;
                    scores[point] = tScore;
                    finalScores[point] = scores[point] + HeuristicCostEstimate(point, end);
                }
            }
            throw new Exception("Something went wrong");
        }
        /// <summary>
        /// Finds shortest path from closest point
        /// </summary>
        /// <param name="starts">Entry point</param>
        /// <param name="target">Destination</param>
        /// <returns></returns>
        public List<Point> GetPath(List<Point> starts, Point target)
        {
            var evaluated = new List<Point>();

            var discovered = new List<Point>();
            foreach(Point s in starts)
            {
                discovered.Add(s);

            }

            var origins = new Dictionary<Point, Point>();
            var scores = new Dictionary<Point, float>();

            for(int x = 0; x < _map.GetUpperBound(0); x++)
            {
                for(int y = 0; y < _map.GetUpperBound(1); y++)
                {
                    scores.Add(new Point(x, y), float.PositiveInfinity);
                }
            }
            foreach(Point s in starts)
            {
                scores[s] = 0;
            }
            var finalScores = new Dictionary<Point, float>();
            for(int x = 0; x < _map.GetUpperBound(0); x++)
            {
                for(int y = 0; y < _map.GetUpperBound(1); y++)
                {
                    finalScores.Add(new Point(x, y), float.PositiveInfinity);
                }
            }

            foreach(Point s in starts)
            {
                finalScores[s] = HeuristicCostEstimate(s, target);
            }
            while(discovered.Count > 0)
            {
                //TODO optimalize
                Point current = discovered.OrderBy(n => finalScores[n]).Take(1).Single();
                if(current == target)
                    return ReconstructPath(origins, current);

                discovered.Remove(current);
                evaluated.Add(current);

                foreach(Point point in GetNeighbors(current))
                {
                    if(ContainsPoint(evaluated, point)) continue;

                    float tScore = scores[current] + (Distance(current, point) * _map[point.X, point.Y]);

                    if(!discovered.Contains(point))
                    {
                        discovered.Add(point);
                    }
                    else if(tScore >= scores[point])
                    {
                        continue;
                    }
                    if(!ContainsPoint(origins, point))
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
                if(p == point) return true;
            }
            return false;
        }
        private static bool ContainsPoint(Dictionary<Point, Point> evaluated, Point point)
        {
            foreach(Point p in evaluated.Keys)
            {
                if(p == point) return true;
            }
            return false;
        }

        private float Distance(Point a, Point b)
        {
            var ac = b.X - a.X;
            var bc = b.Y - a.Y;

            return (float)System.Math.Sqrt(ac * ac + bc * bc);
        }

        private List<Point> ReconstructPath(Dictionary<Point, Point> origins, Point current)
        {
            var path = new List<Point>
            {
                current
            };
            while(origins.ContainsKey(current))
            {
                current = origins[current];
                path.Add(current);
            }
            return path;
        }
    }
}
