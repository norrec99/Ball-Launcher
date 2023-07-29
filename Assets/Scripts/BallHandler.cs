using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] private Rigidbody2D currentBallRigidbody;
    [SerializeField] private SpringJoint2D currentBallSpringJoint;
    [SerializeField] private float detachDelay = 0.1f;
    private Camera mainCamera;

    private bool isDragging;

    // Start is called before the first frame update
    void Awake()
    {
        mainCamera = Camera.main;       
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBallRigidbody == null) { return; }

        if (!Touchscreen.current.press.isPressed)
        {
            if (isDragging)
            {
                LauncBall();
                isDragging = false;
            }
            return;
        }

        isDragging = true;
        currentBallRigidbody.isKinematic = true;

        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

        currentBallRigidbody.position = worldPosition;


    }

    private void LauncBall()
    {
        currentBallRigidbody.isKinematic = false;
        currentBallRigidbody = null;

        Invoke(nameof(DetachBall), detachDelay);
    }

    private void DetachBall()
    {
        currentBallSpringJoint.enabled = false;
        currentBallSpringJoint = null;
    }
}
