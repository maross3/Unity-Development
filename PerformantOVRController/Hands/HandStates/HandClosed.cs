using PerformantOVRController.Hands.Poses;

namespace PerformantOVRController.Hands.HandStates
{
    public class HandClosed : HandStateClass
    {
        public HandPose statePose;
        public Hand thisHand;

        public override HandState handState => HandState.Closed;

        public override void EnterState()
        {
            thisHand.ChangePose(statePose);
            thisHand.GripUp += SwitchToOpenState;
        }

        public override void ExitState()
        {
            thisHand.GripUp -= SwitchToOpenState;
        }

        private void SwitchToOpenState()
        {
            thisHand.ChangeState(HandState.Open);
        }
        
        public override void OverrideToGrabbed()
        {
            thisHand.ChangeState(HandState.Grabbed);
        }
    }
}
