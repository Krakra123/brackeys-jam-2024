using System.Collections;
using UnityEngine;

public class WaitForSecondsWithPause : IEnumerator
{
    private float _delayDuration;
    private float _totalTime;

    public WaitForSecondsWithPause(float delayDurationSecond)
    {
        _delayDuration = delayDurationSecond;
    }

    public object Current
    {
        get
        {
            return new WaitWhilePause();
        }
    }

    public bool MoveNext()
    {
        _totalTime += Time.deltaTime;

        if (_totalTime <= _delayDuration)
        {
            return true;
        }
        return false;
    }

    public void Reset() { }
}
