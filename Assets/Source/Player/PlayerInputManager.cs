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

    public void DelegateStart()
    {

    }

    public void DelegateUpdate()
    {
        _horizontalDirection = 0;
        _jump = false;

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
    }

    public void DelegateFixedUpdate()
    {
        
    }
}
