using System.Collections;
using UnityEngine;

public class YamYam_atk : Defalut_monster_atk
{
    public override void Attack()
    {
        StartCoroutine(Atk());
    }
    IEnumerator Atk()
    {
        monster_move.sense_audio.Play();
        animator.SetTrigger("atk");
        yield return new WaitForSeconds(2f);
        monster_move.is_attacking = false;
    }
}
