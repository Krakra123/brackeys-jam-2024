using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{   
    public Rigidbody2D body;

    public PlayerStateManager stateManager;
    public PlayerInputManager inputManager;
    public PlayerMovementController movementController;

    private void Start()
    {
        stateManager.manager = this;
        inputManager.manager = this;
        movementController.manager = this;

        stateManager.DelegateStart();
        inputManager.DelegateStart();
        movementController.DelegateStart();
    }

    private void Update()
    {
        stateManager.DelegateUpdate();
        inputManager.DelegateUpdate();
        movementController.DelegateUpdate();
    }

    private void FixedUpdate()
    {
        stateManager.DelegateFixedUpdate();
        inputManager.DelegateFixedUpdate();
        movementController.DelegateFixedUpdate();
    }
}
