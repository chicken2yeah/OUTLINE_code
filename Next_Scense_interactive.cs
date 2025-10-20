using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Next_Scense_interactive : interactive_obj
{
    public string scense_name;
    public Image change_scense_pade;

    void FixedUpdate()
    {
        if (playerIn && (Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.I)))
        {
            StartCoroutine(Change_scene());
        }
    }

    IEnumerator Change_scene()
    {
        change_scense_pade.gameObject.SetActive(true);
        for (float a = change_scense_pade.color.a ; a<1 ; a+=0.01f)
        {
            change_scense_pade.color = new Color(change_scense_pade.color.r,change_scense_pade.color.g,change_scense_pade.color.b,a);
            yield return new WaitForSecondsRealtime(0.003f);
        }
        SceneManager.LoadScene(scense_name);
    }
}
