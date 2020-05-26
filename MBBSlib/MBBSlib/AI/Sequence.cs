using System;
using System.Collections.Generic;
using System.Text;

namespace MBBSlib.AI
{
    public class Sequence : Node, ISelector
    {
        Node[] _nodes;
        Func<int> _condition;

        public Sequence(Func<int> condition, params Node[] nodes)
        {
            _nodes = nodes;
            _condition = condition;
        }

        public Node GetNode() => _nodes[_condition.Invoke()];
    }
}
