using System.Collections;

public class WaitWhilePause : IEnumerator
{
    public object Current
    {
        get
        {
            return null;
        }
    }

    public bool MoveNext()
    {
        if (TimeManager.Instance.IsPaused)
        {
            return true;
        }
        return false;
    }

    public void Reset() { }
}
