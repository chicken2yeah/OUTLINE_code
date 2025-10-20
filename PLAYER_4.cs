using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class PLAYER_4 : PLAYER_common
{
    public float speed = 7f;
    public float jump_power = 15f;
    public int max_hp = 10;

    public Animator hp_animator;
    public GameObject PA_;
    public Light2D[] arm_light; 

    bool meet_slope = false;
    bool atk_cool = false;
    bool jump_cool = false;
    int mode = 1;
    bool atk_n_1 = true;
    bool hp_down = false;
    int bonus_atk = 0;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PH") // Player_Hit
        {
            if (!damaged)
            {
                hitted_right = collision.transform.position.x > this.transform.position.x;
                if (hp <= 1)
                {
                    if (full_hp_mode)
                    {
                        hp = 10;
                    }
                    else
                    {
                        StartCoroutine(Player_dead());
                    }
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
        bonus_atk = 0;
        uI_Manager.Stop_fixed();
        damaged = true;
        uI_Manager.ui_group.SetActive(false);
        dead_background.SetActive(true);
        animator.SetTrigger("dead");
        yield return new WaitForSecondsRealtime(3f);
        uI_Manager.scenes_name = "game_over";
        StartCoroutine(uI_Manager.Change_scenes());
    }

    IEnumerator Atk()
    {
        atk_cool = true;
        switch (mode)
        {
            case (1):
                {
                    if (atk_n_1)
                    {
                        animator.SetTrigger("atk_1_1");
                    }
                    else
                    {
                        animator.SetTrigger("atk_1_2");
                    }

                    GameObject p_a_;
                    p_a_ = Instantiate(PA_, new Vector3(0, 0), Quaternion.identity, this.transform);
                    if (animator.transform.localScale.x > 0)
                    {
                        p_a_.transform.position = new Vector3(this.transform.position.x + 5f, this.transform.position.y - 0.6f, this.transform.position.z);
                        p_a_.transform.localScale = new Vector3(5, 3, 1);
                    }
                    else
                    {
                        p_a_.transform.position = new Vector3(this.transform.position.x - 5f, this.transform.position.y - 0.6f, this.transform.position.z);
                        p_a_.transform.localScale = new Vector3(-5, 3, 1);
                    }
                    attack_effect a = p_a_.GetComponent<attack_effect>();
                    a.atk_mode = 1;
                    p_a_.SetActive(true);
                    if (bonus_atk > 0)
                    {
                        bonus_atk -= 1;
                        if ((int)(Random.Range(1f,3f)) == 1)
                        {
                            hp += 1;
                        }
                        yield return new WaitForSecondsRealtime(0.6f);
                    }
                    else
                    {
                        yield return new WaitForSecondsRealtime(0.8f);
                    }
                    
                    break;
                }
            case (2):
                {
                    if (atk_n_1)
                    {
                        animator.SetTrigger("atk_2_1");
                    }
                    else
                    {
                        animator.SetTrigger("atk_2_2");
                    }

                    GameObject p_a_;
                    p_a_ = Instantiate(PA_, new Vector3(0, 0), Quaternion.identity, this.transform);
                    if (animator.transform.localScale.x > 0)
                    {
                        p_a_.transform.position = new Vector3(this.transform.position.x + 5f, this.transform.position.y - 0.6f, this.transform.position.z);
                        p_a_.transform.localScale = new Vector3(6, 3, 1);
                    }
                    else
                    {
                        p_a_.transform.position = new Vector3(this.transform.position.x - 5f, this.transform.position.y - 0.6f, this.transform.position.z);
                        p_a_.transform.localScale = new Vector3(-6, 3, 1);
                    }
                    attack_effect a = p_a_.GetComponent<attack_effect>();
                    a.atk_mode = 2;
                    p_a_.SetActive(true);
                    if (bonus_atk > 0)
                    {
                        bonus_atk -= 1;
                        if ((int)(Random.Range(1f, 3f)) == 1)
                        {
                            hp += 1;
                        }
                        yield return new WaitForSecondsRealtime(0.4f);
                    }
                    else
                    {
                        yield return new WaitForSecondsRealtime(0.6f);
                    }

                    break;
                }
            case (3):
                {
                    if (atk_n_1)
                    {
                        animator.SetTrigger("atk_3_1");
                    }
                    else
                    {
                        animator.SetTrigger("atk_3_2");
                    }

                    GameObject p_a_;
                    p_a_ = Instantiate(PA_, new Vector3(0, 0), Quaternion.identity, this.transform);
                    if (animator.transform.localScale.x > 0)
                    {
                        p_a_.transform.position = new Vector3(this.transform.position.x + 5f, this.transform.position.y - 1f, this.transform.position.z);
                        p_a_.transform.localScale = new Vector3(7, 3, 1);
                    }
                    else
                    {
                        p_a_.transform.position = new Vector3(this.transform.position.x - 5f, this.transform.position.y - 1f, this.transform.position.z);
                        p_a_.transform.localScale = new Vector3(-7, 3, 1);
                    }
                    attack_effect a = p_a_.GetComponent<attack_effect>();
                    a.atk_mode = 3;
                    p_a_.SetActive(true);
                    if (bonus_atk > 0)
                    {
                        bonus_atk -= 1;
                        if ((int)(Random.Range(1f, 3f)) == 1)
                        {
                            hp += 1;
                        }
                        yield return new WaitForSecondsRealtime(0.1f);
                    }
                    else
                    {
                        yield return new WaitForSecondsRealtime(0.3f);
                    }

                    break;
                }
        }
        atk_n_1 = !atk_n_1;
        atk_cool = false;
    }

    IEnumerator Jump()
    {
        jump_cool = true;
        animator.SetTrigger("jump");
        int first_p = power;
        power += (int)(bonus_atk/2);
        bonus_atk = 0;
        GameObject p_a_;
        p_a_ = Instantiate(PA_, new Vector3(0, 0), Quaternion.identity, this.transform);
        if (animator.transform.localScale.x > 0)
        {
            p_a_.transform.position = new Vector3(this.transform.position.x + 2f, this.transform.position.y , this.transform.position.z);
            p_a_.transform.rotation = Quaternion.Euler(0,0,90);
            p_a_.transform.localScale = new Vector3(-4, 5, 1);
        }
        else
        {
            p_a_.transform.position = new Vector3(this.transform.position.x - 2f, this.transform.position.y, this.transform.position.z);
            p_a_.transform.rotation = Quaternion.Euler(0, 0, -90);
            p_a_.transform.localScale = new Vector3(4, 5, 1);
        }
        attack_effect a = p_a_.GetComponent<attack_effect>();
        a.atk_mode = 3;
        p_a_.SetActive(true);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump_power);
        yield return new WaitForSecondsRealtime(0.1f);
        power = first_p;
        yield return new WaitUntil(()=>on_ground);
        jump_cool = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hp > max_hp)
        {
            hp = max_hp;
        }

        if (hp > ((max_hp / 3) * 2))
        {
            mode = 1;
        }
        else if (hp > (max_hp/3))
        {
            mode = 2;
        }
        else if (hp > 0)
        {
            mode = 3;
        }
        for (int a = 0; a < arm_light.Length; a++)
        {
            arm_light[a].intensity = (float)bonus_atk/5;
        }

        if ((Input.GetKeyUp(KeyCode.S) || (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.DownArrow))))
        {
            hp_down = false;
        }
        hp_animator.SetInteger("hp", hp);
        on_ground = Physics2D.Raycast(this.transform.position, Vector2.down, 2.1f, LayerMask.GetMask("Tile"));
        Debug.DrawRay(this.transform.position, Vector2.down * 2.1f);
        Debug.DrawRay(new Vector2(this.transform.position.x - 0.7f, this.transform.position.y), Vector2.down * 1.9f);
        Debug.DrawRay(new Vector2(this.transform.position.x + 0.7f, this.transform.position.y), Vector2.down * 1.9f);
        if (animator.transform.localScale.x > 0)
        {
            meet_slope = Physics2D.Raycast(new Vector2(this.transform.position.x + 0.7f, this.transform.position.y), Vector2.down, 1.9f, LayerMask.GetMask("Tile"));
            if (Physics2D.Raycast(new Vector2(this.transform.position.x - 0.7f, this.transform.position.y), Vector2.down, 1.9f, LayerMask.GetMask("Tile")))
            {
                on_ground = true;
            }
            else if (meet_slope)
            {
                on_ground = true;
            }
        }
        else
        {
            meet_slope = Physics2D.Raycast(new Vector2(this.transform.position.x - 0.7f, this.transform.position.y), Vector2.down, 1.9f, LayerMask.GetMask("Tile"));
            if (Physics2D.Raycast(new Vector2(this.transform.position.x + 0.7f, this.transform.position.y), Vector2.down, 1.9f, LayerMask.GetMask("Tile")))
            {
                on_ground = true;
            }
            else if (meet_slope)
            {
                on_ground = true;
            }
        }

        if ((Time.timeScale == 0 && !animator.GetBool("cut_wait")) && !(damaged && hp <= 0))
        {
            bonus_atk = 0;
            animator.SetBool("cut_wait", true);
            animator.SetTrigger("cut");
        }
        else if (Time.timeScale != 0 && animator.GetBool("cut_wait"))
        {
            animator.SetBool("cut_wait", false);
        }

        if (Time.timeScale != 0)
        {
            if (((int)rb.linearVelocity.x) != 0)
            {
                animator.SetBool("move", true);
            }
            else
            {
                animator.SetBool("move", false);
            }

            if (!on_ground && ((int)rb.linearVelocity.y) < 0)
            {
                if (!animator.GetBool("is_down"))
                {
                    animator.SetBool("is_down", true);
                    animator.SetTrigger("down");
                }
            }
            else if (!(!on_ground && animator.GetBool("is_down")))
            {
                animator.SetBool("is_down", false);
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("PLAYER_4_idle"))
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
        if (!stop_fixed)
        {
            if ((Input.GetKey(KeyCode.S) || (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.DownArrow))) && (!hp_down&&!jump_cool))
            {
                if (hp > 1)
                {
                    hp -= 1;
                    bonus_atk += 2;
                    hp_down = true;
                }
            }

            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && !jump_cool)
            {
                StartCoroutine(Jump());
            }

            if ((Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.Z)) && (!atk_cool&&!jump_cool))
            {
                StartCoroutine(Atk());
            }
            Debug.DrawRay(new Vector2(this.transform.position.x - 1.5f, this.transform.position.y + 1.5f), Vector2.down * 3.2f);
            Debug.DrawRay(new Vector2(this.transform.position.x + 1.5f, this.transform.position.y + 1.5f), Vector2.down * 3.2f);
            Debug.DrawRay(new Vector2(this.transform.position.x, this.transform.position.y), Vector2.left * 1.5f);
            Debug.DrawRay(new Vector2(this.transform.position.x, this.transform.position.y), Vector2.right * 1.5f);
            if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))^(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
            {
                if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && (!Physics2D.Raycast(new Vector2(this.transform.position.x - 1.5f, this.transform.position.y + 1.5f), Vector2.down, 3.2f, LayerMask.GetMask("Tile")) && !Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), Vector2.left, 1.5f, LayerMask.GetMask("Tile"))))
                {
                    if (animator.transform.localScale.x > 0 && !atk_cool)
                    {
                        animator.transform.localScale = new Vector3(animator.transform.localScale.x * -1, animator.transform.localScale.y, animator.transform.localScale.z);
                    }

                    if (meet_slope)
                    {
                        rb.linearVelocity = new Vector2(-0.5f * speed, rb.linearVelocity.y);
                    }
                    else if (atk_cool)
                    {
                        rb.linearVelocity = new Vector2(-0.2f * speed, rb.linearVelocity.y);
                    }
                    else if(on_ground)
                    {
                        rb.linearVelocity = new Vector2(-1 * speed, rb.linearVelocity.y);
                    }
                    else
                    {
                        rb.linearVelocity = new Vector2(-0.8f * speed, rb.linearVelocity.y);
                    }
                }
                else if(((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && (!Physics2D.Raycast(new Vector2(this.transform.position.x + 1.5f, this.transform.position.y + 1.5f), Vector2.down, 3.2f, LayerMask.GetMask("Tile")) && !Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), Vector2.right, 1.5f, LayerMask.GetMask("Tile"))))) 
                {
                    if (animator.transform.localScale.x < 0 && !atk_cool)
                    {
                        animator.transform.localScale = new Vector3(animator.transform.localScale.x * -1, animator.transform.localScale.y, animator.transform.localScale.z);
                    }

                    if (meet_slope)
                    {
                        rb.linearVelocity = new Vector2(0.5f * speed, rb.linearVelocity.y);
                    }
                    else if(atk_cool)
                    {
                        rb.linearVelocity = new Vector2(0.2f * speed, rb.linearVelocity.y);
                    }
                    else if(on_ground)
                    {
                        rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
                    }
                    else
                    {
                        rb.linearVelocity = new Vector2(0.8f * speed, rb.linearVelocity.y);
                    }
                }
            }
            else if (on_ground)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
        }
    }
}
