using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PedestrianController : MonoBehaviour
{
    public UnityEvent OnDestroy;
    [SerializeField]
    private float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Translate(); 
    }

    private void Translate()
    {
        transform.Translate(0f, 0f, speed * Time.deltaTime);
    }

    public void Destroy()
    {
        Destroy(gameObject);
        OnDestroy.Invoke();
        OnDestroy.RemoveAllListeners();
    }
}
