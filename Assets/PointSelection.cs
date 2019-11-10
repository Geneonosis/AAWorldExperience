using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSelection : MonoBehaviour
{


    [SerializeField] Vector3 source;
    [SerializeField] Vector3 destination;
    [SerializeField] GameObject pointMarker;

    private string testString;
    public string TestString { get => testString; set => testString = value; }

    // Start is called before the first frame update
    void Start()
    {
        source = Random.onUnitSphere * 3;
        destination = Random.onUnitSphere * 3;

        Instantiate<GameObject>(pointMarker,source,new Quaternion(0,0,0,0),GameObject.FindGameObjectWithTag("World").transform);
        Instantiate<GameObject>(pointMarker,destination, new Quaternion(0, 0, 0, 0), GameObject.FindGameObjectWithTag("World").transform);
    }
}
