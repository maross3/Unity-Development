using System.Collections;
using PerformantOVRController.Locomotion.Walker.Interfaces;
using UnityEngine;

namespace PerformantOVRController.Locomotion.Walker.WalkingStates
{
    public class WalkStateJumping : LocomotionState
    {
        public override WalkStates walkState => WalkStates.Jump;
        private Rigidbody _rb;
        private Collider _col;
        private bool _active;
        private Vector2 _movementAxis;
        private float _distanceToGround;
        
        private void Start()
        {
            _rb = walker.GetComponent<Rigidbody>();
            _col = walker.GetComponent<BoxCollider>();
            _rb.freezeRotation = true;
        }

        private void Update()
        {
            if (!_active) return;
            _distanceToGround = _col.bounds.extents.y;
            
            if (Physics.Raycast(walker.transform.position, -Vector3.up, _distanceToGround + 0.5f)) 
                walker.ChangeState(WalkStates.Walk);
        }
        public override void EnterState()
        {
            var extentsY = _col.bounds.extents.y;
            if (!Physics.Raycast(walker.transform.position, -Vector3.up, extentsY + 0.5f) && _active) 
                return;
            
            _rb.AddForce(0, walker.jumpSpeed, 0);
            walker.leftThumbStick += AirMove;
            walker.StartCoroutine(JumpDelay());
        }

        private IEnumerator JumpDelay()
        {
            yield return new WaitForSeconds(0.1f);
            _active = true;
        }

        private void FixedUpdate()
        {
            walker.transform.Translate(_movementAxis.x * Time.deltaTime, 0, _movementAxis.y * Time.deltaTime);
        }

        void AirMove()
        {
            // _movementAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            _movementAxis.x *= walker.strafeSpeed;
            _movementAxis.y *= walker.walkSpeed;
            
        }

        public override void ExitState()
        {
            _movementAxis = Vector2.zero;
            _active = false;
            walker.leftThumbStick -= AirMove;
        }

        public override void HandleInput()
        {
            
        }

        public WalkStateJumping(StateWalker walker) : base(walker)
        { }

        public WalkStateJumping(Teleporter teleporter) : base(teleporter)
        { }
    }
}