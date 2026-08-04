using UnityEngine;
using System.Collections;

public class PlayerDash : MonoBehaviour
{
    [Header("Dash")]

    [SerializeField] private float dashSpeed = 12f;

    [SerializeField] private float dashDuration = 0.2f;

    [SerializeField] private float dashCooldown = 0.5f;

    private Rigidbody rb;

    private SpriteRenderer spriteRenderer;

    public bool IsDashing { get; private set; }

    public bool CanDash { get; private set; } = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public IEnumerator Dash()
    {
        if (!CanDash || IsDashing)
            yield break;

        CanDash = false;
        IsDashing = true;

        float originalGravity = rb.useGravity ? 1f : 0f;

        rb.useGravity = false;

        float direction = spriteRenderer.flipX ? -1f : 1f;

        rb.linearVelocity = new Vector3(
            direction * dashSpeed,
            0f,
            0f
        );

        yield return new WaitForSeconds(dashDuration);

        rb.useGravity = originalGravity == 1f;

        IsDashing = false;

        yield return new WaitForSeconds(dashCooldown);

        CanDash = true;
    }
}