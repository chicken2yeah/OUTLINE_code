using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Ui_order_btn : MonoBehaviour, IPointerClickHandler
{
    public string scene_name;
    public GameObject fade;

    public bool auto = false;

    private void Start()
    {
        if (auto)
        {
            if (PlayerPrefs.HasKey("SceneName"))
            {
                scene_name = PlayerPrefs.GetString("SceneName");
            }
            else
            {
                scene_name = "Day1_Tutorial_scene";
            }
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Change_scene();
    }

    public void Change_scene()
    {
        fade.SetActive(true);
        SceneManager.LoadScene(scene_name);
    }
}
