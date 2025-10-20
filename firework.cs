using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class firework : MonoBehaviour
{
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (this.transform.localScale.x>0)
        {
            rb.linearVelocity = new Vector2((Random.value * -10),(Random.value * 10));
        }
        else
        {
            rb.linearVelocity = new Vector2((Random.value * 10), (Random.value * 10));
        }
        StartCoroutine(UnScale_destory());
    }

    IEnumerator UnScale_destory()
    {
        yield return new WaitForSecondsRealtime(1);
        Destroy(this.gameObject);
    }
}
