using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class Mouse_script : MonoBehaviour
{
    public VisualEffect effect;
    public Camera cam;
    public GameObject atk_able_sign = null;

    Vector2 mouse_pos;

    Vector3 right_color;
    Vector3 left_color;

    public bool on_atk_able_obj = false;
    public Vector3 col_locate;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "atk_able")
        {
            atk_able_sign.SetActive(true);
            col_locate = collision.transform.position;
            on_atk_able_obj = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "atk_able")
        {
            atk_able_sign.SetActive(false);
            on_atk_able_obj = false;
        }
    }

    private void Start()
    {
        right_color = new Vector3(0,0.2f,1);
        left_color = new Vector3(0,1,1);
    }

    // Update is called once per frame
    void Update()
    {
        mouse_pos = Input.mousePosition;
        mouse_pos = cam.ScreenToWorldPoint(mouse_pos);
        this.gameObject.transform.position = mouse_pos;
        if (Input.GetMouseButtonDown((int)MouseButton.Left))
        {
            effect.SetFloat("Click_X",mouse_pos.x);
            effect.SetFloat("Click_Y",mouse_pos.y);
            effect.SetVector3("Color_XYZ",left_color);
            effect.Play();
        }
        if (Input.GetMouseButtonDown((int)MouseButton.Right))
        {
            effect.SetFloat("Click_X", mouse_pos.x);
            effect.SetFloat("Click_Y", mouse_pos.y);
            effect.SetVector3("Color_XYZ", right_color);
            effect.Play();
        }
    }
}
