using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySound : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DestroySoundGameObject());
    }

    IEnumerator DestroySoundGameObject()
    {
        float soundLength = GetComponent<AudioSource>().clip.length;
        yield return new WaitForSeconds(soundLength + 0.5f);
        Destroy(gameObject);
    }
}
