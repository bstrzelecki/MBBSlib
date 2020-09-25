using System;
using System.Collections.Generic;

namespace MBBSlib.AI
{
    /// <summary>
    /// Code pattern implementation for AI
    /// </summary>
    public class StateMachine
    {
        /// <summary>
        /// Currently active <see cref="IState"/>
        /// </summary>
        public IState CurrentState
        {
            get => _currentState;
            set
            {
                _currentState?.OnExit();
                _currentState = value;
                _currentState.OnEnter();
            }
        }
        /// <summary>
        /// Calls <see cref="IState.OnUpdate()"/> of <see cref="CurrentState"/>
        /// </summary>
        public void Update()
        {
            EvaluateTransitions();

            CurrentState?.OnUpdate();
        }
        /// <summary>
        /// Creates transitions between <see cref="IState"/>
        /// </summary>
        /// <param name="from"><see cref="IState"/> from which transition will be possible</param>
        /// <param name="to"></param>
        /// <param name="condition">If return true will make transition between states</param>
        public void AddLink(IState from, IState to, Func<bool> condition) => _stateLinks[from].Add(new Link(condition, to));
        /// <summary>
        /// Creates transitions between <see cref="IState"/>
        /// </summary>
        /// <param name="to"></param>
        /// <param name="condition">If return true will make transition between states</param>
        public void AddAnyLink(IState to, Func<bool> condition) => _anyLinks.Add(new Link(condition, to));


        private IState _currentState;
        readonly Dictionary<IState, List<Link>> _stateLinks = new Dictionary<IState, List<Link>>();
        readonly List<Link> _anyLinks = new List<Link>();

        private void EvaluateTransitions()
        {
            if (Calculate(_anyLinks)) return;
            if (Calculate(_stateLinks[CurrentState])) return;

            bool Calculate(List<Link> list)
            {
                foreach (var link in list)
                {
                    if (link.Condition)
                    {
                        CurrentState = link.To;
                        return true;
                    }
                }
                return false;
            }
        }

        private struct Link
        {
            private readonly Func<bool> _condition;

            public Link(Func<bool> condition, IState to)
            {
                _condition = condition;
                To = to;
            }

            public bool Condition => _condition.Invoke();
            public IState To { get; }
        }
    }
}
