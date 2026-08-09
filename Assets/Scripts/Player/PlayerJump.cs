using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField] private float jumpForce = 6f;

    [SerializeField] private float lowJumpMultiplier = 2.5f;

    [Header("Better Jump")]
    [SerializeField] private float fallMultiplier = 2.2f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    private bool wasGrounded;
    private float fallVelocity;
    private CameraShake cameraShake;

    public bool JustJumped { get; private set; }

    private Rigidbody rb;

    public bool IsGrounded { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cameraShake = FindFirstObjectByType<CameraShake>();
    }

    private void Update()
    {
        wasGrounded = IsGrounded;

        IsGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundRadius,
            groundLayer
        );

        // Store the maximum falling speed before landing
        if (!IsGrounded && rb.linearVelocity.y < fallVelocity)
        {
            fallVelocity = rb.linearVelocity.y;
        }

        // Player just landed
        if (!wasGrounded && IsGrounded)
        {
            HandleLandingShake();
            fallVelocity = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (float.IsInfinity(rb.linearVelocity.y) ||
            float.IsNaN(rb.linearVelocity.y))
        {
            Debug.LogError(
                "🚨 PlayerJump FOUND INVALID VELOCITY: " +
                rb.linearVelocity
            );
        }

        BetterJump();
    }

    private void BetterJump()
    {
        // Extra gravity while falling
        if (rb.linearVelocity.y < 0f)
        {
            rb.linearVelocity += Vector3.up *
                                 Physics.gravity.y *
                                 (fallMultiplier - 1f) *
                                 Time.fixedDeltaTime;
        }
    }

    public void Jump()
    {
        if (!IsGrounded)
            return;

        JustJumped = true;

        rb.linearVelocity = new Vector3(
            rb.linearVelocity.x,
            0f,
            rb.linearVelocity.z
        );

        rb.AddForce(
            Vector3.up * jumpForce,
            ForceMode.Impulse
        );
    }

    public void ResetJumpFlag()
    {
        JustJumped = false;
    }

    public void CutJump()
    {
        if (rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector3(
                rb.linearVelocity.x,
                rb.linearVelocity.y / lowJumpMultiplier,
                rb.linearVelocity.z
            );
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(
            groundCheck.position,
            groundRadius
        );
    }

    private void HandleLandingShake()
    {
        if (cameraShake == null)
        {
            Debug.LogWarning("CameraShake reference is NULL!");
            return;
        }

        float fallSpeed = Mathf.Abs(fallVelocity);

        Debug.Log(
            "PLAYER LANDED | Fall Speed: " + fallSpeed
        );

        // High Landing
        if (fallSpeed >= 7f)
        {
            Debug.Log(
                "HIGH LANDING SHAKE | Strength: 0.10 | Duration: 0.12"
            );

            cameraShake.HighLanding();
        }

        // Medium Landing
        else if (fallSpeed >= 4f)
        {
            Debug.Log(
                "MEDIUM LANDING SHAKE | Strength: 0.06 | Duration: 0.08"
            );

            cameraShake.MediumLanding();
        }

        // Small Landing
        else if (fallSpeed >= 1.5f)
        {
            Debug.Log(
                "SMALL LANDING SHAKE | Strength: 0.03 | Duration: 0.05"
            );

            cameraShake.SmallLanding();
        }

        // Very small drop / normal ground contact
        else
        {
            Debug.Log(
                "LANDING | Fall speed too low - NO CAMERA SHAKE"
            );
        }
    }
}