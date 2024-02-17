using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : GenericSingleton<GameManager>
{
    public PlayerManager playerManager;

    [SerializeField]
    private Animator _screenCoverAnimator;

    [SerializeField]
    private float _startDelay;
    [SerializeField]
    private float _endDelay;

    public bool levelOver { get; private set; }

    public float maxVelocity { get; private set; }
    public float sumVelocity { get; private set; }
    private int _mediumCountInstance = 0;

    private float _bonusTimer;

    private float _gameTimer;
    public float gameTime { get => _gameTimer; }
    private bool _startTimer = false;

    private void Start()
    {
        _startTimer = false;
        StartLevel();
        levelOver = false;
        _bonusTimer = 100f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) Restart();

        if (!levelOver)
        {
            maxVelocity = Mathf.Max(maxVelocity, playerManager.movementController.currentVelocity);
            sumVelocity += playerManager.movementController.currentVelocity;
            _mediumCountInstance++;
        }
    }

    private void FixedUpdate()
    {
        _bonusTimer -= Time.deltaTime;

        if (_startTimer)
        {
            _gameTimer += Time.deltaTime;
        }
    }

    public void StartTimer()
    {
        _startTimer = true;
    }

    public void NextLevel()
    {
        if (!levelOver)
        {
            StartCoroutine(NextLevelCoroutine());
        }
    }

    private IEnumerator NextLevelCoroutine()
    {
        levelOver = true;
        _startTimer = false;

        playerManager.inputManager.LockControl();

        ScoreManager.Instance.AddScoreQueue((int)maxVelocity * 2, $"Max Velocity\n+{(int)maxVelocity} x2");
        ScoreManager.Instance.AddScoreQueue((int)(sumVelocity / _mediumCountInstance) * 2, $"Average Velocity\n+{(int)(sumVelocity / _mediumCountInstance)} x2");
        if ((int)(_bonusTimer * 2f) > 0)
        {
            ScoreManager.Instance.AddScoreQueue((int)(_bonusTimer * 2f), $"Time Bonus\n+{(int)(_bonusTimer * 2f)}");
        }

        ScoreManager.Instance.AddTime(gameTime);

        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => !ScoreManager.Instance.onDisplaying);

        _screenCoverAnimator.Play("Shoot");

        yield return new WaitForSeconds(_endDelay);

        AsyncOperation nextSceneLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void StartLevel()
    {
        StartCoroutine(StartLevelCoroutine());
    }

    private IEnumerator StartLevelCoroutine()
    {
        playerManager.spriteRenderer.color = new Color(0f, 0f, 0f, 0f);
        _screenCoverAnimator.Play("ReShoot");
        playerManager.inputManager.LockControl();

        yield return new WaitForSeconds(_startDelay);

        playerManager.spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        playerManager.inputManager.UnlockControl();
    }

    public void Restart()
    {
        if (!levelOver)
        {
            StartCoroutine(RestartLevelCoroutine());
        }
    }

    private IEnumerator RestartLevelCoroutine()
    {
        levelOver = true;

        _screenCoverAnimator.Play("Shoot");

        yield return new WaitForSeconds(_endDelay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
