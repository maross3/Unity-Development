using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public int foodValue;
    public float decayTime;
    
    private Rigidbody2D _rigidbody2D;

    private void OnCollisionEnter2D(Collision2D other)
    {
        other.gameObject.TryGetComponent(out LilDude lilDude);
        if (lilDude == null) return;
        
        lilDude.Eat(foodValue);
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DecayFood());
    }

    private IEnumerator DecayFood()
    {
        yield return new WaitForSeconds(decayTime);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
