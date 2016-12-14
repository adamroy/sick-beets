using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BeetGenerator : MonoBehaviour
{
    public Beet[] beetPrefabs;
    public AutoGrid gridRoot;
    public float generationRate;

    private List<BeetPot> pots;

    private void Start()
    {
        pots = gridRoot.GetAllAttached<BeetPot>();
        // Shuffle
        for (int i = 0; i < pots.Count - 1; i++)
        {
            var index = Random.Range(i + 1, pots.Count);
            var temp = pots[i];
            pots[i] = pots[index];
            pots[index] = temp;
        }
        InvokeRepeating("GenerateBeet", 0f, 10f);
    }

    private void GenerateBeet()
    {
        var emptyPot = GetEmptyPot();
        if (emptyPot == null) return;
        int index = Random.Range(0, beetPrefabs.Length);
        var beet = Instantiate(beetPrefabs[index].gameObject).GetComponent<Beet>();
        emptyPot.SetBeet(beet);
    }

    private BeetPot GetEmptyPot()
    {
        return pots.FirstOrDefault(p => p.IsEmpty);
    }
}
