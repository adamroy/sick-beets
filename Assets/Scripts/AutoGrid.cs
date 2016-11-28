using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

// Grid is assumed x-z axis aligned!
public class AutoGrid : MonoBehaviour
{
    // Make the same for all items that belong to the same grid
    public string gridKey;

    // Different grid/cols cannot be closer than this, 
    // same grid/cols cannot be further than this
    public float gridMargin = 0.5f;
    
    public List<AutoGrid> Neighbors { private set; get; }

    private void Awake()
    {
        CalculateNeighbors2();
    }

    private void CalculateNeighbors()
    {
        var grid = FindObjectsOfType<AutoGrid>().Where(g => g.gridKey == this.gridKey && g != this);
        var chebyshevDist = new Func<AutoGrid, float>(g => Mathf.Max(Mathf.Abs(g.transform.position.x - this.transform.position.x), Mathf.Abs(g.transform.position.z - this.transform.position.z)));
        var chebyshevSort = grid.OrderBy(chebyshevDist);
        float currDist = 0;
        int numJumps = 0;
        this.Neighbors = chebyshevSort.TakeWhile(g =>
        {
            float dist = chebyshevDist(g);
            if (Mathf.Abs(dist - currDist) > gridMargin)
            {
                numJumps++;
                currDist = dist;
                if (numJumps > 1) return false;
            }
            return true;
        }).ToList();
    }

    // Calculates neighbors if they are immediately adjacent (no diagonals)
    private void CalculateNeighbors2()
    {
        var grid = FindObjectsOfType<AutoGrid>().Where(g => g.gridKey == this.gridKey && g != this);
        var cartesianDist = new Func<AutoGrid, float>(g => Mathf.Max(Mathf.Abs(g.transform.position.x - this.transform.position.x), Mathf.Abs(g.transform.position.z - this.transform.position.z)));
        var cartesianSort = grid.OrderBy(cartesianDist);
        this.Neighbors = cartesianSort.TakeWhile(g =>
        {
            var pos = this.transform.position;
            var otherPos = g.transform.position;
            var diff = pos - otherPos;
            return Mathf.Abs(diff.x) < gridMargin ^ Mathf.Abs(diff.z) < gridMargin;

        }).ToList();
    }

    // Gets all the neighbors at specified distance and then get the component T
    public IEnumerable<T> GetNeighbors<T>(int distance)
    {
        var ret = new List<AutoGrid>();
        var seen = new HashSet<AutoGrid>();
        var queue = new Queue<AutoGrid>();
        var depth = new Dictionary<AutoGrid, int>();
        queue.Enqueue(this);
        depth.Add(this, 0);

        while(queue.Count != 0)
        {
            var current = queue.Dequeue();
            if (seen.Contains(current))
                continue;
            seen.Add(current);

            int d = depth[current];
            if (d == distance)
                ret.Add(current);
            if (d > distance)
                break;

            foreach (var g in current.Neighbors)
            {
                queue.Enqueue(g);
                if (!depth.ContainsKey(g))
                    depth.Add(g, d + 1);
            }
        }

        return ret.Select(g => g.GetComponent<T>());
    }

    // Picks a random grid element from all attached
    public List<T> GetAllAttached<T>()
    {
        var seen = new HashSet<AutoGrid>();
        var queue = new Queue<AutoGrid>();
        queue.Enqueue(this);

        while (queue.Count != 0)
        {
            var current = queue.Dequeue();
            if (seen.Contains(current))
                continue;
            seen.Add(current);

            foreach (var g in current.Neighbors)
                queue.Enqueue(g);
        }

        return seen.Select(g => g.GetComponent<T>()).ToList();

        // This one liner gets all AutoGrid elements belonging to the same grid whether they are neighbors or not
        //return GameObject.FindObjectsOfType<AutoGrid>().ToList().Where(g => g.gridKey == this.gridKey).Select(g => g.GetComponent<T>()).ToList();
    }
}
