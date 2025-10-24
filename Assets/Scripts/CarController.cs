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

        // ��������� normalFriction
        normalFriction = new WheelFrictionCurve();
        normalFriction.extremumSlip = 0.4f;
        normalFriction.extremumValue = 1f;
        normalFriction.asymptoteSlip = 0.8f;
        normalFriction.asymptoteValue = 0.5f;
        normalFriction.stiffness = 1f;

        // ��������� lowFriction (��� ������)
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

    // ����� ���������� ������� WASD
    void ControlCar()
    {
        if (IsControl)
        {
            float motor = maxMotorTorque * Input.GetAxis("Vertical"); // ���� ������� (�����/�����)
            float steering = maxSteeringAngle * Input.GetAxis("Horizontal"); // ������ �������� (����/�����)
            bool handBrake = Input.GetKey(KeyCode.Space);

            foreach (var wheel in wheelForward) // �������� �����
            {
                var wheelCollider = wheel.GetComponent<WheelCollider>();

                // ������ ������������ (����/�����)
                wheelCollider.steerAngle = steering;
                ApplyLocalPositionToVisuals(wheelCollider);
            }

            foreach (var wheel in wheelBack) // ������ �����
            {
                var wheelCollider = wheel.GetComponent<WheelCollider>();
                wheelCollider.motorTorque = motor;

                // ������ ������� ������ (�����/�����)
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

    // ����� �������� ���� (��������/�������)
    void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0) // ���� �� ������� ��� �������� ������ - �������
            return;

        Transform visualWheel = collider.transform.GetChild(0); // �������� �������� ������ (������� ������)
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation); // �������� �������/������ �������� WheelColider ������������ ����

        // �������/������������ �������� ������
        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }
}
