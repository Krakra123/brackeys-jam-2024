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

    private bool levelOver;

    public float maxVelocity { get; private set; }
    public float sumVelocity { get; private set; }
    private int _mediumCountInstance = 0;

    private void Start()
    {
        StartLevel();
        levelOver = false;
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

    public void NextLevel()
    {
        StartCoroutine(NextLevelCoroutine());
    }

    private IEnumerator NextLevelCoroutine()
    {
        levelOver = true;

        playerManager.inputManager.LockControl();

        ScoreManager.Instance.AddScore((int)maxVelocity, $"Max Velocity: + {(int)maxVelocity}");
        ScoreManager.Instance.AddScore((int)(sumVelocity / _mediumCountInstance), $"Average Velocity: + {(int)(sumVelocity / _mediumCountInstance)}");

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
