using System.Collections;
using UnityEngine;

public class WhooshBoom_atk : Defalut_monster_atk
{
    public override void Attack()
    {
        StartCoroutine(Atk());
    }
    IEnumerator Atk()
    {
        animator.SetTrigger("atk");
        yield return new WaitForSeconds(0.5f);
        if (animator.transform.localScale.x > 0)
        {
            monster_move.rb.linearVelocity = new Vector2(-10, 0);
        }
        else
        {
            monster_move.rb.linearVelocity = new Vector2(10, 0);
        }
        yield return new WaitForSeconds(0.5f);
        monster_move.rb.linearVelocity = new Vector2(0, 0);
        yield return new WaitForSeconds(2f);
        monster_move.is_attacking = false;
    }
}
