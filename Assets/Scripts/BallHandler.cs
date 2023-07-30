using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

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

    private void OnEnable() 
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable() 
    {
        EnhancedTouchSupport.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBallRigidbody == null) { return; }

        if (Touch.activeTouches.Count == 0)
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

        Vector2 touchPosition = new Vector2();

        foreach (Touch touch in Touch.activeTouches)
        {
            touchPosition += touch.screenPosition;
        }

        touchPosition /= Touch.activeTouches.Count;

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
