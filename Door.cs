using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    public UI_manager ui_manager;
    public PLAYER_common player;
    public GameObject active_room;
    public GameObject[] active_obj;
    public GameObject inactive_room;
    public GameObject[] inactive_obj;
    public GameObject interact_alarm;
    public Door connect_door;

    bool player_in = false;
    GameObject delete_alarm = null;
    RectTransform delete_alarm_rect = null;
    bool is_alarm = false;

    public bool is_hide = false;

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
    public void Run_show()
    {
        StartCoroutine(Show());
    }

    public IEnumerator Show()
    {
        ui_manager.Run_fade_out();
        yield return new WaitForSecondsRealtime(2f);
        ui_manager.Start_fixed();
        connect_door.is_hide = false;
    }

    public IEnumerator Hide()
    {
        ui_manager.Run_fade_in_faster();
        yield return new WaitForSecondsRealtime(2f);
        active_room.SetActive(true);
        for (int i = 0; i<active_obj.Length; i++)
        {
            active_obj[i].SetActive(true);
        }
        player.transform.position = new Vector3(connect_door.transform.position.x,player.transform.position.y,player.transform.position.z);
        connect_door.Run_show();
        for (int i = 0; i<inactive_obj.Length; i++)
        {
            inactive_obj[i].SetActive(false);
        }
        inactive_room.SetActive(false);
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
        if ((player_in && !is_hide)&& player.on_ground)
        {
            if (!is_alarm)
            {
                is_alarm = true;
                delete_alarm = Instantiate(interact_alarm, new Vector3(0, 0, 0), Quaternion.identity, ui_manager.ui_group.transform);
                delete_alarm.SetActive(true);
                delete_alarm_rect = delete_alarm.GetComponent<RectTransform>();
            }

            if ((Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.O)))
            {
                is_hide = true;
                StartCoroutine(Hide());
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
