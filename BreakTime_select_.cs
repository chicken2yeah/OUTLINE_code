using UnityEngine;
using UnityEngine.EventSystems;

public class BreakTime_select_ : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public RectTransform rect_;
    public AudioSource audioSource;
    public Stage_manager stage_;
    public string area_name;

    public void OnPointerClick(PointerEventData eventData)
    {
        stage_.change_scene_name = area_name;
        stage_.Change_scene();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        audioSource.Play();
        rect_.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rect_.transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
