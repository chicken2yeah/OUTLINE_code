using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.VFX;

public class GUARDIAN_stage_1_boss : MonoBehaviour
{
    public A1_script player;
    public int hp = 1500;
    public Animator animator;
    public Rigidbody2D rb;
    public Collider2D collider_;
    public VisualEffect hurt_effect;
    public AudioSource hurt_source;
    public boss_hp_bar hp_bar;
    public GameObject scene_change;

    public PlayableDirector end_cutscene;

    bool atk_wait = false;

    // idle, slash, jump, dash_atk, dash
    float[] each_atk_cool = { 1f, 2f, 3f, 4f, 0.75f };


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PA")
        {
            if (hp > 0)
            {
                float h_x = UnityEngine.Random.Range(this.collider_.bounds.min.x, this.collider_.bounds.max.x);
                float h_y = UnityEngine.Random.Range(this.collider_.bounds.min.y, this.collider_.bounds.max.y);
                hurt_effect.transform.position = new Vector2(h_x, h_y);
                hurt_effect.Play();
                hurt_source.Play();
                hp -= player.power;
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        atk_wait=true;
        StartCoroutine(Atk_cool(7f));
    }

    // Update is called once per frame
    void Update()
    {
        hp_bar.hp = this.hp;
        if (hp <= 0 && Time.timeScale > 0)
        {
            scene_change.SetActive(true);
            player.hp = 5;
            animator.Play("Boss_1_idle");
            player.animator.Play("Idle");
            Time.timeScale = 0f;
            StartCoroutine(End_Stage_1());
        }
    }

    IEnumerator End_Stage_1()
    {
        yield return new WaitForSecondsRealtime(3f);
        end_cutscene.Play();
    }

    private void FixedUpdate()
    {
        if (!atk_wait)
        {
            if (this.transform.position.x <= player.transform.position.x)
            {
                if (animator.transform.localScale.x < 0)
                {
                    animator.transform.localScale = new Vector3(-animator.transform.localScale.x,animator.transform.localScale.y,animator.transform.localScale.z);
                }
            }
            else
            {
                if (animator.transform.localScale.x > 0)
                {
                    animator.transform.localScale = new Vector3(-animator.transform.localScale.x, animator.transform.localScale.y, animator.transform.localScale.z);
                }
            }
            int atk_set_num = 0;

            // 원거리일때 확률 변경
            if (player.transform.position.x > this.transform.position.x + 5 || player.transform.position.x < this.transform.position.x - 5)
            {
                atk_set_num = Random.Range(0,7);
                if (atk_set_num > 4)
                {
                    atk_set_num = 4;
                }
            }
            else
            {
                atk_set_num = Random.Range(0, 5);
                if (atk_set_num > 4)
                {
                    atk_set_num = 1;
                }
            }

            switch (atk_set_num)
            {
                case 2:
                    {
                        StartCoroutine(Jump());
                        break;
                    }
                case 4:
                    {
                        StartCoroutine(Dash());
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            animator.SetInteger("Atk_num", atk_set_num);
            animator.SetTrigger("Atk");
            StartCoroutine(Atk_cool(each_atk_cool[atk_set_num]));
        }
    }

    IEnumerator Dash()
    {
        if (animator.transform.localScale.x > 0)
        {
            rb.linearVelocity = new Vector2(10,rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(-10, rb.linearVelocity.y);
        }
        yield return new WaitForSeconds(0.5f);
        rb.linearVelocity = rb.linearVelocity * 0.2f;
    }

    IEnumerator Jump()
    {
        yield return new WaitForSeconds(1.25f);
        this.transform.position = new Vector3(player.transform.position.x , this.transform.position.y, this.transform.position.z);
    }

    IEnumerator Atk_cool(float cool)
    {
        atk_wait = true;
        yield return new WaitForSeconds(cool);
        atk_wait = false;
    }
}
