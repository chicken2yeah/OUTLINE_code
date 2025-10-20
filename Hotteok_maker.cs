using UnityEngine;
using UnityEngine.Playables;

public class Hotteok_maker : MonoBehaviour
{
    public PlayableDirector[] playableDirectors;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 0f;
        int[] selection_choice = new int[3];

        if (PlayerPrefs.HasKey("Stage1_Selection_1"))
        {
            selection_choice[0] = PlayerPrefs.GetInt("Stage1_Selection_1");
        }
        else
        {
            selection_choice[0] = 0;
        }

        if (PlayerPrefs.HasKey("Stage1_Selection_2"))
        {
            selection_choice[1] = PlayerPrefs.GetInt("Stage1_Selection_2");
        }
        else
        {
            selection_choice[1] = 0;
        }

        if (PlayerPrefs.HasKey("Stage1_Selection_3"))
        {
            selection_choice[2] = PlayerPrefs.GetInt("Stage1_Selection_3");
        }
        else
        {
            selection_choice[2] = 0;
        }

        if (PlayerPrefs.GetString("SceneName") != "Day7_scene")
        {
            int hotteok_selection = 2;
            for (int a = selection_choice.Length - 1; a >= 0; a--)
            {
                if (selection_choice[hotteok_selection] < selection_choice[a])
                {
                    hotteok_selection = a;
                }
            }

            playableDirectors[hotteok_selection].Play();
        }
    }
}
