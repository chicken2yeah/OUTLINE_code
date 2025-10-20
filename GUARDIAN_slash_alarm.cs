using System.Collections;
using UnityEngine;

public class GUARDIAN_slash_alarm : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Destroying());
    }

    IEnumerator Destroying()
    {
        for (float a = 0; a<=0.5f; a+=0.1f )
        {
            this.transform.localScale = new Vector3(2 - a,1 - a,0);
            yield return new WaitForSecondsRealtime(0.05f);
        }
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
