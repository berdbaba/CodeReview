using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class WheelVisualManager : MonoBehaviour
{
    [Header("Wheel Colliders (FL, FR, RL, RR)")]
    [SerializeField]
    private WheelCollider[] _wheelColliders = new WheelCollider[4];

    [Header("Wheel Visual Transforms (FL, FR, RL, RR)")]
    [SerializeField]
    private Transform[] _wheelVisuals = new Transform[4];

    [Header("Effects")]
    [SerializeField]
    private ParticleSystem _leftTireSmoke;
    [SerializeField]
    private ParticleSystem _rightTireSmoke;
    [SerializeField]
    private TrailRenderer _leftTireSkid;
    [SerializeField]
    private TrailRenderer _rightTireSkid;

    public void UpdateWheelVisuals()
    {
        int count = Mathf.Min(_wheelColliders.Length, _wheelVisuals.Length);
        for (int i = 0; i < count; i++)
        {
            var collider = _wheelColliders[i];
            var visual = _wheelVisuals[i];
            if (collider == null || visual == null)
                continue;

            collider.GetWorldPose(out Vector3 pos, out Quaternion rot);
            visual.position = pos;
            visual.rotation = rot;
        }
    }

    public void PlaySmoke()
    {
        if (_leftTireSmoke != null && !_leftTireSmoke.isPlaying)
            _leftTireSmoke.Play();

        if (_rightTireSmoke != null && !_rightTireSmoke.isPlaying)
            _rightTireSmoke.Play();
    }

    public void StopSmoke()
    {
        if (_leftTireSmoke != null && _leftTireSmoke.isPlaying)
            _leftTireSmoke.Stop();

        if (_rightTireSmoke != null && _rightTireSmoke.isPlaying)
            _rightTireSmoke.Stop();
    }

    public void SetTrailActive(bool isActive)
    {
        if (_leftTireSkid != null)
            _leftTireSkid.emitting = isActive;

        if (_rightTireSkid != null)
            _rightTireSkid.emitting = isActive;
    }

    public WheelCollider[] GetWheelColliders() => _wheelColliders;
    public Transform[] GetWheelVisuals() => _wheelVisuals;
}
