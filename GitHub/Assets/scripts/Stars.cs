using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour
{
    [SerializeField] private int repulseForce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int xForce = Random.Range(1, 5);
        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(xForce , repulseForce), ForceMode2D.Impulse);
    }
}
