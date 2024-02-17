using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : GenericSingleton<ScoreManager>
{
    private int _finalScore;
    public int finalScore { get => _finalScore; }
    public int displayScore { get; private set; }
    
    private float _finalTime;
    public float finalTime { get => _finalTime; }

    [SerializeField]
    private float _scoreDisplayDelay;
    private float _scoreDisplayTimer;

    [SerializeField]
    private float _messageDisplayTime;

    [SerializeField]
    private GameObject _scorePopupPrototype;

    private int _queueCount = 0;

    private Queue<string> _messageDisplayQueue = new();
    private Queue<int> _scoreQueue = new();
    public bool onDisplaying { get; private set; }

    public void AddScoreQueue(int ammout, string message)
    {
        _finalScore += ammout;

        _messageDisplayQueue.Enqueue(message);
        _scoreQueue.Enqueue(ammout);
    }

    public void AddScoreRaw(int ammout, string message)
    {
        _finalScore += ammout;
        displayScore += ammout;

        StartCoroutine(DisplayMessageRaw(message));
    }

    public void AddTime(float ammout)
    {
        _finalTime += ammout;
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

                displayScore += _scoreQueue.Peek();
                _scoreQueue.Dequeue();
            }
        }
    }

    private IEnumerator DisplayMessage(string message)
    {
        _queueCount++;
        ScorePopup popup = Instantiate(_scorePopupPrototype, GameManager.Instance.playerManager.transform.position, Quaternion.identity).GetComponent<ScorePopup>();
        popup.Display(message);

        AudioManager.Instance.PlaySound(GameSound.Ding);
        
        yield return new WaitForSeconds(_messageDisplayTime);

        _queueCount--;
        if (_queueCount == 0) onDisplaying = false;
    }

    private IEnumerator DisplayMessageRaw(string message)
    {
        _queueCount++;
        ScorePopup popup = Instantiate(_scorePopupPrototype, GameManager.Instance.playerManager.transform.position, Quaternion.identity).GetComponent<ScorePopup>();
        popup.Display(message);
        
        yield return new WaitForSeconds(_messageDisplayTime);

        _queueCount--;
        if (_queueCount == 0) onDisplaying = false;
    }
}
