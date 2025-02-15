using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float RightDir = 1;
    float LeftDir = -1;
    float dir = 0;
    [SerializeField]float Speed = 1;
    Rigidbody2D rigidbody2;

    private void Awake()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        dir= RightDir;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Pipe")
        {
            ReverseDirection();
        }
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        rigidbody2.velocity = new Vector2(dir * Speed * Time.fixedDeltaTime, 0);
    }

    public void ReverseDirection()
    {
        if ((dir == RightDir))
        {
            dir = LeftDir;
        }
        else if ((dir == LeftDir)) 
        { 
            dir = RightDir;
        }
    }
}
