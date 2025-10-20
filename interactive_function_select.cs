using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class interactive_function_select : MonoBehaviour
{
    public PLAYER_common player;
    public GameObject player_talk = null;
    public bool player_talk_start = false;
    public bool player_question = false;
    public bool player_answer = false;
    public bool player_talk_end = false;
    public UI_manager ui_manager;
    public GameObject interact_alarm;
    public float default_speed = 0.1f;
    public GameObject talking_effect_obj;
    public GameObject selection_obj;
    public GameObject question_obj;
    public bool fade_in_out_active = true;
    public bool can_interact = true;
    public PlayableDirector conntect_timeline = null;

    public string[] before_plot;
    public string[] after_plot;

    public AudioSource effect_sound;
    public AudioSource this_effect_sound;

    bool player_in = false;
    bool is_talking = false;
    public bool stop_active = false;
    public bool log_timing = false;

    public void Stop_active_switch()
    {
        stop_active = !stop_active;
    }

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

    [System.Serializable]
    public class selections
    {
        public string answer;
        public string[] plot_after_answer;
        public GameObject[] active_obj;
        public GameObject[] inactive_obj;
        public question_and_selections question_And_Selections = null; // waring
    }

    [System.Serializable]
    public class question_and_selections
    {
        public string question;
        public selections[] selections_array;
    }

    public question_and_selections question_selection;

    GameObject delete_alarm = null;
    RectTransform delete_alarm_rect = null;
    bool is_alarm = false;

    public void Run_talking_and_selecting()
    {
        StartCoroutine(Talking_and_selecting());
    }

    bool is_selecting = false;
    IEnumerator Selecting(string question, selections[] selections_array)
    {
        is_selecting = true;
        if (conntect_timeline != null)
        {
            conntect_timeline.Pause();
        }
        GameObject question_effect;
        Talking_effect question_effect_srcipt;
        question_effect = Instantiate(question_obj);
        question_effect.transform.SetParent(ui_manager.transform, false);
        question_effect_srcipt = question_effect.GetComponent<Talking_effect>();
        if (player_question)
        {
            question_effect_srcipt.target_obj = player_talk;
        }
        else
        {
            question_effect_srcipt.target_obj = this.gameObject;
        }
        question_effect.SetActive(true);
        for (int index = 1; index <= question.Length; index++)
        {
            question_effect_srcipt.tmp_text.text = question.Substring(0, index);
            yield return new WaitForSecondsRealtime(default_speed);
        }

        int select_num = 0;
        GameObject selection_effect;
        Talking_effect selection_effect_script;
        selection_effect = Instantiate(selection_obj);
        selection_effect.transform.SetParent(ui_manager.transform, false);
        selection_effect_script = selection_effect.GetComponent<Talking_effect>();
        selection_effect_script.target_obj = player_talk;
        selection_effect.SetActive(true);
        while (!Input.GetKey(KeyCode.Space))
        {
            for (int index = 0; index <= selections_array[select_num].answer.Length; index++)
            {
                selection_effect_script.tmp_text.text = selections_array[select_num].answer.Substring(0, index);
                yield return new WaitForSecondsRealtime(default_speed / 3);
            }
            yield return new WaitUntil(() => (Input.GetKey(KeyCode.Space) ^ ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) ^ (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))));
            if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)))
            {
                if (select_num <= 0)
                {
                    select_num = (selections_array.Length - 1);
                }
                else
                {
                    select_num -= 1;
                }
            }
            else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
            {
                if (select_num >= (selections_array.Length - 1))
                {
                    select_num = 0;
                }
                else
                {
                    select_num += 1;
                }
            }
        }
        Destroy(selection_effect);
        Destroy(question_effect);

        if (player_answer)
        {
            for (int a = 0; a < selections_array[select_num].plot_after_answer.Length; a++)
            {
                StartCoroutine(Speaking_co(selections_array[select_num].plot_after_answer[a], default_speed, player_talk.gameObject, talking_effect_obj));
                yield return new WaitForSecondsRealtime((selections_array[select_num].plot_after_answer[a].Length * default_speed) + 1.3f);
            }
        }
        else
        {
            for (int a = 0; a < selections_array[select_num].plot_after_answer.Length; a++)
            {
                StartCoroutine(Speaking_co(selections_array[select_num].plot_after_answer[a], default_speed, this.gameObject, talking_effect_obj));
                yield return new WaitForSecondsRealtime((selections_array[select_num].plot_after_answer[a].Length * default_speed) + 1.3f);
            }
        }
        if (selections_array[select_num].active_obj.Length >0 || selections_array[select_num].inactive_obj.Length > 0)
        {
            if (fade_in_out_active)
            {
                ui_manager.Run_fade_in_faster();
                yield return new WaitForSecondsRealtime(1f);
            }
            stop_active = false;
            for (int a = 0; a<selections_array[select_num].active_obj.Length; a++)
            {
                selections_array[select_num].active_obj[a].SetActive(true);
            }
            for (int a = 0; a < selections_array[select_num].inactive_obj.Length; a++)
            {
                selections_array[select_num].inactive_obj[a].SetActive(false);
            }
            yield return new WaitUntil(()=>(stop_active));
            for (int a = 0; a < selections_array[select_num].active_obj.Length; a++)
            {
                selections_array[select_num].active_obj[a].SetActive(false);
            }
            for (int a = 0; a < selections_array[select_num].inactive_obj.Length; a++)
            {
                selections_array[select_num].inactive_obj[a].SetActive(true);
            }
            if (fade_in_out_active)
            {
                ui_manager.Run_fade_out();
            }
        }
        if (selections_array[select_num].question_And_Selections.selections_array.Length > 0)
        {
            StartCoroutine(Selecting(selections_array[select_num].question_And_Selections.question, selections_array[select_num].question_And_Selections.selections_array));
        }
        else
        {
            is_selecting = false;
        }
        if (conntect_timeline != null)
        {
            conntect_timeline.Play();
        }
    }

    IEnumerator Talking_and_selecting()
    {
        if (before_plot.Length > 0)
        {
            if (player_talk_start)
            {
                for (int a = 0; a < before_plot.Length; a++)
                {
                    StartCoroutine(Speaking_co(before_plot[a],default_speed,player_talk.gameObject, talking_effect_obj));
                    yield return new WaitForSecondsRealtime((before_plot[a].Length * default_speed) + 1.3f);
                }
            }
            else
            {
                for (int a = 0; a < before_plot.Length; a++)
                {
                    StartCoroutine(Speaking_co(before_plot[a], default_speed, this.gameObject, talking_effect_obj));
                    yield return new WaitForSecondsRealtime((before_plot[a].Length * default_speed) + 1.3f);
                }
            }
        }

        StartCoroutine(Selecting(question_selection.question, question_selection.selections_array));
        yield return new WaitUntil(()=>(!is_selecting));

        if (after_plot.Length > 0)
        {
            if (player_talk_end)
            {
                for (int a = 0; a < after_plot.Length; a++)
                {
                    StartCoroutine(Speaking_co(after_plot[a], default_speed, player_talk.gameObject, talking_effect_obj));
                    yield return new WaitForSecondsRealtime((after_plot[a].Length * default_speed) + 1.3f);
                }
            }
            else
            {
                for (int a = 0; a < after_plot.Length; a++)
                {
                    StartCoroutine(Speaking_co(after_plot[a], default_speed, this.gameObject, talking_effect_obj));
                    yield return new WaitForSecondsRealtime((after_plot[a].Length * default_speed) + 1.3f);
                }
            }
        }
        if (can_interact)
        {
            ui_manager.Start_fixed();
        }
        is_talking = false;
    }

    IEnumerator Speaking_co(string word, float speed, GameObject target, GameObject talk_eft_obj)
    {
        GameObject talk_effect;
        Talking_effect talk_effect_srcipt;
        talk_effect = Instantiate(talk_eft_obj);
        talk_effect.transform.SetParent(ui_manager.transform, false);
        talk_effect_srcipt = talk_effect.GetComponent<Talking_effect>();
        talk_effect_srcipt.target_obj = target;
        talk_effect.SetActive(true);
        if (target == this.gameObject)
        {
            for (int index = 1; index <= word.Length; index++)
            {
                talk_effect_srcipt.tmp_text.text = word.Substring(0, index);
                this_effect_sound.Play();
                yield return new WaitForSecondsRealtime(speed);
            }
        }
        else
        {
            for (int index = 1; index <= word.Length; index++)
            {
                talk_effect_srcipt.tmp_text.text = word.Substring(0, index);
                effect_sound.Play();
                yield return new WaitForSecondsRealtime(speed);
            }
        }
        yield return new WaitForSecondsRealtime(1f);
        Destroy(talk_effect);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (this_effect_sound == null)
        {
            this_effect_sound = effect_sound;
        }
        if (log_timing)
        {
            float timing = 0;
            for (int a = 0; a < before_plot.Length; a++)
            {
                Debug.Log("before : " + a + " : " + timing * 60);
                timing += (before_plot[a].Length * default_speed) + ((1.3f));
            }
            Debug.Log("before : last :" + timing * 60);
            timing = 0;
            for (int a = 0; a < after_plot.Length; a++)
            {
                Debug.Log("after : " + a + " : " + timing * 60);
                timing += (after_plot[a].Length * default_speed) + ((1.3f));
            }
            Debug.Log("after : last : " + timing * 60);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (can_interact)
        {
            if (delete_alarm != null)
            {
                float[] locate = ui_manager.Get_Sprites_Uilocate(this.transform);
                delete_alarm_rect.anchoredPosition = new Vector2(locate[0], locate[1]);
            }
            if ((player_in && !is_talking) && player.on_ground)
            {
                if (!is_alarm)
                {
                    is_alarm = true;
                    delete_alarm = Instantiate(interact_alarm, new Vector3(0, 0, 0), Quaternion.identity, ui_manager.ui_group.transform);
                    delete_alarm.SetActive(true);
                    delete_alarm_rect = delete_alarm.GetComponent<RectTransform>();
                }

                if ((Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.O)) && !is_talking)
                {
                    is_talking = true;
                    ui_manager.Stop_fixed();
                    Run_talking_and_selecting();
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
}
