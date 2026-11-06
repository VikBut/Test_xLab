using UnityEngine;

public class Water : MonoBehaviour
{
    [Header("Water Movement")]
    public float waveHeight = 0.05f;
    public float waveSpeed = 1f;

    [Header("Texture Movement")]
    public float textureSpeedX = 0.02f;
    public float textureSpeedY = 0.01f;

    private Vector3 startPosition;
    private Renderer waterRenderer;

    void Start()
    {
        startPosition = transform.position;
        waterRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        // Покачивание воды вверх-вниз
        float yOffset = Mathf.Sin(Time.time * waveSpeed) * waveHeight;
        transform.position = startPosition + Vector3.up * yOffset;

        // Движение текстуры
        if (waterRenderer != null)
        {
            float offsetX = Time.time * textureSpeedX;
            float offsetY = Time.time * textureSpeedY;

            waterRenderer.material.mainTextureOffset =
                new Vector2(offsetX, offsetY);
        }
    }
}