using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Splines.SplineInstantiate;

public class PLAYER_2 : PLAYER_common
{
    public float speed = 7;
    public float jump_power = 25;
    public bool atk_mode = false;
    public bool atk_lock = false;

    public PLAYER_2_atk player_2_atk;
    public GameObject player_2_atk_view;
    public Animator hp_animator;

    bool is_mode_change = false;
    bool atk_change_cool = false;
    bool is_dash_cool = false;
    bool is_atk_cool = false;
    bool is_dash = false;
    bool is_jump_cool = false;
    bool meet_slope = false;
    bool atk_switch = true;
    int dash_count = 2;
    int jump_count = 2;
    Coroutine dash_ienumerator = null;
    Coroutine jump_ienumerator = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    //피격 감지
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PH") // Player_Hit
        {
            if (!damaged)
            {
                hitted_right = collision.transform.position.x > this.transform.position.x;
                if (hp <= 1)
                {
                    StartCoroutine(Player_dead());
                }
                else
                {
                    StartCoroutine(Player_hitted());
                }
            }
        }
    }

    //피격 처리
    IEnumerator Player_hitted()
    {
        if (hitted_right)
        {
            rb.linearVelocity = new Vector2(-2, 4);
        }
        else
        {
            rb.linearVelocity = new Vector2(2, 4);
        }
        hitted_impurse.GenerateImpulse();
        hp -= 1;
        stop_fixed = true;
        damaged = true;
        animator.SetTrigger("hitted");
        yield return new WaitForSeconds(0.4f);
        stop_fixed = false;
        yield return new WaitForSeconds(1.6f);
        damaged = false;
    }

    IEnumerator Player_dead()
    {
        hp -= 1;
        uI_Manager.Stop_fixed();
        damaged = true;
        player_2_atk_view.SetActive(false);
        player_2_atk.gameObject.SetActive(false);
        uI_Manager.ui_group.SetActive(false);
        dead_background.SetActive(true);
        animator.SetTrigger("dead");
        yield return new WaitForSecondsRealtime(3f);
        uI_Manager.scenes_name = "game_over";
        StartCoroutine(uI_Manager.Change_scenes());
    }

    IEnumerator Atk_on()
    {
        atk_change_cool = true;
        animator.SetTrigger("atk_mode");
        yield return new WaitForSecondsRealtime(0.5f);
        player_2_atk.gameObject.SetActive(true);
        player_2_atk_view.SetActive(true);
        hp_animator.gameObject.SetActive(false);
        atk_change_cool = false;
    }

    IEnumerator Atk_off()
    {
        atk_change_cool = true;
        yield return new WaitForSecondsRealtime(0.43f);
        player_2_atk.gameObject.SetActive(false);
        player_2_atk_view.SetActive(false);
        hp_animator.gameObject.SetActive(true);
        atk_change_cool = false;
    }

    IEnumerator Atk()
    {
        atk_change_cool = true;
        is_atk_cool = true;
        animator.SetTrigger("atk");
        player_2_atk.ATK();
        yield return new WaitForSecondsRealtime(0.4f);
        atk_change_cool = false;
        is_atk_cool = false;
    }

    IEnumerator Dash_cool()
    {
        atk_change_cool = true;
        atk_mode = false;
        if (!atk_lock)
        {
            player_2_atk.gameObject.SetActive(false);
            player_2_atk_view.SetActive(false);
            hp_animator.gameObject.SetActive(true);
        }
        is_dash = true;
        is_dash_cool = true;
        animator.SetBool("dash_wait", true);
        if (dash_count == 1)
        {
            animator.SetTrigger("dash");
        }
        else
        {
            bool rb_left = rb.linearVelocity.x < 0;
            bool animator_left = animator.transform.localScale.x < 0;

            if (animator_left)
            {
                if (rb_left)
                {
                    animator.SetTrigger("dash_2_f");
                }
                else
                {
                    animator.SetTrigger("dash_2_b");
                }
            }
            else
            {
                if (rb_left)
                {
                    animator.SetTrigger("dash_2_b");
                }
                else
                {
                    animator.SetTrigger("dash_2_f");
                }
            }
        }
        yield return new WaitForSecondsRealtime(0.1f);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x * 0.1f, rb.linearVelocity.y);
        yield return new WaitForSecondsRealtime(0.4f);
        is_dash_cool = false;
        yield return new WaitForSecondsRealtime(0.1f);
        if (on_ground)
        {
            animator.SetBool("dash_wait", false);
            yield return new WaitForSecondsRealtime(0.2f);
            is_dash = false;
        }
        else
        {
            atk_mode = false;
            animator.SetBool("is_down", true);
            animator.SetTrigger("down");
            animator.SetBool("dash_wait", false);
            is_dash = false;
        }
        atk_change_cool = false;
    }

    IEnumerator Jump_cool()
    {
        atk_change_cool = true;
        atk_mode = false;
        is_jump_cool = true;
        if (!atk_lock)
        {
            player_2_atk.gameObject.SetActive(false);
            player_2_atk_view.SetActive(false);
            hp_animator.gameObject.SetActive(true);
        }
        if (jump_count == 1)
        {
            animator.SetTrigger("jump_1");
        }
        else
        {
            animator.SetTrigger("jump_2");
        }
        yield return new WaitForSecondsRealtime(0.1f);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x,rb.linearVelocity.y* 0.3f);
        yield return new WaitForSecondsRealtime(0.3f);
        is_jump_cool = false;
        atk_change_cool = false;
    }

    // Update is called once per frame
    void Update()
    {
        hp_animator.SetInteger("hp",hp);
        on_ground = Physics2D.Raycast(this.transform.position, Vector2.down, 2.2f, LayerMask.GetMask("Tile"));
        Debug.DrawRay(this.transform.position, Vector2.down * 2.2f);
        Debug.DrawRay(new Vector2(this.transform.position.x - 0.7f, this.transform.position.y), Vector2.down * 2f);
        Debug.DrawRay(new Vector2(this.transform.position.x + 0.7f, this.transform.position.y), Vector2.down * 2f);
        if (animator.transform.localScale.x > 0)
        {
            meet_slope = Physics2D.Raycast(new Vector2(this.transform.position.x + 0.7f, this.transform.position.y), Vector2.down, 2f, LayerMask.GetMask("Tile"));
            if (Physics2D.Raycast(new Vector2(this.transform.position.x - 0.7f, this.transform.position.y), Vector2.down, 2f, LayerMask.GetMask("Tile")))
            {
                on_ground = true;
            }else if (meet_slope)
            {
                on_ground = true;
            }
        }
        else
        {
            meet_slope = Physics2D.Raycast(new Vector2(this.transform.position.x - 0.7f, this.transform.position.y), Vector2.down, 2f, LayerMask.GetMask("Tile"));
            if (Physics2D.Raycast(new Vector2(this.transform.position.x + 0.7f, this.transform.position.y), Vector2.down, 2f, LayerMask.GetMask("Tile")))
            {
                on_ground = true;
            }
            else if (meet_slope)
            {
                on_ground = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            atk_switch = true;
        }

        if ((Time.timeScale == 0 && !animator.GetBool("cut_wait")) && !(damaged && hp <= 0))
        {
            animator.SetBool("cut_wait", true);
            animator.SetTrigger("cut");
            player_2_atk.gameObject.SetActive(false);
            player_2_atk_view.SetActive(false);
        }
        else if (Time.timeScale != 0 && animator.GetBool("cut_wait"))
        {
            animator.SetBool("cut_wait", false);
        }

        if (Time.timeScale != 0)
        {
            if (!atk_change_cool && is_mode_change)
            {
                if (atk_mode)
                {
                    animator.SetBool("atk_on", atk_mode);
                    StartCoroutine(Atk_on());
                    is_mode_change = false;
                }
                else
                {
                    animator.SetBool("atk_on",atk_mode);
                    StartCoroutine(Atk_off());
                    is_mode_change = false;
                }
            }
            if (((int)rb.linearVelocity.x) != 0)
            {
                animator.SetBool("move",true);
            }
            else
            {
                animator.SetBool("move",false);
            }

            if (!on_ground && ((int)rb.linearVelocity.y) < 0)
            {
                if (!animator.GetBool("is_down"))
                {
                    animator.SetBool("is_down", true);
                    animator.SetTrigger("down");
                }
            }
            else if(!(!on_ground&&animator.GetBool("is_down")))
            {
                animator.SetBool("is_down", false);
            }

            if (!atk_lock)
            {
                if (!atk_mode && (player_2_atk_view.activeSelf || player_2_atk.gameObject.activeSelf))
                {
                    player_2_atk.gameObject.SetActive(false);
                    player_2_atk_view.SetActive(false);
                    hp_animator.gameObject.SetActive(true);
                }
            }

            if (!is_dash && on_ground)
            {
                dash_count = 2;
            }

            if (on_ground)
            {
                jump_count = 2;
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("PLAYER_2_idle"))
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    dance_num = 1;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    dance_num = 2;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    dance_num = 3;
                }
            }
            else
            {
                dance_num = 0;
            }

            animator.SetInteger("dance", dance_num);
        }
    }

    private void FixedUpdate()
    {
        if (full_hp_mode)
        {
            hp = 3;
        }
        if (!stop_fixed)
        {
            if (on_ground)
            {
                if (Input.GetKey(KeyCode.Q) && !atk_lock)
                {
                    if (!atk_change_cool && atk_switch)
                    {
                        is_mode_change = true;
                        atk_switch = false;
                        atk_mode = !atk_mode;
                    }
                }

                if ((atk_mode && !atk_change_cool) && (Input.GetMouseButton((int)MouseButton.Left) && !is_atk_cool))
                {
                    StartCoroutine(Atk());
                }
            }

            if (Input.GetKey(KeyCode.W))
            {
                if ((jump_count > 0 && !is_jump_cool))
                {
                    jump_count--;
                    on_ground = false;
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump_power);
                    if (jump_ienumerator != null)
                    {
                        StopCoroutine(jump_ienumerator);
                    }
                    jump_ienumerator = StartCoroutine(Jump_cool());
                }
            }
            Debug.DrawRay(new Vector2(this.transform.position.x - 1.5f, this.transform.position.y + 1.5f), Vector2.down * 3.2f);
            Debug.DrawRay(new Vector2(this.transform.position.x + 1.5f, this.transform.position.y + 1.5f), Vector2.down * 3.2f);
            Debug.DrawRay(new Vector2(this.transform.position.x, this.transform.position.y), Vector2.left * 1.5f);
            Debug.DrawRay(new Vector2(this.transform.position.x, this.transform.position.y), Vector2.right * 1.5f);
            if (Input.GetKey(KeyCode.A) ^ Input.GetKey(KeyCode.D))
            {
                if (Input.GetKey(KeyCode.A))
                {
                    if ((Input.GetKey(KeyCode.Space) && !is_dash_cool) && (dash_count > 0 && !is_atk_cool))
                    {
                        dash_count--;
                        if (!is_dash)
                        {
                            if (animator.transform.localScale.x > 0)
                            {
                                animator.transform.localScale = new Vector3(animator.transform.localScale.x * -1, animator.transform.localScale.y, animator.transform.localScale.z);
                            }
                        }
                        rb.linearVelocity = new Vector2(0, 0);
                        rb.linearVelocity = new Vector2(speed * -5, 0);
                        if (dash_ienumerator != null)
                        {
                            StopCoroutine(dash_ienumerator);
                        }
                        dash_ienumerator = StartCoroutine(Dash_cool());
                    }
                    else if ((on_ground && !atk_mode) && !is_dash)
                    {
                        if (animator.transform.localScale.x > 0)
                        {
                            animator.transform.localScale = new Vector3(animator.transform.localScale.x * -1, animator.transform.localScale.y, animator.transform.localScale.z);
                        }

                        if (meet_slope)
                        {
                            rb.linearVelocity = new Vector2(-0.5f * speed, rb.linearVelocity.y);
                        }
                        else
                        {
                            rb.linearVelocity = new Vector2(-1 * speed, rb.linearVelocity.y);
                        }
                    }
                    else if (!atk_mode && (!is_dash && (!Physics2D.Raycast(new Vector2(this.transform.position.x - 1.5f, this.transform.position.y + 1.5f), Vector2.down, 3.2f, LayerMask.GetMask("Tile")) && !Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), Vector2.left, 1.5f, LayerMask.GetMask("Tile")))))
                    {
                        if (animator.transform.localScale.x > 0)
                        {
                            animator.transform.localScale = new Vector3(animator.transform.localScale.x * -1, animator.transform.localScale.y, animator.transform.localScale.z);
                        }
                        rb.linearVelocity = new Vector2(-0.8f * speed, rb.linearVelocity.y);
                    }
                }
                else
                {
                    if ((Input.GetKey(KeyCode.Space) && !is_dash_cool) && (dash_count > 0 && !is_atk_cool))
                    {
                        dash_count--;
                        if (!is_dash)
                        {
                            if (animator.transform.localScale.x < 0)
                            {
                                animator.transform.localScale = new Vector3(animator.transform.localScale.x * -1, animator.transform.localScale.y, animator.transform.localScale.z);
                            }
                        }
                        rb.linearVelocity = new Vector2(0, 0);
                        rb.linearVelocity = new Vector2(speed * 5, 0);
                        if (dash_ienumerator != null)
                        {
                            StopCoroutine(dash_ienumerator);
                        }
                        dash_ienumerator = StartCoroutine(Dash_cool());
                    }
                    else if ((on_ground && !atk_mode) && !is_dash)
                    {
                        if (animator.transform.localScale.x < 0)
                        {
                            animator.transform.localScale = new Vector3(animator.transform.localScale.x * -1, animator.transform.localScale.y, animator.transform.localScale.z);
                        }

                        if (meet_slope)
                        {
                            rb.linearVelocity = new Vector2(0.5f * speed, rb.linearVelocity.y);
                        }
                        else
                        {
                            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
                        }
                    }
                    else if (!atk_mode && (!is_dash && (!Physics2D.Raycast(new Vector2(this.transform.position.x + 1.5f, this.transform.position.y + 1.5f), Vector2.down, 3.2f, LayerMask.GetMask("Tile")) && !Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), Vector2.right, 1.5f, LayerMask.GetMask("Tile")))))
                    {
                        if (animator.transform.localScale.x < 0)
                        {
                            animator.transform.localScale = new Vector3(animator.transform.localScale.x * -1, animator.transform.localScale.y, animator.transform.localScale.z);
                        }
                        rb.linearVelocity = new Vector2(0.8f * speed, rb.linearVelocity.y);
                    }
                }
            }
            else if (!is_dash || !on_ground)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
        }
    }
}
