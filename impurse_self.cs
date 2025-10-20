using Unity.Cinemachine;
using UnityEngine;

public class impurse_self : MonoBehaviour
{
    public CinemachineImpulseSource impurse_source;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CinemachineImpulseManager.Instance.IgnoreTimeScale = true;
    }

    public void Impurse_self()
    {
        impurse_source.GenerateImpulse();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
