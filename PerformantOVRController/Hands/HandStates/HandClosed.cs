using System;
using UnityEngine;

namespace VR
{
    public class HandClosed : MonoBehaviour, IHandState
    {
        public HandPose statePose;
        public Hand thisHand;

        public void EnterState()
        {
            thisHand.ChangePose(statePose);
            thisHand.GripUp += SwitchToOpenState;
        }

        public void ExitState()
        {
            thisHand.GripUp -= SwitchToOpenState;
        }

        void SwitchToOpenState()
        {
            thisHand.ChangeState(HandState.Open);
        }
        
        public void OverrideToGrabbed()
        {
            thisHand.ChangeState(HandState.Grabbed);
        }
    }
}
