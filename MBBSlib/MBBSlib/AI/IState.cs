using System;
using System.Collections.Generic;
using System.Text;

namespace MBBSlib.AI
{
    interface IState
    {
        void OnUpdate();
        void OnEnter();
        void OnExit();
    }
}
