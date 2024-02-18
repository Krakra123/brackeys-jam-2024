using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    public Animator _coverAnimator;

    private void Start()
    {
        _coverAnimator.Play("ReShoot");
        AudioManager.Instance.PlaySound(GameSound.DoorKick);
    }
}
