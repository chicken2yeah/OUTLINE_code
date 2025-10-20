using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Stage_manager : MonoBehaviour
{
    public A1_script player;

    public GameObject scene_change;
    public GameObject stage_change;

    public GameObject[] stages;
    public GameObject[] stages_spawn_point;


    public AudioSource BGM;

    public string change_scene_name;

    public int stage_num = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!PlayerPrefs.HasKey("SceneName"))
        {
            PlayerPrefs.SetString("SceneName","empty");
        }
        if (!PlayerPrefs.HasKey("StageNum"))
        {
            PlayerPrefs.SetInt("StageNum",0);
        }
        StartCoroutine(Scene_start());
    }

    IEnumerator Scene_start()
    {
        scene_change.SetActive(true);
        Time.timeScale = 0f;
        if (PlayerPrefs.GetString("SceneName") == SceneManager.GetActiveScene().name)
        {
            stages[stage_num].SetActive(false);
            stage_num = PlayerPrefs.GetInt("StageNum");
            Time.timeScale = 1f;
        }
        else
        {
            stage_num = 0;
            PlayerPrefs.SetInt("StageNum",stage_num);
            PlayerPrefs.SetString("SceneName",SceneManager.GetActiveScene().name);
            Time.timeScale = 1f;
        }
        stages[stage_num].SetActive(true);
        player.gameObject.transform.position = stages_spawn_point[stage_num].transform.position;
        yield return new WaitForSecondsRealtime(0.3f);
        scene_change.SetActive(false);
    }

    IEnumerator Scene_change()
    {
        Time.timeScale = 0f;
        BGM.Stop();
        scene_change.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadScene(change_scene_name);
    }

    public void Change_scene()
    {
        StartCoroutine(Scene_change());
    }

    IEnumerator Stage_change()
    {
        Time.timeScale = 0;
        stage_change.SetActive(true);
        yield return new WaitForSecondsRealtime(0.7f);
        stage_num++;
        stages[stage_num].SetActive(true);
        stages[stage_num-1].SetActive(false);
        player.gameObject.transform.position = stages_spawn_point[stage_num].transform.position;
        player.rb.linearVelocity = new Vector2(0,0);
        PlayerPrefs.SetInt("StageNum", stage_num);
        Time.timeScale = 1;
        yield return new WaitForSecondsRealtime(0.8f);
        stage_change.SetActive(false);
    }

    public void Change_stage()
    {
        StartCoroutine(Stage_change());
    }

    // Update is called once per frame
    void Update()
    {
        if (player.hp <= 0)
        {
            change_scene_name = "GameOver";
            Change_scene();
        }
    }
}
