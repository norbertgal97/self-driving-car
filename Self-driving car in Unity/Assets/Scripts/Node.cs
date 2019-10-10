using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{   
    public List<Node> Neighbours { get { return _neighbours; } }
    public List<Node> ShortestPath { get; set; } = new List<Node>();
    public float DistanceFromSource { get; set; } = float.MaxValue;

    [SerializeField]
    private List<Node> _neighbours = new List<Node>();

    public float GetWeightOf(Transform neighbour)
    {
        return Vector3.Distance(transform.position, neighbour.position);
    }

    public void Reset()
    {
        ShortestPath.Clear();
        DistanceFromSource = float.MaxValue;
    }
}
