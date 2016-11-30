using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EventSubscriber))]
public class Beet : MonoBehaviour
{
    private int age = 0;

	public bool HandleTouch()
    {
        return false;
    }

    private void UpdateSimulation(int deltaMillis)
    {
        age += deltaMillis;
    }
}
