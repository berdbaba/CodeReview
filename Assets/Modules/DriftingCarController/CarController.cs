using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [SerializeField] private ActionContainer OnDriftingEnd;
    [SerializeField] private ActionContainer OnDriftingStart;
    private WheelCollider[] wheelColliders;
    private Transform[] wheelTransforms;

    private CarStats carStats;
    private WheelVisualManager wheelVisualManager;

    private Rigidbody rb;
    private bool isDrifting = false;

    private float driftTimer = 0f; 

    void Start()
    {
        carStats = transform.GetComponentInChildren<Car>().CarStats;
        wheelVisualManager = transform.GetComponentInChildren<WheelVisualManager>();
        rb = GetComponent<Rigidbody>();
        InitializeCarPhysics();
        InitializeWheelReferences();
    }

    void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        wheelVisualManager.UpdateWheelVisuals();
    }

    private void InitializeCarPhysics()
    {
        rb.mass = carStats.mass;
        rb.linearDamping = carStats.drag;
        rb.angularDamping = carStats.angularDrag;
        rb.centerOfMass = new Vector3(0, carStats.centerOfMassY, 0);
    }

    private void InitializeWheelReferences()
    {
        wheelColliders = wheelVisualManager.GetWheelColliders();
        wheelTransforms = wheelVisualManager.GetWheelVisuals();
    }

    void HandleMotor()
    {
        float motor = carStats.maxMotorTorque * Input.GetAxis("Vertical");
        wheelColliders[2].motorTorque = motor; // Rear Left
        wheelColliders[3].motorTorque = motor; // Rear Right
    }

    void HandleSteering()
    {
        float steeringInput = Input.GetAxis("Horizontal");
        float steering = carStats.maxSteeringAngle * steeringInput;

        if (isDrifting)
        {
            float driftSteer = carStats.driftSteerFactor * rb.linearVelocity.magnitude * Mathf.Sign(rb.angularVelocity.y);
            steering += Mathf.Lerp(0, driftSteer, carStats.autoSteerSpeed * Time.deltaTime);
        }

        wheelColliders[0].steerAngle = steering; // Front Left
        wheelColliders[1].steerAngle = steering; // Front Right

        AdjustDrift(steeringInput);
    }

    void AdjustDrift(float steeringInput)
    {
        if (Input.GetKey(KeyCode.Space)) // Drifting starts
        {
            if (!isDrifting)
            {
                isDrifting = true;
                driftTimer = 0f; // Reset timer
                OnDriftingStart.FireAction();
                wheelVisualManager.PlaySmoke(); // Start smoke
                wheelVisualManager.SetTrailActive(true); // Start skid trails
            }

            ApplyDriftFriction(wheelColliders[2]); // Rear Left
            ApplyDriftFriction(wheelColliders[3]); // Rear Right

            driftTimer += Time.deltaTime; // Accumulate drift time
        }
        else // Drifting stops
        {
            if (isDrifting)
            {
                isDrifting = false;
                OnDriftingEnd.FireAction();
                wheelVisualManager.StopSmoke(); // Stop smoke
                wheelVisualManager.SetTrailActive(false); // Stop skid trails
            }

            ResetFriction(wheelColliders[2]); // Rear Left
            ResetFriction(wheelColliders[3]); // Rear Right
        }
    }

    void ApplyDriftFriction(WheelCollider wheel)
    {
        WheelFrictionCurve friction = wheel.sidewaysFriction;
        friction.stiffness = carStats.driftFactor;
        wheel.sidewaysFriction = friction;
    }

    void ResetFriction(WheelCollider wheel)
    {
        WheelFrictionCurve friction = wheel.sidewaysFriction;
        friction.stiffness = 1f;
        wheel.sidewaysFriction = friction;
    }
}
