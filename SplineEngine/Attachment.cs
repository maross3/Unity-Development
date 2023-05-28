using UnityEngine;

namespace _Dev.Attachment
{
    public class Attachment : MonoBehaviour
    {
        public SplineGenerator splineGenerator;
        public Transform attachmentFixtureObject;
        private Vector3 _offset;
        /// <summary>
        /// The speed of the attachment along the spline.
        /// </summary>
        public float speed = 1.0f; 

        /// <summary>
        /// The distance traveled along the current spline from the previous point.
        /// </summary>
        private float _distanceAlongSegment; 

        private void Start()
        {
            _offset = transform.position - attachmentFixtureObject.position;
            splineGenerator.Spline();
        }
    
        private void Update()
        {
            gameObject.transform.position = splineGenerator.GetClosestPointOnSplineMouseRelative() + _offset;

        }
    
    }
}
