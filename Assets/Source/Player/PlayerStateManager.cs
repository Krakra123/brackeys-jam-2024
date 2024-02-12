using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public PlayerManager manager { get; set; }

    [SerializeField]
    private LayerMask _groundMask;
    [SerializeField]
    private Transform[] _groundChecks;

    private bool _onGround;
    public bool onGround { get => _onGround; }

    public void DelegateStart()
    {

    }

    public void DelegateUpdate()
    {

    }

    public void DelegateFixedUpdate()
    {
        if (Physics2D.OverlapArea(_groundChecks[0].position, _groundChecks[1].position, _groundMask))
        {
            _onGround = true;
        }
        else
        {
            _onGround = false;
        }
    }
}
