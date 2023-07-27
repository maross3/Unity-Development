using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Dev.Attachment
{
    public class SplineGenerator : MonoBehaviour
    {
        /// <summary>
        /// The old position of the mouse.
        /// </summary>
        private Vector3 _oldPosition;
        
        /// <summary>
        /// The index of the active control point, selected by the mouse input.
        /// </summary>
        private int _activeControlIndex;

        /// <summary>
        /// Point game objects on the spline. Will be replaced by Spline Nodes
        /// </summary>
        private readonly List<GameObject> _pointsOnSpline = new List<GameObject>();
    
        /// <summary>
        /// Updates the spline to mouse position if true.
        /// </summary>
        private bool _moving;
    
        /// <summary>
        /// Dampening ration for each of the spring joints in the spline. 0.0f is no dampening, 1.0f is critical dampening.
        /// </summary>
        [Range(0.0f, 1.0f)]
        public float lineJointDampeningRatio;
   
        /// <summary>
        /// Automatically configures the distance between the spline points by setting the parameter of the spring joint.
        /// </summary>
        public bool autoConfigureDistance;
    
        /// <summary>
        /// The force that will be applied to the spline points when the mouse is clicked, in the direction of the mouse.
        /// </summary>
        public float grabForce;
    
        /// <summary>
        /// The frequency of the spring joints in the spline.
        /// </summary>
        public float lineJointFrequency;
    
        /// <summary>
        /// Transforms of the game objects that will be used to define the starting shape of the curve.
        /// </summary>
        public Transform[] controlPoints;

        /// <summary>
        /// Spline points for easy access.
        /// </summary>
        private Vector3[] _splinePoints;

        /// <summary>
        /// The number of points on the spline curve.
        /// </summary>
        public int splineResolution = 10;

        /// <summary>
        /// The line renderer to render the spine. Thank you r3dux :)
        /// </summary>
        private LineRenderer _lineRenderer;
        
        /// <summary>
        /// Generates the spline curve.
        /// </summary>
        /// <returns>The points in the resulting curve.</returns>
        public void Spline() =>
            _splinePoints = GeneratePointsOnSpline(splineResolution, controlPoints.Length);

        public void Awake()
        {
            _lineRenderer = gameObject.CreateSplineRenderer(Array.Empty<Vector3>());
        }

        /// <summary>
        /// Adds the spline node object and initializes the spring joints, rb and spline. Make this happen via interface.
        /// </summary>
        [Button]
        public void AddPhysics()
        {
            for (var i = 0; i < _splinePoints.Length; i++)
            {
                var go = new GameObject(); 
                var splineNode = go.AddComponent<SplineNode>();
                go.transform.position = _splinePoints[i];
                splineNode.rb.gravityScale = 0;
                splineNode.rb.angularDrag = 200;
                splineNode.rb.drag = 150;
            
                if(_pointsOnSpline.Count > 0)
                {
                    var springJoint = splineNode.spring;
                    springJoint.connectedBody = _pointsOnSpline[i - 1].GetComponent<Rigidbody2D>();
                    springJoint.frequency = lineJointFrequency;
                    springJoint.dampingRatio = lineJointDampeningRatio;
                    springJoint.autoConfigureDistance = autoConfigureDistance;
                }
                _pointsOnSpline.Add(go);
            }

            _pointsOnSpline[^1].GetComponent<Rigidbody2D>().isKinematic = true;
        
        }

        /// <summary>
        /// Generates the points on the spline curve.
        /// </summary>
        /// <param name="numPointsPerSegment">How many points each segment contains.</param>
        /// <param name="numSegments">The total amount of segments the spline has.</param>
        /// <returns>An array of positions for each of the calculated nodes.</returns>
        /// <remarks>Based on the Catmull-Rom spline algorithm. Use these points to create and initialize nodes.</remarks>
        private Vector3[] GeneratePointsOnSpline(int numPointsPerSegment, int numSegments)
        {
            var splinePoints = new Vector3[numPointsPerSegment * numSegments];
            var tStep = 1f / numPointsPerSegment;

            for (var i = 0; i < numSegments - 1; i++)
            {
                var p0 = i == 0 ? 0 : i - 1;
                var p2 = i == numSegments - 1 ? numSegments - 1 : i + 1;
                var p3 = i == numSegments - 2 ? numSegments - 1 : i + 2;

                for (var j = 0; j < numPointsPerSegment; j++)
                {
                    var t = j * tStep;
                    var t2 = t * t;
                    var t3 = t2 * t;

                    var pointOnSpline = 0.5f * ((2 * controlPoints[i].position) +
                                                (-controlPoints[p0].position + controlPoints[p2].position) * t +
                                                (2 * controlPoints[p0].position - 5 * controlPoints[i].position + 4 * controlPoints[p2].position - controlPoints[p3].position) * t2 +
                                                (-controlPoints[p0].position + 3 * controlPoints[i].position - 3 * controlPoints[p2].position + controlPoints[p3].position) * t3);

            
                    
                    Debug.DrawLine(pointOnSpline, pointOnSpline + Vector3.up, Color.magenta, 10.0f);
                    if (pointOnSpline != Vector3.zero)    
                        splinePoints[i * numPointsPerSegment + j] = pointOnSpline;
                }
            }
            splinePoints = splinePoints.Where(x => x != Vector3.zero).ToArray();
            
            _lineRenderer.UpdateSplineRenderer(splinePoints);
            return splinePoints;
        }

        [Button(ButtonSizes.Medium)]
        public void DebugSpline()
        {
            if (_splinePoints == null || _splinePoints.Length == 0) Spline();
            if (_pointsOnSpline == null || _pointsOnSpline.Count == 0) return;
            
            for (var i = 0; i < _splinePoints.Length - 1; i++)
            {
                Debug.DrawLine(_pointsOnSpline[i].transform.position, 
                    _pointsOnSpline[i + 1].transform.position, Color.green);
                _lineRenderer.SetPosition(i, _pointsOnSpline[i].transform.position);
            }
        }

        /// <summary>
        /// Calculates the position of a point on the spline curve at a given time
        /// (0 less than or equal to t less than or equal to 1).
        /// </summary>
        /// <param name="t">The time step to calculate the point at.</param>
        /// <returns>The spline point at t.</returns>
        private Vector3 GetSplinePoint(float t)
        {
            // Get the indices of the control points that surround the time t
            var p0 = Mathf.FloorToInt(t * (_pointsOnSpline.Count - 1));
            var p1 = Mathf.Clamp(p0, 0, _pointsOnSpline.Count - 1);
            var p2 = Mathf.Clamp(p1 + 1, 0, _pointsOnSpline.Count - 1);
            var p3 = Mathf.Clamp(p2 + 2, 0, _pointsOnSpline.Count - 1);

            // Calculate the tangents at each control point
            var tangent1 = 0.5f * (_pointsOnSpline[p2].transform.position
                                   - _pointsOnSpline[p0].transform.position);
            var tangent2 = 0.5f * (_pointsOnSpline[p3].transform.position - _pointsOnSpline[p1].transform.position);

            // Calculate the position of the point on the spline curve
            var t2 = t * t;
            var t3 = t * t * t;

            var pointOnSpline = (2 * t3 - 3 * t2 + 1) * _pointsOnSpline[p1].transform.position +
                                (t3 - 2 * t2 + t) * tangent1 +
                                (-2 * t3 + 3 * t2) * _pointsOnSpline[p2].transform.position +
                                (t3 - t2) * tangent2;
        
            return pointOnSpline;
        }

        /// <summary>
        /// Gets the closest point on the spline relative to the screen space mouse position.
        /// </summary>
        /// <returns>The point closest to the mouse's position.</returns>
        public Vector3 GetClosestPointOnSplineMouseRelative()
        {
            return GetClosestPointOnSpline(GetGlobalMousePosition());
        }

        /// <summary>
        /// Get the global mouse position.
        /// </summary>
        /// <TODO>Export to input class</TODO>
        /// <returns>The mouse position in screen space.</returns>
        private static Vector3 GetGlobalMousePosition()
        {
            var mousePosition = Input.mousePosition;
            mousePosition.z = -Camera.main.transform.position.z;
            return Camera.main.ScreenToWorldPoint(mousePosition);
        }

        /// <summary>
        /// Calculate the closest point on the spline to the provided position using binary search.
        /// </summary>
        /// <param name="position">The relative position to calculate the closest spline point.</param>
        /// <returns>The closest spline point on the curve.</returns>
        private Vector3 GetClosestPointOnSpline(Vector3 position)
        {
            if (_pointsOnSpline == null || _pointsOnSpline.Count == 0) return Vector3.zero;
            
            // todo make this serialized as attachment sensitivity
            const float STEP_SIZE = 0.01f;
            var t = 0f;
            var minDistance = float.MaxValue;
            var closestPoint = Vector3.zero;
        
            while (t <= 1)
            {
                var pointOnSpline = GetSplinePoint(t);
                var distance = Vector3.Distance(pointOnSpline, position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestPoint = pointOnSpline;
                }

                t += STEP_SIZE;
            }

            return closestPoint;
        }

        private void Update()
        {
            DebugSpline();
            if (Input.GetMouseButtonDown(0))
            {
                var closestPoint = GetClosestPointOnSplineMouseRelative();
                _activeControlIndex = GCMath.FindClosestPoint(_splinePoints, closestPoint);

                if (_activeControlIndex == -1) return;
                _moving = true;
                _oldPosition = GetGlobalMousePosition();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _moving = false;
                _oldPosition = Vector3.zero;
            }
            else if (Input.GetMouseButton(0) && _moving)
            {
                var newPosition = GetGlobalMousePosition();
                var delta = newPosition - _oldPosition;
                _pointsOnSpline[_activeControlIndex].GetComponent<Rigidbody2D>().MovePosition(
                    _pointsOnSpline[_activeControlIndex].transform.position + delta
                    * grabForce); 
                _splinePoints[_activeControlIndex] = _pointsOnSpline[_activeControlIndex].transform.position;
                _oldPosition = newPosition;
            }
        }
    }
}
