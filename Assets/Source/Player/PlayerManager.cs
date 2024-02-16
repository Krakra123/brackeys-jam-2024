using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{   
    public Rigidbody2D body;

    public PlayerInputManager inputManager;
    public PlayerMotionManager motionManager;
    public PlayerMovementController movementController;
    public PlayerAnimationController pAnimation;
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        inputManager.manager = this;
        motionManager.manager = this;
        movementController.manager = this;
        pAnimation.manager = this;

        inputManager.DelegateStart();
        
        movementController.DelegateStart();
        motionManager.DelegateStart();

        pAnimation.DelegateStart();
    }

    private void Update()
    {
        inputManager.DelegateUpdate();

        movementController.DelegateUpdate();
        motionManager.DelegateUpdate();
        
        pAnimation.DelegateUpdate();
    }

    private void FixedUpdate()
    {
        inputManager.DelegateFixedUpdate();

        movementController.DelegateFixedUpdate();
        motionManager.DelegateFixedUpdate();
        
        // animation.DelegateFixedUpdate();
    }
}
