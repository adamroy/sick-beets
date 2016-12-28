using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BeetGenerator : MonoBehaviour
{
    public Beet[] beetPrefabs;
    public AutoGrid gridRoot;
    public float generationRate;

    private List<BeetContainer> containers;

    private void Start()
    {
        containers = gridRoot.GetAllAttached<BeetContainer>();
        // Shuffle
        for (int i = 0; i < containers.Count - 1; i++)
        {
            var index = Random.Range(i + 1, containers.Count);
            var temp = containers[i];
            containers[i] = containers[index];
            containers[index] = temp;
        }
        InvokeRepeating("GenerateBeet", 0f, 10f);
    }

    private void GenerateBeet()
    {
        var emptyCont = GetEmptyContainer();
        if (emptyCont == null) return;
        int index = Random.Range(0, beetPrefabs.Length);
        var beet = Instantiate(beetPrefabs[index].gameObject).GetComponent<Beet>();
        beet.prefab = beetPrefabs[index].gameObject;
        emptyCont.SetBeet(beet);
    }

    private BeetContainer GetEmptyContainer()
    {
        return containers.FirstOrDefault(p => p.IsEmpty);
    }
}
