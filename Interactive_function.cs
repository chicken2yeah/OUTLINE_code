using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.U2D.Animation;

public class Interactive_function : MonoBehaviour
{
    public GameObject talking_effect_obj;
    public UI_manager ui_manager;

    public string[] plot = null;
    public int plot_num = 0;

    public AudioSource effect_sound;

    public float default_speed = 0.1f;
    public float plot_destroy_wait_time = 1;

    public bool is_loop = false;

    public bool can_interact = false;
    public bool speak_all_when_interact = true;

    public bool is_stop_time_when_speakall = false;
    public bool log_timing = false;

    IEnumerator spk_all;

    bool player_in = false;
    public GameObject interact_alarm = null;

    GameObject delete_alarm = null;
    RectTransform delete_alarm_rect = null;
    bool is_alarm = false;

    bool now_speaking_all = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player_in = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player_in = false;
        }
    }
    public void Run_speak_all()
    {
        StartCoroutine(spk_all);
    }
    IEnumerator Speak_all()
    {
        now_speaking_all = true;
        if (is_stop_time_when_speakall)
        {
            ui_manager.Stop_fixed();
        }
        for (int a = 0; a<plot.Length; a++)
        {
            Self_speak_for_plot();
            yield return new WaitForSecondsRealtime((plot[plot_num - 1].Length * default_speed) + plot_destroy_wait_time + 0.5f);
        }
        if (is_stop_time_when_speakall)
        {
            ui_manager.Start_fixed();
        }
        now_speaking_all=false;
    }
    public void Self_speak_for_plot()
    {
        Speaking(plot[plot_num]);
        plot_num++;
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
        GameObject talk_effect;
        Talking_effect talk_effect_srcipt;
        talk_effect = Instantiate(talking_effect_obj);
        talk_effect.transform.SetParent(ui_manager.transform,false);
        talk_effect_srcipt = talk_effect.GetComponent<Talking_effect>();
        talk_effect_srcipt.target_obj = this.gameObject;
        talk_effect.SetActive(true);

        for (int index = 1; index<=word.Length; index++)
        {
            talk_effect_srcipt.tmp_text.text = word.Substring(0, index);
            effect_sound.Play();
            yield return new WaitForSecondsRealtime(speed);
        }
        yield return new WaitForSecondsRealtime(plot_destroy_wait_time);
        Destroy(talk_effect);
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
                timing += (plot[a].Length * default_speed) + ((plot_destroy_wait_time + 0.5f));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (is_loop)
        {
            if (plot.Length <= plot_num)
            {
                plot_num = 0;
            }
        }
        if (delete_alarm != null)
        {
            float[] locate = ui_manager.Get_Sprites_Uilocate(this.transform);
            delete_alarm_rect.anchoredPosition = new Vector2(locate[0], locate[1]);
        }
        if ((player_in && can_interact) && !now_speaking_all)
        {
            if (!is_alarm)
            {
                is_alarm = true;
                delete_alarm = Instantiate(interact_alarm, new Vector3(0, 0, 0), Quaternion.identity, ui_manager.ui_group.transform);
                delete_alarm.SetActive(true);
                delete_alarm_rect = delete_alarm.GetComponent<RectTransform>();
            }

            if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.O))
            {
                if (speak_all_when_interact)
                {
                    spk_all = Speak_all();
                    Run_speak_all();
                }
                else
                {
                    Self_speak_for_plot();
                }
            }
        }
        else
        {
            if (delete_alarm != null)
            {
                is_alarm = false;
                Destroy(delete_alarm);
                delete_alarm = null;
                delete_alarm_rect = null;
            }
        }
    }
}
