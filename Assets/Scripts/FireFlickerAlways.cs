using UnityEngine;
public class FireFlickerAlways : MonoBehaviour 

{   
    public Light fireLight; 
    public float minIntensity = 2.8f; 
    public float maxIntensity = 4f; 
    public float speed = 8f; 
    private float noise;

    void Update()
    {
        noise += Time.deltaTime * speed; 
        fireLight.intensity = Mathf.Lerp( minIntensity, maxIntensity, Mathf.PerlinNoise(noise, 0) );
    } 
}