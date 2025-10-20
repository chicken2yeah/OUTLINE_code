using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class GUARDIAN : MonoBehaviour
{
    public int hp = 250;
    public boss_hp_bar hp_bar;
    public Animator guardian_animator;
    public GameObject ph_obj;
    public Rigidbody2D guardian_rb;
    public LineRenderer shot_alarm_lineRenderer;
    public LineRenderer shot_lineRenderer;
    public GameObject[] ph_alarms;
    public EdgeCollider2D shot_collider;
    public AudioSource hurt_source;
    public PLAYER_common player;
    public float min_cooltime = 0.5f;
    public float max_cooltime = 1f;
    public UI_manager uI_manager;

    public PlayableDirector stage_1_end;

    public bool atk_cool = false;
    GameObject player_obj;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PA")
        {
            hurt_source.Play();
            hp -= player.power;
            if (hp <= 0)
            {
                StartCoroutine(Stage_1_end());
            }
        }
    }

    IEnumerator Stage_1_end()
    {
        guardian_animator.SetTrigger("end");
        uI_manager.Run_fade_in_faster();
        yield return new WaitForSecondsRealtime(3f);
        stage_1_end.Play();
    }

    IEnumerator Shot()
    {
        atk_cool=true;
        Vector3 target = player_obj.transform.position;
        guardian_animator.SetBool("out_wait", true);
        guardian_animator.SetTrigger("shot");
        yield return new WaitForSecondsRealtime(0.5f);
        shot_alarm_lineRenderer.gameObject.SetActive(true);
        shot_alarm_lineRenderer.startWidth = 0.31f;
        shot_alarm_lineRenderer.endWidth = 0.31f;
        shot_lineRenderer.startWidth = 0.3f;
        shot_lineRenderer.endWidth = 0.3f;
        shot_alarm_lineRenderer.positionCount = 2;
        shot_lineRenderer.positionCount = 2;
        if (guardian_animator.transform.localScale.x > 0)
        {
            for (int repeat = 0; repeat < 100; repeat += 1)
            {
                if (player_obj.transform.position.x - 5 < this.transform.position.x)
                {
                    break;
                }
                shot_alarm_lineRenderer.startWidth -= 0.003f;
                shot_alarm_lineRenderer.endWidth -= 0.003f;
                target = player_obj.transform.position;
                shot_alarm_lineRenderer.SetPositions(new Vector3[] { new Vector3(this.transform.position.x + 2, this.transform.position.y, 1), target });
                yield return new WaitForSecondsRealtime(0.02f);
            }
            if (!(player_obj.transform.position.x - 7 < this.transform.position.x))
            {
                yield return new WaitForSecondsRealtime(0.5f);
            }
            shot_alarm_lineRenderer.gameObject.SetActive(false);
            shot_lineRenderer.gameObject.SetActive(true);
            float x = target.x - (this.transform.position.x + 2);
            float y = target.y - (this.transform.position.y);
            float newY = y / x;
            shot_lineRenderer.SetPositions(new Vector3[] { new Vector3(this.transform.position.x + 2, this.transform.position.y, 1), new Vector3(target.x + 100, target.y + (100 * newY), 0) });
            shot_collider.points = new Vector2[] { new Vector2(this.transform.position.x + 2, this.transform.position.y), new Vector2(target.x + 100, target.y + (100 * newY)) };

            guardian_animator.SetBool("out_wait", false);
            for (int repeat = 0; repeat < 10; repeat += 1)
            {
                shot_lineRenderer.startWidth += 0.1f;
                shot_lineRenderer.endWidth += 0.1f;
                yield return new WaitForSecondsRealtime(0.01f);
            }
            for (int repeat = 0; repeat < 20; repeat += 1)
            {
                shot_lineRenderer.startWidth -= 0.05f;
                shot_lineRenderer.endWidth -= 0.05f;
                yield return new WaitForSecondsRealtime(0.02f);
            }
            shot_lineRenderer.gameObject.SetActive(false);
        }
        else
        {
            for (int repeat = 0; repeat < 100; repeat += 1)
            {
                if (player_obj.transform.position.x + 5 > this.transform.position.x)
                {
                    break;
                }
                shot_alarm_lineRenderer.startWidth -= 0.002f;
                shot_alarm_lineRenderer.endWidth -= 0.002f;
                target = player_obj.transform.position;
                shot_alarm_lineRenderer.SetPositions(new Vector3[] { new Vector3(this.transform.position.x - 2, this.transform.position.y, 1), target });
                yield return new WaitForSecondsRealtime(0.02f);
            }
            if (!(player_obj.transform.position.x + 7 > this.transform.position.x))
            {
                yield return new WaitForSecondsRealtime(0.5f);
            }

            shot_alarm_lineRenderer.gameObject.SetActive(false);
            shot_lineRenderer.gameObject.SetActive(true);
            float x = target.x - (this.transform.position.x - 2);
            float y = target.y - (this.transform.position.y);
            float newY = y / x;
            shot_lineRenderer.SetPositions(new Vector3[] { new Vector3(this.transform.position.x - 2, this.transform.position.y, 1), new Vector3(target.x - 100, target.y + (-100 * newY), 0) });
            shot_collider.points = new Vector2[] { new Vector2(this.transform.position.x - 2, this.transform.position.y), new Vector2(target.x - 100, target.y + (-100 * newY)) };

            guardian_animator.SetBool("out_wait", false);
            for (int repeat = 0; repeat < 10; repeat += 1)
            {
                shot_lineRenderer.startWidth += 0.1f;
                shot_lineRenderer.endWidth += 0.1f;
                yield return new WaitForSecondsRealtime(0.01f);
            }
            for (int repeat = 0; repeat < 20; repeat += 1)
            {
                shot_lineRenderer.startWidth -= 0.05f;
                shot_lineRenderer.endWidth -= 0.05f;
                yield return new WaitForSecondsRealtime(0.02f);
            }
            shot_lineRenderer.gameObject.SetActive(false);
        }
        yield return new WaitForSecondsRealtime(0.5f);
        float wait = Random.Range(min_cooltime,max_cooltime);
        yield return new WaitForSecondsRealtime(wait);
        atk_cool = false;
    }

    IEnumerator Slash()
    {
        atk_cool = true;
        guardian_animator.SetBool("out_wait", true);
        guardian_animator.SetTrigger("slash");
        if (guardian_animator.transform.localScale.x > 0)
        {
            for (int count = 0; count < 10; count += 1)
            {
                GameObject slash_alarm = Instantiate(ph_alarms[1], new Vector3(this.transform.position.x + 4f, this.transform.position.y + 3 - (count * 0.7f), 1), Quaternion.identity, this.transform);
                slash_alarm.transform.localScale = new Vector3(3, 0.3f, 1);
                slash_alarm.SetActive(true);
            }
            yield return new WaitForSecondsRealtime(1f);
            guardian_animator.SetBool("out_wait", false);
            GameObject slash_ph = Instantiate(ph_obj, new Vector3(this.transform.position.x + 4f, this.transform.position.y, 1), Quaternion.identity, this.transform);
            slash_ph.transform.localScale = new Vector3(7, 6, 1);
            slash_ph.SetActive(true);
            yield return new WaitForSecondsRealtime(0.25f);
            Destroy(slash_ph);
        }
        else
        {
            for (int count = 0; count < 10; count += 1)
            {
                GameObject slash_alarm = Instantiate(ph_alarms[1], new Vector3(this.transform.position.x - 4f, this.transform.position.y + 3 - (count * 0.7f), 1), Quaternion.identity, this.transform);
                slash_alarm.transform.localScale = new Vector3(3, 0.3f, 1);
                slash_alarm.SetActive(true);
            }
            yield return new WaitForSecondsRealtime(1f);
            guardian_animator.SetBool("out_wait", false);
            GameObject slash_ph = Instantiate(ph_obj, new Vector3(this.transform.position.x - 4f, this.transform.position.y, 1), Quaternion.identity, this.transform);
            slash_ph.transform.localScale = new Vector3(7, 6, 1);
            slash_ph.SetActive(true);
            yield return new WaitForSecondsRealtime(0.25f);
            Destroy(slash_ph);
        }
        yield return new WaitForSecondsRealtime(0.75f);
        float wait = Random.Range(min_cooltime, max_cooltime);
        yield return new WaitForSecondsRealtime(wait);
        atk_cool = false;
    }

    IEnumerator Jump()
    {
        atk_cool = true;
        guardian_animator.SetBool("out_wait", true);
        guardian_animator.SetTrigger("jump");
        yield return new WaitForSecondsRealtime(0.33f);
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 70, this.transform.position.z);
        guardian_rb.gravityScale = 0;
        yield return new WaitForSecondsRealtime(3f);
        guardian_rb.gravityScale = 1;
        this.transform.position = new Vector3(player_obj.transform.position.x, this.transform.position.y, this.transform.position.z);
        guardian_rb.linearVelocity = new Vector2(0, -100);
        yield return new WaitUntil(() => Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), Vector2.down, 5f, LayerMask.GetMask("Tile")));
        guardian_animator.SetBool("out_wait", false);
        GameObject jump_ph = Instantiate(ph_obj, new Vector3(this.transform.position.x, this.transform.position.y, 0), Quaternion.identity, this.transform);
        jump_ph.transform.localScale = new Vector3(50, 3, 1);
        jump_ph.SetActive(true);
        yield return new WaitForSecondsRealtime(0.05f);
        Destroy(jump_ph);
        yield return new WaitForSecondsRealtime(1f);
        float wait = Random.Range(min_cooltime, max_cooltime);
        yield return new WaitForSecondsRealtime(wait);
        atk_cool = false;
    }

    IEnumerator Dash()
    {
        atk_cool = true;
        guardian_animator.SetBool("out_wait", true);
        guardian_animator.SetTrigger("dash");
        if (guardian_animator.transform.localScale.x > 0)
        {
            float alarm_x = this.transform.position.x + 1;
            for (int t = 0; t < 50; t += 1)
            {
                GameObject alarm = Instantiate(ph_alarms[0], new Vector3(alarm_x, this.transform.position.y, 1), Quaternion.identity);
                alarm.transform.localScale = new Vector3(1, 3, 1);
                alarm.SetActive(true);
                alarm_x += 1;
                yield return new WaitForSecondsRealtime(0.02f);
            }
            GameObject dash_ph = Instantiate(ph_obj, this.transform.position, Quaternion.identity, this.transform);
            dash_ph.transform.localScale = new Vector3(1, 7, 1);
            dash_ph.SetActive(true);
            dash_ph.transform.localScale = new Vector3(5, 7, 1);
            dash_ph.SetActive(true);
            guardian_rb.linearVelocity = new Vector2(50, 0);
            yield return new WaitForSecondsRealtime(1f);
            Destroy(dash_ph);
            guardian_rb.linearVelocity = new Vector2(0, 0);
            guardian_animator.SetBool("out_wait", false);
        }
        else
        {
            float alarm_x = this.transform.position.x - 1;
            for (int t = 0; t < 50; t += 1)
            {
                GameObject alarm = Instantiate(ph_alarms[0], new Vector3(alarm_x, this.transform.position.y, 1), Quaternion.identity);
                alarm.transform.localScale = new Vector3(-1, 3, 1);
                alarm.SetActive(true);
                alarm_x -= 1;
                yield return new WaitForSecondsRealtime(0.02f);
            }
            GameObject dash_ph = Instantiate(ph_obj, this.transform.position, Quaternion.identity, this.transform);
            dash_ph.transform.localScale = new Vector3(1, 7, 1);
            dash_ph.SetActive(true);
            guardian_rb.linearVelocity = new Vector2(-50, 0);
            yield return new WaitForSecondsRealtime(1f);
            Destroy(dash_ph);
            guardian_rb.linearVelocity = new Vector2(0, 0);
            guardian_animator.SetBool("out_wait", false);
        }
        yield return new WaitForSecondsRealtime(1);
        atk_cool = false;
        float wait = Random.Range(min_cooltime, max_cooltime);
        yield return new WaitForSecondsRealtime(wait);
    }

    IEnumerator Idle()
    {
        atk_cool = true;
        float wait = Random.Range(min_cooltime, max_cooltime);
        yield return new WaitForSecondsRealtime(wait);
        atk_cool = false;
    }

    private void OnEnable()
    {
        StartCoroutine(Idle());
    }

    private void Start()
    {
        player_obj = player.gameObject;
        StartCoroutine(Idle());
    }

    private void FixedUpdate()
    {
        uI_manager.Alarm_locate_for_monster(this.transform);
        hp_bar.hp = hp;
        if (!atk_cool)
        {
            if (player.gameObject.transform.position.x > this.transform.position.x)
            {
                if (guardian_animator.transform.localScale.x < 0)
                {
                    guardian_animator.transform.localScale = new Vector3(guardian_animator.transform.localScale.x * -1,guardian_animator.transform.localScale.y,guardian_animator.transform.localScale.z);
                }
            }
            else
            {
                if (guardian_animator.transform.localScale.x > 0)
                {
                    guardian_animator.transform.localScale = new Vector3(guardian_animator.transform.localScale.x * -1, guardian_animator.transform.localScale.y, guardian_animator.transform.localScale.z);
                }
            }
            int act = Random.Range(1,6);
            switch (act)
            {
                case 1:
                    {
                        if (this.transform.position.x + 10 < player.transform.position.x || this.transform.position.x - 10 > player.transform.position.x)
                        {
                            StartCoroutine(Shot());
                        }
                        break;
                    }
                case 2:
                    {
                        if (this.transform.position.x + 6 > player.transform.position.x && this.transform.position.x - 6 < player.transform.position.x)
                        {
                            StartCoroutine(Slash());
                        }
                        break;
                    }
                case 3:
                    {
                        StartCoroutine(Jump());
                        break;
                    }
                case 4:
                    {
                        StartCoroutine(Dash());
                        break;
                    }
                case 5:
                    {
                        StartCoroutine(Idle());
                        break;
                    }
            }
        }
    }

    private void Update()
    {
        
    }
}
