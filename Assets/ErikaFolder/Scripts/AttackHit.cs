using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHit : MonoBehaviour
{
    public GameObject particleEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            GameObject obj = Instantiate(particleEffect);
            obj.transform.position = this.transform.position;

            Destroy(this.gameObject);
        }
    }
}
