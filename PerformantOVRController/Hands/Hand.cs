using System;
using PerformantOVRController.Hands.HandStates;
using PerformantOVRController.Hands.Poses;
using UnityEngine;

namespace PerformantOVRController.Hands
{
    public enum HandSide
    {
        Right,
        Left
    }

    public enum HandState
    {
        Open,
        Closed,
        Grabbing,
        Grabbed,
        Teleport
    }
    public class Hand : MonoBehaviour
    {
        [SerializeField] private HandPose openPose;
        [SerializeField] private HandPose grabbedPose;
        [SerializeField] private HandPose closedPose;
        [SerializeField] private HandPose teleportPose;
        
        public Transform gripTransform;
        public HandSide handSide;
        public Transform handAnchor;
        public HandPoser handPoser;

        private HandTeleport _teleportState;
        public HandStateClass currentState;
        public StateController handStateController;
        
        private bool _gripping;
        private bool _leftGripping;
        
        public Action grip;
        public Action gripUp;

        // TODO grabbables
        // private void OnTriggerStay(Collider other)
        // {
        //     if(other.GetComponent(typeof(IGrabbable)) != null)
        //     {
        //         _grabbingState.grabbedObject = other.gameObject;
        //         grabbedState.heldObject = other.gameObject;
        //
        //     }
        // }
        //
        // private void OnTriggerExit(Collider other)
        // {
        //     if(other.GetComponent(typeof(IGrabbable)) != null)
        //     {
        //         _grabbingState.grabbedObject = null;
        //         grabbedState.heldObject = null;
        //
        //     }
        // }

        private void Start()
        {
            //if(grabber == null) {
                //grabber = GetComponentInChildren<Grabber>();
           // }
           
           AddStateComponents();
           handPoser = GetComponentInChildren<HandPoser>();
           grip += DebugActions;
           gripUp += DebugActions;
            
            // Subscribe to grab / release events
            //grabber.onAfterGrabEvent.AddListener(OnGrabberGrabbed);
            //grabber.onReleaseEvent.AddListener(OnGrabberReleased);

        }

        private static void DebugActions()
        {
            
        }
        
        private void AddStateComponents()
        {
            handStateController = new StateController(this);
            currentState.EnterState();
        }

        public void HandleInput()
        {
            // if (handSide == HandSide.Left)
            // {
            //     handAnchor.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            //     handAnchor.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
            //     
            //      if(!_leftGripping && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.001f)
            //     {
            //         Grip.Invoke();
            //         _leftGripping = true;
            //     }
            //      
            //     if(_leftGripping && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) < 0.001f)
            //     { 
            //         GripUp.Invoke();
            //         _leftGripping = false;
            //     }
            // }
            // else
            // {
            //     if (OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0.001f)
            //     {
            //         Grip.Invoke();
            //         _gripping = true;
            //     }
            //
            //     if (_gripping && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) < 0.001f)
            //     {
            //         GripUp.Invoke();
            //         _gripping = false;
            //     }
            //
            //     handAnchor.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            //     handAnchor.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
            // }

        }

        public void ChangeState(HandState newState) =>
            handStateController.ChangeState(newState);

        public void ChangePose(HandPose newPose)
        {
            handPoser.currentPose = newPose;
        }
    }
}
