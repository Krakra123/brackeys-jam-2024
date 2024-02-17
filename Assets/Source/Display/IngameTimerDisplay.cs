using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IngameTimerDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _textGUI;

    private void Update()
    {
        _textGUI.text = FormatTime(GameManager.Instance.gameTime);
    }

    private string FormatTime(float totalSeconds)
    {
        int minutes = Mathf.FloorToInt(totalSeconds / 60);
        float remainingSeconds = totalSeconds % 60;

        return string.Format("{0:00}:{1:00.00}", minutes, remainingSeconds);
    }
}
