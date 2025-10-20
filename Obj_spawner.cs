using System.Collections;
using UnityEngine;

public class Obj_spawner : MonoBehaviour
{
    public float min_cool = 5;
    public float max_cool = 7;
    public GameObject spawn_obj;
    public float spawn_rangeX = 0;
    public float spawn_rangeY = 0;

    IEnumerator Spawner()
    {
        while (true)
        {
            if (Time.timeScale == 0)
            {
                break;
            }
            else
            {
                Instantiate(spawn_obj,new Vector3(
                    Random.Range(this.transform.position.x - spawn_rangeX,this.transform.position.x + spawn_rangeX), 
                    Random.Range(this.transform.position.y - spawn_rangeY, this.transform.position.y + spawn_rangeY),
                    this.transform.position.z),Quaternion.identity).SetActive(true);
                float wait = Random.Range(min_cool,max_cool);
                yield return new WaitForSecondsRealtime(wait);
            }
        }
    }
    private void OnEnable()
    {
        StartCoroutine(Spawner());
    }
}
