using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class target_alarm : MonoBehaviour
{
    Image image;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = this.GetComponent<Image>();
        StartCoroutine(Fade_out());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Fade_out()
    {
        for (int a = 0; a< 3; a++)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x * 0.7f, this.transform.localScale.y * 0.7f, this.transform.localScale.z);
            image.color = new Color(image.color.r,image.color.g,image.color.b,image.color.a*0.7f);
            yield return new WaitForSecondsRealtime(0.1f);
        }
        Destroy(this.gameObject);
    }
}
