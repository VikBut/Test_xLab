using UnityEngine;

public class CharacterSounds : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip footstep;

    public void PlayFootstep()
    {
        audioSource.PlayOneShot(footstep);
    }
    
    public void PlayFootstep2()
    {
        audioSource.PlayOneShot(footstep);
    }
}