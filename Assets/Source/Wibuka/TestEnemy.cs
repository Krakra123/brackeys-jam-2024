using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    [SerializeField] private Enemy Enemy;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Enemy.Kill();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Enemy.Reset();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Enemy.Kick(Vector2.right);
        }
    }
}
