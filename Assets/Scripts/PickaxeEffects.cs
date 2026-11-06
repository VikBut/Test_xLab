using UnityEngine;

public class PickaxeEffects : MonoBehaviour
{
    public ParticleSystem rockHitEffect;

    public void PlayRockHit()
    {
        rockHitEffect.Play();
    }
}