using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MBBSlib.AI;

namespace MBBSUnitTests
{
    [TestClass]
    public class PathfindingTests
    {
        [TestMethod]
        public void NoErrorsAllPosibilities()
        {
            float[,] map = new float[16, 16];
            map.Initialize();
            for (int xs = 0; xs < 16; xs++)
            {
                for (int ys = 0; ys < 16; ys++)
                {
                    map[xs, ys] = 1;
                }
            }

            Pathfinding pathfinding = new Pathfinding(map);
            int errors = 0;
            Point firstFail = new Point(0,0);
            Point firstFailend = new Point(0,0);
            for (int xs = 0; xs < 16; xs++)
            {
                for(int ys = 0; ys < 16; ys++)
                {
                    Point p = new Point(xs, ys);
                    for (int xe = 0; xe < 16; xe++)
                    {
                        for (int ye = 0; ye < 16; ye++)
                        {
                            Point e = new Point(xe, ye);
                            if (e == p) continue;
                            try
                            {
                                pathfinding.GetPath(p, e);
                            }
                            catch 
                            {
                                if (firstFail == new Point(0,0))
                                    firstFail = p;
                                if (firstFailend == new Point(0, 0))
                                    firstFailend = e;
                                errors++;
                            }
                        }
                    }
                }
            }
            Assert.IsTrue(errors == 0, $"Errors in test: {errors}, first failed at {firstFail} and {firstFailend}");
        }
    }
}
