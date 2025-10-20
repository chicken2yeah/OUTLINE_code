using UnityEngine;

public class Get_or_set_scenename : MonoBehaviour
{
    //�������� ui �� �̸��� �����ϰų� �̹� ���� ui �̸��� ����
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
