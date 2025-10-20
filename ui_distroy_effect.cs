using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ui_distroy_effect : MonoBehaviour
{
    // 이펙트 비활성 효과 간이 커스텀 (ui)

    public float destory_start_delay_time_s = 1f;
    public float destory_end_delay_time_s = 0f;
    public enum time_influence
    {
        Scaled = 0,
        Unscaled = 1
    }

    public float reduce_delay = 0.01f;

    public time_influence check_scaled;
    bool isScale = true;

    public bool cut_x = false;
    public bool cut_y = false;

    public float move_x = 0f;
    public float move_y = 0f;

    RectTransform this_rect;

    // 조명 값은 수량이 많을 수도 있으므로 직렬화 후 배열로 제작
    [System.Serializable]
    public class Change_Light2D
    {
        public Light2D light = null;

        public bool is_change_intensity = false;
        public float target_intensity = 0f;

        public bool is_change_falloff = false;
        public float target_falloff = 0f;
    }

    public Change_Light2D[] change_Light2Ds;

    // x축 크기 변화
    IEnumerator Cutting_x_y()
    {
        if (cut_x && cut_y)
        {
            float first_x = this_rect.localScale.x;
            float reduce = first_x / 100;
            float first_y = this_rect.localScale.y;
            float reduce2 = first_y / 100;
            for (float x = first_x, y = first_y; x >= 0f; x -= reduce, y -= reduce2)
            {
                this_rect.localScale = new Vector2(x, y);
                yield return isScale ? new WaitForSeconds(reduce_delay) : new WaitForSecondsRealtime(reduce_delay);
            }
        }
        else
        {
            if (cut_x)
            {
                float first_x = this_rect.localScale.x;
                float reduce = first_x / 100;
                for (float x = first_x; x >= 0f; x -= reduce)
                {
                    this_rect.localScale = new Vector2(x, this_rect.localScale.y);
                    yield return isScale ? new WaitForSeconds(reduce_delay) : new WaitForSecondsRealtime(reduce_delay);
                }
            }
            else if (cut_y)
            {
                float first_y = this_rect.localScale.y;
                float reduce = first_y / 100;
                for (float y = first_y; y >= 0f; y -= reduce)
                {
                    this_rect.localScale = new Vector2(this_rect.localScale.x, y);
                    yield return isScale ? new WaitForSeconds(reduce_delay) : new WaitForSecondsRealtime(reduce_delay);
                }
            }
        }

    }

    // 좌표이동
    IEnumerator Move_loacte()
    {
        float addition_x = move_x / 100;
        float addition_y = move_y / 100;
        for (int count = 0; count < 100; count++)
        {
            this_rect.position = new Vector2(this_rect.position.x + addition_x, this_rect.position.y + addition_y);
            yield return isScale ? new WaitForSeconds(reduce_delay) : new WaitForSecondsRealtime(reduce_delay);
        }
    }

    // 조명값 변화
    IEnumerator Change_light(Change_Light2D _change_Light)
    {
        float now_intensity_index;
        float addition_intensity_index;
        if (_change_Light.is_change_intensity)
        {
            now_intensity_index = _change_Light.light.intensity;
            addition_intensity_index = (_change_Light.target_intensity - now_intensity_index) / 100;
        }
        else
        {
            now_intensity_index = 0;
            addition_intensity_index = 0;
        }

        float now_falloff_index;
        float addition_falloff_index;
        if (_change_Light.is_change_falloff)
        {
            now_falloff_index = _change_Light.light.falloffIntensity;
            addition_falloff_index = (_change_Light.target_falloff - now_falloff_index) / 100;
        }
        else
        {
            now_falloff_index = 0;
            addition_falloff_index = 0;
        }

        if (addition_intensity_index == 0 && addition_falloff_index == 0)
        {
            yield return null;
        }
        else
        {
            for (int a = 0; a < 100; a++)
            {
                _change_Light.light.intensity += addition_intensity_index;
                _change_Light.light.falloffIntensity += addition_falloff_index;
                yield return isScale ? new WaitForSeconds(reduce_delay) : new WaitForSecondsRealtime(reduce_delay);
            }
        }

    }

    // 작업 분배
    IEnumerator Destroy_self()
    {
        yield return isScale ? new WaitForSeconds(destory_start_delay_time_s) : new WaitForSecondsRealtime(destory_start_delay_time_s);
        if (cut_x || cut_y)
        {
            StartCoroutine(Cutting_x_y());
        }

        if (move_x != 0 && move_y != 0)
        {
            StartCoroutine(Move_loacte());
        }

        if (change_Light2Ds.Length > 0)
        {
            for (int a = 0; a < change_Light2Ds.Length; a++)
            {
                StartCoroutine(Change_light(change_Light2Ds[a]));
            }
        }
        yield return isScale ? new WaitForSeconds(reduce_delay * 100) : new WaitForSecondsRealtime(reduce_delay * 100);
        yield return isScale ? new WaitForSeconds(destory_end_delay_time_s) : new WaitForSecondsRealtime(destory_end_delay_time_s);
        Destroy(this.gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this_rect = this.GetComponent<RectTransform>();
        isScale = check_scaled == time_influence.Scaled;
        StartCoroutine(Destroy_self());
    }
}
