using UnityEngine;

public class Play_sound : MonoBehaviour
{
    public AudioSource[] sounds;

    public void PlaySound(int num)
    {
        sounds[num].Play();
    }
}
