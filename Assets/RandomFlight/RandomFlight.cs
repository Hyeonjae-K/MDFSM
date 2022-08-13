using UnityEngine;


public class RandomFlight : Drone
{
    [Header("Random Settings")]
    [SerializeField]
    bool isRandomSpeed = true;
    [SerializeField]
    bool isRandomTurningForece = true;
    [SerializeField]
    float waypointHeightRange = 15;


    // 속도 랜덤 조정 함수
    void RandomizeSpeed()
    {
        // minSpeed와 maxSpeed 사이의 랜덤 속도 생성
        targetSpeed = Random.Range(minSpeed, maxSpeed);
        isAcceleration = (speed < targetSpeed);

        // 생성한 속도로 조정
        currentAccelerate = 0;
        if (isAcceleration == true && speed < targetSpeed)
            currentAccelerate = accelerateAmount;
        else if (isAcceleration == false && speed > targetSpeed)
            currentAccelerate = -accelerateAmount;
        speed += currentAccelerate * Time.deltaTime;
    }

    // 선회력 랜덤 조정 함수
    void RandomizeTurn()
    {
        // turningForce의 75% ~ 100% 사이의 랜덤 선회력 생성
        currentTurningForce = Random.Range(0.75f * turningForce, turningForce);
        turningTime = 1 / currentTurningForce;

        // 생성한 선회력으로 조정
        currentTurningTime = Mathf.Lerp(currentTurningTime, turningTime, 1);
    }

    // 지정한 범위 내의 랜덤 좌표 반환
    Vector3 GetWaypoint()
    {
        Bounds bounds = areaCollider.bounds;
        float minHeight = currentWaypoint.y - waypointHeightRange < bounds.min.y ? bounds.min.y : currentWaypoint.y - waypointHeightRange;
        float maxHeight = currentWaypoint.y + waypointHeightRange > bounds.max.y ? bounds.max.y : currentWaypoint.y + waypointHeightRange;
        Vector3 waypoint = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(minHeight, maxHeight),
            Random.Range(bounds.min.z, bounds.max.z)
        );
        return waypoint;
    }

    protected override void Start()
    {
        base.Start();
        targetSpeed = defaultSpeed;
        targetSpeed = defaultSpeed;

        currentTurningForce = turningForce;
        currentTurningTime = turningTime = 1 / turningForce;

        prevRotY = 0;
        currRotY = 0;

        currentWaypoint = main.transform.position;

        UpdateWaypoint(GetWaypoint());
    }

    void Update()
    {
        if (CheckWaypoint())
        {
            UpdateWaypoint(GetWaypoint());
        }
        ZAxisRotate();
        Rotate(gameObject);

        if (isRandomSpeed) RandomizeSpeed();
        if (isRandomTurningForece) RandomizeTurn();
        Move(gameObject);
    }
}
