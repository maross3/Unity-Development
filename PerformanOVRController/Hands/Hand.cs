using System;
using UnityEngine;

namespace VR
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
        
        [SerializeField] private HandOpen openState;
        [SerializeField] private HandClosed closedState;
        [SerializeField] private HandGrabbed grabbedState;
        private HandGrabbing _grabbingState;
        private HandTeleport _teleportState;
        private IHandState _currentState;
        private bool _gripping;
        private bool _leftGripping;
        
        public Action Grip;
        public Action GripUp;

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

        void Start()
        {            
            //if(grabber == null) {
                //grabber = GetComponentInChildren<Grabber>();
           // }
           
           AddStateComponents();
           handPoser = GetComponentInChildren<HandPoser>();
           _currentState = openState;
           _currentState.EnterState();
           Grip += DebugActions;
           GripUp += DebugActions;
            
            // Subscribe to grab / release events
            //grabber.onAfterGrabEvent.AddListener(OnGrabberGrabbed);
            //grabber.onReleaseEvent.AddListener(OnGrabberReleased);

        }

        void DebugActions()
        {
            
        }
        
        private void AddStateComponents()
        {
            openState = gameObject.AddComponent<HandOpen>();
            openState.thisHand = this;
            openState.statePose = openPose;
            
            closedState = gameObject.AddComponent<HandClosed>();
            closedState.thisHand = this;
            closedState.statePose = closedPose;
            
            grabbedState = gameObject.AddComponent<HandGrabbed>();
            grabbedState.thisHand = this;
            grabbedState.statePose = grabbedPose;

            _teleportState = gameObject.AddComponent<HandTeleport>();
            _teleportState.thisHand = this;
            _teleportState.statePose = teleportPose;

            _grabbingState = gameObject.AddComponent<HandGrabbing>();
            _grabbingState.thisHand = this;
            _grabbingState.statePose = closedPose;
        }

        public void HandleInput()
        {
            if (handSide == HandSide.Left)
            {
                handAnchor.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
                handAnchor.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
                
                 if(!_leftGripping && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.001f)
                {
                    Grip.Invoke();
                    _leftGripping = true;
                }
                 
                if(_leftGripping && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) < 0.001f)
                { 
                    GripUp.Invoke();
                    _leftGripping = false;
                }
            }
            else
            {
                if (OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0.001f)
                {
                    Grip.Invoke();
                    _gripping = true;
                }

                if (_gripping && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) < 0.001f)
                {
                    GripUp.Invoke();
                    _gripping = false;
                }

                handAnchor.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
                handAnchor.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
            }

        }
        
        public void ChangeState(HandState newState)
        {
            _currentState.ExitState();

            _currentState = newState switch
            {
                HandState.Open => openState,
                HandState.Grabbed => grabbedState,
                HandState.Closed => closedState,
                HandState.Teleport => _teleportState,
                HandState.Grabbing => _grabbingState,
                _ => _currentState
            };
            
            _currentState.EnterState();
        }

        public void ChangePose(HandPose newPose)
        {
            handPoser.CurrentPose = newPose;
        }
    }
}
