using UnityEngine;
using System.Collections;
using System;
using MathPlus;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class spline : MonoBehaviour {

	public Vector3[] points;


	[Range(0, 200)]
	public int quality = 10; //waypoint to waypoint 의 좌표간 보조좌표 개수

	public GameObject sphere; //좌표 가시화 도구

	GameObject[] spheres = new GameObject[1000]; //좌표 생성간 오차발생에 대한 예비 좌표값들
	Vector3[] point;
	
	//=================================================== ref
	
	public WaypointList waypointList = new WaypointList();
	public GameObject waypointCircuitSinglePoint;
	public float editorVisualisationSubsteps = 100;
	public float Length { get; private set; }
	public float getTotalLengthOfCircuit() { if (distances.Length > 0) return distances[distances.Length - 1]; else return 0; }
	
	// public override bool isCircuit() { return true; }
	
	private float[] distances;
	private int numPoints;
	
	
	int actualChildrenIndx = 2;
	int minChildrenIndx = 0;
	const int digitsNumber = 3;
	
	private int p0n;
	private int p1n;
	private int p2n;
	private int p3n;
	
	private float i;
	private Vector3 P0;
	private Vector3 P1;
	private Vector3 P2;
	private Vector3 P3;
	
	// public Transform S1;
	// public Transform S2;
	// public Transform S3;
	// public Transform S4;
	public class WaypointList
	{
		public WaypointCircuit circuit;
		public Transform[] items = new Transform[0];
	}
	public Transform[] Waypoints
	{
		get { updatePointsList(); return waypointList.items; }
	}
	
// waypoint 등록시 position으로 자동 갱신 및 마지막 점을 초기점으로 초기화.
	void updatePointsList()
	{
		var children = new Transform[transform.childCount];
		int n = 0;
		foreach (Transform child in transform)
		{
			children[n++] = child;
		}
		Array.Sort(children, new TransformNameComparer());
		waypointList.items = new Transform[children.Length];
		for (n = 0; n < children.Length; ++n)
		{
			waypointList.items[n] = children[n];
			// Debug.Log("updated");
			// Debug.Log(waypointList.items[n]);
			
		}
	}
	
	public class TransformNameComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			return ((Transform)x).name.CompareTo(((Transform)y).name);
		}
	}
	public void addPointAtTheEndOfTheCircuit(Vector3 pt) {
		//Debug.Log("added point at pos:" + pt.ToString());
		GameObject g = (GameObject)Instantiate(waypointCircuitSinglePoint, pt, Quaternion.identity);
		g.name = "Waypoint " + toStringOfXdigits(actualChildrenIndx++);
		g.transform.parent = this.transform;
		//updatePointsList();
		//Debug.Log(waypointCircuitSinglePoint);
		//Debug.Log(g.name);
	}
	string toStringOfXdigits(int number)
	{
		String tmp = "";
		int k = digitsNumber - 1;
		while (k >= 0)
		{
			int res = number / (int)Mathf.Pow(10, k);
			tmp += res;
			number -= res * (int) Mathf.Pow(10, k);
			k--;
		}
		return tmp;
	}
	private void CachePositionsAndDistances()   //dist 찾기1
	{

		points = new Vector3[Waypoints.Length + 1];
		distances = new float[Waypoints.Length + 1];
        
		float accumulateDistance = 0;
		for (int i = 0; i < points.Length; ++i)
		{
			var t1 = Waypoints[(i)%Waypoints.Length];
			var t2 = Waypoints[(i + 1)%Waypoints.Length];
			if (t1 != null && t2 != null)
			{
				Vector3 p1 = t1.position;
				Vector3 p2 = t2.position;
				points[i] = Waypoints[i%Waypoints.Length].position;
				distances[i] = accumulateDistance;
				accumulateDistance += (p1 - p2).magnitude;
			}
		}
		editorVisualisationSubsteps = (int) distances[Waypoints.Length];
	}
	
	// mathplus bezier, catmullrome spline 알고리즘 원형 코드
	private Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float i)
	{
		return 0.5f*
		       ((2*p1) + (-p0 + p2)*i + (2*p0 - 5*p1 + 4*p2 - p3)*i*i +
		        (-p0 + 3*p1 - 3*p2 + p3)*i*i*i);
	}
	
	public Vector3 GetRoutePosition(float dist)
	{
		int point = 0;
	
		if (Length == 0)
		{
			Length = distances[distances.Length - 1];
		}
	
		dist = Mathf.Repeat(dist, Length);
	
		while (distances[point] < dist)
		{
			++point;
		}
	
	
		// get nearest two points, ensuring points wrap-around start & end of circuit
		p1n = ((point - 1) + numPoints)%numPoints;
		p2n = point;
         
		// found point numbers, now find interpolation value between the two middle points
	
		i = Mathf.InverseLerp(distances[p1n], distances[p2n], dist);
	
		// --- smooth catmull-rom calculation between the two relevant points ---
        
		// get indices for the surrounding 2 points, because
		// four points are required by the catmull-rom function
		p0n = ((point - 2) + numPoints)%numPoints;
		p3n = (point + 1)%numPoints;
	
		// 2nd point may have been the 'last' point - a dupe of the first,
		// (to give a value of max track distance instead of zero)
		// but now it must be wrapped back to zero if that was the case.
		p2n = p2n%numPoints;
	
		P0 = points[p0n];
		P1 = points[p1n];
		P2 = points[p2n];
		P3 = points[p3n];
	
		return CatmullRom(P0, P1, P2, P3, i);
  
	}
	public struct RoutePoint
	{
		public Vector3 position;
		public Vector3 direction;
	
	
		public RoutePoint(Vector3 position, Vector3 direction)
		{
			this.position = position;
			this.direction = direction;
		}
	}
	public RoutePoint GetRoutePoint(float dist)  //override
	{
		// position and direction
		Vector3 p1 = GetRoutePosition(dist);
		Vector3 p2 = GetRoutePosition(dist + 0.1f);
		Vector3 delta = p2 - p1;
		return new RoutePoint(p1, delta.normalized);
	}
	public Vector3[] pointsBetween(Vector3 pt1, Vector3 pt2, int nPoints) //override
	{
		//CachePositionsAndDistances(); // 거리간 기록
		Length = distances[distances.Length - 1];
        
		// nearest point 계산
		float dPt1 = 0, dPt2 = 0;
		float distance1 = float.MaxValue;
		float distance2 = float.MaxValue;
	
		for (float dist = 0; dist < Length; dist += Length / editorVisualisationSubsteps)
		{
			Vector3 p = GetRoutePosition(dist + 1);
			float distToPt1 = Vector3.Distance(pt1, p);
			float distToPt2 = Vector3.Distance(pt2, p);
            
			if (distToPt1 < distance1) { dPt1 = dist; distance1 = distToPt1; }
			if (distToPt2 < distance2) { dPt2 = dist; distance2 = distToPt2; }
		}
	
		// 두번째 점 생성
		float distanceBetweenPoints = dPt2 > dPt1 ? dPt2 - dPt1 : dPt2 + Length - dPt1;
		float step = distanceBetweenPoints / nPoints;
		Vector3[] POINTS = new Vector3[nPoints + 2];
	
		for (int i = 0; i < POINTS.Length; i++){
			POINTS[i] = GetRoutePosition((dPt1 + step * (i +1)) % Length);
			Debug.Log(i);
			Debug.Log(POINTS[i]);
		}
		return POINTS;        
	}
	
	public float getNearestPointTo(Vector3 position)
	{
		Length = distances[distances.Length - 1];
		float dPt = 0;
		float actualDistance = float.MaxValue;
		for (float dist = 0; dist < Length; dist += Length / editorVisualisationSubsteps)
		{
			Vector3 p = GetRoutePosition(dist + 1);
			float distToPt1 = Vector3.Distance(position, p);
			if (distToPt1 < actualDistance) { dPt = dist; actualDistance = distToPt1; }          
		}
		return dPt;
	}
	
	private void Awake()
	{        
		if (Waypoints.Length > 1)
		{
			CachePositionsAndDistances();
		}
		numPoints = Waypoints.Length;
	}
	
	// private void OnDrawGizmos()
	// {
	// 	DrawGizmos(false);
	// }
	//
	// private void OnDrawGizmosSelected()
	// {
	// 	DrawGizmos(true);
	// }
	//
	// private void DrawGizmos(bool selected)
	// {
	// 	waypointList.circuit = this;
	// 	if (Waypoints.Length > 1)
	// 	{
	// 		numPoints = Waypoints.Length;
	//
	// 		CachePositionsAndDistances();
	// 		Length = distances[distances.Length - 1];
	//
	// 		Gizmos.color = selected ? Color.green : Color.green;  //new Color(1, 1, 0, 0.5f);
	// 		Vector3 prev = Waypoints[0].position;
 //                    
	// 		for (float dist = 0; dist < Length; dist += Length/editorVisualisationSubsteps)
	// 		{
	// 			Vector3 next = GetRoutePosition(dist + 1);
	// 			Gizmos.DrawLine(prev, next);
	// 			prev = next;
	// 		}
	// 		Gizmos.DrawLine(prev, Waypoints[0].position);            
 //            
	// 	}
	// }
	
	
	//=================================================== ref

	public Vector3[] Update ()
	{
		// float time = 1;
		for (int i = 0; i < spheres.Length; i++) { //기존 좌표값들이 변함에 따라 spline이 변하므로 변화에 따른 기존 spline 보조 좌표들을 삭제  삭제하지 않으면 객체들이 계속해서 생겨나 overflow 발생 (
			Destroy(spheres[i]);
			// Destroy(transform.Find("Waypoint " + toStringOfXdigits(minChildrenIndx++)).gameObject);
			// updatePointsList();
			// Destroy(spheres[i],time);
			
		}
		
		point = mathPlus.CreateCatmullSpline (points, quality, true);
		// Debug.Log(points);


		for (int i = 0; i < point.Length; i++) { //객체 생성을 위한 코드 ( 굳이 없어도 됨 (가시화 작업을 위한 도구 -> ray나 line으로 만들 방법 생각 필요))
			GameObject newSpehere = GameObject.Instantiate (sphere, new Vector3 (point [i].x, point [i].y, point [i].z), Quaternion.identity) as GameObject;
			// Debug.Log(point [i].x); // 좌표값들 반환(x,y,z)
			// Debug.Log(point [i].y);
			// Debug.Log(point [i].z);
			// Debug.LogFormat("x{0} : {1}, y{2} : {3}, z{4} : {5}", i, point[i].x, i, point[i].y, i, point[i].z);
			spheres [i] = newSpehere; //변화에 따른 보조 좌표들이 지워짐에 따라 다시 메울 객체들 담아놓기
		}								// quaternion -> direction + rotation https://docs.unity3d.com/ScriptReference/Quaternion.html

		// print(point);
		return point;
		// Debug.Log(point [i].x, point [i].y, point [i].z);
	}
}
