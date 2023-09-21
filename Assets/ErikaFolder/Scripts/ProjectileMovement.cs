using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    public float speed = 5;

    void Update()
    {
        transform.Translate (Vector3.forward * speed * Time.deltaTime);
    }
}
