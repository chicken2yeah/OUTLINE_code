using TMPro;
using UnityEngine;

public class Text_module : MonoBehaviour
{
    public TMP_Text tmp_text;
    public RectTransform this_rect;
    public Camera cam;
    public GameObject target_obj;

    // Update is called once per frame
    void Update()
    {
        tmp_text.ForceMeshUpdate();
        this_rect.position = cam.WorldToScreenPoint(target_obj.transform.position);
        float width_size = tmp_text.textBounds.size.x + 30;
        this_rect.sizeDelta = new Vector2(width_size, 20);
        tmp_text.rectTransform.sizeDelta = this_rect.sizeDelta;
        if (this_rect.anchoredPosition.x <= 400)
        {
            this_rect.pivot = new Vector2(0f, 0.5f);
            this_rect.anchoredPosition = new Vector2(this.this_rect.anchoredPosition.x + 20,this_rect.anchoredPosition.y);
        }
        else
        {
            this_rect.pivot = new Vector2(1f, 0.5f);
            this_rect.anchoredPosition = new Vector2(this.this_rect.anchoredPosition.x - 20, this_rect.anchoredPosition.y);
        }
    }
}
