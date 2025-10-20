using UnityEngine;

public class Next_stage : MonoBehaviour
{
    public Stage_manager manager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            manager.Change_stage();
        }
    }
}
