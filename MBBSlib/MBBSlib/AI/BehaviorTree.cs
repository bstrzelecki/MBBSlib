using System.Collections.Generic;

namespace MBBSlib.AI
{
    /// <summary>
    /// Ai calculation pattern
    /// </summary>
    public class BehaviorTree
    {
        /// <summary>
        /// Entry point for calculating ai
        /// </summary>
        public RootNode Root { get; }
        /// <summary>
        /// Creates tree with empty root node
        /// </summary>
        public BehaviorTree() => Root = new RootNode();
        /// <summary>
        /// Creates tree and assings selectors to root node
        /// </summary>
        /// <param name="selectors">first branches of the tree</param>
        public BehaviorTree(params ISelector[] selectors)
        {
            Root = new RootNode();
            foreach(var s in selectors)
            {
                Root.Add(s);
            }
        }
        /// <summary>
        /// Calculates ai cycle
        /// </summary>
        public void Update()
        {
            Leaf leaf = GetLeaf(GetFirstNode());
            leaf?.Execute();
        }
        private Leaf GetLeaf(Node node) => node is Leaf leaf ? leaf : node is ISelector selector ? GetLeaf(selector.GetNode()) : null;
        private Node GetFirstNode()
        {
            foreach(var node in (List<ISelector>)Root)
            {
                Node n = node.GetNode();
                if(n != null) return n;
            }
            return null;
        }
        /// <summary>
        /// Entry point for behavior tree
        /// </summary>
        public class RootNode
        {
            readonly List<ISelector> _selectors = new List<ISelector>();
            /// <summary>
            /// Add selector that will be evaluated every update
            /// </summary>
            /// <param name="selector"></param>
            public void Add(ISelector selector) => _selectors.Add(selector);
            /// <summary>
            /// Converts rootNode to list of selectors
            /// </summary>
            /// <param name="node">Root node of the tree</param>
            public static explicit operator List<ISelector>(RootNode node) => node._selectors;
        }
    }
}
