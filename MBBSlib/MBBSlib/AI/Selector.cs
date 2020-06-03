using System;

namespace MBBSlib.AI
{
    /// <summary>
    /// Provedes next node in <see cref="BehaviorTree"/> (binary selector)
    /// </summary>
    public class Selector : Node, ISelector
    {
        /// <summary>
        /// Node that will bve returned by <see cref="GetNode()"/> when <see cref="Selector._condition"/> returns true
        /// </summary>
        public Node OnTrue;
        /// <summary>
        /// Node that will bve returned by <see cref="GetNode()"/> when <see cref="Selector._condition"/> returns false
        /// </summary>
        public Node OnFalse;
        readonly Func<bool> _condition;
        /// <summary>
        /// Default constructor for <see cref="BehaviorTree"/> binary selector
        /// </summary>
        /// <param name="condition">Condition of <see cref="Selector"/></param>
        /// <param name="trueNode"><see cref="OnTrue"/></param>
        /// <param name="falseNode"><see cref="OnFalse"/></param>
        public Selector(Func<bool> condition, Node trueNode, Node falseNode)
        {
            _condition = condition;
            OnTrue = trueNode;
            OnFalse = falseNode;
        }
        /// <summary>
        /// Implementation of <see cref="ISelector"/>
        /// </summary>
        /// <returns><seealso cref="ISelector"/></returns>
        public Node GetNode() => _condition.Invoke() ? OnTrue : OnFalse;
    }
}
