using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManage : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    public void Kick(Vector2 KickDirection)
    {
        enemy.Kick(KickDirection);
    }

    public void Kill()
    {
        enemy.Kill();
    }

    public void Reset()
    {
        enemy.Reset();
    }
}
