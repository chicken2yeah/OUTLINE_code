using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class Actve_switch : MonoBehaviour, IPointerClickHandler
{
    public float active_delay = 0;
    public GameObject[] target_obj;
    public GameObject[] inactive_target_obj;
    bool is_active=false;
    bool is_changed = false;
    public UI_manager ui_manager = null;
    public bool is_fade_when_change = false;
    public bool is_mouse = true;
    bool player_in = false;
    public GameObject interact_alarm = null;

    GameObject delete_alarm = null;
    RectTransform delete_alarm_rect = null;
    bool is_alarm = false;

    IEnumerator Change()
    {
        is_changed = true;
        if (is_fade_when_change)
        {
            ui_manager.Run_fade_in_faster();
        }
        yield return new WaitForSecondsRealtime(active_delay);
        is_active = !is_active;
        if (target_obj.Length > 0)
        {
            for (int a = 0; a < target_obj.Length; a++)
            {
                target_obj[a].SetActive(is_active);
            }
        }
        if (inactive_target_obj.Length > 0)
        {
            for (int a = 0; a < inactive_target_obj.Length; a++)
            {
                inactive_target_obj[a].SetActive(!is_active);
            }
        }
        is_changed = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "PLAYER")
        {
            player_in = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "PLAYER")
        {
            player_in = false;
        }
    }

    private void Update()
    {
        if (delete_alarm != null)
        {
            float[] locate = ui_manager.Get_Sprites_Uilocate(this.transform);
            delete_alarm_rect.anchoredPosition = new Vector2(locate[0], locate[1]);
        }
        if ((player_in) && !is_mouse)
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
                StartCoroutine(Change());
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

    private void OnEnable()
    {
        is_changed = false;
        is_active = false;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (is_mouse && !is_changed)
        {
            StartCoroutine(Change());
        }
    }
}
