using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFlip : MonoBehaviour
{
    Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = this.gameObject.GetComponent<Camera>();
        camera.projectionMatrix = camera.projectionMatrix * Matrix4x4.Scale(new Vector3(-1, 1, 1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
