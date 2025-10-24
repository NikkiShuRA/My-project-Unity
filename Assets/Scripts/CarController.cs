using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public bool IsControl;

    [SerializeField]
    private List<GameObject> wheelForward;
    [SerializeField]
    private List<GameObject> wheelBack;

    private Rigidbody rb;

    [SerializeField]
    private float maxMotorTorque;
    [SerializeField]
    private float maxSteeringAngle;

    public float handBrakeForce = 3000f;
    [SerializeField]
    private WheelFrictionCurve lowFriction;
    [SerializeField]
    private WheelFrictionCurve normalFriction;


    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Настройка normalFriction
        normalFriction = new WheelFrictionCurve();
        normalFriction.extremumSlip = 0.4f;
        normalFriction.extremumValue = 1f;
        normalFriction.asymptoteSlip = 0.8f;
        normalFriction.asymptoteValue = 0.5f;
        normalFriction.stiffness = 1f;

        // Настройка lowFriction (для дрифта)
        lowFriction = new WheelFrictionCurve();
        lowFriction.extremumSlip = 0.8f;
        lowFriction.extremumValue = 0.7f;
        lowFriction.asymptoteSlip = 1f;
        lowFriction.asymptoteValue = 0.1f;
        lowFriction.stiffness = 0.5f;

    }

    void FixedUpdate()
    {
        ControlCar();
    }

    // Метод управление машиной WASD
    void ControlCar()
    {
        if (IsControl)
        {
            float motor = maxMotorTorque * Input.GetAxis("Vertical"); // Сила привода (вперёд/назад)
            float steering = maxSteeringAngle * Input.GetAxis("Horizontal"); // Градус поворота (лево/право)
            bool handBrake = Input.GetKey(KeyCode.Space);

            foreach (var wheel in wheelForward) // Передние колёса
            {
                var wheelCollider = wheel.GetComponent<WheelCollider>();

                // Только поворачивают (лево/право)
                wheelCollider.steerAngle = steering;
                ApplyLocalPositionToVisuals(wheelCollider);
            }

            foreach (var wheel in wheelBack) // Задние колёса
            {
                var wheelCollider = wheel.GetComponent<WheelCollider>();
                wheelCollider.motorTorque = motor;

                // Только двигают машину (вперёд/назад)
                if (handBrake)
                {
                    wheelCollider.brakeTorque = handBrakeForce;
                    SetFriction(wheelCollider, lowFriction);
                }
                else
                {
                    wheelCollider.brakeTorque = 0;
                    SetFriction(wheelCollider, normalFriction);
                }
                ApplyLocalPositionToVisuals(wheelCollider);
            }
        }
    }

    void SetFriction(WheelCollider wheelCollider, WheelFrictionCurve friction)
    {
        wheelCollider.sidewaysFriction = friction;
        wheelCollider.forwardFriction = friction;
    }

    // Метод анимации колёс (вращение/поворот)
    void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0) // Если на объекте нет модельки колеса - пропуск
            return;

        Transform visualWheel = collider.transform.GetChild(0); // Получаем модельку колеса (первого ребёнка)
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation); // Получаем позицию/градус вращения WheelColider относительно мира

        // Вращаем/поворачиваем модельку колеса
        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }
}
