using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PLAYER_2_atk : MonoBehaviour
{
    public Animator animator;
    public LineRenderer lineRenderer;
    public LineRenderer _lineRenderer_atk;
    public PLAYER_common player_2;
    public GameObject PA_;
    public Vector3 t_position;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 2;
    }

    IEnumerator Atk_co(Vector3 target_position)
    {
        GameObject atk_obj;
        animator.SetTrigger("atk");
        LineRenderer lineRenderer_atk = Instantiate(_lineRenderer_atk,new Vector3(0,0,0),Quaternion.identity);
        Destroy(lineRenderer_atk.gameObject,0.6f);
        lineRenderer_atk.startWidth = 0.5f;
        lineRenderer_atk.endWidth = 0.5f;
        lineRenderer_atk.positionCount = 2;
        lineRenderer_atk.SetPositions(new Vector3[] { new Vector3(player_2.transform.position.x, player_2.transform.position.y + 15, 100), new Vector3(target_position.x, target_position.y, 100) });
        atk_obj = Instantiate(PA_);
        atk_obj.transform.position = target_position;
        atk_obj.transform.localScale = new Vector3(0.5f,0.5f,1);
        atk_obj.SetActive(true);
        yield return new WaitForSecondsRealtime(0.1f);
        Destroy(atk_obj);
        for (int a = 0; a<50; a++)
        {
            lineRenderer_atk.startWidth -= 0.01f;
            lineRenderer_atk.endWidth -= 0.01f;
            yield return new WaitForSecondsRealtime(0.003f);
        }
    }

    private void OnEnable()
    {
        lineRenderer.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        lineRenderer.gameObject.SetActive(false);
    }

    public void ATK()
    {
        StartCoroutine(Atk_co(t_position));
    }
    private void Update()
    {
        t_position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100));
        this.transform.position = t_position;
        lineRenderer.SetPositions(new Vector3[] {new Vector3(player_2.transform.position.x,player_2.transform.position.y + 15, 100) ,new Vector3(t_position.x,t_position.y,100)});
    }
}
