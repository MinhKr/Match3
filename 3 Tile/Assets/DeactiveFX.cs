using UnityEngine;

public class DeactiveFX : MonoBehaviour
{
    public float delayTime = 2f; // time delay to active Particle System

    private void OnEnable()
    {
        Invoke("ActivateParticleSystem", delayTime);
    }

    void ActivateParticleSystem()
    {
        SimplePool.Despawn(gameObject);
    }
}
