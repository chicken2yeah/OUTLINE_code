using UnityEngine;

public class qusetion_bot : MonoBehaviour
{
    public Animator animator;

    public bool is_question = false; 
    public void Qusetion_switch()
    {
        is_question = !is_question;
        animator.SetBool("question",is_question);
    }
}
