using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private Sprite idle;
    [SerializeField] private Sprite down;
    [SerializeField] private Sprite up;

    private Rigidbody2D rb;
    private SpriteRenderer spriteR;

    private Camera mainCamera;


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

        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.y < -0.04 * Screen.height)
        {
            SceneManager.LoadScene(0);
        }
    }

}
