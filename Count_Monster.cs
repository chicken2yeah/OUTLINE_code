using UnityEngine;

public class Count_Monster : MonoBehaviour
{
    public Alarm_system alarm_system;
    public SpriteRenderer door;
    public Sprite door_open_sprite;
    public GameObject next;
    public AudioSource audioSource;

    int before_count;

    private void Start()
    {
        before_count = this.transform.childCount;
    }
    // Update is called once per frame
    void Update()
    {
        if (before_count != this.transform.childCount)
        {
            alarm_system.Top_alarm("잔여 처리물 : " + this.transform.childCount);
        }
        before_count = this.transform.childCount;
        if (!next.activeSelf && this.transform.childCount == 0)
        {
            door.sprite = door_open_sprite;
            next.SetActive(true);
            alarm_system.Middle_alarm("구역 정화 완료");
            audioSource.Play();
        }
    }
}
