using System.Collections;
using UnityEngine;

public class HotDog_atk : Defalut_monster_atk
{
    public override void Attack()
    {
        StartCoroutine(Atk());
    }
    IEnumerator Atk()
    {

        animator.SetTrigger("atk");
        if (animator.transform.localScale.x > 0)
        {
            monster_move.rb.linearVelocity = new Vector2(1, 0);
            yield return new WaitForSeconds(0.5f);
            monster_move.rb.linearVelocity = new Vector2(-30, 0);
        }
        else
        {
            monster_move.rb.linearVelocity = new Vector2(-1, 0);
            yield return new WaitForSeconds(0.5f);
            monster_move.rb.linearVelocity = new Vector2(30, 0);
        }
        yield return new WaitForSeconds(0.33f);
        monster_move.rb.linearVelocity = monster_move.rb.linearVelocity * 0.3f;
        yield return new WaitForSeconds(3.67f);
        monster_move.is_attacking = false;
    }
}
