using System.Collections;
using TMPro;
using UnityEngine;

public class Alarm_system : MonoBehaviour
{
    public Canvas parent_canvas;
    public float size = 1;
    public AudioSource audio_source;

    int topAlarm_num = 0;
    int middleAlarm_num = 0;

    public void Top_alarm(string plot)
    {
        topAlarm_num++;
        GameObject alarm = Instantiate(this.gameObject);
        RectTransform rect = alarm.GetComponent<RectTransform>();
        alarm.transform.SetParent(parent_canvas.transform,false);
        rect.anchorMin = new Vector2(0.5f, 1);
        rect.anchorMax = new Vector2(0.5f, 1);
        rect.localScale = new Vector3(size, size, size);
        rect.pivot = new Vector2(0.5f, 1);
        rect.anchoredPosition = new Vector3(0,-10,0);
        alarm.GetComponent<TMP_Text>().text = "[[[[[ " + plot + " ]]]]]";
        StartCoroutine(Destroy_alarm(alarm, topAlarm_num));
    }

    public void Middle_alarm(string plot)
    {
        middleAlarm_num++;
        GameObject alarm = Instantiate(this.gameObject);
        RectTransform rect = alarm.GetComponent<RectTransform>();
        alarm.transform.SetParent(parent_canvas.transform, false);
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.localScale = new Vector3(size, size, size);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector3(0, 0, 0);
        alarm.GetComponent<TMP_Text>().text = "[[[[[" + plot + "]]]]]";
        StartCoroutine(Destroy_alarm(alarm, middleAlarm_num,1));
    }

    IEnumerator Destroy_alarm(GameObject target, int alarm_num, int mode = 0)
    {
        audio_source.Play();
        TMP_Text text = target.GetComponent<TMP_Text>();
        for (float a = 1; a > 0; a-=0.01f)
        {
            if (mode == 0)
            {
                if (topAlarm_num != alarm_num)
                {
                    Destroy(target);
                }
            }
            else if (mode == 1)
            {
                if (middleAlarm_num != alarm_num)
                {
                    Destroy(target);
                }
            }
            text.alpha = a;
            yield return new WaitForSecondsRealtime(0.02f);
        }
        if (mode == 0)
        {
            topAlarm_num = 0;
        }
        else if (mode == 1)
        {
            middleAlarm_num = 0;
        }
        Destroy(target);
    }
}
