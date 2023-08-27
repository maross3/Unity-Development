namespace PerformantOVRController.Hands
{
    public abstract class HandStateClass
    {
        public abstract HandState handState { get; }
        public abstract void EnterState();
        public abstract void ExitState();
        public abstract void OverrideToGrabbed();
    }
}
