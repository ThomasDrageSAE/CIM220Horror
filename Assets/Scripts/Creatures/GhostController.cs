using UnityEngine;


public class GhostController : MonoBehaviour
{
    [Header("Behaviour")]
    public float moveSpeed = 2f;
    public bool patrolHorizontal = true;
    public float patrolDistance = 3f;

    [Header("On Capture")]
    [Tooltip("What happens when this ghost is successfully photographed.")]
    public GhostCaptureResponse captureResponse = GhostCaptureResponse.Dissolve;

    public enum GhostCaptureResponse
    {
        Dissolve,   // Ghost fades out and is destroyed
        Nothing     // Ghost ignores the photo
    }

    private Vector3 _startPosition;
    private float _patrolDir = 1f;

    void Start()
    {
        _startPosition = transform.position;

        CameraApp cam = FindObjectOfType<CameraApp>();
        if (cam != null)
            cam.OnPhotoTaken += OnPhotoTaken;
    }

    void OnDestroy()
    {
        CameraApp cam = FindObjectOfType<CameraApp>();
        if (cam != null)
            cam.OnPhotoTaken -= OnPhotoTaken;
    }

    void Update()
    {
        if (!patrolHorizontal) return;

        transform.Translate(Vector3.right * _patrolDir * moveSpeed * Time.deltaTime);

        float dist = transform.position.x - _startPosition.x;
        if (Mathf.Abs(dist) >= patrolDistance)
            _patrolDir *= -1f;
    }

    private void OnPhotoTaken(bool ghostCaptured, GameObject ghost)
    {
        if (!ghostCaptured || ghost != gameObject) return;

        switch (captureResponse)
        {
            case GhostCaptureResponse.Dissolve:
                StartCoroutine(DissolveAndDestroy());
                break;
            case GhostCaptureResponse.Nothing:
                Debug.Log($"[GhostController] {name} ignored the camera.");
                break;
        }
    }

    private System.Collections.IEnumerator DissolveAndDestroy()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float t = 0f;
        float duration = 0.8f;
        Color start = sr != null ? sr.color : Color.white;

        while (t < duration)
        {
            t += Time.deltaTime;
            if (sr != null)
            {
                Color c = start;
                c.a = Mathf.Lerp(1f, 0f, t / duration);
                sr.color = c;
            }
            yield return null;
        }

        Destroy(gameObject);
    }

    private System.Collections.IEnumerator Flee()
    {
        float timer = 0f;
        float fleeTime = 2f;
        Vector3 fleeDir = (transform.position - Camera.main.transform.position).normalized;
        fleeDir.z = 0f;

        while (timer < fleeTime)
        {
            timer += Time.deltaTime;
            transform.Translate(fleeDir * moveSpeed * 4f * Time.deltaTime);
            yield return null;
        }

        Destroy(gameObject);
    }
}