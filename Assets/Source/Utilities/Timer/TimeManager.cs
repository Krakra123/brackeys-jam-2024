using UnityEngine;

public class TimeManager : GenericSingleton<TimeManager>
{
    private float _timer;
    private bool _isPaused;

    public bool IsPaused { get => _isPaused; }

    private void FixedUpdate()
    {
        _timer += Time.fixedDeltaTime;
    }

    /// <summary>
    /// Add time of a Timer
    /// </summary>
    /// <param name="amountSecond">Time in second want to add</param>
    public void AddTime(float amountSecond)
    {
        _timer += amountSecond;
    }

    /// <summary>
    /// Set time of a Timer
    /// </summary>
    /// <param name="timeSecond">Time in second want to set</param>
    public void SetTime(float timeSecond)
    {
        _timer = timeSecond;
    }

    /// <summary>
    /// Pause a Timer
    /// </summary>
    public void PauseTimer()
    {
        _isPaused = true;
    }

    /// <summary>
    /// Unpause a Timer
    /// </summary>
    public void UnpauseTimer()
    {
        _isPaused = false;
    }

    /// <summary>
    /// Unpause a Timer if it paused, Pause a Timer if it not paused
    /// </summary>
    public void SwitchTimerState()
    {
        _isPaused = !_isPaused;
    }
}
