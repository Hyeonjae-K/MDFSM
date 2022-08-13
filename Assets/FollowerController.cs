using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerController : MonoBehaviour
{
    // 리더드론
    public GameObject leader;
    //이동 할 좌표값
    public float x, y, z;

    Vector3 vel = Vector3.zero;

    void Start()
    {
        string name = gameObject.name.Substring(gameObject.name.IndexOf('_') + 1).Trim();
        leader = GameObject.Find(name);
        //MovetoTarget(leader.transform.position.x + x, leader.transform.position.y + y, leader.transform.position.z + z) ;
    }

    // Update is called once per frame
    void Update()
    {
        CopyMove();
        //  MovetoTarget(x, y, z);
    }

    // 리더드론의 이동 함수를 복사해 실행하는 함수
    void CopyMove()
    {
        leader.GetComponent<Drone>().Rotate(gameObject);
        leader.GetComponent<Drone>().Move(gameObject);
    }

    //해당 좌표로 이동하는 함수
    void MovetoTarget(float x1, float y1, float z1)
    {
        transform.position = new Vector3(x1, y1, z1);
    }
}
