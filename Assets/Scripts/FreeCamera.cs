using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

public class FreeCamera : MonoBehaviour
{
    public float speed = 1.5f;
    public float acceleration = 10f;
    public float sensitivity = 5f;
    public Camera mainCamera;

    [Header("First Person")]
    public Transform[] cameraPoints;

    private int currentCharacter = -1;
    private bool followHead = false;

    private Rigidbody body;
    private float rotY;
    private Vector3 direction;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        body.freezeRotation = true;
        body.useGravity = false;
        body.mass = 0.1f;
        body.drag = 10;
    }

    void Update()
    {
        // Переключение на следующего персонажа
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (cameraPoints.Length > 0)
            {
                currentCharacter++;

                if (currentCharacter >= cameraPoints.Length)
                    currentCharacter = 0;

                followHead = true;

                // Сбрасываем локальный поворот камеры
                mainCamera.transform.localRotation = Quaternion.identity;
                rotY = 0f;
            }
        }

        // Камера следует за головой
        if (followHead)
        {
            transform.position = cameraPoints[currentCharacter].position;
            transform.rotation = cameraPoints[currentCharacter].rotation;

            // Камера всегда смотрит туда же, куда CameraPoint
            mainCamera.transform.localRotation = Quaternion.identity;

            body.velocity = Vector3.zero;

            // При начале движения отцепляемся
            if (Input.GetAxisRaw("Horizontal") != 0 ||
                Input.GetAxisRaw("Vertical") != 0)
            {
                followHead = false;

                // Выравниваем свободную камеру
                Vector3 forward = transform.forward;
                forward.y = 0f;

                if (forward.sqrMagnitude > 0.0001f)
                {
                    transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
                }

                mainCamera.transform.localRotation = Quaternion.identity;
                rotY = 0f;

                body.velocity = Vector3.zero;
            }
        }

        Move();
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Mouse1) && !followHead)
        {
            // Поворот всей камеры по горизонтали
            transform.Rotate(0f, Input.GetAxis("Mouse X") * sensitivity, 0f);

            // Поворот только MainCamera по вертикали
            rotY += Input.GetAxis("Mouse Y") * sensitivity;
            rotY = Mathf.Clamp(rotY, -90f, 90f);

            mainCamera.transform.localRotation = Quaternion.Euler(-rotY, 0f, 0f);
        }

        direction = new Vector3(h, 0, v);
        direction = mainCamera.transform.TransformDirection(direction);
    }

    void FixedUpdate()
    {
        if (!followHead)
        {
            body.AddForce(direction.normalized * speed * acceleration);

            if (Mathf.Abs(body.velocity.x) > speed)
                body.velocity = new Vector3(Mathf.Sign(body.velocity.x) * speed, body.velocity.y, body.velocity.z);

            if (Mathf.Abs(body.velocity.z) > speed)
                body.velocity = new Vector3(body.velocity.x, body.velocity.y, Mathf.Sign(body.velocity.z) * speed);

            if (Mathf.Abs(body.velocity.y) > speed)
                body.velocity = new Vector3(body.velocity.x, Mathf.Sign(body.velocity.y) * speed, body.velocity.z);
        }
    }
}