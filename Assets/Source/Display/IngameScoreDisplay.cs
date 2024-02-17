using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IngameScoreDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _textGUI;

    private void Update()
    {
        _textGUI.text = ScoreManager.Instance.displayScore.ToString();
    }
}
