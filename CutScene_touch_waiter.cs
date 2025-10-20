using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class CutScene_touch_waiter : MonoBehaviour
{
    public PlayableDirector cutScene;

    bool touch = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            touch = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            touch = false;
        }
    }

    public void WaitStart()
    {
        StartCoroutine(Waitting());
    }

    IEnumerator Waitting()
    {
        touch = false;
        cutScene.Pause();
        yield return new WaitUntil(()=>(touch));
        cutScene.Resume();
    }
}
