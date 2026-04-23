using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class PersistentMusicPlayer : MonoBehaviour
{
    public static PersistentMusicPlayer Instance;

    [Header("Audio Source")]
    [SerializeField] private AudioSource audioSource;

    [Header("Music Clips")]
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip playerOneGameMusic;
    [SerializeField] private AudioClip playerTwoGameMusic;

    [Header("Fade")]
    [SerializeField] private float fadeDuration = 1f;

    private Coroutine fadeRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        PlayMusicForScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    private void PlayMusicForScene(string sceneName)
    {
        AudioClip targetClip = GetClipForScene(sceneName);

        if (targetClip == null || audioSource == null)
            return;

        if (audioSource.clip == targetClip)
            return;

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeToTrack(targetClip));
    }

    private AudioClip GetClipForScene(string sceneName)
    {
        if (sceneName == "MainMenu")
            return mainMenuMusic;

        if (sceneName == "Game")
        {
            if (NetworkManager.Singleton == null)
                return playerOneGameMusic;

            if (NetworkManager.Singleton.IsHost)
                return playerOneGameMusic;

            if (NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsHost)
                return playerTwoGameMusic;
        }

        return null;
    }

    private System.Collections.IEnumerator FadeToTrack(AudioClip newClip)
    {
        float startVolume = audioSource.volume;

        if (audioSource.isPlaying)
        {
            float timer = 0f;

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
                yield return null;
            }
        }

        audioSource.Stop();
        audioSource.clip = newClip;
        audioSource.Play();

        float fadeInTimer = 0f;

        while (fadeInTimer < fadeDuration)
        {
            fadeInTimer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, 1f, fadeInTimer / fadeDuration);
            yield return null;
        }

        audioSource.volume = 1f;
        fadeRoutine = null;
    }
}