using UnityEngine;

public class Set_W_H_rect_for_child : MonoBehaviour
{
    // rectTransform »ó¼Ó±â

    public RectTransform parent;
    public RectTransform child;
    public float percentX;
    public float percentY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        child.sizeDelta = new Vector2((parent.rect.width / 100) * percentX, (parent.rect.height / 100) * percentY);
    }

    // Update is called once per frame
    void Update()
    {
        //child.sizeDelta = new Vector2((parent.rect.width/100)*percentX,(parent.rect.height/100)*percentY);
    }
}
