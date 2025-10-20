using UnityEngine;

public class Get_or_set_scenename : MonoBehaviour
{
    //다음씬에 ui 씬 이름을 설정하거나 이번 씬의 ui 이름을 설정
    public UI_manager uI_manager;
    public bool set_this_name = true;
    public string set_next_names_string;

    private void OnEnable()
    {
        if (set_this_name)
        {
            string name = PlayerPrefs.GetString("SceneName");
            uI_manager.scenes_name = name;
        }
        else
        {
            PlayerPrefs.SetString("SceneName",set_next_names_string);
        }
    }
}
