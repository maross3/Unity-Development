using System.Collections.Generic;
using UnityEngine;

namespace PerformantOVRController.Hands.Poses
{
    [System.Serializable]
    public class HandPoseDefinition
    {
        [SerializeField] [Header("Wrist")] 
        public FingerJoint wristJoint;

        [SerializeField] [Header("Thumb")] 
        public List<FingerJoint> thumbJoints;

        [SerializeField] [Header("Index")] 
        public List<FingerJoint> indexJoints;

        [SerializeField] [Header("Middle")] 
        public List<FingerJoint> middleJoints;

        [SerializeField] [Header("Ring")] 
        public List<FingerJoint> ringJoints;

        [SerializeField] [Header("Pinky")] 
        public List<FingerJoint> pinkyJoints;

        [SerializeField] [Header("Other")] 
        public List<FingerJoint> otherJoints;
    }
}