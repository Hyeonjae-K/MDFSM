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
    [SerializeField]
    protected float startLocationX = 0;
    [SerializeField]
    protected float startLocationY = 0;
    [SerializeField]
    protected float startLocationZ = 0;


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


    [Header("Collider")]
    [SerializeField]
    protected Main main;
    protected BoxCollider areaCollider;
    public SphereCollider sphereCollider;



    // 3차원 벡터를 입력받아 해당 위치로 이동
    // 입력값이 없을 경우 startLocation으로 이동
    protected void SetLocation(Vector3? location = null)
    {
        if (location == null) transform.position = new Vector3(startLocationX, startLocationY, startLocationZ);

        else transform.position = (Vector3)location;
    }

    // 군집 랜덤 스폰 좌표 리턴
    Vector3 ClusterPoint(float range)
    {
        Bounds clusterBounds = sphereCollider.bounds;

        Vector3 spawnpoint = Random.onUnitSphere;
        float r = Random.Range(0.0f, range);

        spawnpoint = (spawnpoint * r) + transform.position;

        Debug.Log("spawn: " + spawnpoint);
        return spawnpoint;
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

                Debug.Log("drone:" + gameObject.transform.position);
                clust_drone.name = "clust_" + gameObject.name;
            }
        }
    }
}
