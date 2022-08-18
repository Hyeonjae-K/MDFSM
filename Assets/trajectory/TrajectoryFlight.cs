using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryFlight : Drone
{
    Vector3[] points;
    int idx = 0;

    Vector3 GetWaypoint()
    {
        idx++;
        idx %= points.Length;
        return points[idx];
    }

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();

        points = new spline().GetTrajectoryWaypoints(main.trajectoryPoints, main.trajectoryQuality);

        currentWaypoint = points[1];

        SetLocation(points[0]);
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckWaypoint())
        {
            UpdateWaypoint(GetWaypoint());
        }
        ZAxisRotate();
        Rotate(gameObject);
        Move(gameObject);
    }
}