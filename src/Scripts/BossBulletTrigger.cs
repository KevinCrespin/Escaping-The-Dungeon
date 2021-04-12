using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BossBulletTrigger : MonoBehaviour
{
    public HealthBarHUDTester healthBarHUD;
    private float damage;
    public Material[] material;
    Renderer rend;
    private GameObject player;
    public CameraShakeController cameraShake;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rend = GameObject.FindWithTag("Material").GetComponent<Renderer>();
        damage = 1f;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(HitPlayer());
        }
    }
    IEnumerator HitPlayer()
    {
        StartCoroutine(cameraShake.Shake(.15f, .5f));
        bool isDead = healthBarHUD.Hurt(damage);
        if (isDead)
        {
            SceneManager.LoadScene(2);
        }
        rend.sharedMaterial = material[1];
        yield return new WaitForSeconds(0.1f);
        rend.sharedMaterial = material[0];
    }
}
