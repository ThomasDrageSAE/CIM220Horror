using UnityEngine;

public class MenuBackgroundPan : MonoBehaviour
{
    [SerializeField] private RectTransform target;
    [SerializeField] private Vector2 startPosition = new Vector2(-100f, 0f);
    [SerializeField] private Vector2 endPosition = new Vector2(100f, 0f);
    [SerializeField] private float duration = 20f;

    private float timer;

    private void Reset()
    {
        target = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (target == null || duration <= 0f)
            return;

        timer += Time.deltaTime;
        float t = Mathf.PingPong(timer / duration, 1f);

        target.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);
    }
}