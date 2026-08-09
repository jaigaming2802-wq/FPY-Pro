using System.Collections;
using UnityEngine;

public class EnemyHitFlash : MonoBehaviour
{
    [Header("Hit Flash")]
    [SerializeField] private float flashDuration = 0.08f;

    [SerializeField] private float flashIntensity = 2f;

    private SpriteRenderer spriteRenderer;

    private MaterialPropertyBlock propertyBlock;

    private Coroutine flashCoroutine;

    private static readonly int ColorProperty =
        Shader.PropertyToID("_Color");

    private void Awake()
    {
        // Get the SpriteRenderer from the SAME Enemy object.
        spriteRenderer =
            GetComponent<SpriteRenderer>();

        propertyBlock =
            new MaterialPropertyBlock();
    }

    public void PlayHitFlash()
    {
        if (spriteRenderer == null)
            return;

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine =
            StartCoroutine(HitFlashRoutine());
    }

    private IEnumerator HitFlashRoutine()
    {
        // Get current renderer properties.
        spriteRenderer.GetPropertyBlock(
            propertyBlock);

        // Apply stronger white brightness.
        propertyBlock.SetColor(
            ColorProperty,
            Color.white * flashIntensity);

        spriteRenderer.SetPropertyBlock(
            propertyBlock);

        // Keep the flash for a short time.
        yield return new WaitForSeconds(
            flashDuration);

        // Clear the temporary material property.
        spriteRenderer.SetPropertyBlock(null);

        flashCoroutine = null;
    }
}