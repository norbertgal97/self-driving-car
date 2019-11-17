using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public static int noteID = 0;
    public int ID = 0;
    public List<Node> shortestPath = new List<Node>();
    public float distanceFromSource = float.MaxValue;
    public List<Node> neighbours = new List<Node>();

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
        shortestPath.Clear();
        distanceFromSource = float.MaxValue;
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
