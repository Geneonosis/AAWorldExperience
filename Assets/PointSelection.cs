using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(LineRenderer))]
public class PointSelection : MonoBehaviour
{
    private string testString;
    public GameObject Line_Renderer;
    [SerializeField] GlobePosition gp;
    [SerializeField] API_Interface api;

    [SerializeField] Vector3 source;
    [SerializeField] Vector3 destination;
    [SerializeField] GameObject pointMarker;
    //[SerializeField] GameObject bezier_drawer;
    float heightAdjustment = 0.5f;//0.05f seemed nice
    int earthScale = 3;

    public int numberOfPoints = 21;
    LineRenderer lineRenderer;


    Vector3 midpoint;

    Vector3 climax;

    // Start is called before the first frame update
    void Start()
    {
        if(api == null)
        {
            Debug.LogError("api not initialized yet");
        }
        lineRenderer = GetComponent<LineRenderer>();
        //source = Random.onUnitSphere * earthScale;
        //destination = Random.onUnitSphere * earthScale;

        ArrayList prevFlights = new ArrayList();
        //prevFlights.Add()

        Debug.Log("Num Flights " + api.flights.Length);
        int i = 0;
        for (i = 0; i < 120 && i < api.flights.Length; i+=4)
        {
			Flight f = api.flights[i];
            
            source = getLocation(f.origin.location.latitude, -f.origin.location.longitude) * earthScale;
            destination = getLocation(f.destination.location.latitude, -f.destination.location.longitude) * earthScale;
            if (prevFlights.Contains((source, destination)) || prevFlights.Contains((destination, source)))
            {
                continue;
            }
            prevFlights.Add((source, destination));

            GameObject myLR = Instantiate(Line_Renderer);
            lineRenderer = myLR.GetComponent<LineRenderer>();
            drawFlight(source, destination);
            
            drawArc();
            
            GameObject plane = Instantiate<GameObject>(pointMarker, climax, new Quaternion(), GameObject.FindGameObjectWithTag("World").transform);
        }
        
        //Vector3 testVector = new Vector3(source.x, source.y, source.z);
        //Instantiate<GameObject>(pointMarker, testVector, new Quaternion(), GameObject.FindGameObjectWithTag("World").transform).transform.LookAt(GameObject.FindGameObjectWithTag("World").transform);
        //plane.transform.rotation = Quaternion.FromToRotation(source, destination);
        //plane.transform.LookAt(destination);
        //plane.transform.LookAt(destination,midpoint - GameObject.FindGameObjectWithTag("World").transform.position);

        //plane.transform.rotation = new Quaternion(plane.transform.rotation.x, plane.transform.rotation.y, z, plane.transform.rotation.w);


    }

    void drawFlight(Vector3 dSource, Vector3 dDestination)
    {
        midpoint = (dSource + dDestination) / 3;

        float travDist = Vector3.Distance(dSource, dDestination);

        float dist = Vector3.Distance(midpoint, Vector3.zero);

        float scalar = (1/(dist/earthScale)) + (heightAdjustment* travDist);

        Debug.Log(midpoint);
        midpoint.Scale(new Vector3(scalar, scalar, scalar));
        Debug.Log(midpoint);

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
            position = (1.0f - t) * (1.0f - t) * p0 + 2.0f * (1.0f - t) * t * p1 + t * t * p2;
            lineRenderer.SetPosition(i, position);
        }

        climax = lineRenderer.GetPosition(numberOfPoints / 2);
    }

    void drawArc2()
    {
        if (numberOfPoints > 0)
        {
            lineRenderer.positionCount = numberOfPoints;
        }
        lineRenderer.transform.SetParent(GameObject.FindGameObjectWithTag("World").transform);
        lineRenderer.useWorldSpace = false;

        float xradius = 3.1f;
        float yradius = 3.1f;
        float x = 0f;
        float y = 0f;
        float z;
        float angle = 20f;

        for (int i = 0; i < numberOfPoints; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;

            lineRenderer.SetPosition(i, new Vector3(x, y, z) + midpoint);

            angle += (180f / numberOfPoints);
        }
    }

    public Vector3 getLocation(float lat, float lon)
    {
        float radius = 1;

        lat = Mathf.PI* lat / 180;
        lon = Mathf.PI* lon / 180;

        // adjust position by radians	
        lat -= 1.570795765134f; // subtract 90 degrees (in radians)
        lon += 1.570795765134f; // subtract 90 degrees (in radians)
        //lon = lon;
        //lon -= 1;

        // and switch z and y (since z is forward)
        float zPos = (radius) * Mathf.Sin(lat) * Mathf.Cos(-lon);
        float xPos = (radius) * Mathf.Sin(lat) * Mathf.Sin(-lon);
        float yPos = (radius) * Mathf.Cos(lat);

        Vector3 retval = new Vector3(xPos, yPos, zPos);
        return retval;
        }

}
