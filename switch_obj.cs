using UnityEngine;

public class switch_obj : MonoBehaviour
{
    public interactive_function_move move_target;
    public GameObject interact_alarm;
    public UI_manager ui_manager;
    public AudioSource audio_source;

    bool is_alarm = false;
    bool player_in = false;
    GameObject delete_alarm = null;
    RectTransform delete_alarm_rect;

    int mode = 0;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (delete_alarm != null)
        {
            float[] locate = ui_manager.Get_Sprites_Uilocate(this.transform);
            delete_alarm_rect.anchoredPosition = new Vector2(locate[0], locate[1]);
        }
        if (player_in)
        {
            if (!is_alarm)
            {
                is_alarm = true;
                delete_alarm = Instantiate(interact_alarm,new Vector3(0,0,0),Quaternion.identity,ui_manager.ui_group.transform);
                delete_alarm.SetActive(true);
                delete_alarm_rect = delete_alarm.GetComponent<RectTransform>();
            }

            if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.O))
            {
                audio_source.Play();
                mode = mode == 0 ? 1 : 0;
                move_target.mode = this.mode;
            }
        }
        else
        {
            if (delete_alarm != null)
            {
                is_alarm = false;
                Destroy(delete_alarm);
                delete_alarm = null;
                delete_alarm_rect= null;
            }
        }
    }
}
