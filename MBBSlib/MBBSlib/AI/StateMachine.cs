using System;
using System.Collections.Generic;
using System.Text;

namespace MBBSlib.AI
{
    public class StateMachine
    {
        public IState CurrentState {
            get => _currentState;
            set
            {
                _currentState?.OnExit();
                _currentState = value;
                _currentState.OnEnter();
            }
        }
        public void Update()
        {
            EvaluateTransitions();

            CurrentState?.OnUpdate();
        }
        public void AddLink(IState from, IState to, Func<bool> condition) => _stateLinks[from].Add(new Link(condition, to));
        public void AddAnyLink(IState to, Func<bool> condition) => _anyLinks.Add(new Link(condition, to));
        
        
        private IState _currentState;

        Dictionary<IState, List<Link>> _stateLinks = new Dictionary<IState, List<Link>>();
        List<Link> _anyLinks = new List<Link>();

        private void EvaluateTransitions()
        {
            if(Calculate(_anyLinks)) return;
            if(Calculate(_stateLinks[CurrentState]))return;
            
            bool Calculate(List<Link> list)
            {
                foreach(var link in list)
                {
                    if(link.Condition)
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
