using System.Collections;
using NUnit.Framework.Constraints;
using UnityEngine;

public class PLAYER_3 : PLAYER_common
{
    public float speed = 6f;
    public float jump_power = 15f;

    public Animator hp_animator;
    public GameObject PA_;
    public GameObject fire_work;

    bool is_jump_cool = false;
    bool is_nomal_atk = false;
    bool is_nomal_atk_cool = false;
    bool is_parrying = false;
        bool is_parrying_atk = false;
    bool is_parrying_cool = false;
    bool parrying_success = false;
    bool charge_down_atk = false;
    bool is_down_atk = false;
    bool is_down_out = false;
    bool charge_parrying_atk = false;
    Coroutine now_act_coroutine = null;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PH") // Player_Hit
        {
            if (is_parrying && !parrying_success)
            {
                hitted_right = collision.transform.position.x > this.transform.position.x;
                parrying_success = true;
                StartCoroutine(Parrying_success());
            }
            else
            {
                if (!damaged && !parrying_success)
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

        if (collision.gameObject.layer == LayerMask.NameToLayer("Tile") && !on_ground)
        {
            on_ground = true;
            StartCoroutine(Down_out());
        }
    }

    IEnumerator Player_hitted()
    {
        if (hitted_right)
        {
            rb.linearVelocity = new Vector2(-10, 0);
        }
        else
        {
            rb.linearVelocity = new Vector2(10, 0);
        }
        hitted_impurse.GenerateImpulse();
        hp -= 1;
        stop_fixed = true;
        damaged = true;
        animator.SetTrigger("hitted");
        yield return new WaitForSeconds(0.2f);
        stop_fixed = false;
        yield return new WaitForSeconds(1.6f);
        damaged = false;
    }

    IEnumerator Player_dead()
    {
        hp -= 1;
        uI_Manager.Stop_fixed();
        damaged = true;
        uI_Manager.ui_group.SetActive(false);
        dead_background.SetActive(true);
        animator.SetTrigger("dead");
        yield return new WaitForSecondsRealtime(3f);
        uI_Manager.scenes_name = "game_over";
        StartCoroutine(uI_Manager.Change_scenes());
    }

    IEnumerator Down()
    {
        is_jump_cool = true;
        yield return new WaitForSecondsRealtime(0.5f);
        if (!on_ground)
        {
            charge_down_atk = true;
        }
    }

    IEnumerator Jump()
    {
        is_jump_cool = true;
        animator.SetTrigger("jump");
        yield return new WaitForSecondsRealtime(0.5f);
        on_ground = false;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y + jump_power*2);
        yield return new WaitForSecondsRealtime(0.25f);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.4f);
    }

    IEnumerator Down_out()
    {
        is_down_out = true;
        now_act_coroutine = null;
        yield return new WaitForSecondsRealtime(0.5f);
        is_jump_cool = false;
        is_down_atk = false;
        charge_down_atk = false;
        is_down_out = false;
    }

    IEnumerator Parrying()
    {
        if (now_act_coroutine != null)
        {
            StopCoroutine(now_act_coroutine);
        }
        is_parrying_atk = false;
        parrying_success = false;
        is_parrying_cool = true;
        is_parrying = true;
        animator.SetBool("parrying_wait", true);
        animator.SetTrigger("parrying");
        yield return new WaitForSecondsRealtime(0.5f);
        is_parrying = false;
        if (!parrying_success)
        {
            animator.SetBool("parrying_wait", false);
            yield return new WaitForSecondsRealtime(3f);
            is_parrying_cool = false;
            now_act_coroutine=null;
        }
    }

    IEnumerator Parrying_success()
    {
        if (now_act_coroutine != null)
        {
            StopCoroutine(now_act_coroutine);
        }
        is_parrying = false;
        animator.SetTrigger("P_S");
        if (hitted_right)
        {
            rb.linearVelocity = new Vector2(-3,rb.linearVelocity.y);
            for (int a = 40; a > 0; a--)
            {
                GameObject f;
                f = Instantiate(fire_work, new Vector3(this.transform.position.x + 1.3f, this.transform.position.y, -1), Quaternion.identity, this.transform);
                f.transform.localScale = new Vector3(-1 * f.transform.localScale.x, f.transform.localScale.y, 6);
                f.SetActive(true);
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(3, rb.linearVelocity.y);
            for (int a = 40; a > 0; a--)
            {
                GameObject f;
                f = Instantiate(fire_work, new Vector3(this.transform.position.x - 1.3f, this.transform.position.y, -1), Quaternion.identity, this.transform);
                f.SetActive(true);
            }
        }
        yield return new WaitForSecondsRealtime(0.5f);
        charge_parrying_atk = true;
        yield return new WaitForSecondsRealtime(1.75f);
        if (!is_parrying_atk && !is_parrying)
        {
            animator.SetBool("parrying_wait", false);
            charge_parrying_atk = false;
            is_parrying_cool = false;
            now_act_coroutine = null;
            parrying_success = false;
        }
    }

    IEnumerator Parrying_atk()
    {
        if (now_act_coroutine != null)
        {
            StopCoroutine(now_act_coroutine);
        }
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        is_parrying_atk = true;
        charge_parrying_atk = false;
        animator.SetTrigger("parrying_atk");
        GameObject pa;
        if (animator.transform.localScale.x>0)
        {
            pa = Instantiate(PA_, new Vector3(this.transform.position.x + 2.5f, this.transform.position.y, 0), Quaternion.identity, this.transform);
        }
        else
        {
            pa = Instantiate(PA_, new Vector3(this.transform.position.x - 2.5f, this.transform.position.y, 0), Quaternion.identity, this.transform);
        }
        pa.transform.localScale = new Vector3(5, 3);
        pa.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);
        Destroy(pa);
        yield return new WaitForSecondsRealtime(0.2f);
        int first_power = power;
        power = power * 2;
        if (animator.transform.localScale.x > 0)
        {
            pa = Instantiate(PA_, new Vector3(this.transform.position.x + 3.5f, this.transform.position.y, 0), Quaternion.identity, this.transform);
        }
        else
        {
            pa = Instantiate(PA_, new Vector3(this.transform.position.x - 3.5f, this.transform.position.y, 0), Quaternion.identity, this.transform);
        }
        pa.transform.localScale = new Vector3(7, 3);
        pa.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        Destroy(pa);
        power = first_power;
        yield return new WaitForSecondsRealtime(0.6f);
        animator.SetBool("parrying_wait", false);
        now_act_coroutine = null;
        is_parrying_cool = false;
        parrying_success = false;
        is_parrying_atk = false;
    }

    IEnumerator Down_atk()
    {
        is_down_atk = true;
        animator.SetTrigger("down_atk");
        rb.linearVelocity = new Vector2 (rb.linearVelocity.x, rb.linearVelocity.y - jump_power*2);
        yield return new WaitUntil(()=>on_ground);
        animator.ResetTrigger("down_atk");
        is_down_atk = false;
        charge_down_atk = false;
        GameObject pa = Instantiate(PA_, this.transform.position, Quaternion.identity, this.transform);
        pa.transform.localScale = new Vector3(12, 6);
        pa.SetActive(true);
        for (int a = 20; a > 0; a--)
        {
            GameObject f;
            if (animator.transform.localScale.x > 0)
            {
                f = Instantiate(fire_work, new Vector3(this.transform.position.x + 1, this.transform.position.y - 2, -1), Quaternion.identity, this.transform);
                f.transform.localScale = new Vector3(-1 * f.transform.localScale.x, f.transform.localScale.y, 6);
            }
            else
            {
                f = Instantiate(fire_work, new Vector3(this.transform.position.x-1, this.transform.position.y - 2, -1), Quaternion.identity, this.transform);
            }
            f.SetActive(true);
        }
        yield return new WaitForSecondsRealtime(0.3f);
        Destroy(pa);
    }

    IEnumerator Nomal_atk()
    {
        is_nomal_atk = true;
        is_nomal_atk_cool = true;
        animator.SetTrigger("nomal_atk");
        yield return new WaitForSecondsRealtime(1.5f);
        is_nomal_atk = false;
        now_act_coroutine=null;
        GameObject pa;
        if (animator.transform.localScale.x > 0)
        {
            pa = Instantiate(PA_, new Vector3(this.transform.position.x + 4, this.transform.position.y,0), Quaternion.identity, this.transform);
        }
        else
        {
            pa = Instantiate(PA_, new Vector3(this.transform.position.x - 4, this.transform.position.y, 0), Quaternion.identity, this.transform);
        }
        pa.transform.localScale = new Vector3(8, 3);
        pa.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);
        Destroy(pa);
        yield return new WaitForSecondsRealtime(0.5f);
        is_nomal_atk_cool = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Tile"))
        {
            on_ground = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Tile"))
        {
            on_ground = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        hp_animator.SetInteger("hp", hp);

        if ((Time.timeScale == 0 && !animator.GetBool("cut_wait")) && !(damaged && hp <= 0))
        {
            animator.SetBool("cut_wait", true);
            animator.SetTrigger("cut");
        }
        else if (Time.timeScale != 0 && animator.GetBool("cut_wait"))
        {
            animator.SetBool("cut_wait", false);
        }

        if (Time.timeScale != 0)
        {
            animator.SetBool("down_atk_charge", charge_down_atk);

            if (!on_ground && ((int)rb.linearVelocity.y) < 0)
            {
                if (!animator.GetBool("is_down"))
                {
                    StartCoroutine(Down());
                    animator.SetBool("is_down", true);
                    animator.SetTrigger("down");
                }
            }
            else if (!(!on_ground && animator.GetBool("is_down")))
            {
                animator.SetBool("is_down", false);
            }

            if (((int)rb.linearVelocity.x) > 0 || ((int)rb.linearVelocity.x) < 0)
            {
                animator.SetBool("move", true);
            }
            else
            {
                animator.SetBool("move", false);
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("PLAYER_3_idle"))
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
            hp = 4;
        }
        if (!stop_fixed)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                if (!is_jump_cool && now_act_coroutine == null)
                {
                    now_act_coroutine = StartCoroutine(Jump());
                }
            }

            if (Input.GetKey(KeyCode.Space))
            {
                if ((on_ground && !is_down_out) && !is_parrying_cool)
                {
                    if (now_act_coroutine != null)
                    {
                        StopCoroutine(now_act_coroutine);
                    }
                    is_jump_cool = false;
                    is_down_atk = false;
                    charge_down_atk = false;
                    is_nomal_atk = false;
                    is_nomal_atk_cool = false;
                    now_act_coroutine = StartCoroutine(Parrying());
                }
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                if (charge_down_atk && !is_down_atk)
                {
                    StartCoroutine(Down_atk());
                }
            }

            if (Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.Z))
            {
                if (charge_parrying_atk)
                {
                    StartCoroutine(Parrying_atk());
                }
                else if(!parrying_success)
                {
                    if (!is_nomal_atk_cool && now_act_coroutine == null)
                    {
                        now_act_coroutine = StartCoroutine(Nomal_atk());
                    }
                }
            }

            Debug.DrawRay(new Vector2(this.transform.position.x - 1.5f, this.transform.position.y + 1.4f), Vector2.down * 3.6f);
            Debug.DrawRay(new Vector2(this.transform.position.x + 1.5f, this.transform.position.y + 1.4f), Vector2.down * 3.6f);
            Debug.DrawRay(new Vector2(this.transform.position.x, this.transform.position.y), Vector2.left * 1.5f);
            Debug.DrawRay(new Vector2(this.transform.position.x, this.transform.position.y), Vector2.right * 1.5f);

            if (((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))^(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))) && (!is_nomal_atk && (!(is_parrying || parrying_success)) || is_parrying_atk))
            {
                if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)))
                {
                    if (on_ground)
                    {
                        if (animator.transform.localScale.x > 0)
                        {
                            animator.transform.localScale = new Vector3(animator.transform.localScale.x * -1, animator.transform.localScale.y, animator.transform.localScale.z);
                        }
                        rb.linearVelocity = new Vector2(-1f * speed, rb.linearVelocity.y);
                    }
                    else
                    {
                        if (!is_down_atk && (!Physics2D.Raycast(new Vector2(this.transform.position.x - 1.5f, this.transform.position.y + 1.4f), Vector2.down, 3.6f, LayerMask.GetMask("Tile")) && !Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), Vector2.left, 1.5f, LayerMask.GetMask("Tile"))))
                        {
                            if (animator.transform.localScale.x > 0)
                            {
                                animator.transform.localScale = new Vector3(animator.transform.localScale.x * -1, animator.transform.localScale.y, animator.transform.localScale.z);
                            }
                            rb.linearVelocity = new Vector2(-0.7f * speed, rb.linearVelocity.y);
                        }
                    }
                }
                else
                {
                    if (on_ground)
                    {
                        if (animator.transform.localScale.x < 0)
                        {
                            animator.transform.localScale = new Vector3(animator.transform.localScale.x * -1, animator.transform.localScale.y, animator.transform.localScale.z);
                        }
                        rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
                    }
                    else
                    {
                        if (!is_down_atk && (!Physics2D.Raycast(new Vector2(this.transform.position.x + 1.5f, this.transform.position.y + 1.4f), Vector2.down, 3.6f, LayerMask.GetMask("Tile")) && !Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), Vector2.right, 1.5f, LayerMask.GetMask("Tile"))))
                        {
                            if (animator.transform.localScale.x < 0)
                            {
                                animator.transform.localScale = new Vector3(animator.transform.localScale.x * -1, animator.transform.localScale.y, animator.transform.localScale.z);
                            }
                            rb.linearVelocity = new Vector2(0.7f * speed, rb.linearVelocity.y);
                        }
                    }
                }
            }
            else if(!is_nomal_atk && !(is_parrying || parrying_success))
            {
                rb.linearVelocity = new Vector2(0,rb.linearVelocity.y);
            }
        }
    }
}
