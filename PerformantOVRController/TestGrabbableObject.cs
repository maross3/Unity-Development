using UnityEngine;

namespace PerformantOVRController
{

    public class TestGrabbableObject : MonoBehaviour, IGrabbableObject
    {
        [SerializeField] private Collider bladeBlockingCollider;
        public Transform gripPoint;
        [SerializeField] private float travelSpeed;
        [SerializeField] private Vector3 offset;
        private void OnCollisionEnter(Collision collision)
        {
            
            // if (collision.gameObject.GetComponent<KnifeSliceableAsync>() != null)
            // {
                // bladeBlockingCollider.isTrigger = true;
            // }
        }

        private void OnTriggerExit(Collider other)
        {
            // if (other.gameObject.GetComponent<KnifeSliceableAsync>() != null)
            // {
                // bladeBlockingCollider.isTrigger = false;
            // }
        }

        private Rigidbody _rb;
        private float lerpTime;
        private Vector3 gripOffset;
        public bool lerp;
        
        // needs to rotate to grip
        // player should not be able to jump off grabbable collider
        // needs a better method to detect if we are in the grab area.
        void FixedUpdate()
        {
               
            
            if (playerHand == null || playerHand == transform.parent) return;
            
            _rb.useGravity = false;
            if (!Mathf.Approximately(playerHand.position.x, gripPoint.position.x)
                && !Mathf.Approximately(playerHand.position.y, gripPoint.position.y)
                && !Mathf.Approximately(playerHand.position.z, gripPoint.position.z))
             {
                 lerpTime = Time.time;
                 gripOffset = transform.position - gripPoint.position;

                 if(!lerp)
                    transform.position = Vector3.MoveTowards(gripPoint.position, playerHand.position + gripOffset, 0.5f) ;
                 else
                    transform.position = Lerp(transform.position, playerHand.position + gripOffset, lerpTime, 2);
             }
             else
             {
                 transform.parent = playerHand;
                 _rb.isKinematic = true;
             }
        }

        private Vector3 Lerp(Vector3 strt, Vector3 end, float f, float currentLerpTime = 1)
        {
            //float timeSinceStart = Time.time - f;
            var percentComplete = currentLerpTime / f;
            var result = Vector3.Lerp(strt, end, percentComplete);
            return result;
            
        }

        private Vector3 startPos;
        private Vector3 endPos;
        void Start()
        {
            _rb = GetComponent<Rigidbody>();
            lerpTime = 0;
            startPos = gripPoint.transform.position;
            endPos = Vector3.zero;
        }

        // Update is called once per frame
        public Transform playerHand { get; set; }
    }
}