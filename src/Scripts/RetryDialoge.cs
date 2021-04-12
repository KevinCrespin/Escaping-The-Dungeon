using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RetryDialoge : MonoBehaviour
{
    private string temp;
    public TextMeshPro textMesh;
    public IEnumerator TypeMessage(string message)
    {
        temp = "";
        foreach (char letter in message.ToCharArray())
        {
            temp += letter;
            textMesh.SetText(temp);
            yield return new WaitForSeconds(0.15f);
        }
    }
}
