using System;
using System.Collections.Generic;
using System.Text;

namespace MBBSlib.AI
{
    /// <summary>
    /// Interfeace for <see cref="BehaviorTree"/> branch selectors
    /// </summary>
    public interface ISelector
    {
        /// <summary>
        /// Chooses nodes acording to set condition
        /// </summary>
        /// <returns>Next node</returns>
        Node GetNode();
    }
}
