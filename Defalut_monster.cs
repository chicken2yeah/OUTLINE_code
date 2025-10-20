using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Defalut_monster : MonoBehaviour
{
    public A1_script player;
    public Rigidbody2D rb;
    public Defalut_monster_atk monster_Attack;
    public Animator animator;
    public AudioSource hurt_source;
    public Collider2D collider_;
    public AudioSource sense_audio;
    public VisualEffect hurt_effect;

    public float sensing_distance = 7;
    public float attack_distance = 2;
    public float walk_speed = 3;
    public float run_speed = 5;
    public int hp = 10;
    public bool is_attacking = false;
    public bool hurt_resistance = false;

    bool check_player = false;
    bool player_left = false;
    bool in_attack_distance = false;
    bool before_check_player = false;
    bool hurt = false;
    bool dead = false;

    int hurt_count = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PA")
        {
            if (hp > 1)
            {
                hurt_count++;
                StartCoroutine(Hurt(hurt_count));
            }
        }
    }

    IEnumerator Hurt(int count)
    {
        hurt = true;
        if (!hurt_resistance)
        {
            animator.SetTrigger("hurt");
            monster_Attack.StopAllCoroutines();
            is_attacking = false;
        }
        if (player.transform.position.x >= this.transform.position.x)
        {
            this.transform.eulerAngles = new Vector3(0,0,10);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x - 2,rb.linearVelocity.y);
        }
        else
        {
            this.transform.eulerAngles = new Vector3(0, 0, -10);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x + 2, rb.linearVelocity.y);
        }
        float h_x = UnityEngine.Random.Range(this.collider_.bounds.min.x,this.collider_.bounds.max.x);
        float h_y = UnityEngine.Random.Range(this.collider_.bounds.min.y, this.collider_.bounds.max.y);
        hurt_effect.transform.position = new Vector2(h_x, h_y);
        hurt_effect.Play();
        hurt_source.Play();
        hp -= player.power;
        yield return new WaitForSeconds(0.1f);
        if (hurt_count == count)
        {
            hurt = false;
            this.transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    IEnumerator Dead()
    {
        if (!player_left)
        {
            rb.linearVelocity = new Vector2(UnityEngine.Random.Range(3, 8), 3);
            rb.freezeRotation = false;
            rb.angularVelocity = UnityEngine.Random.Range(36, 360);
        }
        else
        {
            rb.linearVelocity = new Vector2(UnityEngine.Random.Range(-3, -8), 3);
            rb.freezeRotation = false;
            rb.angularVelocity = UnityEngine.Random.Range(-36, -360);
        }
        Destroy(collider_);
        Destroy(this.GetComponent<BoxCollider2D>());
        yield return new WaitForSecondsRealtime(5f);
        Destroy(this.gameObject);
    }

    bool onground = true;

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            if (hp>0)
            {
                Debug.DrawRay(collider_.bounds.center, Vector2.down * (collider_.bounds.extents.y + 0.1f));
                onground = Physics2D.Raycast(new Vector2(collider_.bounds.max.x, collider_.bounds.min.y), Vector2.down, collider_.bounds.extents.y + 0.2f, LayerMask.GetMask("Tile"))
                    ||
                           Physics2D.Raycast(new Vector2(collider_.bounds.min.x, collider_.bounds.min.y), Vector2.down, collider_.bounds.extents.y + 0.2f, LayerMask.GetMask("Tile"));

            }
            if (hp <= 0)
            {
                dead = true;
                Destroy(monster_Attack);
                StartCoroutine(Dead());
            }
            else if (onground)
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

                if (rb.linearVelocity.x != 0)
                {
                    if (check_player)
                    {
                        animator.SetInteger("move", 2);
                    }
                    else
                    {
                        animator.SetInteger("move", 1);
                    }
                }
                else
                {
                    animator.SetInteger("move", 0);
                }
            }

            before_check_player = check_player;
        }
        
    }


    int random_move_count = 0;

    void FixedUpdate()
    {
        if (hp > 0 && onground)
        {
            if (!is_attacking && !hurt)
            {
                if (check_player)
                {
                    if (player_left)
                    {
                        animator.transform.localScale = new Vector2(animator.transform.localScale.x < 0 ? animator.transform.localScale.x : (animator.transform.localScale.x * -1), animator.transform.localScale.y);
                    }
                    else
                    {
                        animator.transform.localScale = new Vector2(animator.transform.localScale.x < 0 ? (animator.transform.localScale.x * -1) : animator.transform.localScale.x, animator.transform.localScale.y);
                    }
                    if (in_attack_distance)
                    {
                        rb.linearVelocity = new Vector2(0, 0);
                        is_attacking = true;
                        monster_Attack.Attack();
                    }
                    else
                    {
                        moving();
                    }
                }
                else
                {
                    if (random_move_count > 0)
                    {
                        animator.transform.localScale = new Vector2(animator.transform.localScale.x > 0 ? animator.transform.localScale.x : (animator.transform.localScale.x * -1), animator.transform.localScale.y);
                        random_move_count--;
                        moving();
                    }
                    else if (random_move_count < 0)
                    {
                        animator.transform.localScale = new Vector2(animator.transform.localScale.x < 0 ? animator.transform.localScale.x : (animator.transform.localScale.x * -1), animator.transform.localScale.y);
                        random_move_count++;
                        moving();
                    }
                    else
                    {
                        random_move_count = UnityEngine.Random.Range(-300, 300);
                    }
                }
            }
        }
    }

    void moving()
    {
        if (animator.transform.localScale.x > 0)
        {
            Debug.DrawLine(new Vector2(collider_.bounds.min.x, collider_.bounds.min.y), new Vector2(collider_.bounds.min.x, collider_.bounds.min.y - collider_.bounds.extents.y - 0.2f));
            if (Physics2D.Raycast(new Vector2(collider_.bounds.min.x, collider_.bounds.min.y), Vector2.down, collider_.bounds.extents.y + 0.2f, LayerMask.GetMask("Tile")))
            {
                rb.linearVelocity = new Vector2((check_player ? run_speed : walk_speed) * -1, rb.linearVelocity.y);
            }
            else
            {
                random_move_count *= -1;
                rb.linearVelocity = new Vector2(0, 0);
            }

        }
        else
        {
            Debug.DrawLine(new Vector2(collider_.bounds.max.x, collider_.bounds.min.y), new Vector2(collider_.bounds.max.x, collider_.bounds.min.y - collider_.bounds.extents.y - 0.2f));
            if (Physics2D.Raycast(new Vector2(collider_.bounds.max.x, collider_.bounds.min.y), Vector2.down, collider_.bounds.extents.y + 0.2f, LayerMask.GetMask("Tile")))
            {
                rb.linearVelocity = new Vector2((check_player ? run_speed : walk_speed), rb.linearVelocity.y);
            }
            else
            {
                random_move_count *= -1;
                rb.linearVelocity = new Vector2(0, 0);
            }
        }
    }
}
