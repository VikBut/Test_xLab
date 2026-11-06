using UnityEngine;

public class CloudMoving : MonoBehaviour
{
    [SerializeField] private Transform[] points;      // точки, между которыми движется объект
    [SerializeField] private float speed = 5f;        // скорость движения
    [SerializeField] private float heightOffset = 2f; // высота над точкой
    [SerializeField] private ParticleSystem rain;
    [SerializeField] private AudioSource rainAudio;
    [SerializeField] private AudioSource thunderAudio;

    [SerializeField] private float thunderInterval = 4f; // интервал между раскатами грома

    private int currentIndex = 0;     // индекс текущей цели
    private bool isMoving = false;    // движется ли объект сейчас

    private float thunderTimer = 0f;

    void Start()
    {
        // На старте дождь выключен
        if (rain != null)
            rain.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        if (rainAudio != null)
            rainAudio.Stop();

        thunderTimer = 0f;
    }

    void Update()
    {
        if (points.Length == 0) return;

        // Нажали Z и объект сейчас не движется
        if (Input.GetKeyDown(KeyCode.Z) && !isMoving)
        {
            isMoving = true;

            if (rain != null)
                rain.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            if (rainAudio != null)
                rainAudio.Stop();

            thunderTimer = 0f;
        }

        if (isMoving)
        {
            // Цель: позиция точки + смещение вверх
            Vector3 targetPosition = points[currentIndex].position + Vector3.up * heightOffset;

            // Плавно двигаем объект к цели
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                speed * Time.deltaTime
            );

            // Если почти дошли до точки — считаем, что дошли
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isMoving = false;

                if (rain != null)
                    rain.Play();

                if (rainAudio != null)
                    rainAudio.Play();

                // Первый гром сразу
                if (thunderAudio != null)
                    thunderAudio.PlayOneShot(thunderAudio.clip);

                // Следующий гром через заданный интервал
                thunderTimer = thunderInterval;

                // Переходим к следующей точке
                currentIndex++;

                // Если вышли за пределы массива — начинаем сначала
                if (currentIndex >= points.Length)
                    currentIndex = 0;
            }
        }

        // Гром каждые thunderInterval секунд, пока идет дождь
        if (rain != null && rain.isPlaying)
        {
            thunderTimer -= Time.deltaTime;

            if (thunderTimer <= 0f)
            {
                if (thunderAudio != null)
                    thunderAudio.PlayOneShot(thunderAudio.clip);

                thunderTimer = thunderInterval;
            }
        }
    }
}