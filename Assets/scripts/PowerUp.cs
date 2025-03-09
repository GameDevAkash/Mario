using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum Type
    {
        magicMushroom,
        starMan
    }
    
    public Type type;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            Collect(collision.gameObject);
        }
    }

    void Collect(GameObject Player)
    {
        switch (type) { 
            
            case Type.magicMushroom:
                Player.GetComponent<PlayerMovement>().Grow();
                Destroy(this.gameObject);
                break;

            case Type.starMan:
                break;
        }
    }

}
