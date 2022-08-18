using UnityEngine;

public class Drone : MonoBehaviour
{
    [Header("Drone Settings")]
    [SerializeField]
    protected float maxSpeed = 40;
    [SerializeField]
    protected float minSpeed = 0;
    [SerializeField]
    protected float defaultSpeed = 20;
    protected float speed;


    [Header("Accel/Rotate Values")]
    [SerializeField]
    protected float accelerateAmount = 15.0f;
    [SerializeField]
    protected float turningForce = 1.5f;


    [Header("Z Rotate Values")]
    [SerializeField]
    protected float zRotateMaxThreshold = 0.3f;
    [SerializeField]
    protected float zRotateAmount = 135;
    [SerializeField]
    protected float zRotateLerpAmount = 1.5f;

    // speed & rotate settings
    protected float targetSpeed;
    protected bool isAcceleration;

    protected float currentAccelerate;
    protected float currentTurningForce;

    protected float turningTime;
    protected float currentTurningTime;


    [Header("Collider")]
    [SerializeField]
    protected Main main;
    protected BoxCollider areaCollider;
    public SphereCollider sphereCollider;

    // waypoint settings
    protected Vector3 currentWaypoint;

    float prevWaypointDistance;
    float waypointDistance;
    bool isComingClose;

    protected float prevRotY;
    protected float currRotY;
    protected float rotateAmount;
    protected float zRotateValue;


    // 3���� ���͸� �Է¹޾� �ش� ��ġ�� �̵�
    // �Է°��� ���� ��� startLocation���� �̵�
    protected void SetLocation(Vector3 location)
    {
        transform.position = location;
    }

    // ���� ���� ���� ��ǥ ����
    Vector3 ClusterPoint(float range)
    {
        Bounds clusterBounds = sphereCollider.bounds;

        Vector3 spawnpoint = Random.onUnitSphere;
        float r = Random.Range(0.0f, range);

        spawnpoint = (spawnpoint * r) + transform.position;

        return spawnpoint;
    }

    // waypoint ������Ʈ �Լ�
    protected void UpdateWaypoint(Vector3 newWaypoint)
    {
        // ��ǥ�� �޾� currentWaypoint ������Ʈ
        currentWaypoint = newWaypoint;

        // ����� ���� ������ ��ǥ�� cube ������Ʈ ����
        if (main.isDebug)
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
    protected bool CheckWaypoint()
    {
        waypointDistance = Vector3.Distance(transform.position, currentWaypoint);

        if (waypointDistance >= prevWaypointDistance)
        {
            // waypoint�� ������ ��� ���� waypoint�� ������Ʈ
            if (isComingClose)
                return true;
        }
        else
            isComingClose = true;

        prevWaypointDistance = waypointDistance;
        return false;
    }

    // waypoint �������� ����ü ȸ�� �Լ�
    public void Rotate(GameObject mainObject)
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

            mainObject.transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, lerpAmount);
        }
    }

    // waypoint �������� z�� ȸ�� �Լ�
    public void ZAxisRotate()
    {
        currRotY = transform.eulerAngles.y;
        float diff = prevRotY - currRotY;

        if (diff > 180) diff -= 360;
        if (diff < -180) diff += 360;

        prevRotY = transform.eulerAngles.y;
        zRotateValue = Mathf.Lerp(zRotateValue, Mathf.Clamp(diff / zRotateMaxThreshold, -1, 1), zRotateLerpAmount * Time.deltaTime);
    }

    // ����ü �̵� �Լ�
    public void Move(GameObject mainObject)
    {
        mainObject.transform.Translate(new Vector3(0, 0, speed) * Time.deltaTime);
    }

    protected virtual void Start()
    {
        speed = defaultSpeed;
        main = GameObject.Find("Area").GetComponent<Main>();
        areaCollider = main.areaCollider;

        if (main.isCluster)
        {
            for (int i = 0; i < main.clusterSize; i++)
            {
                GameObject clust_drone = Instantiate(main.clusterDronePrefab, ClusterPoint(main.clusterRange), Quaternion.identity);

                clust_drone.name = "clust_" + gameObject.name;
            }
        }
    }
}
