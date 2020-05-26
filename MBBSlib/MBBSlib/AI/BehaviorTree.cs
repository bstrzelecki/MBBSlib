using System.Collections.Generic;

namespace MBBSlib.AI
{
    public class BehaviorTree
    {
        public RootNode Root { get; }
        public BehaviorTree() => Root = new RootNode();
        public BehaviorTree(params ISelector[] selectors)
        {
            Root = new RootNode();
            foreach(var s in selectors)
            {
                Root.Add(s);
            }
        }
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

        public class RootNode
        {
            readonly List<ISelector> _selectors = new List<ISelector>();
            public void Add(ISelector selector) => _selectors.Add(selector);

            public static explicit operator List<ISelector>(RootNode node) => node._selectors;
        }
    }
}
