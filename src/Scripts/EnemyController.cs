using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class EnemyController : MonoBehaviour
{
    public HealthBarHUDTester healthBarHUD;
    public CameraShakeController cameraShake;
    public bool largeEnemy, smallEnemy, mediumEnemy;
    public float damage;
    public float health;
    public Material[] material;
    Renderer rend;
    private Animator anim;
    private PlayerController controller;
    private GameObject player;
    public float sightRange, attackRange;
    private bool playerInSightRange, playerInAttackRange;
    public LayerMask whatIsPlayer;
    private UnityEngine.AI.NavMeshAgent agent;
    private Vector3 homePoint;
    private float savedTime = 0f;
    private float delayTime = 1f;
    public bool randomizeSpeed;
    public bool randomizeSpeedFast;
    public GameObject deathEffect;
    public GameObject consumableHeart;
    public GameObject consumableBomb;
    public GameObject[] childrens;
    public bool isDragon;
    public bool skeleton;
    public GameObject bulletObject;
    public GameObject shootingPoint;
    public float fireRate;
    public bool isCleanner;
    private bool healthSpawned;
    private int probabilityHeart;
    private int probabilityBomb;
    private bool laughed;
    void  Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        homePoint = agent.transform.position;
        player = GameObject.FindWithTag("Player");
        controller = player.GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
        rend = GameObject.FindWithTag("Material").GetComponent<Renderer>();
        probabilityHeart = Random.Range(1, 100);
        probabilityBomb = Random.Range(1, 100);
        agent.enabled = false;
        if (largeEnemy)
        {
            damage = 1.0f;
            health = 50f;
        }
        else if (mediumEnemy)
        {
            damage = 0.50f;
            health = 25f;
        }
        else if (smallEnemy)
        {
            damage = 0.25f;
            health = 10f;
        }
        if (randomizeSpeed)
        {
            agent.speed = Random.Range(1.0f, 5.0f);
        }
        if (randomizeSpeedFast)
        {
            agent.speed = Random.Range(5.0f, 10.0f);
        }
        if (isDragon)
        {
            InvokeRepeating("ShootNowAndThen", 0, fireRate);

        }
        if (isCleanner)
        {
            InvokeRepeating("CleanPlayer", 0, 0.5f);
        } 
        laughed = false;
    }
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        anim.SetBool("playerInSight", playerInSightRange);
        if (isDragon == false)
        {
            anim.SetBool("playerInAttackRange", playerInAttackRange);
        }
        else if (isDragon == true && health > 0)
        {
            transform.LookAt(new Vector3(player.transform.position.x, 0, player.transform.position.z));
            anim.SetBool("playerInAttackRange2", playerInAttackRange);
        }
        if (agent.enabled)
        {
            if (!playerInSightRange)
            {
                Stop();
            }
            if (playerInSightRange)
            {
                if (skeleton)
                {
                    if (laughed == false)
                    {
                        FindObjectOfType<AudioManager>().Play("EskeletonLaugh");
                        laughed = true;
                    }

                }
                ChasePlayer();
            }
        }
    }
    private void Stop()
    {
        agent.SetDestination(homePoint);
    }
    private void ChasePlayer()
    {
        agent.SetDestination(player.transform.position);
    }
    void ShootNowAndThen()
    {
        if (playerInAttackRange == false && agent.enabled)
        {
            agent.enabled = false;
            transform.LookAt(new Vector3(player.transform.position.x, 0, player.transform.position.z));
            anim.SetBool("playerInAttackRange", playerInSightRange);
        }
    }
    public void AlertObservers(string message)
    {
        if (message.Equals("AttackAnimationEnded"))
        {
            GameObject bullet = Instantiate(bulletObject, new Vector3(shootingPoint.transform.position.x, shootingPoint.transform.position.y, shootingPoint.transform.position.z), Quaternion.identity) as GameObject;
            FindObjectOfType<AudioManager>().Play("DragonFire");
            bullet.AddComponent<Rigidbody>().useGravity = false;
            bullet.GetComponent<ParticleSystem>().Play();
            bullet.GetComponent<Rigidbody>().velocity = (new Vector3(player.transform.position.x, 0, player.transform.position.z) - new Vector3(bullet.transform.position.x, 0, bullet.transform.position.z)).normalized * 10;
            anim.SetBool("playerInAttackRange", false);
            agent.enabled = true;
        }
    }
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Bomb")
        {
            FindObjectOfType<AudioManager>().Play("EnemyHit");
            print("Bomb");
            health = -1;
            anim.SetFloat("health", health);
            if (health < 0)
            {
                agent.enabled = false;
                GetComponent<Collider>().enabled = false;
                StartCoroutine(DeathEnemy());
            }
        }
        if (collision.gameObject.tag == "Bullet")
        {
            FindObjectOfType<AudioManager>().Play("EnemyHit");
            collision.gameObject.GetComponent<BulletController>().KillBullet();
            StartCoroutine(HitEnemy());
            StartCoroutine(cameraShake.Shake2(.05f, .25f));
            health -= controller.damage;
            anim.SetFloat("health", health);
            if (health < 0)
            {
                agent.enabled = false;
                GetComponent<Collider>().enabled = false;
                StartCoroutine(DeathEnemy());
            }
        }
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(HitPlayer());
            if (damage == 0.25f)
            {
                healthBarHUD.Heal(0.25f);
            }
            else if (damage == 0.5f)
            {
                healthBarHUD.Heal(0.5f);
            }
            else
            {
                healthBarHUD.Heal(1f);
            }
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
        FindObjectOfType<AudioManager>().Play("Hit");
        StartCoroutine(cameraShake.Shake(.15f, .5f));
        bool isDead = healthBarHUD.Hurt(damage);
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
        Destroy(GetComponent<Collider>());
        foreach (GameObject children in childrens)
        {
            Destroy(children);
        }
        yield return new WaitForSeconds(0.3f);
        GetComponentInChildren<Renderer>().sharedMaterial = material[2];
        yield return new WaitForSeconds(0.4f);
        GetComponentInChildren<Renderer>().sharedMaterial = material[3];
        yield return new WaitForSeconds(0.3f);
        GetComponentInChildren<Renderer>().sharedMaterial = material[2];
        yield return new WaitForSeconds(0.4f);
        GetComponentInChildren<Renderer>().sharedMaterial = material[3];
        FindObjectOfType<AudioManager>().Play("EnemyDeath");
        GameObject effect = Instantiate(deathEffect, new Vector3(transform.position.x, 0.3f, transform.position.z), Quaternion.Euler(new Vector3(90, 0, 0)));
        if (probabilityHeart < 10) // 10 in 100
        {
            healthSpawned = true;
            GameObject healthConsumable = Instantiate(consumableHeart, transform.position, Quaternion.identity);
        }
        if (probabilityBomb < 5 && healthSpawned == false) // 5 in 100
        {
            GameObject bombConsumable = Instantiate(consumableBomb, transform.position, Quaternion.identity);
        }
        player.GetComponent<PlayerController>().souls++;
        Destroy(gameObject);
    }
    void CleanPlayer()
    {
        if (rend.sharedMaterial == material[1])
        {
            rend.sharedMaterial = material[0];
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}