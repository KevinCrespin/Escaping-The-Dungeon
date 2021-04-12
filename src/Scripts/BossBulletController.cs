using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletController : MonoBehaviour
{
    public GameObject hitEffect;
    void OnCollisionEnter(Collision collision)
    {
        if (hitEffect != null)
        {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 4f);
        }
        Destroy(gameObject);
    }
}

