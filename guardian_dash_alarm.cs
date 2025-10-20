using System.Collections;
using UnityEngine;

public class guardian_dash_alarm : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Disappear());
        if (this.transform.position.x < 0)
        {
            this.transform.localScale = new Vector3(-5,5,0);
        }
    }

    IEnumerator Disappear()
    {
        for (int a = 30;a>0 ; a-=1)
        {
            this.transform.rotation = new Quaternion(0,0,this.gameObject.transform.rotation.z + 0.3f,0); // w = 완전 회전 할지말지
            this.transform.localScale = new Vector3(this.transform.localScale.x - 0.3f,this.transform.localScale.y - 0.1f,1);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
