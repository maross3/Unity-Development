using PerformantOVRController.Locomotion.Walker.Interfaces;

namespace PerformantOVRController.Locomotion.Walker.WalkingStates
{
    public class WalkStateIdle : LocomotionState
    {
        public override WalkStates walkState => WalkStates.Idle;

        public override void EnterState()
        {
            walker.leftThumbStick += SwitchToWalk;
            walker.leftThumbStickDown += SwitchToSprint;
            walker.buttonOneDown += SwitchToJump;
        }

        public override void ExitState()
        {
            walker.leftThumbStick -= SwitchToWalk;
            walker.leftThumbStickDown -= SwitchToSprint;
            walker.buttonOneDown -= SwitchToJump;
        }

        public override void HandleInput()
        {
            
        }

        public WalkStateIdle(StateWalker walker) : base(walker)
        { }

        public WalkStateIdle(Teleporter teleporter) : base(teleporter)
        { }
    }
}
