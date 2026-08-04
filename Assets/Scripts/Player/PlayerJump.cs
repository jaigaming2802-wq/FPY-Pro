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

    public bool JustJumped { get; private set; }

    private Rigidbody rb;

    public bool IsGrounded { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        IsGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundRadius,
            groundLayer
        );
    }

    private void FixedUpdate()
    {
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
}