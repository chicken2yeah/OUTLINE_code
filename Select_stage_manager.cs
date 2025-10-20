using System;
using UnityEngine;
using UnityEngine.Playables;

public class Select_stage_manager : MonoBehaviour
{
    public string selection_name; // "Stage" + num + "_Selection" + num
    public PlayableDirector[] cutscenes;
    public Ui_order_btn skip_btn;

    int select_num = 1; // count

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string before_scene = PlayerPrefs.GetString("SceneName");

        skip_btn.scene_name = "Day"+(Convert.ToInt32(before_scene.Substring(3, 1)) + 1)+"_scene";

        if (PlayerPrefs.HasKey(selection_name))
        {
            select_num = PlayerPrefs.GetInt(selection_name);
            select_num++;
            PlayerPrefs.SetInt(selection_name,select_num);
        }
        else
        {
            PlayerPrefs.SetInt(selection_name, select_num);
        }

        cutscenes[select_num-1].Play();
    }
}
