using PerformantOVRController.Hands.Poses;
using UnityEngine;

namespace PerformantOVRController.Hands.HandStates
{
    public class HandGrabbed : HandStateClass
    {
        public HandPose statePose;
        public Hand thisHand;
        public GameObject heldObject;

        private void SwitchToOpenState()
        {
            thisHand.ChangeState(HandState.Open);
        }

        public override HandState handState => HandState.Grabbed;

        public override void EnterState()
        {
            thisHand.ChangePose(statePose);
            thisHand.GripUp += SwitchToOpenState;
        }

        public override void ExitState()
        {
            heldObject.transform.parent = null;
            thisHand.GripUp -= SwitchToOpenState;
        }

        public override void OverrideToGrabbed()
        {
            thisHand.ChangeState(HandState.Grabbed);
        }
    }
}
