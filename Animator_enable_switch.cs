using UnityEngine;

public class Animator_enable_switch : MonoBehaviour
{
    public Animator animator;
    public void Switch_enable()
    {
        animator.enabled = !animator.enabled;
    }
}
