using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Talking_effect : MonoBehaviour
{
    //대화 시스템 ui
    public GameObject tmp;
    public GameObject target_obj;
    public TMP_Text tmp_text;
    public UI_manager ui_manager;

    RectTransform this_rect;
    RectTransform tmp_rect;
    Renderer target_renderer;

    float[] target_bound;
    float[] index_value;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this_rect = this.GetComponent<RectTransform>();
        tmp_rect = tmp.GetComponent<RectTransform>();
        tmp_text = tmp.GetComponent<TMP_Text>();
        target_renderer = target_obj.GetComponent<Renderer>();
        target_bound = ui_manager.Get_Sprites_Uibounds(target_renderer);
        index_value = new float[2];
        index_value[0] = target_bound[0];
        index_value[1] = (target_bound[1]/3);
    }

    // Update is called once per frame
    void Update()
    {
        //크기조정
        this_rect.sizeDelta = new Vector2(30 + tmp_text.textBounds.extents.x*2,20);
        tmp_rect.sizeDelta = new Vector2(30 + tmp_text.textBounds.extents.x*2, 20);

        
        //위치조정
        float[] xy = ui_manager.Get_Sprites_Uilocate(target_obj.transform);
        if (xy[0] > 0.01f)
        {
            this_rect.anchoredPosition = new Vector2(xy[0] - index_value[0] - (tmp_text.textBounds.extents.x), xy[1] + index_value[1]);
        }
        else if (xy[0] < -0.01f)
        {
            this_rect.anchoredPosition = new Vector2(xy[0] + index_value[0] + (tmp_text.textBounds.extents.x), xy[1] + index_value[1]);
        }
        else
        {
            if (target_obj.transform.localScale.x > 0)
            {
                this_rect.anchoredPosition = new Vector2(xy[0] + index_value[0] + (tmp_text.textBounds.extents.x), xy[1] + index_value[1]);
            }
            else
            {
                this_rect.anchoredPosition = new Vector2(xy[0] - index_value[0] - (tmp_text.textBounds.extents.x), xy[1] + index_value[1]);
            }
        }
        tmp_text.ForceMeshUpdate();
    }
}
