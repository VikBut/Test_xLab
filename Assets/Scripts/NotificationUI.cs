using UnityEngine;
using System.Collections;

public class NotificationUI : MonoBehaviour
{
    public CanvasGroup notification;
    public float fadeTime = 0.5f;
    public float visibleTime = 2f;

    private Coroutine currentRoutine;

    public void ShowNotification()
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(NotificationRoutine());
    }

    IEnumerator NotificationRoutine()
    {
        // Показываем объект
        notification.gameObject.SetActive(true);

        // Начальная прозрачность
        notification.alpha = 0f;

        // Плавное появление
        float t = 0f;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            notification.alpha = Mathf.Lerp(0f, 1f, t / fadeTime);
            yield return null;
        }

        notification.alpha = 1f;

        // Ждем
        yield return new WaitForSeconds(visibleTime);

        // Плавное исчезновение
        t = 0f;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            notification.alpha = Mathf.Lerp(1f, 0f, t / fadeTime);
            yield return null;
        }

        notification.alpha = 0f;

        // Скрываем объект
        notification.gameObject.SetActive(false);
    }
}