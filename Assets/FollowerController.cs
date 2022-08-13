using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerController : MonoBehaviour
{
    // �������
    public GameObject leader;
    //�̵� �� ��ǥ��
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

    // ��������� �̵� �Լ��� ������ �����ϴ� �Լ�
    void CopyMove()
    {
        leader.GetComponent<Drone>().Rotate(gameObject);
        leader.GetComponent<Drone>().Move(gameObject);
    }

    //�ش� ��ǥ�� �̵��ϴ� �Լ�
    void MovetoTarget(float x1, float y1, float z1)
    {
        transform.position = new Vector3(x1, y1, z1);
    }
}
