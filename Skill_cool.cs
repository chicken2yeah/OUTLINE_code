using System.Collections;
using UnityEngine;

public class Skill_cool : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public void Skill_cooltime(float cooltime)
    {
        StartCoroutine(Skill_cooling(cooltime));
    }

    private void FixedUpdate()
    {
        
    }

    IEnumerator Skill_cooling(float cooltime)
    {
        float wait = cooltime / 50;
        for (int loop = 0; loop<=50; loop++)
        {
            this.spriteRenderer.material.SetFloat("_Cool", loop * 0.02f);
            yield return new WaitForSeconds(wait);
        }
    }
}
