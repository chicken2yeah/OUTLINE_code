using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class skip_button : MonoBehaviour, IPointerClickHandler
{
    public PlayableDirector target;
    public float target_sec;

    public void OnPointerClick(PointerEventData eventData)
    {
        target.Stop();
        target.time = target_sec;
        target.Play();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
