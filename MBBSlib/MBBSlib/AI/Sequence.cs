using System;
using System.Collections.Generic;
using System.Text;

namespace MBBSlib.AI
{
    /// <summary>
    /// More advanced version of <see cref="Selector"/> for <see cref="BehaviorTree"/>
    /// </summary>
    public class Sequence : Node, ISelector
    {
        Node[] _nodes;
        Func<int> _condition;

        /// <summary>
        /// COnstructor for ai sequence
        /// </summary>
        /// <param name="condition">Integer function</param>
        /// <param name="nodes">Index of <paramref name="nodes"/>must corespond to <paramref name="condition"/> output</param>
        public Sequence(Func<int> condition, params Node[] nodes)
        {
            _nodes = nodes;
            _condition = condition;
        }
        /// <summary>
        /// Implementation of <see cref="ISelector"/>
        /// </summary>
        /// <returns><seealso cref="ISelector"/></returns>
        public Node GetNode() => _nodes[_condition.Invoke()];
    }
}
