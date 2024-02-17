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

    private float _timer;

    private void Start()
    {
        StartLevel();
        levelOver = false;
        _timer = 100f;
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
        _timer -= Time.deltaTime;
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

        playerManager.inputManager.LockControl();

        ScoreManager.Instance.AddScoreQueue((int)maxVelocity * 2, $"Max Velocity\n+{(int)maxVelocity} x2");
        ScoreManager.Instance.AddScoreQueue((int)(sumVelocity / _mediumCountInstance) * 2, $"Average Velocity\n+{(int)(sumVelocity / _mediumCountInstance)} x2");
        if ((int)(_timer * 2f) > 0)
        {
            ScoreManager.Instance.AddScoreQueue((int)(_timer * 2f), $"Time Bonus\n+{(int)(_timer * 2f)}");
        }

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
        _screenCoverAnimator.Play("ReShoot");
        playerManager.inputManager.LockControl();

        yield return new WaitForSeconds(_startDelay);

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
