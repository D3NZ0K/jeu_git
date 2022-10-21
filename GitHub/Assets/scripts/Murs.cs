using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Murs : MonoBehaviour
{
    [SerializeField] Vector2 repulseForce;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(repulseForce, ForceMode2D.Impulse);
    }
}
