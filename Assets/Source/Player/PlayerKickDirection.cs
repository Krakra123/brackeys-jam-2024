using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKickDirection : MonoBehaviour
{
    [SerializeField]
    private GameObject _arrowPivotObject;
    [SerializeField]
    private SpriteRenderer _renderer;

    private PlayerManager _manager;

    private void Start()
    {
        _manager = GetComponent<PlayerManager>();
    }

    private void Update()
    {
        if (_manager.movementController.canKick && !_manager.inputManager.controlLockState)
        {
            _renderer.color = new Color(1f, 1f, 1f, 1f);
        }
        else 
        {
            _renderer.color = new Color(0f, 0f, 0f, 0f);
        }

        float angle = Mathf.Atan2(_manager.inputManager.cursorDirection.y, _manager.inputManager.cursorDirection.x) * Mathf.Rad2Deg;

        _arrowPivotObject.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
