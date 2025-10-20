using System.Collections;
using UnityEngine;

public class Ui_button : MonoBehaviour
{
    public Animator animator;
    private void OnMouseEnter()
    {
        animator.SetInteger("mode", 1);
    }

    private void OnMouseExit()
    {
        animator.SetInteger("mode", 0);
    }
}
