using System.Collections;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_manager : MonoBehaviour
{
    public string scenes_name = "";

    public GameObject change_scene;
    public GameObject ui_group;
    public GameObject main_camera;

    public GameObject monster_alarm = null;

    RectTransform rectTransform;
    Image change_scene_image;

    public int w_aspect_game = 16;
    public int h_aspect_game = 9;

    public void Stop_fixed()
    {
        Time.timeScale = 0;
    }

    public void Start_fixed()
    {
        Time.timeScale = 1;
    }

    //ui에서의 sprite위치를 계산
    public float[] Get_Sprites_Uilocate(Transform target_obj)
    {
        float[] XY = new float[2];

        float C_y = Camera.main.orthographicSize; //카메라 수직 사이즈(절반)
        float C_x = C_y * Camera.main.aspect; //카메라 수직 사이즈 * 카메라 비율 = 카메라 수평 사이즈(절반)

        float xp = ((target_obj.position.x - main_camera.transform.position.x) / (C_x / 100));
        XY[0] = ((rectTransform.rect.width / 200) * xp);

        float yp = ((target_obj.position.y - main_camera.transform.position.y) / (C_y / 100));
        XY[1] = ((rectTransform.rect.height / 200) * yp);

        return XY;
    }

    //ui에서의 sprite크기를 계산(절반)
    public float[] Get_Sprites_Uibounds(Renderer target_renderer)
    {
        float[] rate = new float[2];
        float[] XY = new float[2];

        float C_y = Camera.main.orthographicSize; //카메라 수직 사이즈(절반)
        float C_x = C_y * Camera.main.aspect; //카메라 수직 사이즈 * 카메라 비율 = 카메라 수평 사이즈(절반)

        rate[0] = (rectTransform.rect.width / 2) / C_x;
        rate[1] = (rectTransform.rect.height / 2) / C_y;

        XY[0] = target_renderer.bounds.extents.x * rate[0];
        XY[1] = target_renderer.bounds.extents.y * rate[1];

        return XY;
    }

    public IEnumerator Change_scenes()
    {
        ui_group.SetActive(false);
        Time.timeScale = 0f;
        change_scene.SetActive(true);
        change_scene_image.color = new Color(0,0,0,0);
        for (float a = 0; a<=1.5f; a += 0.05f )
        {
            change_scene_image.color = new Color(0,0,0,a);
            yield return new WaitForSecondsRealtime(0.1f);
        }
        SceneManager.LoadScene(scenes_name);
    }

    public void Run_change_scenes()
    {
        StartCoroutine(Change_scenes());
    }

    IEnumerator Start_scenes()
    {
        ui_group.SetActive(false);
        Time.timeScale = 0f;
        change_scene.SetActive(true);
        change_scene_image.color = new Color(0,0,0,1);
        for (float a = 1; a>=0f ;a-=0.05f)
        {
            change_scene_image.color = new Color(0,0,0,a);
            yield return new WaitForSecondsRealtime(0.1f);
        }
        change_scene.SetActive(false);
        ui_group.SetActive(true);
    }

    IEnumerator Fade_in(float speed = 0.05f)
    {
        ui_group.SetActive(false);
        Time.timeScale = 0f;
        change_scene.SetActive(true);
        change_scene_image.color = new Color(0, 0, 0, 0);
        for (float a = 0; a <= 1.5f; a += speed)
        {
            change_scene_image.color = new Color(0, 0, 0, a);
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    public void Run_fade_in()
    {
        StartCoroutine(Fade_in());
    }

    public void Run_fade_in_faster()
    {
        StartCoroutine(Fade_in(0.2f));
    }

    public IEnumerator Fade_out()
    {
        ui_group.SetActive(false);
        Time.timeScale = 0f;
        change_scene.SetActive(true);
        change_scene_image.color = new Color(0, 0, 0, 1);
        for (float a = 1; a >= 0f; a -= 0.05f)
        {
            change_scene_image.color = new Color(0, 0, 0, a);
            yield return new WaitForSecondsRealtime(0.1f);
        }
        change_scene.SetActive(false);
        ui_group.SetActive(true);
    }

    public void Run_fade_out()
    {
        StartCoroutine(Fade_out());
    }

    public void Alarm_locate_for_monster(Transform _transform)
    {
        float[] _locate = new float[2];
        _locate = Get_Sprites_Uilocate(_transform);
        int count = 0;
        if (_locate[0] > (rectTransform.rect.width / 2))
        {
            count++;
            _locate[0] = (rectTransform.rect.width / 2);
        }
        else if (_locate[0] < (-1 * (rectTransform.rect.width / 2)))
        {
            _locate[0] = (-1 * (rectTransform.rect.width / 2));
            count++;
        }

        if (_locate[1] > (rectTransform.rect.height / 2))
        {
            count++;
            _locate[1] = (rectTransform.rect.height / 2);
        }
        else if (_locate[1] < (-1 * (rectTransform.rect.height / 2)))
        {
            count++;
            _locate[1] = -1 * (rectTransform.rect.height / 2);
        }

        if (count > 0)
        {
            GameObject alarm = Instantiate(monster_alarm,new Vector3(0, 0,0),Quaternion.identity,this.ui_group.transform);
            RectTransform alarm_rect = alarm.GetComponent<RectTransform>();
            alarm_rect.anchoredPosition = new Vector2(_locate[0], _locate[1]);
            alarm.SetActive(true);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        change_scene_image = change_scene.GetComponent<Image>();
        rectTransform = this.GetComponent<RectTransform>();
        StartCoroutine(Start_scenes());
    }


    int b_w = Screen.width;
    int b_h = Screen.height;
    // Update is called once per frame
    void Update()
    {
        int n_w = Screen.width;
        int n_h = Screen.height;
        if (!(Screen.fullScreenMode == FullScreenMode.MaximizedWindow))
        {
            if (b_w != n_w)
            {
                Screen.SetResolution(n_w, (int)(n_w / w_aspect_game * h_aspect_game), FullScreenMode.Windowed);
            }
            else if (b_h != n_h)
            {
                Screen.SetResolution((int)(n_h / h_aspect_game * w_aspect_game), n_h, FullScreenMode.Windowed);
            }
        }
        b_w = n_w;
        b_h = n_h;
    }
}
