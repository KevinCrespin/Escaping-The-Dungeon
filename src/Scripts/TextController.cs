using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextController : MonoBehaviour
{
    public TextMeshPro textMesh;
    public PlayerController player;
    private Animator anim;
    public bool souls;
    public bool bombs;
    public bool speed;
    public bool range;
    public bool bulletRate;
    public bool bulletSpeed;
    public bool damage;
    private string temp;
    public DialogeController dialogeController;
    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }
    void Update()
    {
        if (souls)
        {
            textMesh.SetText($"x {player.souls}");
        }
        if (bombs)
        {
            textMesh.SetText($"x {player.bombs}");
        }
        if (speed)
        {
            textMesh.SetText($"{System.Math.Round(player.speed, 1)}");
        }
        if (range)
        {
            textMesh.SetText($"{System.Math.Round(player.bulletSpeed * player.bulletLifetime, 1)}");
        }
        if (bulletRate)
        {
            textMesh.SetText($"{System.Math.Round(player.bulletDelay, 1)}");
        }
        if (bulletSpeed)
        {
            textMesh.SetText($"{System.Math.Round(player.bulletSpeed, 1)}");
        }
        if (damage)
        {
            textMesh.SetText($"{System.Math.Round(player.damage, 1)}");
        }
    }
    public IEnumerator TypeMessageWithSlow(string message, float pSpeed, GameObject powerUp, bool callDialoge)
    {
        temp = "";
        foreach (char letter in message.ToCharArray())
        {
            temp += letter;
            textMesh.SetText(temp);
            yield return new WaitForSeconds(0.05f);
            FindObjectOfType<AudioManager>().Play("TopDialoge");
        }
        temp = "";
        StartCoroutine(UntypeMessageWithSlow(pSpeed, powerUp, callDialoge));
    }
    public IEnumerator UntypeMessageWithSlow(float pSpeed, GameObject powerUp, bool callDialoge)
    {
        yield return new WaitForSeconds(1f);
        textMesh.SetText("");
        StartCoroutine(DestroyAnimationWithSlow(pSpeed, powerUp, callDialoge));
    }

    public IEnumerator DestroyAnimationWithSlow(float pSpeed, GameObject powerUp, bool callDialoge)
    {
        yield return new WaitForSeconds(1f);
        while (pSpeed > player.speed)
        {
            player.speed += 0.1f;
        }
        if (callDialoge)
        {
            int random = Random.Range(0, 4);
            if (random == 0)
            {
                dialogeController.CallDialoge("Fox: Items for souls, so weird...");
            }
            else if (random == 1)
            {
                dialogeController.CallDialoge("Fox: I gotta comeback for more...");
            }
            else if (random == 2)
            {
                dialogeController.CallDialoge("Fox: Poweeeeeeer! I mean, yes!...");
            }
            else
            {
                dialogeController.CallDialoge("Fox: This will be really useful!...");
            }
        }
        Destroy(powerUp);
    }
}
