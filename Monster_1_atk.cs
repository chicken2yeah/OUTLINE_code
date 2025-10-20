using System.Collections;
using UnityEngine;

public class Monster_1_atk : Monster_attack
{

    public override void Attack()
    {
        StartCoroutine(Atk());
    }

    IEnumerator Atk()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        animator.SetTrigger("atk");
        monster_move.attack_audio.Play();
        yield return new WaitForSecondsRealtime(0.75f);
        GameObject ph_ = Instantiate(PH);
        ph_.transform.localScale = new Vector3(2,2,1);
        if (animator.transform.localScale.x > 0)
        {
            ph_.transform.position = new Vector3(monster_move.collider_.bounds.min.x,monster_move.collider_.bounds.center.y,0);
        }
        else
        {
            ph_.transform.position = new Vector3(monster_move.collider_.bounds.max.x, monster_move.collider_.bounds.center.y, 0);
        }
        
        ph_.SetActive(true);
        yield return new WaitForSecondsRealtime(0.1f);
        Destroy(ph_);
        yield return new WaitForSecondsRealtime(0.15f);
        monster_move.is_attacking = false;
    }
}
