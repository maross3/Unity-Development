using UnityEngine;
using System;

namespace VR
{
    public enum WalkStates
    {
        idle,
        jump,
        sprint,
        walk
    }
    public class StateWalker : MonoBehaviour, ILocomotion
    {
        public Vector2 thumbstickDeadZone;
        
        private ILocomotionState currentState;
        private Vector2 axis;
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

            _idleState = gameObject.AddComponent<WalkStateIdle>();
            _idleState._walker = this;
            
            _jumpState = gameObject.AddComponent<WalkStateJumping>();
            _sprintState = gameObject.AddComponent<WalkStateSprinting>();
            _walkState = gameObject.AddComponent<WalkStateWalking>();
            
            currentState = _idleState;
            currentState.EnterState();
        }

        void VerboseInput()
        {
            //Debug.Log("input recieved");
        }

        public void HandleInput()
        {
            if(ApplyDeadZones(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick), thumbstickDeadZone.x, thumbstickDeadZone.y) != Vector2.zero) leftThumbStick.Invoke();
            if(ApplyDeadZones(OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick), thumbstickDeadZone.x, thumbstickDeadZone.y).x != 0) rightStickXAxis.Invoke();
            if(OVRInput.Get(OVRInput.Button.PrimaryThumbstick)) leftThumbStickDown.Invoke();
            if (OVRInput.Get(OVRInput.Button.One)) buttonOneDown.Invoke();
        }

        public void ChangeState(WalkStates newState)
        {
            currentState.ExitState();

            currentState = newState switch
            {
                WalkStates.idle => _idleState,
                WalkStates.jump => _jumpState,
                WalkStates.walk => _walkState,
                WalkStates.sprint => _sprintState,
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
            
            axis = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
            transform.rotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y +
                snapDistance * axis.x, transform.eulerAngles.z));
        }
    }
}