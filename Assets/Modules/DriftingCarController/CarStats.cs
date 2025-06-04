using UnityEngine;

[CreateAssetMenu(fileName = "New CarStats", menuName = "Car/CarStats")]
public class CarStats : ScriptableObject
{
    public string carID;
    [Header("Driving Settings")]
    public float maxMotorTorque = 1500f;     // Forward acceleration force
    public float maxSteeringAngle = 30f;     // Max steering angle
    public float brakeTorque = 3000f;        // Braking force
    [Header("Drift Settings")]
    public float driftFactor = 0.6f;         // Adjust sideways friction during drift
    public float driftSteerFactor = 0.5f;    // Countersteering effect
    public float steeringDamping = 0.8f;     // Damping when countersteering
    public float driftTriggerSpeed = 5f;
    public float autoSteerSpeed = 3f;
    // Minimum speed to allow drifting

    [Header("Physics Settings")]
    public float centerOfMassY = -0.5f;      // Center of mass (lower for stability)
    public float mass = 1500f;               // Car mass
    public float drag = 0.02f;               // Drag for air resistance
    public float angularDrag = 0.5f;         // Angular drag for stability

    [Header("High-Speed Settings")]
    public float highSpeedThreshold = 50f; // Speed at which steering reduces
    public float downforce = 5f;           // Aerodynamic downforce applied at speed

}
