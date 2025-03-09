using System;
using System.Collections;
using UnityEngine;
using static UnityEditor.Progress;

public class BlockHit : MonoBehaviour
{
    /*public GameObject item;
    public Sprite emptyBlock;
    public int maxHits = -1;
    private bool animating;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!animating && maxHits != 0 && collision.gameObject.CompareTag("Player"))
        {
            if (collision.relativeVelocity.y > 0)
            {
                Hit();
            }
        }
    }

    private void Hit()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true; // show if hidden

        maxHits--;

        if (maxHits == 0) {
            spriteRenderer.sprite = emptyBlock;
        }

        if (item != null) {
            Instantiate(item, transform.position, Quaternion.identity);
        }

        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        animating = true;

        Vector3 restingPosition = transform.localPosition;
        Vector3 animatedPosition = restingPosition + Vector3.up * 0.5f;

        yield return Move(restingPosition, animatedPosition);
        yield return Move(animatedPosition, restingPosition);

        animating = false;
    }

    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        float elapsed = 0f;
        float duration = 0.125f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            transform.localPosition = Vector3.Lerp(from, to, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = to;
    }*/

    public Sprite emptySprite;
    public int MaxHits = 1;

    public GameObject item;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            if (collision.relativeVelocity.y > 0)
                Hit();
        }
    }

    private void Hit()
    {
        MaxHits--;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true; // show if hidden

        if(MaxHits == 0)
        {
            spriteRenderer.sprite = emptySprite;
            if (item != null)
            {
                //Spawn the required item i.e. Mushrooms, Starman, Coins
                Instantiate(item, transform.position, Quaternion.identity);

            }
        }
    }
}
