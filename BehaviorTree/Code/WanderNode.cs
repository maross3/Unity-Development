using System.Collections;
using UnityEngine;

public class WanderNode : DecisionNode
{
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

    // overridden members don't need docs if base class has docs
    // try hovering over this function in the editor
    protected override int CalculateScore() => 100;

    public override bool Enact()
    {
        if (_moving) 
        {
            return true;
        }
        _moving = true;
        
        var direction = Random.Range(0, 4);
        var moves = Random.Range(1, 6);
        StartCoroutine(Move(direction, moves));
        return true;
    }
     
    // less docs, yay!
    protected override int IndexOfBestChildScore() => 0;

    /// <summary>
    /// Enacts movement for the character.
    /// </summary>
    /// <param name="direction">Number based direction.</param>
    /// <param name="moves">Number of moves in a directional sequence.</param>
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
        }
        _moving = false;
    }
}