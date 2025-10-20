using System.Collections;
using UnityEngine;

public class Monster_5_atk : Monster_attack
{
    public override void Attack()
    {
        StartCoroutine(Atk());
    }
    IEnumerator Atk()
    {
        monster_move.sense_audio.Play();
        animator.SetTrigger("atk");
        yield return new WaitForSecondsRealtime(1.33f);
        monster_move.attack_audio.Play();
        GameObject ph_ = Instantiate(PH);
        ph_.transform.localScale = new Vector3(3f, 3f, 1);
        if (animator.transform.localScale.x>0)
        {
            ph_.transform.position = new Vector3(this.transform.position.x - 0.5f,this.transform.position.y,0);
        }
        else
        {
            ph_.transform.position = new Vector3(this.transform.position.x + 0.5f, this.transform.position.y, 0);
        }
        
        ph_.SetActive(true);
        yield return new WaitForSecondsRealtime(0.17f);
        Destroy(ph_);
        yield return new WaitForSecondsRealtime(0.5f);
        monster_move.is_attacking = false;
    }
}
