using System;
using UnityEngine;

public class EffectBehavior : MonoBehaviour
{
    private Action _onRelease;

    public void Initialized(Action onRelease)
    {
        _onRelease = onRelease;
    }
    void OnParticleSystemStopped()
    {
        _onRelease?.Invoke();
    }
}
