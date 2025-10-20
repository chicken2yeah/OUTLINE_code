using System.Collections;
using UnityEngine;

public class AirTile : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public BoxCollider2D collider_;

    float move;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PH")
        {
            StartCoroutine(Dead());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Boss")
        {
            StartCoroutine(Dead());
        }
    }

    IEnumerator Dead()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        if (Random.Range(0,2) == 0)
        {
            rb.linearVelocity = new Vector2(UnityEngine.Random.Range(3, 8), UnityEngine.Random.Range(-5, -30));
            rb.freezeRotation = false;
            rb.angularVelocity = UnityEngine.Random.Range(36, 360);
            this.transform.localScale = this.transform.localScale * 0.7f;
            Destroy(collider_);
            yield return new WaitForSecondsRealtime(5f);
            Destroy(this.gameObject);
        }
        else
        {
            rb.linearVelocity = new Vector2(UnityEngine.Random.Range(-3, -8), UnityEngine.Random.Range(-5, -30));
            rb.freezeRotation = false;
            rb.angularVelocity = UnityEngine.Random.Range(-36, -360);
            this.transform.localScale = this.transform.localScale * 0.7f;
            Destroy(collider_);
            yield return new WaitForSecondsRealtime(5f);
            Destroy(this.gameObject);
        }
    }

    private void OnEnable()
    {
        spriteRenderer.size = new Vector2(Random.Range(5,20),2);
        collider_.size = spriteRenderer.size;
        if (Random.Range(0,2) == 0)
        {
            move = Random.Range(3,5);
        }
        else
        {
            move = Random.Range(-3, -5);
        }
    }

    private void Update()
    {
        if (Time.timeScale == 0)
        {
            Destroy(this.gameObject,1);
        }
        if (rb.linearVelocity.x == 0)
        {
            StartCoroutine(Dead());
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocityX = move;
    }
}
