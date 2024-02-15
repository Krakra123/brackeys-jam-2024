using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManage : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    public void kick(Vector2 KickDirection)
    {
        enemy.Kick(KickDirection);
    }

    public void kill()
    {
        enemy.Kill();
    }

    public void reset()
    {
        enemy.reset();
    }
}
