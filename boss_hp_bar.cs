using TMPro;
using UnityEngine;

public class boss_hp_bar : MonoBehaviour
{
    public Animator animator;
    public int hp = 100;
    public float hp_width = 500;
    public TMP_Text hp_text;
    public GameObject hp_bar;

    int before_hp;
    float width_per_one;
    RectTransform rectTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        before_hp = hp;
        width_per_one = hp_width/hp;
        rectTransform = hp_bar.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        hp_text.text = hp.ToString();
        rectTransform.sizeDelta = new Vector2(hp * width_per_one,rectTransform.sizeDelta.y);

        if (hp != before_hp && !animator.GetBool("wait"))
        {
            animator.SetTrigger("hp_down");
            animator.SetBool("wait", true);
        }else if (animator.GetBool("wait"))
        {
            animator.SetBool("wait", false);
        }

        before_hp = hp;
    }
}
