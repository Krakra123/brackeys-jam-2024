using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField]
    private float _yBound;

    private void Update()
    {
        if (GameManager.Instance.playerManager.transform.position.y < _yBound)
        {
            GameManager.Instance.Restart();
        }
    }
}
