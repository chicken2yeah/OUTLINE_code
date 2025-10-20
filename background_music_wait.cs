using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class background_music_wait : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(1.5f + (float)playableDirector.duration);
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
