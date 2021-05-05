using System;
using System.Collections;
using UnityEngine;

public class VOManager : MonoBehaviour
{
    [SerializeField] private AudioClip introDialogue;
    [SerializeField] private GameObject teleporting;

    private AudioSource _source;
    
    private void Start()
    {
        _source = GetComponent<AudioSource>();
        
    }

    private IEnumerator BeginDialogue()
    {
        yield return new WaitForSeconds(2f);
        _source.clip = introDialogue;
        _source.Play();
        yield return new WaitForSeconds(introDialogue.length);
        teleporting.SetActive(true);
    }
}
