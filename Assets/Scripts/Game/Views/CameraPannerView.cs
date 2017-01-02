using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;

// Controls navigation between game screens
// Swipe horizontally to switch screens or use buttons
// Assumes x axis aligned
[RequireComponent(typeof(Camera))]
public class CameraPannerView : View, IInputEnabler
{
    // Where the camera should stop to be on different screens
    public Transform[] cameraPositions;
    public float distanceToCancelClick;
    public float allowedDistanceOverBoundry;
    public float boundryGravity;
    
    private Transform currentPosition;
    private new Camera camera;
    private Coroutine movementCoroutine;
    private bool inputEnabled = true;
    
    protected override void Start()
    {
        base.Start();
        camera = GetComponent<Camera>();
        if (currentPosition != null)
            camera.transform.position = currentPosition.position;
        else
            InitializeCurrentPosition();
        StartCoroutine(NavigationCoroutine());
    }

    private void InitializeCurrentPosition()
    {
        float minDistance = float.PositiveInfinity;
        foreach (var transf in cameraPositions)
        {
            var distance = Vector3.Distance(transform.position, transf.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                currentPosition = transf;
            }
        }
    }

    private bool CanMove()
    {
        return inputEnabled;
    }
    
    private IEnumerator NavigationCoroutine()
    {
        while(true)
        {
            if (Input.GetMouseButtonDown(0) && CanMove())
            {
                RaycastHit hit;
                var ray = camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, float.PositiveInfinity))
                {
                    if (movementCoroutine != null)
                    {
                        StopCoroutine(movementCoroutine);
                        InitializeCurrentPosition();
                    }

                    var initialPosition = camera.ScreenToWorldPoint(Input.mousePosition).x;
                    float originalCameraWorldX = camera.transform.position.x;
                    int orignalLayer = hit.collider.gameObject.layer;

                    while (!Input.GetMouseButtonUp(0))
                    {
                        var deltaPosition = GetDeltaPosition(initialPosition, camera.ScreenToWorldPoint(Input.mousePosition).x);
                        initialPosition += deltaPosition - (initialPosition - camera.ScreenToWorldPoint(Input.mousePosition).x);
                        camera.transform.Translate(Vector3.right * deltaPosition, Space.Self);

                        float cameraWorldDeltaX =  originalCameraWorldX - camera.transform.position.x;
                        if (Mathf.Abs(cameraWorldDeltaX) > distanceToCancelClick && hit.collider.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast"))
                        {
                            // Moving layer to IgnoreRaycast cancels OnMouseUpAsButton()
                            // hit.collider.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                        }

                        yield return null;
                    }
                    
                    hit.collider.gameObject.layer = orignalLayer;
                    var targetPosition = GetNextPosition();
                    movementCoroutine = StartCoroutine(SnapToPosition(targetPosition));
                }
            }

            yield return null;
        }
    }

    private float GetDeltaPosition(float initialPositionX, float inputWorldX)
    {
        float delta = initialPositionX - inputWorldX;
        float destinationCameraX = camera.transform.position.x + delta;

        float minX = float.MaxValue, maxX = float.MinValue;
        foreach(var t in cameraPositions)
        {
            if (t.position.x < minX)
                minX = t.position.x;
            if (t.position.x > maxX)
                maxX = t.position.x;
        }

        if (destinationCameraX < minX)
        {
            float distanceOver = minX - destinationCameraX;
            float ratio = distanceOver / allowedDistanceOverBoundry;
            delta *= 1 / (boundryGravity * ratio + 1);
        }
        else if (destinationCameraX > maxX)
        {
            float distanceOver = destinationCameraX - maxX;
            float ratio = distanceOver / allowedDistanceOverBoundry;
            delta *= 1 / (boundryGravity * ratio + 1);
        }

        return delta;
    }

    private IEnumerator SnapToPosition(Transform target)
    {
        currentPosition = target;

        while (Vector3.Distance(transform.position, target.position) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, 0.1f);
            yield return null;
        }

        movementCoroutine = null;
    }

    private Transform GetNextPosition()
    {
        Transform targetPosition = null;
        float minDistance = float.PositiveInfinity;
        foreach (var transf in cameraPositions)
        {
            if (transf == currentPosition) continue;

            var distance = Vector3.Distance(this.transform.position, transf.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                targetPosition = transf;
            }
        }

        // targetPosition now has nearest postion aside from current
        float diff = Vector3.Distance(targetPosition.position, currentPosition.position);
        float distanceToCurrent = Vector3.Distance(currentPosition.position, this.transform.position);
        bool inCorrectDirection = (this.transform.position.x - currentPosition.position.x) * (targetPosition.position.x - currentPosition.position.x) > 0;

        // If we are over one sixth the way to the nearest position, go there
        if (inCorrectDirection && distanceToCurrent / diff > 0.15f)
            return targetPosition;
        else
            return currentPosition;
    }

    #region ITouchEnabler

    public InputLayer InputLayer { get { return InputLayer.Camera; } }

    public void SetInputEnabled(bool enabled)
    {
        inputEnabled = enabled;
    }

    #endregion
}
