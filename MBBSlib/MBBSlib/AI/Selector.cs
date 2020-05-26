using System;
using System.Collections.Generic;
using System.Text;

namespace MBBSlib.AI
{
    public class Selector : Node, ISelector
    {
        public Node OnTrue;
        public Node OnFalse;
        Func<bool> _condition;
        public Selector(Func<bool> condition, Node trueNode, Node falseNode)
        {
            _condition = condition;
            OnTrue = trueNode;
            OnFalse = falseNode;
        }
        public Node GetNode() => _condition.Invoke() ? OnTrue : OnFalse;
    }
}
