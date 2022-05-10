using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VR
{
    public class WalkStateJumping : MonoBehaviour, ILocomotionState
    {
        private StateWalker _walker;
        private Rigidbody _rb;
        private Collider _col;
        private bool _active;
        private Vector2 _movementAxis;
        private float _distanceToGround;
        
        void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _col = GetComponent<BoxCollider>();
            _rb.freezeRotation = true;
            _walker = GetComponent<EMStateWalker>();
        }

        void Update()
        {
            if (!_active) return;
            _distanceToGround = _col.bounds.extents.y;
            
            if (Physics.Raycast(transform.position, -Vector3.up, _distanceToGround + 0.5f)) 
                _walker.ChangeState(WalkStates.walk);
        }
        public void EnterState()
        {
            var extentsY = _col.bounds.extents.y;
            if (!Physics.Raycast(transform.position, -Vector3.up, extentsY + 0.5f) && _active) 
                return;
            
            _rb.AddForce(0, _walker.jumpSpeed, 0);
            _walker.leftThumbStick += AirMove;
            StartCoroutine(JumpDelay());

        }

        IEnumerator JumpDelay()
        {
            yield return new WaitForSeconds(0.1f);
            _active = true;
        }

        private void FixedUpdate()
        {
            transform.Translate(_movementAxis.x * Time.deltaTime, 0, _movementAxis.y * Time.deltaTime);

        }

        void AirMove()
        {
            _movementAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            _movementAxis.x *= _walker.strafeSpeed;
            _movementAxis.y *= _walker.walkSpeed;
            
        }

        public void ExitState()
        {
            _movementAxis = Vector2.zero;
            _active = false;
            _walker.leftThumbStick -= AirMove;
        }
    }
}