using PerformantOVRController.Locomotion.Walker.Interfaces;
using UnityEngine;

namespace PerformantOVRController.Locomotion.Walker.WalkingStates
{
    public class WalkStateSprinting : LocomotionState
    {
        
        public override WalkStates walkState => WalkStates.Sprint;
        private bool _active;
        private Vector2 _movementAxis;

        private void Update()
        {
            if (!_active) return;
            // if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick) == Vector2.zero)
                SwitchToIdle();
        }

        private void FixedUpdate()
        {
            walker.transform.Translate(_movementAxis.x, 0, _movementAxis.y);
        }

        private void Run()
        {
            // movementAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            
            _movementAxis.x *= Time.deltaTime;
            _movementAxis.x *= walker.sprintSpeed * walker.strafeSpeed;
            
            _movementAxis.y *= Time.deltaTime;
            _movementAxis.y *= walker.sprintSpeed * walker.walkSpeed;
        }

        public override void EnterState()
        {
            _active = true;
            walker.leftThumbStick += Run;
            walker.buttonOneDown += SwitchToJump;
        }
        public override void ExitState()
        {
            _active = false;
            _movementAxis = Vector2.zero;
            walker.leftThumbStick -= Run;
            walker.buttonOneDown -= SwitchToJump;
        }

        public override void HandleInput()
        {
        }

        public WalkStateSprinting(StateWalker walker) : base(walker)
        { }

        public WalkStateSprinting(Teleporter teleporter) : base(teleporter)
        { }
    }
}