using UnityEngine;

public class Sound_play : MonoBehaviour
{
    public AudioSource[] sound;

    public void Play_Sound(int num)
    {
        sound[num].Play();
    }
}
