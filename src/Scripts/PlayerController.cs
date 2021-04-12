using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public float speed;
    public GameObject bulletObject;
    public GameObject bombObject;
    public GameObject explosionObject;
    public CameraShakeController cameraShake;
    public float bulletSpeed;
    public float bulletLifetime;
    private float lastBullet;
    public float bulletDelay;
    public float damage;
    private Animator anim;
    public float bombs;
    public float souls;
    public GameObject bulletPoint;
    public HealthBarHUDTester healthBarHUD;
    public Material[] material;
    Renderer rend;
    private bool warning;
    private bool tenSouls, twentySouls, thirtySouls;
    public DialogeController dialogeController;
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("ThemeDungeon");
        rend = GameObject.FindWithTag("Material").GetComponent<Renderer>();
        anim = GetComponent<Animator>();
        warning = true;
        tenSouls = true;
        twentySouls = true;
        thirtySouls = true;
        StartCoroutine(InitialDialoge());
    }
    void Update()
    {
        // Movement
        float hori = -Input.GetAxisRaw("Horizontal");
        float vert = -Input.GetAxisRaw("Vertical");

        float bulletHori = -Input.GetAxisRaw("BulletHorizontal");
        float bulletVert = -Input.GetAxisRaw("BulletVertical");

        Vector3 direction = new Vector3(hori, 0f, vert).normalized;
        if (direction.magnitude >= 0.1f)
        {
            controller.Move(direction * speed * Time.deltaTime);
            anim.SetFloat("Blend", 1f);
        }
        else
        {
            anim.SetFloat("Blend", 0f);
        }
        // Move rotation
        if (Input.GetKey(KeyCode.W) && !(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
        {
            transform.forward = new Vector3(-1f, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.S) && !(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
        {
            transform.forward = new Vector3(1f, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.A) && !(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
        {
            transform.forward = new Vector3(0f, 0f, -1f);
        }
        if (Input.GetKey(KeyCode.D) && !(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
        {
            transform.forward = new Vector3(0f, 0f, 1f);
        }
        // Shoot rotation
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.forward = new Vector3(-1f, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.forward = new Vector3(1f, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.forward = new Vector3(0f, 0f, -1f);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.forward = new Vector3(0f, 0f, 1f);
        }
        // Shooting
        if ((bulletHori != 0 || bulletVert != 0) && Time.time > lastBullet + bulletDelay)
        {
            Shoot(bulletHori, bulletVert);
            lastBullet = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            useBomb();
        }
        if (tenSouls && souls == 10)
        {
            dialogeController.CallDialoge("Fox: 10 Souls! I can buy something now.");
            tenSouls = false;
        }
        if (twentySouls && souls == 20)
        {
            dialogeController.CallDialoge("Fox: Easiest 20 Souls ever!...");
            twentySouls = false;
        }
        if (thirtySouls && souls == 30)
        {
            dialogeController.CallDialoge("Fox: 30 Souls! I can get something now!.");
            thirtySouls = false;
        }
    }
    void Shoot(float x, float z)
    {
        if (transform.rotation.y == 1)
        {
            GameObject bullet = Instantiate(bulletObject, bulletPoint.transform.position, transform.rotation) as GameObject;
            bullet.AddComponent<Rigidbody>().useGravity = false;
            bullet.GetComponent<Rigidbody>().velocity = new Vector3(
                (x < 0) ? Mathf.Floor(x) * bulletSpeed : Mathf.Ceil(x) * bulletSpeed,
                0,
                0
            );
        }
        if (transform.rotation.y == 0)
        {
            GameObject bullet = Instantiate(bulletObject, bulletPoint.transform.position, transform.rotation) as GameObject;
            bullet.AddComponent<Rigidbody>().useGravity = false;
            bullet.GetComponent<Rigidbody>().velocity = new Vector3(
                (x < 0) ? Mathf.Floor(x) * bulletSpeed : Mathf.Ceil(x) * bulletSpeed,
                0,
                0
            );
        }
        if (transform.rotation.y < 0)
        {
            GameObject bullet = Instantiate(bulletObject, bulletPoint.transform.position, transform.rotation) as GameObject;
            bullet.AddComponent<Rigidbody>().useGravity = false;
            bullet.GetComponent<Rigidbody>().velocity = new Vector3(
                0,
                0,
                (z < 0) ? Mathf.Floor(z) * bulletSpeed : Mathf.Ceil(z) * bulletSpeed
            );
        }
        if (transform.rotation.y > 0 && transform.rotation.y < 1)
        {
            GameObject bullet = Instantiate(bulletObject, bulletPoint.transform.position, transform.rotation) as GameObject;
            bullet.AddComponent<Rigidbody>().useGravity = false;
            bullet.GetComponent<Rigidbody>().velocity = new Vector3(
                0,
                0,
                (z < 0) ? Mathf.Floor(z) * bulletSpeed : Mathf.Ceil(z) * bulletSpeed
            );
        }
    }

    void useBomb()
    {
        if (warning)
        {
            dialogeController.CallDialoge("Fox: I better be careful with these");
            warning = false;
        }
        if (bombs > 0)
        {
            GameObject activeBomb = Instantiate(bombObject, transform.position, Quaternion.identity) as GameObject;
            bombs--;
            FindObjectOfType<AudioManager>().Play("UseBomb");
            StartCoroutine(Explote(activeBomb));
        }
    }

    IEnumerator Explote(GameObject activeBomb)
    {
        yield return new WaitForSeconds(1f);
        explosionObject.transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
        GameObject explosion = Instantiate(explosionObject, activeBomb.transform.position, Quaternion.Euler(new Vector3(90, 0, 0))) as GameObject;
        Destroy(activeBomb);
        FindObjectOfType<AudioManager>().Play("BombExplosion");
        StartCoroutine(ShockWave(explosion));
        StartCoroutine(cameraShake.Shake2(0.3f, 0.60f));
    }
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Bomb")
        {
            FindObjectOfType<AudioManager>().Play("Hit");
            StartCoroutine(cameraShake.Shake(.15f, .5f));
            bool isDead = healthBarHUD.Hurt(1f);
            if (isDead)
            {
                SceneManager.LoadScene(2);
            }
            StartCoroutine(HitPlayer());
        }
        if (collision.gameObject.tag == "End")
        {
            SceneManager.LoadScene(3);
        }
    }
    IEnumerator ShockWave(GameObject explosion)
    {
        yield return new WaitForSeconds(0.1f);
        explosion.GetComponent<Collider>().enabled = false;
    }
    IEnumerator HitPlayer()
    {
        yield return new WaitForSeconds(0.1f);
        rend.sharedMaterial = material[1];
        yield return new WaitForSeconds(0.1f);
        rend.sharedMaterial = material[0];
    }
    IEnumerator InitialDialoge()
    {
        yield return new WaitForSeconds(3f);
        dialogeController.CallDialoge("Fox:  Huh?...  Where am I?...");
    }
}

