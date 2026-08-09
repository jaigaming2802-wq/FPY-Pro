using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private CinemachineImpulseSource impulseSource;

    [Header("Shake Settings")]
    [SerializeField] private float enemyHitStrength = 0.08f;
    [SerializeField] private float smallLandingStrength = 0.03f;
    [SerializeField] private float mediumLandingStrength = 0.06f;
    [SerializeField] private float highLandingStrength = 0.10f;

    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void EnemyHit()
    {
        impulseSource.GenerateImpulse(enemyHitStrength);
    }

    public void SmallLanding()
    {
        impulseSource.GenerateImpulse(smallLandingStrength);
    }

    public void MediumLanding()
    {
        impulseSource.GenerateImpulse(mediumLandingStrength);
    }

    public void HighLanding()
    {
        impulseSource.GenerateImpulse(highLandingStrength);
    }
}