using UnityEngine;

public class time_stop_when_enable : MonoBehaviour
{
    public bool enable_by_selection = true;
    public interactive_function_select select;
    private void OnEnable()
    {
        Time.timeScale = 0f;
    }
    private void OnDisable()
    {
        if (enable_by_selection)
        {
            select.Stop_active_switch();
        }
        Time.timeScale = 1f;
    }
}
