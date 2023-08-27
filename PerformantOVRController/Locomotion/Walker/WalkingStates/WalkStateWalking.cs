using PerformantOVRController.Locomotion.Walker.Interfaces;
using UnityEngine;

namespace PerformantOVRController.Locomotion.Walker.WalkingStates
{
    public class WalkStateWalking : LocomotionState
    {
        public override WalkStates walkState => WalkStates.Walk;
        private bool _active;
        private Vector2 _movementAxis;

        private void Walk()
        {
            if (!_active) return;
            
            // movementAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            _movementAxis.x *= walker.strafeSpeed;
            _movementAxis.y *= walker.walkSpeed;
        }

        private void FixedUpdate()
        {
            walker.transform.Translate(_movementAxis.x * Time.deltaTime, 0, _movementAxis.y * Time.deltaTime);
        }

        private void Update()
        {
            if (!_active) return;
            // if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick) == Vector2.zero)
                SwitchToIdle();
        }
        
        public override void EnterState()
        {
            _active = true;
            walker.leftThumbStick += Walk;
            walker.leftThumbStickDown += SwitchToSprint;
            walker.buttonOneDown += SwitchToJump;
        }

        public override void ExitState()
        {
            _movementAxis = Vector2.zero;
            _active = false;
            walker.leftThumbStick -= Walk;
            walker.leftThumbStickDown -= SwitchToSprint;
            walker.buttonOneDown -= SwitchToJump;
        }

        public override void HandleInput()
        {
            
        }


        public WalkStateWalking(StateWalker walker) : base(walker)
        { }

        public WalkStateWalking(Teleporter teleporter) : base(teleporter)
        { }
    }
}