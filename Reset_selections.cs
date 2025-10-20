using UnityEngine;

public class Reset_selections : MonoBehaviour
{
    private void Start()
    {
        PlayerPrefs.SetInt("Stage1_Selection1", 0);
        PlayerPrefs.SetInt("Stage1_Selection2", 0);
        PlayerPrefs.SetInt("Stage1_Selection3", 0);
    }
}
