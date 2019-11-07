using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public static int noteID = 0;
    public List<Node> Neighbours { get { return _neighbours; } }
    public List<Node> ShortestPath { get; set; } = new List<Node>();
    public float DistanceFromSource { get; set; } = float.MaxValue;
    public int ID { get; set; } = 0;

    [SerializeField]
    private List<Node> _neighbours = new List<Node>();

    private void Awake()
    {
        ID = noteID;
        noteID++;
    }

    public float GetWeightOf(Transform neighbour)
    {
        return Vector3.Distance(transform.position, neighbour.position);
    }

    public void Reset()
    {
        ShortestPath.Clear();
        DistanceFromSource = float.MaxValue;
    }

    public override bool Equals(object other)
    {
        Node node = other as Node;

        if (node == null)
        {
            return false;
        }
        else
        {
            return ID == node.ID;
        }
    }
}
