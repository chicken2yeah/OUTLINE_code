using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PLAYER_1 : PLAYER_common
{
    public int max_speed = 30;
    public float nomal_speed = 0.3f;
    public float dash_speed = 5f;
    public int max_speed_charge = 35;
    public float speed_charge___speed = 0.25f; 
    public float default_jump_power = 7;

    public Volume kill_mode_volume;
    public GameObject fire_work;
    public GameObject _PA;
    public Animator hp_animator;
    // 100% = 70 height
    public RectTransform charge_bar_rect;

    float one_per_charge_height;
    float speed_charge = 0;
    bool is_jump_cool = false;
    bool is_nomal_atk_cool = false;
    Vector3 killing_atk_position;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PH") // Player_Hit
        {
            if (!damaged && !stop_fixed)
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
        uI_Manager.ui_group.SetActive(false);
        dead_background.SetActive(true);
        animator.SetTrigger("dead");
        yield return new WaitForSecondsRealtime(1.5f);
        uI_Manager.scenes_name = "game_over";
        StartCoroutine(uI_Manager.Change_scenes());
    }

    IEnumerator Jump()
    {
        is_jump_cool = true;
        animator.SetTrigger("jump");
        float jump_index; // 기본값 + (속도/3)
        if (rb.linearVelocity.x > 0)
        {
            jump_index = default_jump_power + (rb.linearVelocity.x / 3);
        }
        else
        {
            jump_index = default_jump_power - (rb.linearVelocity.x / 3);
        }
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y + jump_index + (speed_charge/3));
        speed_charge = 0;
        yield return new WaitUntil(()=>on_ground);
        is_jump_cool = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        one_per_charge_height = 70 / max_speed_charge;
    }

    IEnumerator Nomal_atk()
    {
        is_nomal_atk_cool = true;
        animator.SetTrigger("nomal_atk");
        yield return new WaitForSecondsRealtime(0.66f);
        GameObject p_a_;
        p_a_ = Instantiate(_PA,new Vector3(0,0),Quaternion.identity,this.transform);
        if (animator.transform.localScale.x > 0)
        {
            p_a_.transform.position = new Vector3(this.transform.position.x + 2.5f,this.transform.position.y,this.transform.position.z);
            p_a_.transform.localScale = new Vector3(5, 3, 1);
        }
        else
        {
            p_a_.transform.position = new Vector3(this.transform.position.x - 2.5f, this.transform.position.y, this.transform.position.z);
            p_a_.transform.localScale = new Vector3(-5, 3, 1);
        }
        attack_effect a = p_a_.GetComponent<attack_effect>();
        a.atk_mode = 2;
        p_a_.SetActive(true);
        yield return new WaitForSecondsRealtime(0.14f);
        p_a_ = null;
        a = null;
        yield return new WaitForSecondsRealtime(0.2f);
        p_a_ = Instantiate(_PA, new Vector3(0, 0), Quaternion.identity, this.transform);
        if (animator.transform.localScale.x > 0)
        {
            p_a_.transform.position = new Vector3(this.transform.position.x + 2.5f, this.transform.position.y, this.transform.position.z);
            p_a_.transform.localScale = new Vector3(6, 3, 1);
        }
        else
        {
            p_a_.transform.position = new Vector3(this.transform.position.x - 2.5f, this.transform.position.y, this.transform.position.z);
            p_a_.transform.localScale = new Vector3(-6, 3, 1);
        }
        a = p_a_.GetComponent<attack_effect>();
        a.atk_mode = 3;
        p_a_.SetActive(true);
        yield return new WaitForSecondsRealtime(0.1f);
        p_a_ = null;
        a = null;
        yield return new WaitForSecondsRealtime(1.2f);
        is_nomal_atk_cool = false;
    }

    IEnumerator Kill_atk()
    {
        stop_fixed = true;
        int count = (int)(rb.linearVelocity.x/3);
        killing_atk_position = this.transform.position;
        kill_mode_volume.gameObject.SetActive(true);
        kill_mode_volume.weight = 0;
        if (count < 0)
        {
            count *= -1;
        }
        animator.SetTrigger("kill_mode");
        rb.linearVelocity = new Vector2(0,0);
        if (rb.linearVelocity.x < 0)
        {
            for (int a = 0; a < ((int)(rb.linearVelocity.x)); a++)
            {
                GameObject f;
                f = Instantiate(fire_work, new Vector3(this.transform.position.x + 1.0f, this.transform.position.y - 2f, -1), Quaternion.identity, this.transform);
                f.SetActive(true);
            }
        }
        else
        {
            for (int a = 0; a > ((int)(rb.linearVelocity.x)); a--)
            {
                GameObject f;
                f = Instantiate(fire_work, new Vector3(this.transform.position.x - 1.0f, this.transform.position.y - 2f, -1), Quaternion.identity, this.transform);
                f.transform.localScale = new Vector3(-1 * f.transform.localScale.x, f.transform.localScale.y, 6);
                f.SetActive(true);
            }
        }
        yield return new WaitForSecondsRealtime(0.55f);
        for (int a = 0; a<count; a++)
        {
            kill_mode_volume.weight = 1;
            GameObject p_a_;
            p_a_ = Instantiate(_PA, killing_atk_position, Quaternion.identity);
            if (a % 2 != 1)
            {
                if (animator.transform.localScale.x < 0)
                {
                    animator.transform.localScale = new Vector3(animator.transform.localScale.x * -1,animator.transform.localScale.y,animator.transform.localScale.z);
                }
                this.transform.position = killing_atk_position;
                animator.SetTrigger("kill_atk");
                rb.linearVelocity = new Vector2(-20,0);
                p_a_.transform.localScale = new Vector3(20,3,1);
            }
            else
            {
                if (animator.transform.localScale.x > 0)
                {
                    animator.transform.localScale = new Vector3(animator.transform.localScale.x * -1, animator.transform.localScale.y, animator.transform.localScale.z);
                }
                this.transform.position = killing_atk_position;
                animator.SetTrigger("kill_atk");
                rb.linearVelocity = new Vector2(20, 0);
                p_a_.transform.localScale = new Vector3(-20, 3, 1);
            }
            attack_effect atk_ = p_a_.GetComponent<attack_effect>();
            atk_.atk_mode = (int)(Random.Range(1,4));
            p_a_.SetActive(true);
            yield return new WaitForSecondsRealtime(0.25f);
            kill_mode_volume.weight = 0.3f;
            rb.linearVelocity = new Vector2(0,0);
        }
        kill_mode_volume.weight = 0f;
        kill_mode_volume.gameObject.SetActive(false);
        animator.SetTrigger("kill_mode_out");
        GameObject p_a;
        p_a = Instantiate(_PA, new Vector3(0, 0), Quaternion.identity, this.transform);
        if (animator.transform.localScale.x > 0)
        {
            p_a.transform.position = new Vector3(this.transform.position.x + 2.5f, this.transform.position.y, this.transform.position.z);
            p_a.transform.localScale = new Vector3(6, 3, 1);
            rb.linearVelocity = new Vector2(-5,0);
        }
        else
        {
            p_a.transform.position = new Vector3(this.transform.position.x - 2.5f, this.transform.position.y, this.transform.position.z);
            p_a.transform.localScale = new Vector3(-6, 3, 1);
            rb.linearVelocity = new Vector2(5, 0);
        }
        attack_effect atk = p_a.GetComponent<attack_effect>();
        atk.atk_mode = 3;
        p_a.SetActive(true);
        yield return new WaitForSecondsRealtime(1);
        stop_fixed = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(this.transform.position, Vector2.down * 2.1f);
        on_ground = Physics2D.Raycast(this.transform.position, Vector2.down, 2.1f, LayerMask.GetMask("Tile"));

        //비축된 속도 표시
        charge_bar_rect.sizeDelta = new Vector2(charge_bar_rect.sizeDelta.x, speed_charge * one_per_charge_height);

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
            if (on_ground)
            {
                animator.SetFloat("speed", rb.linearVelocity.x);

                if (animator.GetBool("is_down"))
                {
                    animator.SetBool("is_down",false);
                }

                if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
                {
                    if (rb.linearVelocity.x != 0)
                    {
                        if (!animator.GetBool("break_wait"))
                        {
                            animator.SetBool("break_wait", true);
                            animator.SetTrigger("break");
                        }
                    }
                    else
                    {

                        //비축 상태 활성
                        if (speed_charge != 0)
                        {
                            if (!animator.GetBool("charge_wait"))
                            {
                                animator.SetBool("charge_wait", true);
                                animator.SetTrigger("charge");
                            }
                        }
                    }
                }
                else
                {
                    //급감속 상태 해제
                    if (animator.GetBool("break_wait"))
                    {
                        animator.SetBool("break_wait", false);
                    }
                    //비축 상태 해제
                    if (animator.GetBool("charge_wait"))
                    {
                        animator.SetBool("charge_wait", false);
                    }
                }
            }
            else
            {
                if (rb.linearVelocity.y < 0)
                {
                    animator.SetBool("is_down", true);
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PLAYER_1_jump") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PLAYER_1_down"))
                    {
                        animator.SetTrigger("down");
                    }
                }
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("PLAYER_1_idle"))
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

            animator.SetInteger("dance",dance_num);
        }
    }

    private void FixedUpdate()
    {
        if (full_hp_mode)
        {
            hp = 5;
        }
        if (!stop_fixed)
        {
            if ((Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.Z)) && (on_ground && !is_nomal_atk_cool))
            {
                if ((rb.linearVelocity.x > max_speed*0.66f || rb.linearVelocity.x < max_speed*-0.66f) && Input.GetKey(KeyCode.Space))
                {
                    StartCoroutine(Kill_atk());
                }
                else
                {
                    StartCoroutine(Nomal_atk());
                }
            }
            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && (on_ground && !is_jump_cool))
            {
                StartCoroutine(Jump());
            }
            if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && on_ground)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    if (speed_charge < max_speed_charge)
                    {
                        speed_charge += speed_charge___speed;
                    }
                }
                if (rb.linearVelocity.x > 0.1f || rb.linearVelocity.x < -0.1f)
                {
                    if (rb.linearVelocity.x > 0)
                    {
                        for (int a = 0; a < ((int)(rb.linearVelocity.x)); a++)
                        {
                            GameObject f;
                            f = Instantiate(fire_work, new Vector3(this.transform.position.x + 1.0f, this.transform.position.y - 2f, -1), Quaternion.identity, this.transform);
                            f.SetActive(true);
                        }
                    }
                    else
                    {
                        for (int a = 0; a > ((int)(rb.linearVelocity.x)); a--)
                        {
                            GameObject f;
                            f = Instantiate(fire_work, new Vector3(this.transform.position.x - 1.0f, this.transform.position.y - 2f, -1), Quaternion.identity, this.transform);
                            f.transform.localScale = new Vector3(-1 * f.transform.localScale.x, f.transform.localScale.y, 6);
                            f.SetActive(true);
                        }
                    }
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x * 0.9f, rb.linearVelocity.y);
                }
                else
                {
                    rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                }
            }
            else if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))^(Input.GetKey(KeyCode.D)||Input.GetKey(KeyCode.RightArrow)))
            {
                if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
                {
                    if (animator.transform.localScale.x < 0)
                    {
                        animator.transform.localScale = new Vector3(animator.transform.localScale.x * -1, animator.transform.localScale.y, animator.transform.localScale.z);
                    }
                    if (on_ground)
                    {
                        if (rb.linearVelocity.x == 0)
                        {
                            rb.linearVelocity = new Vector2(rb.linearVelocity.x + nomal_speed *10, rb.linearVelocity.y);
                        }
                        rb.linearVelocity = new Vector2(rb.linearVelocity.x + speed_charge + nomal_speed, rb.linearVelocity.y);
                        speed_charge = 0;
                    }
                    else
                    {
                        rb.linearVelocity = new Vector2(rb.linearVelocity.x + (nomal_speed * 0.7f) , rb.linearVelocity.y);
                    }
                }
                else
                {
                    if (animator.transform.localScale.x > 0)
                    {
                        animator.transform.localScale = new Vector3(animator.transform.localScale.x * -1, animator.transform.localScale.y, animator.transform.localScale.z);
                    }
                    if (on_ground)
                    {
                        if (rb.linearVelocity.x == 0)
                        {
                            rb.linearVelocity = new Vector2(rb.linearVelocity.x - nomal_speed * 10, rb.linearVelocity.y);
                        }
                        rb.linearVelocity = new Vector2(rb.linearVelocity.x - speed_charge - nomal_speed, rb.linearVelocity.y);
                        speed_charge = 0;
                    }
                    else
                    {
                        rb.linearVelocity = new Vector2(rb.linearVelocity.x - (nomal_speed * 0.7f), rb.linearVelocity.y);
                    }
                }
            }

            if (rb.linearVelocity.x > max_speed)
            {
                if ((rb.linearVelocity.x - max_speed) < nomal_speed*3)
                {
                    rb.linearVelocity = new Vector2(max_speed, rb.linearVelocity.y);
                }
                else
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x - (nomal_speed * 3), rb.linearVelocity.y);
                }
            }else if (rb.linearVelocity.x < -1*max_speed)
            {
                if ((rb.linearVelocity.x + max_speed) > nomal_speed * -3)
                {
                    rb.linearVelocity = new Vector2(-1*max_speed, rb.linearVelocity.y);
                }
                else
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x + (nomal_speed * 3), rb.linearVelocity.y);
                }
            }
        }
    }
}
