using UnityEngine;

namespace PerformantOVRController
{
    public interface IGrabbableObject
    {
        public abstract Transform playerHand { set; get; }
    }
}