using System;
using System.Collections;
using UnityEngine;

public class VOManager : MonoBehaviour
{
    [SerializeField] private AudioClip introDialogue;
    [SerializeField] private GameObject teleporting;
    [SerializeField] private AudioClip rotateDialogue;
    [SerializeField] private AudioClip numbersDialogue;
    [SerializeField] private AudioClip completeFirstPuzzle;
    [SerializeField] private AudioClip earthPuzzleDialogue;
    [SerializeField] private AudioClip earthPuzzleDialogue2;
    [SerializeField] private AudioClip earthPuzzleDialogue3;
    [SerializeField] private AudioClip earthPuzzleDialogue4;
    [SerializeField] private AudioClip finishEarthPuzzle;
    [SerializeField] private AudioClip congrats;
    [SerializeField] private GameObject overworldPlayer;
    
    private AudioSource _source;
    private bool _floating;
    
    private void Start()
    {
        _source = GetComponent<AudioSource>();
        StartCoroutine(BeginDialogue());
    }

    private void Update()
    {
        if (_floating)
        {
            overworldPlayer.transform.position += Vector3.up * 0.01f;
        }
    }

    private IEnumerator BeginDialogue()
    {
        yield return new WaitForSeconds(2f);
        _source.clip = introDialogue;
        _source.Play();
        yield return new WaitForSeconds(introDialogue.length);
        teleporting.SetActive(true);
    }

    public void FirstPuzzleDialogueDriver()
    {
        StartCoroutine(FirstPuzzleDialogue());
    }

    private IEnumerator FirstPuzzleDialogue()
    {
        _source.clip = rotateDialogue;
        _source.Play();
        yield return new WaitForSeconds(rotateDialogue.length + 2f);
        _source.clip = numbersDialogue;
        _source.Play();
    }

    public void CompleteFirstPuzzle()
    {
        _source.clip = completeFirstPuzzle;
        _source.Play();
    }

    public void StartEarthPuzzle()
    {
        StartCoroutine(EarthPuzzleDialogue());
    }

    private IEnumerator EarthPuzzleDialogue()
    {
        _source.clip = earthPuzzleDialogue;
        _source.Play();
        yield return new WaitForSeconds(earthPuzzleDialogue.length + 1f);
        _source.clip = earthPuzzleDialogue2;
        _source.Play();
        yield return new WaitForSeconds(earthPuzzleDialogue2.length + 1f);
        _source.clip = earthPuzzleDialogue3;
        _source.Play();
        yield return new WaitForSeconds(earthPuzzleDialogue3.length + 1f);
        _source.clip = earthPuzzleDialogue4;
        _source.Play();
    }

    public void FinishEarthPuzzle()
    {
        _source.clip = finishEarthPuzzle;
        _source.Play();
    }

    public void EndingDriver()
    {
        StartCoroutine(Ending());
    }

    private IEnumerator Ending()
    {
        _floating = true;
        _source.clip = congrats;
        _source.Play();
        yield return new WaitForSeconds(congrats.length + 1.5f);
        Application.Quit();
    }
}
