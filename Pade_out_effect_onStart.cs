using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Pade_out_effect_onStart : MonoBehaviour
{
    public Image image;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponent<Image>();
        StartCoroutine(Fade_out());
    }

    IEnumerator Fade_out()
    {
        for (float a = image.color.a ; a>0 ; a-=0.01f)
        {
            if (a<0)
            {
                a = 0;
            }
            image.color = new Color(image.color.r,image.color.g,image.color.b,a);
            yield return new WaitForSecondsRealtime(0.003f);
        }
        this.gameObject.SetActive(false);
    }
}
