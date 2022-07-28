using UnityEngine;
using System.Collections;
using MathPlus;

public class spline : MonoBehaviour {

	public Vector3[] points;


	[Range(0, 100)]
	public int quality = 10; //waypoint to waypoint 의 좌표간 보조좌표 개수

	public GameObject sphere; //좌표 가시화 도구

	GameObject[] spheres = new GameObject[1000]; //좌표 생성간 오차발생에 대한 예비 좌표값들
	Vector3[] point;

	void Update () {
		
		for (int i = 0; i < spheres.Length; i++) { //기존 좌표값들이 변함에 따라 spline이 변하므로 변화에 따른 기존 spline 보조 좌표들을 삭제  삭제하지 않으면 객체들이 계속해서 생겨나 overflow 발생 (
			Destroy(spheres[i]); 
		}

		point = mathPlus.CreateCatmullSpline (points, quality, true);


		for (int i = 0; i < point.Length; i++) { //객체 생성을 위한 코드 ( 굳이 없어도 됨 (가시화 작업을 위한 도구 -> ray나 line으로 만들 방법 생각 필요))
			GameObject newSpehere = GameObject.Instantiate (sphere, new Vector3 (point [i].x, point [i].y, point [i].z), Quaternion.identity) as GameObject;
			spheres [i] = newSpehere; //변화에 따른 보조 좌표들이 지워짐에 따라 다시 메울 객체들 담아놓기
		}								// quaternion -> direction + rotation https://docs.unity3d.com/ScriptReference/Quaternion.html
	}
}
