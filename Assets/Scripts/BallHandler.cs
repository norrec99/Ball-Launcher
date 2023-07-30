using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform pivot;
    [SerializeField] private float detachDelay = 0.1f;
    [SerializeField] private float ballSpawnDelay = 1f;
    
    private SpringJoint2D currentBallSpringJoint;
    private Rigidbody2D currentBallRigidbody;
    private Camera mainCamera;

    private bool isDragging;

    // Start is called before the first frame update
    void Awake()
    {
        mainCamera = Camera.main;       
    }

    private void Start() 
    {
        SpawnBall();
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

    private void SpawnBall()
    {
        GameObject ball = Instantiate(ballPrefab, pivot.position, Quaternion.identity);

        currentBallRigidbody = ball.GetComponent<Rigidbody2D>();
        currentBallSpringJoint = ball.GetComponent<SpringJoint2D>();

        currentBallSpringJoint.connectedBody = pivot.GetComponent<Rigidbody2D>();        
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

        Invoke(nameof(SpawnBall), ballSpawnDelay);
    }
}
