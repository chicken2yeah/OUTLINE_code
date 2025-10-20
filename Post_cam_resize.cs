using UnityEngine;

public class Post_cam_resize : MonoBehaviour
{
    public Camera target_cam;
    public Camera this_cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this_cam.orthographicSize = target_cam.orthographicSize;
    }
}
