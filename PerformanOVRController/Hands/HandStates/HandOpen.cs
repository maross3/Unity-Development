using UnityEngine;

namespace VR
{
    public class HandOpen : MonoBehaviour, IHandState
    {
        public HandPose statePose;
        public Hand thisHand;

        void SwitchToGrabState()
       {
           thisHand.ChangeState(HandState.Grabbing);
       }

       public void EnterState()
        {
            thisHand.ChangePose(statePose);
            thisHand.Grip += SwitchToGrabState;
        }

        public void ExitState()
        {
            thisHand.Grip -= SwitchToGrabState;
        }

        public void OverrideToGrabbed()
        {
            thisHand.ChangeState(HandState.Grabbed);
        }
    }
}
