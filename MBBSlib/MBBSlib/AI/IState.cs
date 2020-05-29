using System;
using System.Collections.Generic;
using System.Text;

namespace MBBSlib.AI
{
    /// <summary>
    /// State interface for <see cref="StateMachine"/>
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// Method called if this state is active and <see cref="StateMachine"/>.Update() is called
        /// </summary>
        void OnUpdate();
        /// <summary>
        /// Method that is called when this state is set by <see cref="StateMachine"/>
        /// </summary>
        void OnEnter();
        /// <summary>
        /// Method that is called when <see cref="StateMachine"/> switches from this state to another
        /// </summary>
        void OnExit();
    }
}
