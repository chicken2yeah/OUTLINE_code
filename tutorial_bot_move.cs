using System.Collections;
using UnityEngine;

public class tutorial_bot_move : Monster_move
{
    bool check_player = false;
    bool player_left = false;
    bool in_attack_distance = false;
    bool before_check_player = false;

    bool dead = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PA")
        {
            hurt_source.Play();
            hp -= player.power;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    IEnumerator Dead()
    {
        if (!player_left)
        {
            rb.linearVelocity = new Vector2(UnityEngine.Random.Range(3, 8), UnityEngine.Random.Range(5, 10));
            rb.freezeRotation = false;
            rb.angularVelocity = UnityEngine.Random.Range(36, 360);
            this.transform.localScale = this.transform.localScale * 0.7f;
            Destroy(collider_);
            yield return new WaitForSecondsRealtime(5f);
            Destroy(this.gameObject);
        }
        else
        {
            rb.linearVelocity = new Vector2(UnityEngine.Random.Range(-3, -8), UnityEngine.Random.Range(5, 10));
            rb.freezeRotation = false;
            rb.angularVelocity = UnityEngine.Random.Range(-36, -360);
            this.transform.localScale = this.transform.localScale * 0.7f;
            Destroy(collider_);
            yield return new WaitForSecondsRealtime(5f);
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0 && !dead)
        {
            Destroy(monster_Attack);
            dead = true;
            StartCoroutine(Dead());
        }
        else
        {
            float distance_to_player = this.transform.position.x - player.transform.position.x;
            Debug.DrawLine(new Vector3(this.transform.position.x - sensing_distance, this.transform.position.y, 0), new Vector3(this.transform.position.x + sensing_distance, this.transform.position.y, 0));
            if (Physics2D.Raycast(new Vector2(this.transform.position.x - sensing_distance, this.transform.position.y), Vector2.right, sensing_distance * 2, LayerMask.GetMask("Player")))
            {
                check_player = true;
                if (!before_check_player)
                {
                    sense_audio.Play();
                }

                player_left = player.transform.position.x > this.transform.position.x;

                if (distance_to_player < attack_distance && distance_to_player > (-1 * attack_distance))
                {
                    in_attack_distance = true;
                }
                else
                {
                    in_attack_distance = false;
                }
            }
            else
            {
                check_player = false;
            }
        }

        before_check_player = check_player;
    }


    void FixedUpdate()
    {
        if (hp > 0)
        {
            if (!is_attacking)
            {
                if (check_player)
                {
                    if (!player_left)
                    {
                        animator.transform.localScale = new Vector2(animator.transform.localScale.x < 0 ? animator.transform.localScale.x : (animator.transform.localScale.x * -1), animator.transform.localScale.y);
                    }
                    else
                    {
                        animator.transform.localScale = new Vector2(animator.transform.localScale.x < 0 ? (animator.transform.localScale.x * -1) : animator.transform.localScale.x, animator.transform.localScale.y);
                    }
                    if (in_attack_distance)
                    {
                        is_attacking = true;
                        monster_Attack.Attack();
                    }
                }
            }
        }
    }
}
