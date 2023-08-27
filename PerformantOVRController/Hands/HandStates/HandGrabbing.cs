using PerformantOVRController.Hands.Poses;
using UnityEngine;

namespace PerformantOVRController.Hands.HandStates
{
    public class HandGrabbing : HandStateClass
        
    {
        public override HandState handState => HandState.Grabbing;
        
        public GameObject grabbedObject;
        public Hand thisHand;
        public HandPose statePose;
        private bool grabbing;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (grabbing && grabbedObject != null)
            {
                if (grabbedObject.layer != 6) grabbedObject.layer = 6;
                //grabbedObject.transform.position = thisHand.gripTransform.position;
                //grabbedObject.transform.parent = thisHand.gameObject.transform;
                //grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                //grabbing = false;
                //Vector3.Lerp(grabbedObject.transform.position, thisHand.gripTransform.position, thisHand.lerpSpeed);
            }
        
        }


        public override void EnterState()
        {
            if (grabbedObject == null)
            {
                thisHand.ChangeState(Hands.HandState.Closed);
                return;
            }

            var obj =(IGrabbableObject) grabbedObject.GetComponent(typeof(IGrabbableObject));
            obj.playerHand = thisHand.transform;

            grabbing = true;
            thisHand.GripUp += SwitchToOpen;
        }

        void SwitchToOpen()
        {
        
            thisHand.ChangeState(Hands.HandState.Open);
            // if (grabbedObject == null) return;
            // if(grabbedObject.transform.parent == thisHand.transform)
            //     grabbedObject.transform.parent = null;
        }
    
        public override void ExitState()
        {
            var obj =(IGrabbableObject) grabbedObject.GetComponent(typeof(IGrabbableObject));
            obj.playerHand = null;
        
            thisHand.GripUp -= SwitchToOpen;
            grabbing = false;
        }

        public override void OverrideToGrabbed()
        {
            throw new System.NotImplementedException();
        }
    }
}
