using TMPro;
using UnityEngine;

public class PhoneTimeManager : MonoBehaviour
{
    [Header("Time")]
    [SerializeField] private int hour = 12;
    [SerializeField] private int minute = 0;

    [Header("Normal Time")]
    [SerializeField] private bool normalTimeRunning = true;
    [SerializeField] private float normalSecondsPerGameMinute = 60f;

    [Header("Fast Time")]
    [SerializeField] private bool fastTimeRunning;
    [SerializeField] private float fastSecondsPerGameMinute = 0.25f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timeText;

    private float timer;

    private void Start()
    {
        RefreshUI();
    }

    private void Update()
    {
        if (!normalTimeRunning && !fastTimeRunning)
            return;

        float speed = fastTimeRunning
            ? fastSecondsPerGameMinute
            : normalSecondsPerGameMinute;

        timer += Time.deltaTime;

        if (timer >= speed)
        {
            timer = 0f;
            AddMinutes(1);
        }
    }

    public void SetTime(int newHour, int newMinute)
    {
        hour = Mathf.Clamp(newHour, 0, 23);
        minute = Mathf.Clamp(newMinute, 0, 59);
        RefreshUI();
    }

    public void AddMinutes(int amount)
    {
        minute += amount;

        while (minute >= 60)
        {
            minute -= 60;
            hour++;
        }

        while (hour >= 24)
            hour -= 24;

        RefreshUI();
    }

    public void StartNormalTime()
    {
        normalTimeRunning = true;
        fastTimeRunning = false;
        timer = 0f;
    }

    public void StartFastTime()
    {
        normalTimeRunning = false;
        fastTimeRunning = true;
        timer = 0f;
    }

    public void StopTime()
    {
        normalTimeRunning = false;
        fastTimeRunning = false;
        timer = 0f;
    }

    public void SyncTime()
    {
        SetTime(12, 0);
        StartNormalTime();
    }

    private void RefreshUI()
    {
        if (timeText != null)
            timeText.text = hour.ToString("00") + ":" + minute.ToString("00");
    }
}