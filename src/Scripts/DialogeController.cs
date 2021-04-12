using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class DialogeController : MonoBehaviour
{
    private string temp;
    public TextMeshPro textMesh;
    public Image[] images;
    public void CallDialoge(string message)
    {
        StartCoroutine(TypeMessage(message));
    }

    public IEnumerator TypeMessage(string message)
    {
        foreach (Image image in images)
        {
            image.GetComponent<Image>();
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0.75f);
        }
        temp = "";
        foreach (char letter in message.ToCharArray())
        {
            FindObjectOfType<AudioManager>().Play("Dialoge");
            temp += letter;
            textMesh.SetText(temp);

            yield return new WaitForSeconds(0.05f);
        }
        temp = "";
        StartCoroutine(UntypeMessage(message));
    }
    public IEnumerator UntypeMessage(string message)
    {
        yield return new WaitForSeconds(3f);
        textMesh.SetText("");

        foreach (Image image in images)
        {
            image.GetComponent<Image>();
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        }
        if (message == "Fox: Phew! That was close!...") {
            CallDialoge("Fox: I better jump into that portal!...");
        }
    }
}
