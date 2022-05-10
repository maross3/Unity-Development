using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VR
{
    public interface ILocomotionState
    {
        public abstract void EnterState();
        public abstract void ExitState();

    }
}
