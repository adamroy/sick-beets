using UnityEngine;
using System.Collections;

// Controls navigation between game screens
// Swipe horizontally to switch screens or use buttons
// Assumes x axis aligned
[RequireComponent(typeof(Camera))]
public class ScreenNavigator : MonoBehaviour
{
    // Where the camera should stop to be on different screens
    public Transform[] cameraPositions;
    public Collider activationCollider;

    private Transform currentPosition;
    private new Camera camera;
    private Coroutine movementCoroutine;

    private void Start()
    {
        InitializeCurrentPosition();
        camera = GetComponent<Camera>();
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

    private IEnumerator NavigationCoroutine()
    {
        while(true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                var ray = camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, float.PositiveInfinity) && hit.collider == activationCollider)
                {

                    if (movementCoroutine != null)
                    {
                        StopCoroutine(movementCoroutine);
                        InitializeCurrentPosition();
                    }
                    var initialPosition = camera.ScreenToWorldPoint(Input.mousePosition).x;

                    while (!Input.GetMouseButtonUp(0))
                    {
                        var deltaPosition = initialPosition - camera.ScreenToWorldPoint(Input.mousePosition).x;
                        transform.Translate(Vector3.right * deltaPosition, Space.Self);
                        yield return null;
                    }


                    var targetPosition = GetNextPosition();
                    movementCoroutine = StartCoroutine(SnapToPosition(targetPosition));
                }
            }

            yield return null;
        }
    }

    private IEnumerator SnapToPosition(Transform target)
    {
        currentPosition = target;

        while (Vector3.Distance(transform.position, target.position) > 0.1f)
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

        // If we are over one quarter the way to the nearest position, go there
        if (inCorrectDirection && distanceToCurrent / diff > 0.25f)
            return targetPosition;
        else
            return currentPosition;
    }
}
