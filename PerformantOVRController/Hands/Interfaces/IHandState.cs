namespace VR
{
    public interface IHandState
    {
        public abstract void EnterState();
        public abstract void ExitState();
        public abstract void OverrideToGrabbed();
    }
}
