using Unity.Cinemachine;
using UnityEngine;

public class Battle_cam : MonoBehaviour
{
    public CinemachineCamera cam;
    public CinemachineFollow cam_follow;
    public float player_max_speed;
    public Rigidbody2D player_rb;
    public Animator player_animator;
    public float follow_offset_x = 1;
    public float follow_offset_change_x = 1f;
    public float follow_offset_change_speed = 0.5f;
    public float min_lens = 10f;
    public float max_lens = 12f;

    float change_lens;
    float target_x;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        change_lens = max_lens - min_lens;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float now_speed;
        if (player_rb.linearVelocity.x >0)
        {
            now_speed = player_rb.linearVelocity.x;
        }
        else
        {
            now_speed = player_rb.linearVelocity.x * -1;
        }
        float rate = now_speed/(player_max_speed/100);
        if (rate > 100)
        {
            rate = 100;
        }
        cam.Lens.OrthographicSize = min_lens + (rate*(change_lens/100));
        
        if (player_animator.transform.localScale.x > 0)
        {
            target_x = follow_offset_x - (rate * (follow_offset_change_x / 100));
            
            if (cam_follow.FollowOffset.x < target_x)
            {
                cam_follow.FollowOffset.Set(cam_follow.FollowOffset.x + follow_offset_change_speed,0,cam_follow.FollowOffset.z);
            }

            if (cam_follow.FollowOffset.x > target_x)
            {
                cam_follow.FollowOffset.Set(target_x, 0, cam_follow.FollowOffset.z);
            }
        }
        else
        {
            target_x = -1*(follow_offset_x - (rate * (follow_offset_change_x / 100)));
            if (cam_follow.FollowOffset.x > target_x)
            {
                cam_follow.FollowOffset.Set(cam_follow.FollowOffset.x - follow_offset_change_speed, 0 , cam_follow.FollowOffset.z);
            }

            if (cam_follow.FollowOffset.x < target_x)
            {
                cam_follow.FollowOffset.Set(target_x, 0, cam_follow.FollowOffset.z);
            }
        }
    }
}
