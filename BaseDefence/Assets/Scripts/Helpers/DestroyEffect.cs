using System.Collections;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    private ParticleSystem particleEffect;
    private float effectDuration;

    private void Awake()
    {
        particleEffect = GetComponent<ParticleSystem>();
        effectDuration = particleEffect.main.duration;
    }

    private void OnEnable()
    {
        if(particleEffect.isPlaying == false)
        {
            particleEffect.Play();
        }

        OnDestroyEffect();
    }

    private void OnDisable()
    {
        particleEffect.Stop();
    }

    private void OnDestroyEffect()
    {
        StartCoroutine(IDestoyEffect(effectDuration));
    }

    private IEnumerator IDestoyEffect(float effectDuration)
    {
        yield return new WaitForSeconds(effectDuration);

        ObjectPoolManager.Instance.ReturnObjectToPool(gameObject);
    }
}
