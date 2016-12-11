using UnityEngine;
using System.Collections;

// Controls navigation between game screens
// Swipe horizontally to switch screens or use buttons
[RequireComponent(typeof(Camera))]
public class ScreenNavigator : MonoBehaviour
{
    // Where the camera should stop to be on different screens
    public Transform[] cameraPositions;

    private new Camera camera;
    private Coroutine movementCoroutine;

    private void Start()
    {
        camera = GetComponent<Camera>();
        StartCoroutine(NavigationCoroutine());
    }

    private IEnumerator NavigationCoroutine()
    {
        while(true)
        {
            if(Input.GetMouseButtonDown(0))
            {
                if (movementCoroutine != null) StopCoroutine(movementCoroutine);
                var initialPosition = camera.ScreenToWorldPoint(Input.mousePosition).x;

                while(!Input.GetMouseButtonUp(0))
                {
                    var deltaPosition = initialPosition - camera.ScreenToWorldPoint(Input.mousePosition).x;
                    transform.Translate(Vector3.right * deltaPosition, Space.Self);
                    yield return null;
                }

                Transform targetPosition = null;
                float minDistance = float.PositiveInfinity;
                foreach(var transf in cameraPositions)
                {
                    var distance = Vector3.Distance(transform.position, transf.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        targetPosition = transf;
                    }
                }

                movementCoroutine = StartCoroutine(SnapToPosition(targetPosition));
            }

            yield return null;
        }
    }

    private IEnumerator SnapToPosition(Transform target)
    {
        while (Vector3.Distance(transform.position, target.position) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, 0.1f);
            yield return null;
        }

        movementCoroutine = null;
    }
}
