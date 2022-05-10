using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VR
{
    public class WalkStateSprinting : MonoBehaviour, ILocomotionState
    {
        private StateWalker _walker;
        private bool active;
        private Vector2 movementAxis;
        void Start()
        {
            _walker = GetComponent<EMStateWalker>();
        }

        private void Update()
        {
            if (!active) return;
            if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick) == Vector2.zero)
                _walker.ChangeState(WalkStates.idle);
        }

        private void FixedUpdate()
        {
            transform.Translate(movementAxis.x, 0, movementAxis.y);
        }

        void Run()
        {
            movementAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            
            movementAxis.x *= Time.deltaTime;
            movementAxis.x *= _walker.sprintSpeed * _walker.strafeSpeed;
            
            movementAxis.y *= Time.deltaTime;
            movementAxis.y *= _walker.sprintSpeed * _walker.walkSpeed;
        }

        public void EnterState()
        {
            active = true;
            _walker.leftThumbStick += Run;
            _walker.buttonOneDown += SwitchToJump;
        }
        public void ExitState()
        {
            active = false;
            movementAxis = Vector2.zero;
            _walker.leftThumbStick -= Run;
            _walker.buttonOneDown -= SwitchToJump;
        }

        void SwitchToWalk()
        {
            _walker.ChangeState(WalkStates.walk);
        }

        void SwitchToJump()
        {
            _walker.ChangeState(WalkStates.jump);
        }


    }
}