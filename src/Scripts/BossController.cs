using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
    public HealthBarHUDTester healthBarHUD;
    private float damage;
    private float health;
    public Material[] material;
    Renderer rend;
    private Animator anim;
    private PlayerController controller;
    private GameObject player;
    public CameraShakeController cameraShake;
    public float firstRange, attackRange;
    public LayerMask whatIsPlayer;
    private float savedTime = 0f;
    private float delayTime = 1f;
    private float savedTeleportTime = 0f;
    private float delayTeleportTime = 2f;
    private int teleportCount = 0;
    private bool trueOnce;
    private bool decoy;
    private Vector3[] possiblePossitions;
    public GameObject bulletObject;
    public GameObject staff;
    public GameObject deathEffect;
    public DialogeController dialogeController;
    public GameObject exitPortal;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        controller = player.GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
        rend = GameObject.FindWithTag("Material").GetComponent<Renderer>();

        damage = 1f;
        health = 500f;
        trueOnce = true;
        decoy = true;

        possiblePossitions = new Vector3[]{new Vector3(-38f, 0f, -158f), new Vector3(-52f, 0f, -158f), new Vector3(-66f, 0f, -158f),
                                           new Vector3(-38f, 0f, -150.5f),                             new Vector3(-66f, 0f, -150.5f),
                                           new Vector3(-38f, 0f, -143f),                               new Vector3(-66f, 0f, -143f)};
    }
    void Update()
    {
        if (decoy)
        {
            if (trueOnce)
            {
                if (Physics.CheckSphere(transform.position, firstRange, whatIsPlayer))
                {
                    FindObjectOfType<AudioManager>().Play("Laugh");
                    FindObjectOfType<AudioManager>().Stop("ThemeDungeon");
                    FindObjectOfType<AudioManager>().Play("BossTheme");
                    dialogeController.CallDialoge("Fox: AHHHHHHHH!");
                    trueOnce = false;
                }
            }
            else
            {
                decoy = false;
            }
        }
        else
        {
            if (Physics.CheckSphere(transform.position, attackRange, whatIsPlayer) && health > 0)
            {
                if ((Time.time - savedTeleportTime) > delayTeleportTime)
                {
                    teleportCount++;
                    savedTeleportTime = Time.time;
                    transform.position = possiblePossitions[new System.Random().Next(7)];
                    if ((teleportCount % 2) == 0 && health > 250)
                    {
                        AttackPlayer();
                    }
                    else if (health <= 250)
                    {
                        AttackPlayer();
                    }
                }
                transform.LookAt(new Vector3(player.transform.position.x, 0, player.transform.position.z));
            }
        }
    }
    private void AttackPlayer()
    {
        anim.SetBool("attack", true);
    }
    public void AlertObservers(string message)
    {
        if (message.Equals("AttackAnimationEnded"))
        {
            FindObjectOfType<AudioManager>().Play("BossFire");
            GameObject bullet = Instantiate(bulletObject, new Vector3(staff.transform.position.x, staff.transform.position.y, staff.transform.position.z), Quaternion.identity) as GameObject;
            bullet.AddComponent<Rigidbody>().useGravity = false;
            bullet.GetComponent<ParticleSystem>().Play();
            bullet.GetComponent<Rigidbody>().velocity = (new Vector3(player.transform.position.x, 0, player.transform.position.z) - new Vector3(bullet.transform.position.x, 0, bullet.transform.position.z)).normalized * 40;
            anim.SetBool("attack", false);
        }
    }
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Bomb")
        {
            health -= 50;
            FindObjectOfType<AudioManager>().Play("EnemyHit");
            anim.SetFloat("health", health);
            if (health < 0)
            {
                GetComponent<Collider>().enabled = false;
                StartCoroutine(DeathEnemy());
            }
        }
        if (collision.gameObject.tag == "Bullet")
        {
            if (!decoy)
            {
                FindObjectOfType<AudioManager>().Play("EnemyHit");
                collision.gameObject.GetComponent<BulletController>().KillBullet();
                StartCoroutine(HitEnemy());
                StartCoroutine(cameraShake.Shake2(.05f, .25f));
                health -= controller.damage;
                anim.SetFloat("health", health);
                if (health < 0)
                {
                    GetComponent<Collider>().enabled = false;
                    StartCoroutine(DeathEnemy());
                }
            }
        }
        if (collision.gameObject.tag == "Player")
        {
            healthBarHUD.Heal(1f);
            StartCoroutine(HitPlayer());
        }
    }
    void OnTriggerStay(Collider collision)
    {
        if ((Time.time - savedTime) > delayTime)
        {
            savedTime = Time.time;
            if (collision.gameObject.tag == "Player")
            {
                StartCoroutine(HitPlayer());
            }
        }
    }
    IEnumerator HitPlayer()
    {
        StartCoroutine(cameraShake.Shake(.15f, .5f));
        bool isDead = healthBarHUD.Hurt(damage);
        FindObjectOfType<AudioManager>().Play("Hit");
        if (isDead)
        {
            SceneManager.LoadScene(2);
        }
        yield return new WaitForSeconds(0.1f);
        rend.sharedMaterial = material[1];
        yield return new WaitForSeconds(0.1f);
        rend.sharedMaterial = material[0];
    }
    IEnumerator HitEnemy()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponentInChildren<Renderer>().sharedMaterial = material[1];
        yield return new WaitForSeconds(0.1f);
        GetComponentInChildren<Renderer>().sharedMaterial = material[2];
    }
    IEnumerator DeathEnemy()
    {
        FindObjectOfType<AudioManager>().Play("BossDie");
        yield return new WaitForSeconds(2f);
        GetComponentInChildren<Renderer>().sharedMaterial = material[2];
        yield return new WaitForSeconds(0.4f);
        GetComponentInChildren<Renderer>().sharedMaterial = material[3];
        yield return new WaitForSeconds(0.3f);
        GetComponentInChildren<Renderer>().sharedMaterial = material[2];
        yield return new WaitForSeconds(0.4f);
        GetComponentInChildren<Renderer>().sharedMaterial = material[3];
        yield return new WaitForSeconds(0.3f);
        GetComponentInChildren<Renderer>().sharedMaterial = material[2];
        yield return new WaitForSeconds(0.3f);
        GetComponentInChildren<Renderer>().sharedMaterial = material[3];
        yield return new WaitForSeconds(0.4f);
        FindObjectOfType<AudioManager>().Stop("BossTheme");
        FindObjectOfType<AudioManager>().Play("ThemeDungeon");
        GameObject effect = Instantiate(deathEffect, new Vector3(transform.position.x, 0.5f, transform.position.z), Quaternion.Euler(new Vector3(90, 0, 0)));
        player.GetComponent<PlayerController>().souls += 50;
        dialogeController.CallDialoge("Fox: Phew! That was close!...");
        GameObject end = Instantiate(exitPortal, new Vector3(transform.position.x, 0.5f, transform.position.z), Quaternion.Euler(new Vector3(90, 0, 0)));
        Destroy(gameObject);

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, firstRange);
    }
}
