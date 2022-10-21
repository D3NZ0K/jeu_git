using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{

    public Transform target;
    public float smoothSpeed = 0.3f;

    private Vector3 currentVelocity;
    public Vector3 offset;
    private Vector3 newPos;

    private void LateUpdate()
    {            
        if (target.gameObject.GetComponent<Rigidbody2D>().velocity.y <= 0) {
                newPos = transform.position;
        }

        else if (target.position.y > transform.position.y)
        {
            newPos = new Vector3(transform.position.x, target.position.y, -10);
            transform.position = Vector3.SmoothDamp(transform.position, newPos, ref currentVelocity, smoothSpeed * Time.deltaTime, 40f);
        }
    }
}


//transform.position = Vector3.SmoothDamp(transform.position, newPos, ref currentVelocity, smoothSpeed * Time.deltaTime)