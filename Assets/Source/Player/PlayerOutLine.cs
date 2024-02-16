using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOutLine : MonoBehaviour
{
    [SerializeField]
    private GameObject _Lineout;
    private PlayerAnimationController _playerAnim;
    public bool _outline;

    private void Start()
    {
        _playerAnim = GetComponent<PlayerAnimationController>();
    }
    void Update()
    {
        if (_outline == true)
        {
            _Lineout.SetActive(true);
            _Lineout.GetComponent<Animator>().Play(_playerAnim.currentAnimation);
            _Lineout.transform.localScale = transform.Find("Sprite").transform.localScale;
        }
        else
        {
            _Lineout.SetActive(false);
        }
    }
}
