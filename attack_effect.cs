using UnityEngine;

public class attack_effect : MonoBehaviour
{
    public Animator animator;
    public int atk_mode = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        switch (atk_mode)
        {
            case (1):
                {
                    animator.Play("attack1");
                    break;
                }
            case (2):
                {
                    animator.Play("attack2");
                    break;
                }
            case (3):
                {
                    animator.Play("attack3");
                    break;
                }
        }
        Destroy(this.gameObject,0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
