using System.Collections;
using UnityEngine;

public class Monster_6_atk : Monster_attack
{
    public AudioSource crack;
    public override void Attack()
    {
        StartCoroutine(Atk());
    }
    IEnumerator Atk()
    {
        crack.Play();
        animator.SetTrigger("atk");
        yield return new WaitForSecondsRealtime(1.1f);
        GameObject ph_ = Instantiate(PH);
        ph_.transform.localScale = new Vector3(4, 4, 1);
        if (animator.transform.localScale.x > 0)
        {
            ph_.transform.position = new Vector3(monster_move.collider_.bounds.min.x - 2, monster_move.collider_.bounds.min.y, 0);
        }
        else
        {
            ph_.transform.position = new Vector3(monster_move.collider_.bounds.max.x + 2, monster_move.collider_.bounds.min.y, 0);
        }
        monster_move.attack_audio.Play();
        ph_.SetActive(true);
        yield return new WaitForSecondsRealtime(0.1f);
        Destroy(ph_);
        yield return new WaitForSecondsRealtime(3.9f);
        monster_move.is_attacking = false;
    }
}
