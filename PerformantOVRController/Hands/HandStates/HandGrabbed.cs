using UnityEngine;

namespace VR
{
    public class HandGrabbed : MonoBehaviour, IHandState
    {
        public HandPose statePose;
        public Hand thisHand;
        public GameObject heldObject;

        private void SwitchToOpenState()
        {
            thisHand.ChangeState(HandState.Open);
        }
        
        public void EnterState()
        {
            thisHand.ChangePose(statePose);
            thisHand.GripUp += SwitchToOpenState;
        }

        public void ExitState()
        {
            heldObject.transform.parent = null;
            thisHand.GripUp -= SwitchToOpenState;
        }

        public void OverrideToGrabbed()
        {
            thisHand.ChangeState(HandState.Grabbed);
        }
    }
}
