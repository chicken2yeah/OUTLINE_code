using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class Text_roader : MonoBehaviour
{
    public string[] plot;

    public float road_speed = 0.1f;
    public AudioSource road_sound;
    public Text_module text_Module;
    public GameObject canvas;
    public InputManager inputManager;

    public bool connect_timeline;
    public PlayableDirector playableDirector;
    public bool full_road = false;

    int plot_number = 0;

    GameObject t;

    public bool now_road = false;
    public bool now_full_road = false;

    public void RoadText()
    {
        if (full_road)
        {
            if (!now_full_road)
            {
                StartCoroutine(Full_road_text());
            }
        }
        else
        {
            if (!now_road)
            {
                t = Instantiate(text_Module.gameObject);
                t.GetComponent<Text_module>().target_obj = this.gameObject;
                t.transform.SetParent(canvas.transform, false);
                StartCoroutine(Roading_text(plot_number));
            }
        }
    }

    IEnumerator Full_road_text()
    {
        now_full_road=true;
        while (plot_number < plot.Length)
        {
            t = Instantiate(text_Module.gameObject);
            t.GetComponent<Text_module>().target_obj = this.gameObject;
            t.transform.SetParent(canvas.transform,false);
            StartCoroutine(Roading_text(plot_number));
            yield return new WaitUntil(() => !now_road);
            yield return new WaitForSecondsRealtime(2);
        }
        now_full_road=false;
        plot_number = 0;
    }


    bool waiter_ = true;
    IEnumerator WaiterWaiter_there_is_fly_in_my_code(float num, int plot_num)
    {
        yield return new WaitForSecondsRealtime(num);
        if (plot_number == plot_num)
        {
            waiter_ = true;
        }
    }

    IEnumerator Roading_text(int plot_num)
    {
        now_road=true;
        t.gameObject.SetActive(true);
        Text_module t_text = t.GetComponent<Text_module>();
        if (connect_timeline)
        {
            playableDirector.Pause();
        }
        for (int index = 1; index < plot[plot_num].Length; index++)
        {
            t_text.tmp_text.text = plot[plot_num].Substring(0,index);
            if (inputManager.interact)
            {
                inputManager.interact = false;
                break;
            }
            road_sound.Play();
            yield return new WaitForSecondsRealtime(road_speed);
        }
        t_text.tmp_text.text = plot[plot_num];
        StartCoroutine(WaiterWaiter_there_is_fly_in_my_code(5, plot_num));
        waiter_ = false;
        yield return new WaitUntil(()=>((inputManager.interact || waiter_)));
        if (!waiter_)
        {
            inputManager.interact = false;
        }
        t_text.tmp_text.text = "";
        if (connect_timeline)
        {
            playableDirector.Resume();
        }
        t.gameObject.SetActive(false);
        Destroy(t);
        now_road = false;
        plot_number++;
    }
}
