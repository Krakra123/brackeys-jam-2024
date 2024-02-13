using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public PlayerManager manager { get; set; }

    private int _horizontalDirection;
    public int horizontalDirection { get => _horizontalDirection; }

    private bool _jump;
    public bool jump { get => _jump; }

    private bool _click;
    public bool click { get => _click; }

    private Vector2 _cursorDirection;
    public Vector2 cursorDirection { get => _cursorDirection; }

    public void DelegateStart()
    {

    }

    public void DelegateUpdate()
    {
        _horizontalDirection = 0;
        _jump = false;
        _click = false;

        if (Input.GetButton("Right"))
        {
            _horizontalDirection = 1;
        }
        if (Input.GetButton("Left"))
        {
            _horizontalDirection = -1;
        }
        if (Input.GetButton("Right") && Input.GetButton("Left"))
        {
            _horizontalDirection = 0;
        }

        if (Input.GetButtonDown("Jump"))
        {
            _jump = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _click = true;
        }

        _cursorDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - manager.transform.position;
        _cursorDirection = _cursorDirection.normalized;
    }

    public void DelegateFixedUpdate()
    {
        
    }
}
