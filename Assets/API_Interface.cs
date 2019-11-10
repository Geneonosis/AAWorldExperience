using System.Net;
using System;
using System.IO;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class API_Interface : MonoBehaviour
{
    public Flight[] flights;
    // Start is called before the first frame updat
    void Awake()
    {
        String requestURL = "https://aairdata.herokuapp.com/flights?date={0}";
        String Date = "2020-01-01";

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format(requestURL, Date));
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();

		Debug.Log(jsonResponse);
		flights = JsonHelper.getJsonArray<Flight>(jsonResponse);

		Debug.Log("Flight[0]: \n"+flights[0]);
		//Debug.Log("Flight[1]: \n" + flights[1]);
	}
}

[Serializable]
public class Flight
{
    public string flightNumber;
    public Airport origin;
	public Airport destination;
	public int distance;
	public Duration duration;
	public Aircraft aircraft;

    public override string ToString()
	{
		return "Flight#: " + this.flightNumber + "\n" + origin +"\n"+ destination + "Distance: " + distance + " " + duration + aircraft;
	}
}

[Serializable]
public class Airport
{
	public string code;
	public string city;
    public Coodinates location;

	public override string ToString()
	{
		return "City: " + city + "\n" + "code: " + code + location;
	}

}

[Serializable]
public class Coodinates
{
    public float latitude;
    public float longitude;
    

	public override string ToString()
	{
		return "(" + latitude + " , " + longitude + ")";
	}
}

[Serializable]
public class Duration
{
    public string locale;
    public int hours;
	public int minutes;

	public override string ToString()
	{
		return locale;
	}
}

[Serializable]
public class Aircraft
{
	public string model;
	public PassengerCapacity passengerCapacity;
	public int speed;

	public override string ToString()
	{
		return "Model: " + model + "," + passengerCapacity + "Speed: " + speed;
	}
}

[Serializable]
public class PassengerCapacity
{
	public int total;
	public int main;
	public int first;
	public override string ToString()
	{
		return " "+total;
	}
}

public class JsonHelper
{
	public static T[] getJsonArray<T>(string json)
	{
		string newJson = "{ \"array\": " + json + "}";
		Debug.Log(newJson);
		Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);

		return wrapper.array;
	}

	[Serializable]
	private class Wrapper<T>
	{
		public T[] array;
	}
}

