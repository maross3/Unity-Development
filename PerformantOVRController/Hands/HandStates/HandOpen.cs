using PerformantOVRController.Hands.Poses;
using UnityEngine;

namespace PerformantOVRController.Hands.HandStates
{
    public class HandOpen : HandStateClass
    {
        public override HandState handState => HandState.Open;
        public HandPose statePose;
        public Hand thisHand;

        private void SwitchToGrabState()
        {
            thisHand.ChangeState(HandState.Grabbing);
        }

        public override void EnterState()
        {
            thisHand.ChangePose(statePose);
            thisHand.Grip += SwitchToGrabState;
        }

        public override void ExitState()
        {
            thisHand.Grip -= SwitchToGrabState;
        }

        public override void OverrideToGrabbed()
        {
            thisHand.ChangeState(HandState.Grabbed);
        }
    }
}