using System;
using PerformantOVRController.Hands;
using PerformantOVRController.Locomotion;
using PerformantOVRController.Locomotion.Walker;
using PerformantOVRController.Locomotion.Walker.Interfaces;
using UnityEngine;
using UnityEngine.UIElements;

namespace PerformantOVRController
{
    public class PlayerController : MonoBehaviour, IPointerCaptureEvent
    {
        [SerializeField] private Transform centerEyeAnchor;
        [SerializeField] private GameObject teleportObject;
        [SerializeField] private Transform trackingSpace;
        public LocomotionType startLocomotionType;
        
        [Header("Hands")]
        [SerializeField] public Hand leftHand;
        [SerializeField] public Hand rightHand;
        
        private LocomotionType _curLocomotion;
        private ILocomotionType _locomotion;
        private StateWalker _walk;
        private Teleporter _teleport;
        
        private delegate void ProcessingInputs();
        private ProcessingInputs _processInput;
        private Action _inputFocusAction;

        public enum LocomotionType
        {
            None,
            Walk,
            Teleport
        }

        public virtual void RotateCharacterWithTrackingSpace() {
            Vector3 initialPosition = trackingSpace.position;
            var initialRotation = trackingSpace.rotation;
            
            transform.rotation = Quaternion.Euler(0.0f, centerEyeAnchor.rotation.eulerAngles.y, 0.0f);
            
            trackingSpace.position = initialPosition;
            trackingSpace.rotation = initialRotation;
        }
        
        

        
        void Start()
        {
            _teleport = teleportObject.GetComponent<Teleporter>();
            _walk = gameObject.GetComponent<StateWalker>();
            
            _curLocomotion = startLocomotionType;
            SetLocomotionType(_curLocomotion);

            if(_curLocomotion != LocomotionType.None) _processInput += _locomotion.HandleInput;
            
            // OVRManager.InputFocusAcquired += _inputFocusAction;

            leftHand.handSide = HandSide.Left;
            rightHand.handSide = HandSide.Right;

            _processInput += leftHand.HandleInput;
            _processInput += rightHand.HandleInput;
        }

        void ToggleLocomotion(LocomotionType newLoc)
        {
            if (newLoc == LocomotionType.None && _locomotion == null) return;
            
            if (_curLocomotion != LocomotionType.None)
                _processInput -= _locomotion.HandleInput;

            _curLocomotion = newLoc;
            SetLocomotionType(newLoc);

            _processInput += _locomotion.HandleInput;
            
        }
        
        private void SetLocomotionType(LocomotionType newLoc)
        {
            _locomotion = newLoc switch
            {
                LocomotionType.Walk => _walk,
                LocomotionType.Teleport => _teleport,
                LocomotionType.None => null,
                _ => _locomotion
            };
        }

        void Update()
        {
            RotateCharacterWithTrackingSpace();
            if(_processInput != null) _processInput();
        }
    }
}
