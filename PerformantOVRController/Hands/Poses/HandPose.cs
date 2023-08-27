using UnityEngine;

namespace PerformantOVRController.Hands.Poses 
{
    [System.Serializable]
    public class HandPose : ScriptableObject {

        // Used to help identify name of the hand pose
        [Header("Pose Name")]
        public string poseName;

        [SerializeField]
        [Header("Joint Definitions")]
        public HandPoseDefinition joints;
    }
}

