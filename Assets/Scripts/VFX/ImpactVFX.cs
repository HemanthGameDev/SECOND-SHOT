using UnityEngine;

public class ImpactVFX : MonoBehaviour
{
    private ParticleSystem particleSystem;

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    public void Play(Vector3 position, Vector3 normal)
    {
        transform.position = position;

        if (normal.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(normal);
        }

        gameObject.SetActive(true);

        particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        particleSystem.Play();
    }

    private void OnParticleSystemStopped()
    {
        gameObject.SetActive(false);
    }
}