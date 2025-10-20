using System.Collections;
using UnityEngine;

public class Monster_counter : MonoBehaviour
{
    public GameObject monsters;
    public GameObject[] active_objects;
    public GameObject[] inactive_objects;
    public UI_manager ui_manager;

    bool runned = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    IEnumerator Change_obj()
    {
        ui_manager.Run_fade_in();
        yield return new WaitForSecondsRealtime(3f);
        for (int a = 0; a< active_objects.Length; a++)
        {
            active_objects[a].gameObject.SetActive(true);
        }
        for (int a = 0; a<inactive_objects.Length; a++)
        {
            inactive_objects[a].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (monsters.transform.childCount <= 0 && !runned)
        {
            runned = true;
            ui_manager.Stop_fixed();
            StartCoroutine(Change_obj());
        }
    }
}
