using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Principal;
using UnityEngine;

[ExecuteInEditMode]
public class CameraGizmoDrawer : MonoBehaviour
{

	public Camera camera;
	public Vector3 planeNormal = Vector3.up;
	public Vector3 planeOrigin;
	public Color interectColor = Color.cyan;
	Plane plane;
	public bool resetPlane;

	private List<Tuple<Vector3[],Color>> m_pinPoints = new List<Tuple<Vector3[], Color>>();
	void Start()
	{
		plane = new Plane(planeNormal, planeOrigin);
		if (camera == null)
			camera = GetComponent<Camera>();
	}

	void Update()
	{
		
		if (resetPlane)
		{
			resetPlane = false;
			plane = new Plane(planeNormal, planeOrigin);
			//ResetInterectPlane();
		}
		//ShowCameraFrustum();
		foreach (Tuple<Vector3[],Color> val in m_pinPoints)
		{
			DrawFrustumIntersectPlane(val.item1,val.item2);
		}
	}
	
	void ShowCameraFrustum()
	{
		Vector3[] nearCorners;
		Vector3[] farCorners;
		CameraUtil.GetCameraCornors(camera,out nearCorners,out farCorners);
		DrawFrustum(nearCorners, farCorners);
		Vector3[] hitPoints = CameraUtil.LinePlaneIntersectPoints(nearCorners, farCorners, plane);
		DrawFrustumIntersectPlane(hitPoints,interectColor);
		Vector3 min;
		Vector3 max;
		
		CameraUtil.GetMinMaxVector3(hitPoints,out min, out max);
		
	}

	[Range(0,1)]
	public float viewX;

	[Range(0,1)]
	public float viewY;

	public float rayLength = 100;
	private Vector3 m_viewPos = Vector3.zero;


	private void OnDrawGizmos()
	{
		Vector3[] nearCorners = CameraUtil.CameraClipPlanePoints(camera, camera.nearClipPlane);
		Vector3[] farCorners = CameraUtil.CameraClipPlanePoints(camera, camera.farClipPlane);
		Vector3[] hitPoints = CameraUtil.GetHitPoints(nearCorners,farCorners,plane);
		Vector3 min;
		Vector3 max;
		CameraUtil.GetMinMaxVector3(hitPoints,out min, out max);
		
		DrawCube(nearCorners,Color.red);
		DrawCube(farCorners,Color.blue);
		DrawLines(nearCorners,farCorners,Color.green);
		DrawCube(hitPoints, interectColor);
		DrawCube(min,max, Color.cyan);  
		DrawViewRay(Color.black);
		
	}

	void DrawViewRay(Color color)
	{
		m_viewPos.x = viewX;
		m_viewPos.y = viewY;
		Ray ray = camera.ViewportPointToRay(m_viewPos);
		var tmp = Gizmos.color;
		Gizmos.color = color;
		Gizmos.DrawRay(ray.origin, ray.direction * rayLength);
		Gizmos.color = tmp;
	}

	void DrawCube(Vector3 min, Vector3 max, Color color)
	{
		Color tmp = Gizmos.color;
		Gizmos.color = color;
		Gizmos.DrawWireCube((min+max)/2,max - min);
		Gizmos.color = tmp;
	}

	void DrawLines(Vector3[] from, Vector3[] to, Color color)
	{
		Color tmp = Gizmos.color;
		Gizmos.color = color;
		for (int i = 0; i < from.Length; i++)
		{
			Gizmos.DrawLine(from[i],to[i]);
		}
		Gizmos.color = tmp;
	}

	void DrawCube(Vector3[] points, Color color)
	{
		Color tmp = Gizmos.color;
		Gizmos.color = color;
		int count = points.Length;
		for (int i = 0; i < count; i++)
		{
			Gizmos.DrawLine(points[i], points[(i+1)%count]);

		}

		Gizmos.color = tmp;
	}

	public void Pin(Color color)
	{
		Vector3[] hitPoints = CameraUtil.GetHitPoints(camera,plane);
		m_pinPoints.Add(new Tuple<Vector3[], Color>(hitPoints,color));
	}

	public void LogHitPoints()
	{
		Vector3[] hitPoints = CameraUtil.GetHitPoints(camera,plane);
		Vector3 min;
		Vector3 max;
		CameraUtil.GetMinMaxVector3(hitPoints,out min, out max);
		Debug.LogFormat("cornors:{0}, min:{1}, max:{2}, max-min:{3}", hitPoints.IEnumerableToString(), min,max, max - min);
	}

	public Vector3 GetHitPointSpin()
	{
		Vector3[] hitPoints = CameraUtil.GetHitPoints(camera, plane);
		Vector3 min;
		Vector3 max;
		CameraUtil.GetMinMaxVector3(hitPoints,out min, out max);
		return max - min;
	}

	public void ClearPins()
	{
		m_pinPoints.Clear();
	}

	void DrawFrustumIntersectPlane(Vector3[] hitPoints, Color color)
	{
		for (int i = 0; i < 4; i++)
		{
			Debug.DrawLine(hitPoints[i],hitPoints[(i+1)%4],color ,Time.deltaTime,true);
		}
	}
	
	void DrawFrustum ( Vector3[] nearCorners, Vector3[] farCorners ) {
		for ( int i = 0; i < 4; i++ ) {
			Debug.DrawLine( nearCorners[i], nearCorners[( i + 1 ) % 4], Color.red, Time.deltaTime, true ); //near corners on the created projection matrix
			Debug.DrawLine( farCorners[i], farCorners[( i + 1 ) % 4], Color.blue, Time.deltaTime, true ); //far corners on the created projection matrix
			Debug.DrawLine( nearCorners[i], farCorners[i], Color.green, Time.deltaTime, true ); //sides of the created projection matrix
		}
	}	
}
