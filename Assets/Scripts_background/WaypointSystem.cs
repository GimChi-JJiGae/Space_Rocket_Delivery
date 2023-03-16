using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaypointSystem : MonoBehaviour
{
    public Transform WayPoints;
    public List<Transform> waypoints = new List<Transform>();
    public float speed = 20f;

    private int currentWaypointIndex = 0;

    void Start()
    {
        // Waypoint�� ã�� ����Ʈ�� �߰�
        foreach (Transform child in WayPoints.transform)
        {
            if (child.CompareTag("WayPoint"))
            {
                waypoints.Add(child);
            }
        }
        Debug.Log(waypoints.Count);
    }

    void Update()
    {
        // ���� Waypoint�� �̵�
        if (currentWaypointIndex < waypoints.Count)
        {
            Transform currentWaypoint = waypoints[currentWaypointIndex];
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, speed * Time.deltaTime);

            // Waypoint�� �����ϸ� ���� Waypoint�� �̵�
            if (transform.position == currentWaypoint.position)
            {
                currentWaypointIndex++;
            }
        }
    }
}