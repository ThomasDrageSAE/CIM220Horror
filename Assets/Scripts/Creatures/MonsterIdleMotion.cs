using UnityEngine;

public class MonsterIdleMotion : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveAmount = 10f;
    [SerializeField] private float moveSpeed = 1f;

    [Header("Scale")]
    [SerializeField] private float scaleAmount = 0.05f;
    [SerializeField] private float scaleSpeed = 1.2f;

    [Header("Rotation")]
    [SerializeField] private float rotationAmount = 2f;
    [SerializeField] private float rotationSpeed = 0.8f;

    [Header("Glitch")]
    [SerializeField] private float glitchChance = 0.02f;
    [SerializeField] private float glitchStrength = 25f;

    private RectTransform rect;
    private Vector2 startPos;
    private Vector3 startScale;
    private float timeOffset;

    private bool glitchEnabled = false;

    private void Awake()
    {
        rect = transform as RectTransform;

        if (rect != null)
            startPos = rect.anchoredPosition;

        startScale = transform.localScale;

        timeOffset = Random.Range(0f, 100f);
    }

    private void OnEnable()
    {
        // Reset position when monster changes
        if (rect != null)
            rect.anchoredPosition = startPos;
    }

    private void Update()
    {
        float t = Time.time + timeOffset;

        // FLOAT
        if (rect != null)
        {
            float x = Mathf.Sin(t * moveSpeed) * moveAmount;
            float y = Mathf.Cos(t * moveSpeed * 0.8f) * moveAmount;

            rect.anchoredPosition = startPos + new Vector2(x, y);
        }

        // SCALE
        float scale = 1f + Mathf.Sin(t * scaleSpeed) * scaleAmount;
        transform.localScale = startScale * scale;

        // ROTATION
        float rot = Mathf.Sin(t * rotationSpeed) * rotationAmount;
        transform.localRotation = Quaternion.Euler(0f, 0f, rot);

        // GLITCH
        if (glitchEnabled && rect != null && Random.value < glitchChance)
        {
            rect.anchoredPosition += Random.insideUnitCircle * glitchStrength;
        }
    }

    // 🔥 THIS IS THE IMPORTANT PART
    public void SetGlitch(bool enabled)
    {
        glitchEnabled = enabled;
    }
}