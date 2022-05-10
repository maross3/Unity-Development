using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VR
{
    public class WalkStateWalking : MonoBehaviour, ILocomotionState
    {
        private StateWalker _walker;
        private bool active;
        private Vector2 movementAxis;
        void Start()
        {
            _walker = GetComponent<EMStateWalker>();
        }

        void Walk()
        {
            if (!active) return;
            
            movementAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

            movementAxis.x *= _walker.strafeSpeed;
            movementAxis.y *= _walker.walkSpeed;
        }

        void FixedUpdate()
        {
            transform.Translate(movementAxis.x * Time.deltaTime, 0, movementAxis.y * Time.deltaTime);
        }
        
        void Update()
        {
            if (!active) return;
            if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick) == Vector2.zero)
                _walker.ChangeState(WalkStates.idle);
        }
        public void EnterState()
        {
            active = true;
            _walker.leftThumbStick += Walk;
            _walker.leftThumbStickDown += SwitchToSprint;
            _walker.buttonOneDown += SwitchToJump;
        }

        public void ExitState()
        {
            movementAxis = Vector2.zero;
            active = false;
            _walker.leftThumbStick -= Walk;
            _walker.leftThumbStickDown -= SwitchToSprint;
            _walker.buttonOneDown -= SwitchToJump;
        }

        void SwitchToSprint()
        {
            _walker.ChangeState(WalkStates.sprint);
        }

        void SwitchToJump()
        {
            _walker.ChangeState(WalkStates.jump);
        }
    }
}