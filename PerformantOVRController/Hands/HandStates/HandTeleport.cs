using PerformantOVRController.Hands.Poses;

namespace PerformantOVRController.Hands.HandStates
{
    public class HandTeleport : HandStateClass
    {
        public override HandState handState => HandState.Teleport;
        
        public Hand thisHand;
        public HandPose statePose;


        public override void EnterState()
        {
            thisHand.ChangePose(statePose);
        }

        public override void ExitState()
        {

        }

        public override void OverrideToGrabbed()
        {
            thisHand.ChangeState(HandState.Grabbed);
        }
    }
}
