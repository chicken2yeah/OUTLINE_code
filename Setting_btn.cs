using UnityEngine;
using UnityEngine.EventSystems;

public class Setting_btn : MonoBehaviour, IPointerClickHandler
{
    public SettingManager settingManager;
    public int index;

    public bool reset_btn = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!settingManager.is_setting)
        {
            if (reset_btn)
            {
                settingManager.Resetting_values();
            }
            else
            {
                settingManager.btn_index = index;
                StartCoroutine(settingManager.SetKey());
            }
        }
    }
}
