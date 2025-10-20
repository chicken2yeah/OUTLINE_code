using UnityEngine;

public class Monster_attack : MonoBehaviour
{
    public GameObject PH;
    public Animator animator;
    public Monster_move monster_move;
    
    public virtual void Attack()
    {
        monster_move.is_attacking = false;
    }
}
