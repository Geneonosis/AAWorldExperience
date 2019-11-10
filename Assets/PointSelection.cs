using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PointSelection : MonoBehaviour
{
	private string testString;


	[SerializeField] Vector3 source;
	[SerializeField] Vector3 destination;
	[SerializeField] GameObject pointMarker;
	//[SerializeField] GameObject bezier_drawer;
	float heightAdjustment = 1f;//0.05f seemed nice
	int earthScale = 3;

	public Color color = Color.white;
	public float width = 0.2f;
	public int numberOfPoints = 21;
	LineRenderer lineRenderer;


    Vector3 midpoint;

    Vector3 climax;

    // Start is called before the first frame update
    void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
		source = Random.onUnitSphere * earthScale;
		destination = Random.onUnitSphere * earthScale;

		drawFlight(source, destination);
        drawArc();
        //Z axis
        float z = midpoint.z;
        GameObject plane = Instantiate<GameObject>(pointMarker, climax, new Quaternion(), GameObject.FindGameObjectWithTag("World").transform);
        //Vector3 testVector = new Vector3(source.x, source.y, source.z);
        //Instantiate<GameObject>(pointMarker, testVector, new Quaternion(), GameObject.FindGameObjectWithTag("World").transform).transform.LookAt(GameObject.FindGameObjectWithTag("World").transform);
        //plane.transform.rotation = Quaternion.FromToRotation(source, destination);
        //plane.transform.LookAt(destination);
        //plane.transform.LookAt(destination,midpoint - GameObject.FindGameObjectWithTag("World").transform.position);
        
        //plane.transform.rotation = new Quaternion(plane.transform.rotation.x, plane.transform.rotation.y, z, plane.transform.rotation.w);


    }

    void drawFlight(Vector3 dSource, Vector3 dDestination)
	{
		midpoint = (dSource + dDestination) / 2;

        float travDist = Vector3.Distance(dSource, dDestination);

		float dist = Vector3.Distance(midpoint, Vector3.zero);

		float scalar = (heightAdjustment * travDist);

		midpoint.Scale(new Vector3(scalar, scalar, scalar));

		//Instantiate<GameObject>(pointMarker, dSource, new Quaternion(0, 0, 0, 0), GameObject.FindGameObjectWithTag("World").transform);
		//Instantiate<GameObject>(pointMarker, dDestination, new Quaternion(0, 0, 0, 0), GameObject.FindGameObjectWithTag("World").transform);
		//GameObject mid = Instantiate<GameObject>(pointMarker, midpoint, new Quaternion(0, 0, 0, 0), GameObject.FindGameObjectWithTag("World").transform);
	}

    void drawArc()
	{
		// update line renderer

		if (numberOfPoints > 0)
		{
			lineRenderer.positionCount = numberOfPoints;
		}
        lineRenderer.transform.SetParent(GameObject.FindGameObjectWithTag("World").transform);
        lineRenderer.useWorldSpace = false;
        // set points of quadratic Bezier curve
        Vector3 p0 = source;
        Vector3 p1 = midpoint;
        Vector3 p2 = destination;
		float t;
		Vector3 position;
		for (int i = 0; i < numberOfPoints; i++)
		{
			t = i / (numberOfPoints - 1.0f);
			position = (1.0f - t) * (1.0f - t) * p0
			+ 2.0f * (1.0f - t) * t * p1 + t * t * p2;
			lineRenderer.SetPosition(i, position);
		}

        climax = lineRenderer.GetPosition(numberOfPoints / 2);
	}
}
