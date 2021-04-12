using System.Collections;
using UnityEngine;
public class BulletController : MonoBehaviour
{
    public GameObject player;
    public GameObject hitEffect;
    void Start()
    {
        StartCoroutine(Delay());
        if ((GetComponent<Rigidbody>().velocity.x == 0) && (GetComponent<Rigidbody>().velocity.z == 0) || (GetComponent<Rigidbody>().velocity.y != 0))
        {
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        FindObjectOfType<AudioManager>().Play("BulletExplosion");
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 4f);
        Destroy(gameObject);
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(GameObject.Find("Fox").GetComponent<PlayerController>().bulletLifetime);
        FindObjectOfType<AudioManager>().Play("BulletExplosion");
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 4f);
        Destroy(gameObject);
    }
    public void KillBullet()
    {
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        FindObjectOfType<AudioManager>().Play("BulletExplosion");
        Destroy(effect, 4f);
        Destroy(gameObject);
    }
}
