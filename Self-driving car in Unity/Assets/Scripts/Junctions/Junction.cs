using System.Collections.Generic;
using UnityEngine;

public abstract class Junction : MonoBehaviour
{
    public static float speedLimit = 3;
    protected List<(CarController, (Node, Node))> canGo = new List<(CarController, (Node, Node))>();
    protected List<(CarController, (Node, Node))> cantGo = new List<(CarController, (Node, Node))>();
    public void Enter(CarController car, (Node, Node) path)
    {
        car.speedLimit = speedLimit;
        cantGo.Add((car, path));
        EvaluateCars();
    }
    public void Exit(CarController car, (Node, Node) path)
    {
        car.speedLimit = car.maxVelocity;
        canGo.Remove((car, path));
        EvaluateCars();
    }
    protected abstract void EvaluateCars();
}
