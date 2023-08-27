namespace PerformantOVRController.Locomotion.Walker.Interfaces
{
    public abstract class LocomotionState
    {
        protected StateWalker walker;
        protected Teleporter teleporter;
        public abstract WalkStates walkState { get; }
        protected LocomotionState(StateWalker walker)
        {
            this.walker = walker;
        }
        
        protected LocomotionState(Teleporter teleporter)
        {
            this.teleporter = teleporter;
        }
        
        public abstract void EnterState();
        public abstract void ExitState();

        public abstract void HandleInput();
        
        protected void SwitchToSprint() =>
            walker.ChangeState(WalkStates.Sprint);

        protected void SwitchToJump() =>
            walker.ChangeState(WalkStates.Jump);
        
        protected void SwitchToIdle() =>
            walker.ChangeState(WalkStates.Idle);
        
        protected void SwitchToWalk() =>
            walker.ChangeState(WalkStates.Walk);
    }
}
