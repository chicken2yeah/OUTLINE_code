using System.Collections;
using UnityEngine;

public class Monster_3_atk : Monster_attack
{
    public override void Attack()
    {
        StartCoroutine(Atk());
    }
    IEnumerator Atk()
    {
        animator.SetTrigger("atk");
        yield return new WaitForSecondsRealtime(1.5f);
        monster_move.attack_audio.Play();
        GameObject ph_ = Instantiate(PH,this.transform);
        ph_.transform.localScale = new Vector3(3, 7, 1);
        if (animator.transform.localScale.x > 0)
        {
            ph_.transform.position = new Vector3(monster_move.collider_.bounds.min.x, monster_move.collider_.bounds.center.y, 0);
            monster_move.rb.linearVelocity = new Vector2(-40,0);
        }
        else
        {
            ph_.transform.position = new Vector3(monster_move.collider_.bounds.max.x, monster_move.collider_.bounds.center.y, 0);
            monster_move.rb.linearVelocity = new Vector2(40, 0);
        }
        ph_.SetActive(true);
        yield return new WaitForSecondsRealtime(0.1875f);
        monster_move.rb.linearVelocity = new Vector2(0, 0);
        ph_.SetActive(false);
        yield return new WaitForSecondsRealtime(0.25f);
        ph_.SetActive(true);
        yield return new WaitForSecondsRealtime(0.0625f);
        Destroy(ph_);
        yield return new WaitForSecondsRealtime(2f);
        monster_move.is_attacking = false;
    }
}
