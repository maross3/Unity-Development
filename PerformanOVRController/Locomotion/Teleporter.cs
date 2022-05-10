using System.Collections;
using System.Collections.Generic;
using ElectroMag._DevVR;
using UnityEngine;

namespace VR
{
    public class Teleporter : MonoBehaviour, ILocomotion
    {
        [SerializeField] private Hand _hand;

        [SerializeField] private float yOffset;
        public GameObject positionMarker;
        public Transform bodyTransforn;
        public LayerMask excludeLayers;
        public float angle = 45f;
        public float strength = 10f;

        private const int MaxVertexCount = 20;
        private readonly float _vertexDelta = 0.08f;
        private LineRenderer _arcRenderer;
        private Vector3 _velocity;
        private Vector3 _groundPos;
        private Vector3 _lastNormal;
        private bool _groundDetected = false;
        private List<Vector3> _vertexList = new List<Vector3>();
        private bool _displayActive = false;
        private bool _teleporting;
        
        public void Teleport()
        {
            if (_groundDetected)
            {
                _groundPos.y += yOffset;

                bodyTransforn.position = _groundPos + _lastNormal * 0.1f;
                positionMarker.SetActive(false);
                _arcRenderer.enabled = false;
                _hand.ChangeState(HandState.Open);
            }
        }
        
        public void ToggleDisplay(bool active)
        {
            _teleporting = active;
            _displayActive = active;
        }
        
        private void Awake()
        {
            _arcRenderer = GetComponentInChildren<LineRenderer>();
            _arcRenderer.enabled = false;
            positionMarker.SetActive(false);
        }

        private void FixedUpdate()
        {
            if (_displayActive)
            {
                UpdatePath();
            }
        }


        private void UpdatePath()
        {
            _groundDetected = false;
            _vertexList.Clear();

            _velocity = Quaternion.AngleAxis(-angle, transform.right) * transform.forward * strength;
            RaycastHit hit;

            Vector3 pos = transform.position;

            _vertexList.Add(pos);

            while (_vertexList.Count < MaxVertexCount)
            {
                Vector3 newPos = pos + _velocity * _vertexDelta
                                     + 0.5f * Physics.gravity * _vertexDelta * _vertexDelta;

                _velocity += Physics.gravity * _vertexDelta;

                _vertexList.Add(newPos);
                if (Physics.Linecast(pos, newPos, out hit, ~excludeLayers))
                {
                    _groundDetected = true;
                    _groundPos = hit.point;
                    _lastNormal = hit.normal;
                    break;
                }

                pos = newPos;
            }

            if (!_groundDetected)
            {
                positionMarker.SetActive(false);
                _arcRenderer.enabled = false;
                return;
            }
            
            positionMarker.SetActive(true);
            _arcRenderer.enabled = true;

            if (_groundDetected)
            {
                positionMarker.transform.position = _groundPos + _lastNormal * 0.1f;
                positionMarker.transform.LookAt(_groundPos);
            }
            
            _arcRenderer.positionCount = _vertexList.Count;
            
            for(int i = 0; i < _vertexList.Count; i++)
                _arcRenderer.SetPosition(i, _vertexList[i]);

        }
        
        public void HandleInput()
        {
            if (!OVRInput.Get(OVRInput.Button.One) && _teleporting) Teleport();
            
            if (OVRInput.Get(OVRInput.Button.One))
                _hand.ChangeState(HandState.Teleport);

            if (_displayActive != OVRInput.Get(OVRInput.Button.One))
                ToggleDisplay(OVRInput.Get(OVRInput.Button.One));


        }
    }
}