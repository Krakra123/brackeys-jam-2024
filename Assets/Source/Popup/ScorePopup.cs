using UnityEngine;
using TMPro;

public class ScorePopup : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _textGUI;

    public void Display(string text)
    {
        _textGUI.text = text;
    }

    private void Start()
    {
        Destroy(gameObject, 3f);
    }
}
