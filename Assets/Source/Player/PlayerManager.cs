using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{   
    public Rigidbody2D body;

    public PlayerInputManager inputManager;
    public PlayerMotionManager motionManager;
    public PlayerMovementController movementController;

    private void Start()
    {
        inputManager.manager = this;
        motionManager.manager = this;
        movementController.manager = this;

        inputManager.DelegateStart();
        
        movementController.DelegateStart();
        motionManager.DelegateStart();
    }

    private void Update()
    {
        inputManager.DelegateUpdate();

        movementController.DelegateUpdate();
        motionManager.DelegateUpdate();
    }

    private void FixedUpdate()
    {
        inputManager.DelegateFixedUpdate();

        movementController.DelegateFixedUpdate();
        motionManager.DelegateFixedUpdate();
    }
}
