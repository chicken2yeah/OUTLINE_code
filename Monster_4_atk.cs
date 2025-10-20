using System.Collections;
using UnityEngine;

public class Monster_4_atk : Monster_attack
{
    public override void Attack()
    {
        StartCoroutine(Atk());
    }
    IEnumerator Atk()
    {

        animator.SetTrigger("atk");
        GameObject ph_ = Instantiate(PH, this.transform);
        ph_.transform.localScale = new Vector3(7, 0.3f, 1);
        monster_move.attack_audio.Play();
        if (animator.transform.localScale.x > 0)
        {
            monster_move.rb.linearVelocity = new Vector2(1, 0);
            yield return new WaitForSecondsRealtime(0.5f);
            ph_.transform.position = new Vector3(this.transform.position.x,this.transform.position.y - 1.2f,0);
            monster_move.rb.linearVelocity = new Vector2(-30, 0);
        }
        else
        {
            monster_move.rb.linearVelocity = new Vector2(-1, 0);
            yield return new WaitForSecondsRealtime(0.5f);
            ph_.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 1.2f, 0);
            monster_move.rb.linearVelocity = new Vector2(30, 0);
        }
        ph_.SetActive(true);
        yield return new WaitForSecondsRealtime(0.33f);
        monster_move.rb.linearVelocity = monster_move.rb.linearVelocity * 0.3f;
        Destroy(ph_);
        yield return new WaitForSecondsRealtime(3.67f);
        monster_move.is_attacking = false;
    }
}
