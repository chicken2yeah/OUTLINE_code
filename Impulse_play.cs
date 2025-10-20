using Unity.Cinemachine;
using UnityEngine;

public class Impulse_play : MonoBehaviour
{
    public CinemachineImpulseSource[] impulseSources;
    public void Play_impulse(int num)
    {
        CinemachineImpulseManager.Instance.IgnoreTimeScale = true;
        impulseSources[num].GenerateImpulse();
    }
}
