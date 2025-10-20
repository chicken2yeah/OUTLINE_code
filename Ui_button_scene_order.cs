using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Ui_button_scene_order : MonoBehaviour, IPointerClickHandler
{
    public UI_manager uI_Manager;
    public string scene_name;

    bool is_order = false;

    private void OnEnable()
    {
        is_order = false;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (!is_order)
        {
            is_order=true;
            uI_Manager.scenes_name = this.scene_name;
            uI_Manager.Run_change_scenes(); 
        }
    }
}
