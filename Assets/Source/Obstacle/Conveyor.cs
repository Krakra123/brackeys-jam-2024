using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField]
    private float _cooldown;

    [SerializeField]
    private float _velocityBoost;
    [SerializeField]
    private float _direction;

    private bool _used;

    private void Use()
    {
        _used = true;

        GameManager.Instance.playerManager.movementController.SwitchDirection(Vector2.right * _direction);
        GameManager.Instance.playerManager.motionManager.AddBonusVelocity(Vector2.right * _direction * _velocityBoost);

        StartCoroutine(Recover());
    }

    private IEnumerator Recover()
    {
        yield return new WaitForSeconds(_cooldown);

        _used = false;
    }

    private void Start()
    {
        _used = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (_used) return;

        if (other.tag == "Player")
        {
            Use();
        }
    }
}
