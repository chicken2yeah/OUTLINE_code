using System.Collections;
using UnityEngine;

public class Tutorial_bot_atk : Monster_attack
{
    public override void Attack()
    {
        StartCoroutine(Atk());
    }
    IEnumerator Atk()
    {
        animator.SetTrigger("atk");
        monster_move.is_attacking = true;
        yield return new WaitForSecondsRealtime(5.5f);
        GameObject ph_ = Instantiate(PH);
        ph_.transform.localScale = new Vector3(2, 2, 1);
        if (animator.transform.localScale.x < 0)
        {
            ph_.transform.position = new Vector3(monster_move.collider_.bounds.min.x, monster_move.collider_.bounds.center.y, 0);
        }
        else
        {
            ph_.transform.position = new Vector3(monster_move.collider_.bounds.max.x, monster_move.collider_.bounds.center.y, 0);
        }
        monster_move.attack_audio.Play();
        ph_.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        Destroy(ph_);
        yield return new WaitForSecondsRealtime(1f);
        monster_move.is_attacking = false;
    }
}
