using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TMP_text_auto_changer : MonoBehaviour
{
    public TMP_Text text;
    public string[] plot = null;
    public int plot_num = 0;

    public AudioSource effect_sound;

    public float default_speed = 0.1f;
    public float wait_time;

    public bool connect_plot_to_img = false;
    public Image img;
    public Sprite[] sprites;

    public bool log_timing = false;
    [System.Serializable]
    public class Change_time_and_img_index
    {
        public int change_time;
        public int index;
    }
    public int change_num = 0;
    public Change_time_and_img_index[] change_and_indexs;

    IEnumerator spk_all;

    public void Run_speak_all()
    {
        StartCoroutine(spk_all);
    }
    IEnumerator Speak_all()
    {
        for (int a = 0; a < plot.Length; a++)
        {
            Self_speak_for_plot();
            yield return new WaitForSecondsRealtime((plot[plot_num].Length * default_speed) + wait_time + 0.5f);
        }
    }
    public void Self_speak_for_plot()
    {
        Speaking(plot[plot_num]);
    }
    public void Speaking(string word)
    {
        StartCoroutine(Speaking_co(word, default_speed));
    }

    public void Stop_Speak_all()
    {
        StopCoroutine(spk_all);
    }

    //대화 시스템
    private IEnumerator Speaking_co(string word, float speed)
    {
        if (connect_plot_to_img)
        {
            for (int index = 1; index <= word.Length; index++)
            {
                if (change_num < change_and_indexs.Length)
                {
                    if (plot_num == change_and_indexs[change_num].change_time)
                    {
                        if (change_and_indexs[change_num].index == -1)
                        {
                            img.gameObject.SetActive(false);
                            change_num++;
                        }
                        else
                        {
                            img.gameObject.SetActive(true);
                            img.sprite = sprites[change_and_indexs[change_num].index];
                            change_num++;
                        }
                    }
                    else if (plot_num+1 == change_and_indexs[change_num].change_time)
                    {
                        img.gameObject.SetActive(false);
                    }
                }
                text.text = word.Substring(0, index);
                effect_sound.Play();
                text.ForceMeshUpdate();
                yield return new WaitForSecondsRealtime(speed);
            }
        }
        else
        {
            for (int index = 1; index <= word.Length; index++)
            {
                text.text = word.Substring(0, index);
                effect_sound.Play();
                text.ForceMeshUpdate();
                yield return new WaitForSecondsRealtime(speed);
            }
        }
        yield return new WaitForSecondsRealtime(wait_time);
        text.text = "";
        plot_num++;
        text.ForceMeshUpdate();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spk_all = Speak_all();
        float timing = 0;
        if (log_timing)
        {
            for (int a = 0; a < plot.Length; a++)
            {
                Debug.Log(a + ":" + timing * 60);
                timing += (plot[a].Length * default_speed) + ((wait_time + 0.5f));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
