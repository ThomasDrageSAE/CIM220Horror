using System.Collections;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    [Header("Behaviour")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private bool patrolHorizontal = true;
    [SerializeField] private float patrolDistance = 3f;

    [Header("Reveal")]
    [SerializeField] private float revealDuration = 0.5f;

    [Header("On Capture")]
    [SerializeField] private GhostCaptureResponse captureResponse = GhostCaptureResponse.Dissolve;

    public enum GhostCaptureResponse
    {
        Dissolve,
        Nothing
    }

    private Vector3 _startPosition;
    private float _patrolDir = 1f;
    private SpriteRenderer _spriteRenderer;
    private bool _revealed = false;

    private void Start()
    {
        _startPosition = transform.position;
        _spriteRenderer = GetComponent<SpriteRenderer>();

        // Start invisible
        SetAlpha(0f);

        CameraApp cam = FindObjectOfType<CameraApp>();
        if (cam != null)
            cam.OnPhotoTaken += OnPhotoTaken;
    }

    private void OnDestroy()
    {
        CameraApp cam = FindObjectOfType<CameraApp>();
        if (cam != null)
            cam.OnPhotoTaken -= OnPhotoTaken;
    }

    private void Update()
    {
        if (!patrolHorizontal)
            return;

        transform.Translate(Vector3.right * _patrolDir * moveSpeed * Time.deltaTime);

        float dist = transform.position.x - _startPosition.x;
        if (Mathf.Abs(dist) >= patrolDistance)
            _patrolDir *= -1f;
    }

    private void OnPhotoTaken(bool ghostCaptured, GameObject ghost)
    {
        if (!ghostCaptured || ghost != gameObject)
            return;

        switch (captureResponse)
        {
            case GhostCaptureResponse.Dissolve:
                if (!_revealed)
                    StartCoroutine(RevealThenDissolve());
                else
                    StartCoroutine(DissolveAndDestroy());
                break;
            case GhostCaptureResponse.Nothing:
                if (!_revealed)
                    StartCoroutine(Reveal());
                Debug.Log("[GhostController] " + name + " ignored the camera.");
                break;
        }
    }

    private IEnumerator Reveal()
    {
        _revealed = true;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / revealDuration;
            SetAlpha(Mathf.Lerp(0f, 1f, t));
            yield return null;
        }

        SetAlpha(1f);
    }

    private IEnumerator RevealThenDissolve()
    {
        yield return StartCoroutine(Reveal());
        yield return StartCoroutine(DissolveAndDestroy());
    }

    private IEnumerator DissolveAndDestroy()
    {
        _revealed = false;
        float t = 0f;
        float duration = 0.8f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            SetAlpha(Mathf.Lerp(1f, 0f, t));
            yield return null;
        }

        Destroy(gameObject);
    }

    private void SetAlpha(float alpha)
    {
        if (_spriteRenderer == null)
            return;

        Color c = _spriteRenderer.color;
        c.a = alpha;
        _spriteRenderer.color = c;
    }
}