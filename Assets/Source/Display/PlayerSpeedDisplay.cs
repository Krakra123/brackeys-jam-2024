using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerSpeedDisplay : MonoBehaviour
{
    [SerializeField]
    private RectTransform _bar;
    [SerializeField]
    private TextMeshProUGUI _textGUI;

    [SerializeField]
    private PlayerMovementController _playerMotion;

    private void Update()
    {
        _textGUI.text = _playerMotion.currentVelocity.ToString("F2");
        _bar.localScale = new Vector2(_playerMotion.currentVelocity / 70f, _bar.localScale.y);
    }
}
