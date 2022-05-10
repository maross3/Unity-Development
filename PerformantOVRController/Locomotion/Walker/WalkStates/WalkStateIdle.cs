using System;
using UnityEngine;

namespace VR
{
    public class WalkStateIdle : MonoBehaviour, ILocomotionState
    {
        public StateWalker _walker;
        

        public void EnterState()
        {
            _walker.leftThumbStick += SwitchToWalk;
            _walker.leftThumbStickDown += SwitchToSprint;
            _walker.buttonOneDown += SwitchToJump;
        }

        public void ExitState()
        {
            _walker.leftThumbStick -= SwitchToWalk;
            _walker.leftThumbStickDown -= SwitchToSprint;
            _walker.buttonOneDown -= SwitchToJump;
        }

        void SwitchToWalk()
        {
            _walker.ChangeState(WalkStates.walk);
        }

        void SwitchToSprint()
        {
            _walker.ChangeState(WalkStates.sprint);
        }

        void SwitchToJump()
        {
            _walker.ChangeState(WalkStates.jump);
        }

    }
}
