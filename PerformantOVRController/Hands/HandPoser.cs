using System.Collections.Generic;
using PerformantOVRController.Hands.Poses;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PerformantOVRController.Hands
{
    [ExecuteInEditMode]
    public sealed class HandPoser : MonoBehaviour
    {
        public bool showGizmos = true;

        // TODO make a default asset here so you can drag and drop a directory in inspector 
        [Tooltip("Path of the directory where hand poses should be stored. Tip : Keep these in a 'Resources' directory so you can use Resources.Load().")]
        public string resourcePath = "Assets/Resources/HandPos";
        public string poseName = "Default";
        public HandPose currentPose;
        public float animationSpeed = 15f;

        // Hand Pose Transform Definitions
        public HandPoseDefinition HandPoseJoints => 
            GetHandPoseDefinition();

        public Transform wristJoint;
        public List<Transform> thumbJoints;
        public List<Transform> indexJoints;
        public List<Transform> middleJoints;
        public List<Transform> ringJoints;
        public List<Transform> pinkyJoints;
        public List<Transform> otherJoints;

        private HandPose _previousPose;
        private bool _doSingleAnimation;

        // Continuously update pose state
        public bool continuousUpdate = false;

        private void Start()
        {
            // Trigger a pose change to start the animation
            OnPoseChanged();
        }

        // This is also run in the editor
        private void Update()
        {
            // Check for pose change event
            CheckForPoseChange();

            // Lerp to Hand Pose
            if (continuousUpdate || _doSingleAnimation)
            {
                DoPoseUpdate();
            }
        }

        private void CheckForPoseChange()
        {
            if (_previousPose != null && (currentPose == null || _previousPose == null ||
                                          _previousPose.name == currentPose.name || currentPose == null)) return;
            OnPoseChanged();
            _previousPose = currentPose;
        }

        private void OnPoseChanged()
        {
            // Allow pose to change animation
            _editorAnimationTime = 0;
            _doSingleAnimation = true;
        }

        private FingerJoint GetWristJoint() =>
            GetJointFromTransform(wristJoint);

        private List<FingerJoint> GetThumbJoints() =>
            GetJointsFromTransforms(thumbJoints);

        private List<FingerJoint> GetIndexJoints() =>
            GetJointsFromTransforms(indexJoints);

        private List<FingerJoint> GetMiddleJoints() =>
            GetJointsFromTransforms(middleJoints);

        private List<FingerJoint> GetRingJoints() =>
            GetJointsFromTransforms(ringJoints);

        private List<FingerJoint> GetPinkyJoints() =>
            GetJointsFromTransforms(pinkyJoints);

        private List<FingerJoint> GetOtherJoints() =>
            GetJointsFromTransforms(otherJoints);

        private Transform GetTip(List<Transform> transforms) =>
            transforms?[^1];

        public Transform GetThumbTip() =>
            GetTip(thumbJoints);

        public Transform GetIndexTip() =>
            GetTip(indexJoints);

        public Transform GetMiddleTip() =>
            GetTip(middleJoints);

        public Transform GetRingTip() =>
            GetTip(ringJoints);

        public Transform GetPinkyTip() =>
            GetTip(pinkyJoints);

        public static Vector3 GetFingerTipPositionWithOffset(List<Transform> jointTransforms, float tipRadius)
        {
            if (jointTransforms == null || jointTransforms.Count == 0)
            {
                return Vector3.zero;
            }

            // Not available
            if (jointTransforms[jointTransforms.Count - 1] == null)
            {
                return Vector3.zero;
            }

            Vector3 tipPosition = jointTransforms[jointTransforms.Count - 1].position;

            if (jointTransforms.Count == 1)
            {
                return tipPosition;
            }

            return tipPosition + (jointTransforms[jointTransforms.Count - 2].position - tipPosition).normalized *
                tipRadius;
        }


        private List<FingerJoint> GetJointsFromTransforms(List<Transform> jointTransforms)
        {
            var joints = new List<FingerJoint>();

            // Check for empty joints
            if (jointTransforms == null)
            {
                return joints;
            }

            // Add any joint information to the list
            for (int x = 0; x < jointTransforms.Count; x++)
            {
                if (jointTransforms[x] != null)
                {
                    joints.Add(GetJointFromTransform(jointTransforms[x]));
                }
            }

            return joints;
        }

        private static FingerJoint GetJointFromTransform(Transform jointTransform)
        {
            // Null check
            if (jointTransform == null)
            {
                return null;
            }

            return new FingerJoint()
            {
                transformName = jointTransform.name,
                localPosition = jointTransform.localPosition,
                localRotation = jointTransform.localRotation
            };
        }

        private void UpdateHandPose(HandPoseDefinition pose, bool lerp)
        {
            UpdateJoint(pose.wristJoint, wristJoint, lerp);
            UpdateJoints(pose.thumbJoints, thumbJoints, lerp);
            UpdateJoints(pose.indexJoints, indexJoints, lerp);
            UpdateJoints(pose.middleJoints, middleJoints, lerp);
            UpdateJoints(pose.ringJoints, ringJoints, lerp);
            UpdateJoints(pose.pinkyJoints, pinkyJoints, lerp);
            UpdateJoints(pose.otherJoints, otherJoints, lerp);
        }

        private void UpdateJoint(FingerJoint fromJoint, Transform toTransform, bool doLerp)
        {
            // Invalid joint
            if (fromJoint == null || toTransform == null)
            {
                return;
            }

            if (doLerp)
            {
                toTransform.localPosition = Vector3.Lerp(toTransform.localPosition, fromJoint.localPosition,
                    Time.deltaTime * animationSpeed);
                toTransform.localRotation = Quaternion.Lerp(toTransform.localRotation, fromJoint.localRotation,
                    Time.deltaTime * animationSpeed);
            }
            else
            {
                toTransform.localPosition = fromJoint.localPosition;
                toTransform.localRotation = fromJoint.localRotation;
            }
        }

        private void UpdateJoints(List<FingerJoint> joints, List<Transform> toTransforms, bool doLerp)
        {
            // Sanity check
            if (joints == null || toTransforms == null)
                return;

            // Cache the count of our lists
            var jointCount = joints.Count;
            var transformsCount = toTransforms.Count;

            // If our joint counts don't add up, then make sure the names match before applying any changes
            // Otherwise there is a good chance the wwrong transforms are being updated
            var verifyTransformName = jointCount != transformsCount;
            for (int x = 0; x < jointCount; x++)
            {
                // Make sure the indexes match
                if (x < toTransforms.Count)
                {
                    // Joint may have not been assigned or destroyed
                    if (joints[x] == null || toTransforms[x] == null)
                    {
                        continue;
                    }

                    if (verifyTransformName && joints[x].transformName == toTransforms[x].name)
                        UpdateJoint(joints[x], toTransforms[x], doLerp);
                    else if (verifyTransformName == false)
                        UpdateJoint(joints[x], toTransforms[x], doLerp);
                }
            }
        }

        private HandPoseDefinition GetHandPoseDefinition()
        {
            return new HandPoseDefinition()
            {
                wristJoint = GetWristJoint(),
                thumbJoints = GetThumbJoints(),
                indexJoints = GetIndexJoints(),
                middleJoints = GetMiddleJoints(),
                ringJoints = GetRingJoints(),
                pinkyJoints = GetPinkyJoints(),
                otherJoints = GetOtherJoints()
            };
        }

        /// <summary>
        /// Saves the current hand configuration as a Unity Scriptable object 
        /// Pose will be saved to the provided 'HandPosePath' (Typically within in a Resources folder)
        /// *Scriptable Objects can only be saved from within the UnityEditor.
        /// </summary>
        /// <param name="poseName"></param>
        /// <returns></returns>
        [Button]
        public void SavePoseAsScriptablObject(string poseName)
        {
#if UNITY_EDITOR

            string fileName = poseName + ".asset";

            var poseObject = GetHandPoseScriptableObject();

            // Creates the file in the folder path
            //string fullPath = Application.dataPath + directory + fileName;
            string fullPath = resourcePath + fileName;
            bool exists = System.IO.File.Exists(fullPath);

            // Delete if asset already exists
            if (exists)
            {
                UnityEditor.AssetDatabase.DeleteAsset(fullPath);
            }

            UnityEditor.AssetDatabase.CreateAsset(poseObject, fullPath);

            UnityEditor.AssetDatabase.SaveAssets();

            UnityEditor.AssetDatabase.Refresh(UnityEditor.ImportAssetOptions.ForceUpdate);

            UnityEditor.EditorUtility.SetDirty(poseObject);

            if (exists)
            {
                Debug.Log("Updated Hand Pose : " + poseName);
            }
            else
            {
                Debug.Log("Created new Hand Pose : " + poseName);
            }
#else
    Debug.Log("Scriptable Objects can only be saved from within the Unity Editor. Consider storing in another format like JSON instead.");
#endif
        }

        /// <summary>
        /// Will create an original handpose with the specified PoseName. If the provided poseName is already taken, a number will be added to the name until a unique name is found.
        /// For example, if you provide the PoseName of "Default" and there are already two poses named "Default" and "Default 1", then a new pose named "Default 2" will be created.
        /// </summary>
        /// <param name="poseName"></param>
        public void CreateUniquePose(string poseName)
        {
            // Don't allow empty pose names
            if (string.IsNullOrEmpty(poseName))
            {
                poseName = "Pose";
            }

            string formattedPoseName = poseName;
            string fullPath = resourcePath + formattedPoseName + ".asset";
            bool exists = System.IO.File.Exists(fullPath);
            int checkCount = 0;
            // Find a path that doesn't exist
            while (exists)
            {
                // Ex : "path/Pose 5.asset"
                formattedPoseName = poseName + " " + checkCount;
                exists = System.IO.File.Exists(resourcePath + formattedPoseName + ".asset");

                checkCount++;
            }

            // Save the new pose
            SavePoseAsScriptablObject(formattedPoseName);
        }

        public HandPose GetHandPoseScriptableObject()
        {
#if UNITY_EDITOR
            var poseObject = UnityEditor.Editor.CreateInstance<HandPose>();
            poseObject.poseName = poseName;

            poseObject.joints = new HandPoseDefinition();
            poseObject.joints.wristJoint = GetWristJoint();
            poseObject.joints.thumbJoints = GetThumbJoints();
            poseObject.joints.indexJoints = GetIndexJoints();
            poseObject.joints.middleJoints = GetMiddleJoints();
            poseObject.joints.ringJoints = GetRingJoints();
            poseObject.joints.pinkyJoints = GetPinkyJoints();
            poseObject.joints.otherJoints = GetOtherJoints();

            return poseObject;
#else
    return null;
#endif
        }


        // How long to check for animations while in the editor mode
        private float _editorAnimationTime = 0f;
        private const float maxEditorAnimationTime = 2f;


        private void DoPoseUpdate()
        {
            if (currentPose != null)
            {
                UpdateHandPose(currentPose.joints, true);
            }

            // Are we done requesting a single animation?
            if (_doSingleAnimation)
            {
                _editorAnimationTime += Time.deltaTime;

                // Reset
                if (_editorAnimationTime >= maxEditorAnimationTime)
                {
                    _editorAnimationTime = 0;
                    _doSingleAnimation = false;
                }
            }
        }

        [Button]
        public void AutoAssignBones()
        {
            // Reset bones
            wristJoint = null;
            thumbJoints = new List<Transform>();
            indexJoints = new List<Transform>();
            middleJoints = new List<Transform>();
            ringJoints = new List<Transform>();
            pinkyJoints = new List<Transform>();
            otherJoints = new List<Transform>();
            bool wristFound = false;

            Transform[] children = GetComponentsInChildren<Transform>();

            int childCount = children.Length;
            for (int x = 0; x < childCount; x++)
            {
                Transform child = children[x];
                if (child == null || child.name == null || child == transform)
                {
                    continue;
                }
                //
                // // Ignore this bone
                // if(ShouldIgnoreJoint(child)) {
                //     continue;
                // }

                string formattedName = child.name.ToLower();

                // Assign Bones to appropriate containers
                if (formattedName.Contains("thumb"))
                {
                    thumbJoints.Add(child);
                }
                else if (formattedName.Contains("index"))
                {
                    indexJoints.Add(child);
                }
                else if (formattedName.Contains("middle"))
                {
                    middleJoints.Add(child);
                }
                else if (formattedName.Contains("ring"))
                {
                    ringJoints.Add(child);
                }
                else if (formattedName.Contains("pinky"))
                {
                    pinkyJoints.Add(child);
                }
                else if (wristFound == false && formattedName.Contains("wrist") ||
                         (formattedName.EndsWith("hand") && child.childCount > 3))
                {
                    wristJoint = child;
                    wristFound = true;
                }
                else
                {
                    otherJoints.Add(child);
                }
            }
        }

        // public virtual void ResetEditorHandles() {
        //     EditorHandle[] handles = GetComponentsInChildren<EditorHandle>();
        //     for (int x = 0; x < handles.Length; x++) {
        //         if(handles[x] != null && handles[x].gameObject != null) {
        //             GameObject.DestroyImmediate((handles[x]));
        //         }
        //     }
        // }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            // Update every frame even while in editor
            if (!Application.isPlaying)
            {
                UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
                UnityEditor.SceneView.RepaintAll();
            }
#endif
        }
    }
}