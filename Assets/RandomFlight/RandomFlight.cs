using UnityEngine;


public class RandomFlight : Drone
{
    [Header("Random Settings")]
    [SerializeField]
    bool isRandomPointStart = true;
    [SerializeField]
    bool isRandomSpeed = true;
    [SerializeField]
    bool isRandomTurningForece = true;
    

    // speed & rotate settings
    float targetSpeed;
    bool isAcceleration;

    float currentAccelerate;
    float currentTurningForce;

    float turningTime;
    float currentTurningTime;

    // waypoint settings
    Vector3 currentWaypoint;

    float prevWaypointDistance;
    float waypointDistance;
    bool isComingClose;

    float prevRotY;
    float currRotY;
    float rotateAmount;
    float zRotateValue;


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
        Vector3 waypoint = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
        return waypoint;
    }

    // waypoint 업데이트 함수
    void UpdateWaypoint()
    {
        // 랜덤 좌표를 받아 currentWaypoint에 저장
        currentWaypoint = GetWaypoint();

        // 생성한 좌표에 cube 오브젝트 생성 (동작 확인용)
        if (isDebug)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = currentWaypoint;
            Debug.Log(currentWaypoint);
        }

        // 생성한 waypoint로 distance 업데이트
        waypointDistance = Vector3.Distance(transform.position, currentWaypoint);
        prevWaypointDistance = waypointDistance;
        isComingClose = false;
    }

    // waypoint 도달 확인 함수
    void CheckWaypoint()
    {
        waypointDistance = Vector3.Distance(transform.position, currentWaypoint);

        if (waypointDistance >= prevWaypointDistance)
        {
            // waypoint에 도달한 경우 다음 waypoint로 업데이트
            if (isComingClose)
                UpdateWaypoint();
        }
        else
            isComingClose = true;

        prevWaypointDistance = waypointDistance;
    }

    // waypoint 방향으로 비행체 회전 함수
    void Rotate()
    {
        if (currentWaypoint == Vector3.zero)
            return;

        Vector3 targetDir = currentWaypoint - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(targetDir);

        float delta = Quaternion.Angle(transform.rotation, lookRotation);
        if (delta > 0.0f)
        {
            float lerpAmount = Mathf.SmoothDampAngle(delta, 0.0f, ref rotateAmount, currentTurningTime);
            lerpAmount = 1.0f - (lerpAmount / delta);

            Vector3 eulerAngle = lookRotation.eulerAngles;
            eulerAngle.z += zRotateValue * zRotateAmount;
            lookRotation = Quaternion.Euler(eulerAngle);

            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, lerpAmount);
        }
    }

    // waypoint 방향으로 z축 회전 함수
    void ZAxisRotate()
    {
        currRotY = transform.eulerAngles.y;
        float diff = prevRotY - currRotY;

        if (diff > 180) diff -= 360;
        if (diff < -180) diff += 360;

        prevRotY = transform.eulerAngles.y;
        zRotateValue = Mathf.Lerp(zRotateValue, Mathf.Clamp(diff / zRotateMaxThreshold, -1, 1), zRotateLerpAmount * Time.deltaTime);
    }

    // 비행체 이동 함수
    void Move()
    {
        transform.Translate(new Vector3(0, 0, speed) * Time.deltaTime);
    }


    protected override void Start()
    {
        base.Start();
        targetSpeed = defaultSpeed;

        currentTurningForce = turningForce;
        currentTurningTime = turningTime = 1 / turningForce;

        prevRotY = 0;
        currRotY = 0;

        Debug.Log(areaCollider);


        if (isRandomPointStart) transform.position = GetWaypoint();

        UpdateWaypoint();
    }

    void Update()
    {
        CheckWaypoint();
        ZAxisRotate();
        Rotate();

        if (isRandomSpeed) RandomizeSpeed();
        if (isRandomTurningForece) RandomizeTurn();
        Move();
    }
}
