using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : GenericSingleton<ScoreManager>
{
    private int _finalScore;
    public int finalScore { get => _finalScore; }

    [SerializeField]
    private float _scoreDisplayDelay;
    private float _scoreDisplayTimer;

    [SerializeField]
    private float _messageDisplayTime;

    private Queue<string> _messageDisplayQueue = new();
    public bool onDisplaying { get; private set; }

    public void AddScore(int ammout, string message)
    {
        _finalScore += ammout;

        _messageDisplayQueue.Enqueue(message);
    }

    private void Update()
    {
        _scoreDisplayTimer -= Time.deltaTime;

        if (_scoreDisplayTimer <= 0f)
        {
            if (_messageDisplayQueue.Count > 0)
            {
                onDisplaying = true;

                StartCoroutine(DisplayMessage(_messageDisplayQueue.Peek()));
                _messageDisplayQueue.Dequeue();

                _scoreDisplayTimer = _scoreDisplayDelay;
            }
        }
    }

    private IEnumerator DisplayMessage(string message)
    {
        Debug.Log(message);
        
        yield return new WaitForSeconds(_messageDisplayTime);

        if (_messageDisplayQueue.Count == 0) onDisplaying = false;
    }
}
