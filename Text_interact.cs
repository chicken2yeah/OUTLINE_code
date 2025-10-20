using UnityEngine;

public class Text_interact : MonoBehaviour
{
    public Text_roader text_Roader;
    public InputManager inputManager;
    public GameObject interact_sign;

    bool player_in = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player_in = true;
            interact_sign.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player_in = false;
            interact_sign.SetActive(false);
        }
    }

    private void Update()
    {
        if (player_in)
        {
            if (inputManager.interact && !text_Roader.now_full_road)
            {
                inputManager.interact = false;
                text_Roader.RoadText();
            }
        }
    }
}
