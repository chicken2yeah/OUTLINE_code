using System.Collections;
using UnityEngine;

public class Defalut_monster_atk : MonoBehaviour
{
    public Animator animator;
    public Defalut_monster monster_move;
    public float wait_cool;

    public virtual void Attack()
    {
        StartCoroutine(Atk());
    }

    private IEnumerator Atk()
    {
        animator.SetTrigger("atk");
        yield return new WaitForSeconds(wait_cool);
        monster_move.is_attacking = false;
    }
}
