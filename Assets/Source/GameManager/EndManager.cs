using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour
{
    public Animator _coverAnimator;

    private bool go = false;

    private void Start()
    {
        go = false;

        _coverAnimator.Play("ReShoot");
        AudioManager.Instance.PlaySound(GameSound.DoorKick);
    }

    private void Update()
    {
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            if (!go) StartCoroutine(GoGame());
        }
    }

    public IEnumerator GoGame()
    {
        go = true;
        _coverAnimator.Play("Shoot");
        AudioManager.Instance.PlaySound(GameSound.Close);

        yield return new WaitForSeconds(.5f);

        ScoreManager.Instance.ResetAll();

        SceneManager.LoadScene(1);
    }
}
