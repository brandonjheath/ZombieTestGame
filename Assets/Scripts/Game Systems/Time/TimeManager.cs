using System;
using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [Header("Time Settings")]
    public int startHour = 6;
    public float timeScale = 60f; // 1 real second = 1 in-game minute

    [Header("Current Time")]
    public int currentHour;
    public int currentMinute;
    public int currentDay;

    [Header("Sleep Settings")]
    public int forcedSleepHour = 4;
    public float sleepDurationSeconds = 2f;

    public static event Action OnDayStart;

    private float accumulator;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        currentHour = startHour;
        currentMinute = 0;
        currentDay = 1;
    }

    private void Update()
    {
        accumulator += Time.deltaTime * timeScale;
        while (accumulator >= 60f)
        {
            accumulator -= 60f;
            AdvanceMinute();
        }
    }

    private void AdvanceMinute()
    {
        currentMinute++;
        if (currentMinute < 60) return;

        // roll over minutes
        currentMinute = 0;
        currentHour++;

        // 1) Wrap the clock at midnight, but keep same day
        if (currentHour >= 24)
        {
            currentHour = 0;
        }

        // 2) When we hit forcedSleepHour exactly, do the sleep/day reset
        if (currentHour == forcedSleepHour)
        {
            StartCoroutine(DoForcedSleep());
        }
    }



    private IEnumerator DoForcedSleep()
    {
        // (Optional) fade to black, play snooze animation, etc.
        yield return new WaitForSeconds(sleepDurationSeconds);

        // Advance into the new day
        currentDay++;
        currentHour = startHour;
        currentMinute = 0;

        // Fire the event exactly once, after time reset
        StartCoroutine(InvokeDayStartNextFrame());
    }

    private IEnumerator InvokeDayStartNextFrame()
    {
        // Wait one frame so any subscribers reorder before controller re-enable
        yield return null;
        OnDayStart?.Invoke();
    }

    public string GetTimeString() => $"Day {currentDay}, {currentHour:D2}:{currentMinute:D2}";
}
