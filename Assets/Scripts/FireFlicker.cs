using UnityEngine;

public class FireFlicker : MonoBehaviour
{
    public Light fireLight;

    [Header("Flicker")]
    public float flickerAmount = 0.4f; // Насколько сильно мерцает свет
    public float speed = 8f;

    [HideInInspector]
    public float baseIntensity = 4f;   // Эту яркость будет менять DayNightCycle

    private float noise;

    void Update()
    {
        if (fireLight == null)
            return;

        noise += Time.deltaTime * speed;

        // Значение от -1 до 1
        float flicker = (Mathf.PerlinNoise(noise, 0f) - 0.5f) * 2f;

        if (baseIntensity <= 0.01f)
        {
            fireLight.enabled = false;
        }
        else
        {
            fireLight.enabled = true;
            fireLight.intensity = Mathf.Max(0f, baseIntensity + flicker * flickerAmount);
        }
    }
}