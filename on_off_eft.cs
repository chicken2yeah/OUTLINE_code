using System.Collections;
using UnityEngine;

public class on_off_eft : MonoBehaviour
{
    public bool is_ui = true;
    public float on_off_time_ms;
    public int eft_count = 100;

    RectTransform rectTransform;
    float first_x;
    float first_y;
    float wait_time;
    IEnumerator Enable_eft()
    {
        float value = first_x / eft_count;
        if (is_ui)
        {
            rectTransform.sizeDelta = new Vector2(0,first_y);
            for (int a = 1;a<=eft_count;a++)
            {
                rectTransform.sizeDelta = new Vector2(a * value,first_y);
                yield return new WaitForSecondsRealtime(wait_time);
            }
        }
        else
        {
            this.transform.localScale = new Vector3(0,first_y,1);
            for (int a = 1;a<=eft_count;a++)
            {
                this.transform.localScale = new Vector3(a*value,first_y,1);
                yield return new WaitForSecondsRealtime(wait_time);
            }
        }
    }
    private void Start()
    {
        wait_time = on_off_time_ms / eft_count;
        if (is_ui)
        {
            rectTransform = GetComponent<RectTransform>();
            first_x = rectTransform.rect.width;
            first_y = rectTransform.rect.height;
        }
        else
        {
            first_x = this.transform.localScale.x;
            first_y = this.transform.localScale.y;
        }
    }
    private void OnEnable()
    {
        StartCoroutine(Enable_eft());
    }
    private void OnDisable()
    {
        StartCoroutine(Disable_eft());
    }
    IEnumerator Disable_eft()
    {
        if (is_ui)
        {
            rectTransform.sizeDelta = new Vector2(first_x, first_y);
            float value = first_y / eft_count;
            for (int a = 1; a <= eft_count; a++)
            {
                rectTransform.sizeDelta = new Vector2(first_x,first_y - (a*value));
                yield return new WaitForSecondsRealtime(wait_time);
            }
        }
        else
        {
            this.transform.localScale = new Vector3(first_x, first_y, 1);
            float value = first_y / eft_count;
            for (int a = 1; a <= eft_count; a++)
            {
                this.transform.localScale = new Vector3(first_x, first_y - (a*value), 1);
                yield return new WaitForSecondsRealtime(wait_time);
            }
        }
    }
}
