using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeController : MonoBehaviour
{
    public IEnumerator Shake(float duration, float magnitude) {
        Vector3 originalPos = transform.localPosition;
        float elapse = 0.0f;
        while (elapse < duration) {
            float x = Random.Range(-1f, 1f) * magnitude;
            float z = Random.Range(-1f, 1f); 
            transform.localPosition = new Vector3(x, originalPos.y, z);
            elapse += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPos;
    }

    public IEnumerator Shake2(float duration, float magnitude) {
        Vector3 originalPos = transform.localPosition;
        float elapse = 0.0f;
        while (elapse < duration) {
            float r = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(r, originalPos.y, r);
            elapse += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPos;
    }
}
