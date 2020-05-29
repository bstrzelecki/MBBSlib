using System;
using System.Collections.Generic;
using System.Text;

namespace MBBSlib.AI
{
    /// <summary>
    /// End point of behavior tree
    /// </summary>
    public abstract class Leaf : Node
    {
        /// <summary>
        /// Calculates ai actions
        /// </summary>
        public abstract void Execute();
    }
}
