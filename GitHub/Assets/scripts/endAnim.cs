using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class endAnim : MonoBehaviour
{


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Camera.main.gameObject.GetComponent<CamFollow>().theEnd = true;
        collision.gameObject.GetComponent<Player>().AnimationEnd();
    }
}
