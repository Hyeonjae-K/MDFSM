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


    // �ӵ� ���� ���� �Լ�
    void RandomizeSpeed()
    {
        // minSpeed�� maxSpeed ������ ���� �ӵ� ����
        targetSpeed = Random.Range(minSpeed, maxSpeed);
        isAcceleration = (speed < targetSpeed);

        // ������ �ӵ��� ����
        currentAccelerate = 0;
        if (isAcceleration == true && speed < targetSpeed)
            currentAccelerate = accelerateAmount;
        else if (isAcceleration == false && speed > targetSpeed)
            currentAccelerate = -accelerateAmount;
        speed += currentAccelerate * Time.deltaTime;
    }

    // ��ȸ�� ���� ���� �Լ�
    void RandomizeTurn()
    {
        // turningForce�� 75% ~ 100% ������ ���� ��ȸ�� ����
        currentTurningForce = Random.Range(0.75f * turningForce, turningForce);
        turningTime = 1 / currentTurningForce;

        // ������ ��ȸ������ ����
        currentTurningTime = Mathf.Lerp(currentTurningTime, turningTime, 1);
    }

    // ������ ���� ���� ���� ��ǥ ��ȯ
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

    // waypoint ������Ʈ �Լ�
    void UpdateWaypoint()
    {
        // ���� ��ǥ�� �޾� currentWaypoint�� ����
        currentWaypoint = GetWaypoint();

        // ������ ��ǥ�� cube ������Ʈ ���� (���� Ȯ�ο�)
        if (isDebug)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = currentWaypoint;
            Debug.Log(currentWaypoint);
        }

        // ������ waypoint�� distance ������Ʈ
        waypointDistance = Vector3.Distance(transform.position, currentWaypoint);
        prevWaypointDistance = waypointDistance;
        isComingClose = false;
    }

    // waypoint ���� Ȯ�� �Լ�
    void CheckWaypoint()
    {
        waypointDistance = Vector3.Distance(transform.position, currentWaypoint);

        if (waypointDistance >= prevWaypointDistance)
        {
            // waypoint�� ������ ��� ���� waypoint�� ������Ʈ
            if (isComingClose)
                UpdateWaypoint();
        }
        else
            isComingClose = true;

        prevWaypointDistance = waypointDistance;
    }

    // waypoint �������� ����ü ȸ�� �Լ�
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

    // waypoint �������� z�� ȸ�� �Լ�
    void ZAxisRotate()
    {
        currRotY = transform.eulerAngles.y;
        float diff = prevRotY - currRotY;

        if (diff > 180) diff -= 360;
        if (diff < -180) diff += 360;

        prevRotY = transform.eulerAngles.y;
        zRotateValue = Mathf.Lerp(zRotateValue, Mathf.Clamp(diff / zRotateMaxThreshold, -1, 1), zRotateLerpAmount * Time.deltaTime);
    }

    // ����ü �̵� �Լ�
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
