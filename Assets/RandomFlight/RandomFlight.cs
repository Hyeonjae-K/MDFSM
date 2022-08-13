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
