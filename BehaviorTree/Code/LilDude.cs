using System.Collections;
using UnityEngine;

public class LilDude : MonoBehaviour
{
    private BehaviorTree basicNeedsTree;
    
    private int _hunger;
    
    /// <summary>
    /// Flag to determine if the character is currently moving.
    /// </summary>
    private bool _moving;
    
    /// <summary>
    /// Horizontal constraints for the character.
    /// </summary>
    private readonly Vector2 _horizontalConstraint = new(-11, 11);
    
    /// <summary>
    /// Vertical constraints for the character.
    /// </summary>
    private readonly Vector2 _verticalConstraint = new(-5, 5);
    
    private void Start()
    { 
        var direction = Random.Range(0, 4);
        var moves = Random.Range(1, 6);
        StartCoroutine(Move(direction, moves));
        FoodManager.foodSpawned += FoodSpawned;
        
        //basicNeedsTree = gameObject.AddComponent<BehaviorTree>();
    }

    private void Update()
    {
        //basicNeedsTree.Evaluate();
    }
    
    private void FoodSpawned(object sender, Vector2 e)
    {
        
        var distance = Vector2.Distance(transform.position, e);
        if (distance > 10) return;
        
        StopCoroutine(Move(0, 0));
        StartCoroutine(ChaseFood(e));
    }

    private IEnumerator ChaseFood(Vector2 foodLocation)
    {
        while (transform.position != (Vector3) foodLocation)
        {
            yield return new WaitForSeconds(2);
            transform.position = Vector2.MoveTowards(transform.position, foodLocation, 1);
        }
    }
    
    private IEnumerator Move(int direction, int moves)
    {
        for (var i = 0; i < moves; i++)
        {
            switch (direction)
            {
                case 0:
                    if (transform.position.y >= _verticalConstraint.y) break;
                    transform.position += Vector3.up;
                    break;
                case 1:
                    if (transform.position.y <= _verticalConstraint.x) break;
                    transform.position += Vector3.down;
                    break;
                case 2:
                    if (transform.position.x <= _horizontalConstraint.x) break;
                    transform.position += Vector3.left;
                    break;
                case 3:
                    if (transform.position.x >= _horizontalConstraint.y) break;
                    transform.position += Vector3.right;
                    break;
            }

            var nextStep = Random.Range(0.25f, 1.5f);
            yield return new WaitForSeconds(nextStep);
            
            direction = Random.Range(0, 4);
            moves = Random.Range(1, 6);
        }
        _moving = false;
        StartCoroutine(Move(direction, moves));
    }

    public void Eat(int foodValue)
    {
        _hunger += foodValue;
        Debug.Log($"I ate food: {foodValue}");
    }
}
