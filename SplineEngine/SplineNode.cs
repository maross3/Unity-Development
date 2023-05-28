using UnityEngine;

public class SplineNode : MonoBehaviour
{
    // Needs accessor to mutate gameObjects transform.
    // Needs accessor to mutate gameObjects rigidbody.
    // Needs accessor to mutate gameObjects joints.
    // Create an interface to update for the joints
    // Create an interface to update for a collider too
    
    
    public Rigidbody2D rb;
    public SpringJoint2D spring;
    
    private void OnEnable()
    {
        rb ??= gameObject.AddComponent<Rigidbody2D>();
        spring ??= gameObject.AddComponent<SpringJoint2D>();
    }
}
