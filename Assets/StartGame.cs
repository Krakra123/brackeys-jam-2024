using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    private bool go = false;
    public Animator _screenCoverAnimator;

    private void Start()
    {
        go = false;
    }

    private void Update()
    {
        string strInput = Input.inputString;
        if (!string.IsNullOrEmpty(strInput) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
        {
            if (!go) StartCoroutine(GoGame());
        }
    }

    public IEnumerator GoGame()
    {
        go = true;
        _screenCoverAnimator.Play("Shoot");
        AudioManager.Instance.PlaySound(GameSound.Close);

        yield return new WaitForSeconds(.5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
