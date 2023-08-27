using System;
using PerformantOVRController.Locomotion.Walker.Interfaces;
using PerformantOVRController.Locomotion.Walker.WalkingStates;
using UnityEngine;

namespace PerformantOVRController.Locomotion.Walker
{
    public enum WalkStates
    {
        Idle,
        Jump,
        Sprint,
        Walk
    }
    public class StateWalker : MonoBehaviour, ILocomotionType
    {
        public Vector2 thumbstickDeadZone;
        
        internal LocomotionState currentState;
        private Vector2 _axis;
        [SerializeField] private float snapDistance;
        public float sprintSpeed;
        public float walkSpeed;
        public float strafeSpeed;
        public float jumpSpeed;
        
        public Action leftThumbStick;
        public Action rightStickXAxis;
        public Action leftThumbStickDown;
        public Action buttonOneDown;
        public Action leftThumbStickUp;
        

        private WalkStateIdle _idleState;
        private WalkStateJumping _jumpState;
        private WalkStateSprinting _sprintState;
        private WalkStateWalking _walkState;
        
        void Start()
        {
            rightStickXAxis += RotateCharacter;
            
            // debugging
            leftThumbStick += VerboseInput;
            leftThumbStickDown += VerboseInput;
            buttonOneDown += VerboseInput;
            leftThumbStickUp += VerboseInput;

        }

        private static void VerboseInput()
        {
            //Debug.Log("input recieved");
        }

        public void HandleInput()
        {
            currentState.HandleInput();
            
            // todo, deprecated OVR code:
            // if(ApplyDeadZones(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick), thumbstickDeadZone.x, thumbstickDeadZone.y) != Vector2.zero) leftThumbStick.Invoke();
            // if(ApplyDeadZones(OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick), thumbstickDeadZone.x, thumbstickDeadZone.y).x != 0) rightStickXAxis.Invoke();
            // if(OVRInput.Get(OVRInput.Button.PrimaryThumbstick)) leftThumbStickDown.Invoke();
            // if (OVRInput.Get(OVRInput.Button.One)) buttonOneDown.Invoke();
        }

        public void ChangeState(WalkStates newState)
        {
            currentState.ExitState();

            currentState = newState switch
            {
                WalkStates.Idle => _idleState,
                WalkStates.Jump => _jumpState,
                WalkStates.Walk => _walkState,
                WalkStates.Sprint => _sprintState,
                _ => throw new Exception($"Wrong state: {currentState}")
            };
            
            currentState.EnterState();
        }

        protected Vector2 ApplyDeadZones(Vector2 pos, float deadZoneX, float deadZoneY)
        {

            if (Mathf.Abs(pos.x) < deadZoneX)
            {
                pos.x = 0f;
            }

            if (Mathf.Abs(pos.y) < deadZoneY)
            {
                pos.y = 0f;
            }

            return pos;
        }

        public void RotateCharacter()
        {
            
            // axis = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
            transform.rotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y +
                snapDistance * _axis.x, transform.eulerAngles.z));
        }
    }
}