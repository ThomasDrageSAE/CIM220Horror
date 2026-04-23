using UnityEngine;

public class MonsterIdleMotion : MonoBehaviour
{
    public enum MotionStyle
    {
        Float,
        Pulse,
        Twitch,
        Sway
    }

    [Header("Style")]
    [SerializeField] private MotionStyle motionStyle = MotionStyle.Float;

    [Header("Base Movement")]
    [SerializeField] private float moveAmountX = 10f;
    [SerializeField] private float moveAmountY = 10f;
    [SerializeField] private float moveSpeed = 1f;

    [Header("Base Scale")]
    [SerializeField] private float scaleAmount = 0.05f;
    [SerializeField] private float scaleSpeed = 1.2f;

    [Header("Base Rotation")]
    [SerializeField] private float rotationAmount = 2f;
    [SerializeField] private float rotationSpeed = 0.8f;

    [Header("Random Variation")]
    [SerializeField] private bool randomizePerMonster = true;
    [SerializeField] private float variationMultiplierMin = 0.85f;
    [SerializeField] private float variationMultiplierMax = 1.2f;

    [Header("Twitch Settings")]
    [SerializeField] private float twitchChance = 0.015f;
    [SerializeField] private float twitchStrength = 12f;

    [Header("Glitch")]
    [SerializeField] private float glitchChance = 0.02f;
    [SerializeField] private float glitchStrength = 25f;

    private RectTransform rect;
    private Vector2 startPos;
    private Vector3 startScale;
    private float timeOffset;

    private bool glitchEnabled = false;

    private float currentMoveAmountX;
    private float currentMoveAmountY;
    private float currentMoveSpeed;
    private float currentScaleAmount;
    private float currentScaleSpeed;
    private float currentRotationAmount;
    private float currentRotationSpeed;
    private float currentTwitchChance;
    private float currentTwitchStrength;

    private void Awake()
    {
        rect = transform as RectTransform;

        if (rect != null)
            startPos = rect.anchoredPosition;

        startScale = transform.localScale;
        timeOffset = Random.Range(0f, 100f);

        ApplyRandomVariation();
    }

    private void OnEnable()
    {
        if (rect != null)
            rect.anchoredPosition = startPos;

        transform.localScale = startScale;
        transform.localRotation = Quaternion.identity;
    }

    private void Update()
    {
        float t = Time.time + timeOffset;

        switch (motionStyle)
        {
            case MotionStyle.Float:
                UpdateFloat(t);
                break;

            case MotionStyle.Pulse:
                UpdatePulse(t);
                break;

            case MotionStyle.Twitch:
                UpdateTwitch(t);
                break;

            case MotionStyle.Sway:
                UpdateSway(t);
                break;
        }

        if (glitchEnabled && rect != null && Random.value < glitchChance)
        {
            rect.anchoredPosition += Random.insideUnitCircle * glitchStrength;
        }
    }

    private void UpdateFloat(float t)
    {
        if (rect != null)
        {
            float x = Mathf.Sin(t * currentMoveSpeed) * currentMoveAmountX;
            float y = Mathf.Cos(t * currentMoveSpeed * 0.75f) * currentMoveAmountY;
            rect.anchoredPosition = startPos + new Vector2(x, y);
        }

        float scale = 1f + Mathf.Sin(t * currentScaleSpeed) * currentScaleAmount;
        transform.localScale = startScale * scale;

        float rot = Mathf.Sin(t * currentRotationSpeed) * currentRotationAmount;
        transform.localRotation = Quaternion.Euler(0f, 0f, rot);
    }

    private void UpdatePulse(float t)
    {
        if (rect != null)
        {
            float y = Mathf.Sin(t * currentMoveSpeed * 0.5f) * (currentMoveAmountY * 0.4f);
            rect.anchoredPosition = startPos + new Vector2(0f, y);
        }

        float scale = 1f + Mathf.Sin(t * currentScaleSpeed * 1.4f) * (currentScaleAmount * 1.5f);
        transform.localScale = startScale * scale;

        transform.localRotation = Quaternion.identity;
    }

    private void UpdateTwitch(float t)
    {
        if (rect != null)
        {
            float x = Mathf.Sin(t * currentMoveSpeed * 1.4f) * (currentMoveAmountX * 0.5f);
            float y = Mathf.Cos(t * currentMoveSpeed * 1.1f) * (currentMoveAmountY * 0.4f);
            rect.anchoredPosition = startPos + new Vector2(x, y);

            if (Random.value < currentTwitchChance)
                rect.anchoredPosition += Random.insideUnitCircle * currentTwitchStrength;
        }

        float scale = 1f + Mathf.Sin(t * currentScaleSpeed) * (currentScaleAmount * 0.6f);
        transform.localScale = startScale * scale;

        float rot = Mathf.Sin(t * currentRotationSpeed * 2f) * (currentRotationAmount * 1.5f);
        transform.localRotation = Quaternion.Euler(0f, 0f, rot);
    }

    private void UpdateSway(float t)
    {
        if (rect != null)
        {
            float x = Mathf.Sin(t * currentMoveSpeed * 0.8f) * (currentMoveAmountX * 1.2f);
            rect.anchoredPosition = startPos + new Vector2(x, 0f);
        }

        transform.localScale = startScale;

        float rot = Mathf.Sin(t * currentRotationSpeed) * (currentRotationAmount * 2f);
        transform.localRotation = Quaternion.Euler(0f, 0f, rot);
    }

    private void ApplyRandomVariation()
    {
        float moveVar = randomizePerMonster ? Random.Range(variationMultiplierMin, variationMultiplierMax) : 1f;
        float scaleVar = randomizePerMonster ? Random.Range(variationMultiplierMin, variationMultiplierMax) : 1f;
        float rotVar = randomizePerMonster ? Random.Range(variationMultiplierMin, variationMultiplierMax) : 1f;
        float twitchVar = randomizePerMonster ? Random.Range(variationMultiplierMin, variationMultiplierMax) : 1f;

        currentMoveAmountX = moveAmountX * moveVar;
        currentMoveAmountY = moveAmountY * moveVar;
        currentMoveSpeed = moveSpeed * moveVar;

        currentScaleAmount = scaleAmount * scaleVar;
        currentScaleSpeed = scaleSpeed * scaleVar;

        currentRotationAmount = rotationAmount * rotVar;
        currentRotationSpeed = rotationSpeed * rotVar;

        currentTwitchChance = twitchChance * twitchVar;
        currentTwitchStrength = twitchStrength * twitchVar;
    }

    public void SetGlitch(bool enabled)
    {
        glitchEnabled = enabled;
    }

    public void SetMotionStyle(MotionStyle newStyle)
    {
        motionStyle = newStyle;
    }
}