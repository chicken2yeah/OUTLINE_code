using UnityEngine;

public class A1_hurt_area : MonoBehaviour
{
    public A1_script player;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player.Self_hurt(this.transform.position.x > collision.transform.position.x);
        }
    }
}
