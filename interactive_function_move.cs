using UnityEngine;

public class interactive_function_move : MonoBehaviour
{
    public Animator main_animator;
    public Animator[] sub_animator;
    public int mode = 0;
    int mode_before = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mode != mode_before)
        {
            Change_animator_mode();
        }
        mode_before = mode;
    }

    void Change_animator_mode()
    {
        for (int a = 0; a<sub_animator.Length; a++)
        {
            sub_animator[a].SetInteger("mode",mode);
        }
        main_animator.SetInteger("mode",mode);
    }
}
