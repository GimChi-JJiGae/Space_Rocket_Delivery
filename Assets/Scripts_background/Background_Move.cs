using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_Move : MonoBehaviour
{
    public float speed = 20f;

    public Transform SpaceShipObj;
    public List<Transform> waypoints = new List<Transform>();
   

    private int currentWaypointIndex = 0;



    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("WayPoint"))
            {
                waypoints.Add(child);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWaypointIndex < waypoints.Count)
        {
            Transform currentWaypoint = waypoints[currentWaypointIndex];
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, speed * Time.deltaTime);

            // Waypoint에 도달하면 다음 Waypoint로 이동
            if (transform.position == currentWaypoint.position)
            {
                currentWaypointIndex++;
            }
        }


        /*
     if (transform.position.z < -400)
     {
         if (transform.position.z > -550)
         {
             transform.RotateAround(SpaceShipObj.position, Vector3.up, 3f*Time.deltaTime);
             //transform.Rotate(Vector3.up * Time.deltaTime * 1.6f);
         }
     }

     if (transform.position.z < - 1900)
     {
         if (transform.position.z > -2100)
         {
             transform.RotateAround(SpaceShipObj.position, -Vector3.up, 8f * Time.deltaTime);
         }
     }


     if (transform.position.z > -30)
     {
         transform.Translate(-Vector3.forward * Time.deltaTime * speed, SpaceShipObj);

         if (transform.position.z < -20)
         {

             transform.Rotate(Vector3.up * Time.deltaTime * 10f);
         }
         Vector3 temp = Vector3.Normalize(transform.position);


     }




     else
     {


         transform.Translate(-Vector3.forward * speed * Time.deltaTime, SpaceShipObj);
         //transform.Translate(Vector3.forward * Time.deltaTime * -speed);
         //Debug.Log(Vector3.forward);
         //Debug.Log(transform.position);
         //Vector3 d = new Vector3(0, 0, transform.eulerAngles.z);

         //transform.Translate(new_direction * Time.deltaTime * speed);
     }

 }
     */


    }
}
