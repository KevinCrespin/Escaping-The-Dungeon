using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ItemController : MonoBehaviour
{
    private GameObject player;
    public bool speed;
    public bool fireRate;
    public bool damage;
    public bool range;
    public bool health;
    public bool bomb;
    public HealthBarHUDTester healthBarHUD;
    [TextArea(3, 10)]
    public string message;
    public GameObject children;
    public ParticleSystem[] particles;
    public bool itemPicked;
    public GameObject[] textController;
    public GameObject effect;
    public PlayerController playerC;
    void  Awake()
    {
        player = GameObject.Find("Fox");
        playerC = player.GetComponent<PlayerController>();
        itemPicked = false;
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (health)
            {
                healthBarHUD.Heal(0.5f);
                Destroy(gameObject);
                FindObjectOfType<AudioManager>().Play("PickBomb");
            }
            if (bomb)
            {
                playerC.bombs++;
                Destroy(gameObject);
                FindObjectOfType<AudioManager>().Play("PickBomb");
            }
            if (speed && (playerC.souls >= 30))
            {
                playerC.speed += 5f;
                playerC.bulletSpeed += 20f;
                playerC.souls -= 30;
                Controller();
            }
            
            if (damage && (playerC.souls >= 20))
            {
                playerC.damage += 1f;
                playerC.souls -= 20;
                Controller();
            }
            if (fireRate && (playerC.souls >= 10))
            {
                playerC.bulletDelay -= 0.3f;
                playerC.souls -= 10;
                Controller();
            }
            if (range)
            {
                playerC.bulletLifetime += 2f;
                playerC.damage += 1f;
                Controller();
            }
        }
    }
    void Controller()
    {
        itemPicked = true;
        FindObjectOfType<AudioManager>().Play("ItemGrabbed");
        children.GetComponent<MeshRenderer>().enabled = false;
        foreach (ParticleSystem particle in particles)
        {
            Destroy(particle);
        }
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        GameObject powerUp = Instantiate(effect, new Vector3(transform.position.x, 0.5f, transform.position.z), Quaternion.Euler(new Vector3(90, 0, 0)));
        float pSpeed = playerC.speed;
        playerC.speed = 2;
        StartCoroutine(textController[0].GetComponent<TextController>().TypeMessageWithSlow(message, pSpeed, powerUp, true));
        StartCoroutine(textController[1].GetComponent<TextController>().TypeMessageWithSlow(message, pSpeed, powerUp, false));
        StartCoroutine(WaitForTextController());
    }

    public IEnumerator WaitForTextController()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }

}
