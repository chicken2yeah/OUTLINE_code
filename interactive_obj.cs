using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class interactive_obj : MonoBehaviour
{
    public GameObject alarm;
    public Collider2D this_collider;

    public bool playerIn = false;

    GameObject g_obj_alarm;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            playerIn = true;
            g_obj_alarm = Instantiate(alarm, new Vector3(this.transform.position.x, this_collider.bounds.size.y, this.transform.position.z) , Quaternion.identity);
            g_obj_alarm.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            playerIn = false;
            Destroy(g_obj_alarm);
        }
    }
}
