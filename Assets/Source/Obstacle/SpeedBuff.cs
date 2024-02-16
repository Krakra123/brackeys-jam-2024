using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _renderer;
    
    [SerializeField]
    private float _cooldown;

    [SerializeField]
    private float _speedBoost;

    private bool _used;

    private void Consume()
    {
        _used = true;
        _renderer.color = new Color(0f, 0f, 0f, 0f);
        GameManager.Instance.playerManager.movementController.AddSpeedBoost(_speedBoost);

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(_cooldown);

        _used = false;
        _renderer.color = new Color(1f, 1f, 1f, 1f);
    }

    private void Start()
    {
        _used = false;
        _renderer.color = new Color(1f, 1f, 1f, 1f);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (_used) return;

        if (other.tag == "Player")
        {
            Consume();
        }
    }
}
