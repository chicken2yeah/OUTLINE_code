using Unity.Cinemachine;
using UnityEngine;

public class PLAYER_common : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;

    public int hp = 5;
    public int power = 1;
    public bool damaged = false;
    public bool full_hp_mode = false;

    public bool on_ground = true;
    public bool stop_fixed = false;
    public UI_manager uI_Manager;
    public GameObject dead_background;
    public CinemachineImpulseSource hitted_impurse;

    protected bool hitted_right = false;

    protected int dance_num = 0;
}
