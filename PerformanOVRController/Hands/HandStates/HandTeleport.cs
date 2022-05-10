using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VR
{
    public class HandTeleport : MonoBehaviour, IHandState
    {
        public Hand thisHand;
        public HandPose statePose;

        public void EnterState()
        {
            thisHand.ChangePose(statePose);
        }

        public void ExitState()
        {

        }

        public void OverrideToGrabbed()
        {
            thisHand.ChangeState(HandState.Grabbed);
        }
    }
}
