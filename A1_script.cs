using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.U2D.Animation;

public class A1_script : MonoBehaviour
{
    public int hp = 5;
    public int power = 10;

    public Rigidbody2D rb;
    public Animator animator;
    public SpriteResolver[] eye_mouth;
    public InputManager inputManager;
    public Mouse_script mouse;
    public Animator hp_animator;

    public float speed = 0.3f;
    public float max_speed = 20f;
    public float min_speed = 1f;
    float min_jump = 5f;

    bool hurt = false;
    bool hurt_cool = false;
    bool onGround = true;
    bool is_down = false;
    bool atk_cool = false;
    bool is_parry = false;
    bool is_parry_success = false;
    bool parry_cool = false;

    int parry_count = 0;

    int jump_count = 1;
    int set_jump_count = 1;
    int atk_combo = 0;

    public Skill_cool[] skill_Cools;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PH")
        {
            if (is_parry)
            {
                is_parry_success = true;
                if (onGround)
                {
                    animator.SetTrigger("parry");
                    rb.linearVelocity = new Vector2((this.transform.position.x <= collision.transform.position.x ? -1 : 1) * 7, rb.linearVelocity.y);
                }
                else
                {
                    animator.SetTrigger("sky_parry");
                    if (this.transform.position.x <= collision.transform.position.x)
                    {
                        if (animator.transform.localScale.x < 0)
                        {
                            animator.transform.localScale = new Vector3(-animator.transform.localScale.x, animator.transform.localScale.y, 1);
                        }
                        rb.linearVelocity = new Vector2(-15, rb.linearVelocity.y);
                    }
                    else
                    {
                        if (animator.transform.localScale.x > 0)
                        {
                            animator.transform.localScale = new Vector3(-animator.transform.localScale.x, animator.transform.localScale.y, 1);
                        }
                        rb.linearVelocity = new Vector2(15, rb.linearVelocity.y);
                    }
                }
            }
            else if(!hurt_cool)
            {
                hp--;
                StartCoroutine(Hurt(this.transform.position.x <= collision.transform.position.x));
            }
        }
    }

    public void Self_hurt(bool hit_right)
    {
        if (!hurt_cool && rb.linearVelocity.y <= 0)
        {
            hp--;
            StartCoroutine(Hurt(hit_right));
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hp_animator.SetInteger("hp", hp);
    }

    public void SwitchAnimatorMode()
    {
        // 놀라운 사실! 플레이어블 디렉터의 일시정지는 일시정지가 아니랍니다! 시간대를 저장하고 멈춰버리는거에 가까워요! 그 말인 즉슨 일시정지 상태에서는 값이 유지되지 않는 거죠!
        if (animator.enabled)
        {
            animator.enabled = false;
        }
        else
        {
            animator.enabled = true;
        }
    }

    bool before_break = false;
    // Update is called once per frame
    void Update()
    {
        if (!hurt && Time.timeScale != 0)
        {
            Debug.DrawRay(this.transform.position, Vector2.down * 1.3f);
            onGround = Physics2D.Raycast(this.transform.position, Vector2.down, 1.3f, LayerMask.GetMask("Tile")) && (rb.linearVelocity.y > -0.1f && rb.linearVelocity.y < 0.1f);

            animator.SetFloat("speed", rb.linearVelocity.x);
            animator.SetBool("break", inputManager.break_control);
            animator.SetBool("onground",onGround);
            if ((!onGround && rb.linearVelocity.y < 0)&&!is_down)
            {
                jump_count = 0;
                is_down = true;
                animator.SetTrigger("down_tr");
                animator.SetBool("down",is_down);
            }
            if (onGround && jump_count < set_jump_count)
            {
                is_down = false;
                animator.SetBool("down", is_down);
                jump_count = set_jump_count;
            }
            if ((!before_break && inputManager.break_control)&&(onGround&&jump_count>0))
            {
                animator.SetTrigger("break_tr");
            }
            before_break = inputManager.break_control;
        }
    }

    void FixedUpdate()
    {
        if (!hurt && Time.timeScale != 0)
        {
            if (jump_count > 0 && inputManager.up_move)
            {
                animator.SetTrigger("jump");
                onGround = false;
                jump_count--;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, min_jump + (rb.linearVelocity.x > 0 ? rb.linearVelocity.x:-rb.linearVelocity.x)*0.3f);
            }

            // 판정 우선순위 : 패링 -> 대쉬공격 -> 브레이크 -> 기본공격 -> 좌우이동
            if ((inputManager.parry_control && !hurt_cool) && !parry_cool)
            {
                parry_count++;
                StartCoroutine(Parry(parry_count));
            }
            else if ((inputManager.dash_atk && mouse.on_atk_able_obj) && !atk_cool)
            {
                atk_combo++;
                StartCoroutine(Dash_atk(atk_combo));
            }
            else if (inputManager.break_control && onGround)
            {
                if (!(rb.linearVelocity.x < min_speed && rb.linearVelocity.x > -min_speed))
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x * 0.9f, rb.linearVelocity.y);
                }
            }
            else if ((inputManager.nomal_atk && onGround)&&!atk_cool)
            {
                atk_combo++;
                StartCoroutine(Nomal_atk(atk_combo));
            }
            else if ((inputManager.right_move ^ inputManager.left_move))
            {
                if (inputManager.right_move)
                {
                    if (rb.linearVelocity.x < min_speed && rb.linearVelocity.x > -1)
                    {
                        rb.linearVelocity = new Vector2(rb.linearVelocity.x + min_speed, rb.linearVelocity.y);
                    }
                    else
                    {
                        rb.linearVelocity = new Vector2(rb.linearVelocity.x + speed, rb.linearVelocity.y);
                    }
                }
                else
                {
                    if (rb.linearVelocity.x > -min_speed && rb.linearVelocity.x < 1)
                    {
                        rb.linearVelocity = new Vector2(rb.linearVelocity.x - min_speed, rb.linearVelocity.y);
                    }
                    else
                    {
                        rb.linearVelocity = new Vector2(rb.linearVelocity.x - speed, rb.linearVelocity.y);
                    }
                }
            }

            if (rb.linearVelocity.x >= 0.1f && !is_parry_success)
            {
                if (animator.transform.localScale.x < 0)
                {
                    animator.transform.localScale = new Vector3(-animator.transform.localScale.x, animator.transform.localScale.y, 1);
                }
                if (rb.linearVelocity.x > max_speed)
                {
                    rb.linearVelocity = new Vector2(max_speed, rb.linearVelocity.y);
                }
            }
            else if (rb.linearVelocity.x <= -0.1f && !is_parry_success)
            {
                if (animator.transform.localScale.x > 0)
                {
                    animator.transform.localScale = new Vector3(-animator.transform.localScale.x, animator.transform.localScale.y, 1);
                }
                if (rb.linearVelocity.x < -max_speed)
                {
                    rb.linearVelocity = new Vector2(-max_speed, rb.linearVelocity.y);
                }
            }
        }
    }

    IEnumerator Parry(int parry_count_)
    {
        inputManager.parry_control = false;
        parry_cool = true;
        is_parry = true;
        yield return new WaitForSeconds(0.2f);
        is_parry = false;
        if (is_parry_success)
        {
            skill_Cools[2].Skill_cooltime(0.01f);
            hurt_cool = true;
            yield return new WaitForSeconds(0.01f);
            is_parry_success = false;
            parry_cool = false;
            yield return new WaitForSeconds(0.1f);
            if (parry_count_ == parry_count)
            {
                hurt_cool = false;
                parry_count = 0;
            }

        }
        else
        {
            skill_Cools[2].Skill_cooltime(3);
            yield return new WaitForSeconds(3f);
            parry_cool = false;
        }
    }

    IEnumerator Nomal_atk(int atk_c)
    {
        atk_cool = true;
        animator.SetInteger("atk_combo", atk_c%4);
        animator.SetTrigger("atk");
        if (atk_c%4 == 3)
        {
            skill_Cools[0].Skill_cooltime(0.6f);
            yield return new WaitForSeconds(0.6f);
        }
        else
        {
            skill_Cools[0].Skill_cooltime(0.3f);
            yield return new WaitForSeconds(0.3f);
        }
        atk_cool = false;
        yield return new WaitForSeconds(0.5f);
        if (atk_combo == atk_c)
        {
            atk_combo = -1;
            animator.SetInteger("atk_combo", -1);
        }
    }

    IEnumerator Hurt(bool hit_right)
    {
        hurt = true;
        hurt_cool = true;
        hp_animator.SetInteger("hp",hp);
        animator.SetTrigger("hurt");
        atk_combo = -1;
        animator.SetInteger("atk_combo", -1);
        if (hit_right)
        {
            rb.linearVelocity = new Vector2(-5,3);
        }
        else
        {
            rb.linearVelocity = new Vector2(5,3);
        }
        yield return new WaitForSeconds(1f);
        hurt = false;
        yield return new WaitForSeconds(0.5f);
        hurt_cool = false;
    }

    IEnumerator Dash_atk(int atk_c)
    {
        atk_cool = true;
        animator.SetInteger("atk_combo", atk_c);
        if (mouse.col_locate.y > this.transform.position.y)
        {
            animator.SetInteger("dash_locate", 2);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, min_jump + (rb.linearVelocity.x > 0 ? rb.linearVelocity.x : -rb.linearVelocity.x) * 0.3f);
        }
        else if (mouse.col_locate.y < this.transform.position.y - 1)
        {
            animator.SetInteger("dash_locate", 0);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -min_jump - (rb.linearVelocity.x > 0 ? rb.linearVelocity.x : -rb.linearVelocity.x) * 0.3f);
        }
        else
        {
            animator.SetInteger("dash_locate", 1);
        } 

        if (mouse.col_locate.x >= this.transform.position.x)
        {
            if (animator.transform.localScale.x < 0)
            {
                animator.transform.localScale = new Vector3(-animator.transform.localScale.x, animator.transform.localScale.y, 1);
            }
            if (rb.linearVelocity.x < 7)
            {
                rb.linearVelocity = new Vector2(7, rb.linearVelocity.y);
            }
        }
        else
        {
            if (animator.transform.localScale.x > 0)
            {
                animator.transform.localScale = new Vector3(-animator.transform.localScale.x, animator.transform.localScale.y, 1);
            }
            if (rb.linearVelocity.x > -7)
            {
                rb.linearVelocity = new Vector2(-7, rb.linearVelocity.y);
            }
        }
        this.transform.position = mouse.col_locate;
        animator.SetTrigger("dash_atk_tr");
        skill_Cools[1].Skill_cooltime(0.5f);
        yield return new WaitForSecondsRealtime(0.5f);
        atk_cool = false;
        yield return new WaitForSecondsRealtime(1f);
        if (atk_combo == atk_c)
        {
            atk_combo = -1;
            animator.SetInteger("atk_combo", -1);
        }
    }
}
