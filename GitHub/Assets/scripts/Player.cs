using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Sprite idle;
    [SerializeField] private Sprite down;
    [SerializeField] private Sprite up;

    private Rigidbody2D rb;
    private SpriteRenderer spriteR;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteR = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (rb.velocity.y > 0)
        {
            spriteR.sprite = up;
        }

        else if (rb.velocity.y < 0)
        {
            spriteR.sprite = down;
        }
        
        else
        {
            spriteR.sprite = idle;
        }
    }
}
