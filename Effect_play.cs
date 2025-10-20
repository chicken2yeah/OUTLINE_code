using UnityEngine;
using UnityEngine.VFX;

public class Effect_play : MonoBehaviour
{
    public VisualEffect[] effect;
    
    public void Play_Effect(int num)
    {
        effect[num].Play();
    }

    public void Stop_Effect(int num)
    {
        effect[num].Stop();
    }
}
