using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupMovement : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody2D rigidbody2;
    public float Dir;
    public float Speed;
    void Start()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody2.velocity = new Vector2(Dir * Speed * Time.deltaTime, 0f);
    }
}
