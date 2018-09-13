using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public enum IntersectionType
{
	Disjoint, // 不相交
	Intersects, //相交但不包含
	Contains //包含
}

public struct CullingBox
{
	public Vector3 BoxCenter;
	public Vector3 BoxExtents;
	public bool IsInFrustum;
}

public struct CullingSphere
{
	public Vector3 SphereCenter;
	public float SphereRadius;
	public bool IsInFrustum;
}

[ExecuteInEditMode]
public static class CameraUtil
{

	/// <summary>
	/// box 和 frustum 是否相交
	/// </summary>
	public static bool IntersectsBox(Plane[] planes,ref Bounds box, float frustumPadding = 0)
	{
		var center = box.center;
		var extents = box.extents;

		for (int i = 0; i < planes.Length; i++)
		{
			var planeNormal = planes[i].normal;
			var abs = new Vector3(Mathf.Abs(planeNormal.x), Mathf.Abs(planeNormal.y), Mathf.Abs(planeNormal.z));
			var planeDistance = planes[i].distance;

			float r = extents.x * abs.x + extents.y * abs.y + extents.z * abs.z;
			float s = planeNormal.x * center.x + planeNormal.y * center.y + planeNormal.z * center.z;

			if (s + r < -planeDistance - frustumPadding)
			{
				return false;
			}
		}

		return true;
	}

	/// <summary>
	/// boundingbox 和 frustum 之间的位置关系
	/// </summary>
	public static IntersectionType GetBoxIntersection(Plane[] planes,ref Bounds box, float frustumPadding = 0)
	{
		var center = box.center;
		var extents = box.extents;

		var intersecting = false;

		for (int i = 0; i < planes.Length; i++)
		{
			Plane plane = planes[i];
			var planeNormal =  plane.normal;
			var abs = new Vector3(Mathf.Abs(plane.normal.x), Mathf.Abs(plane.normal.y), Mathf.Abs(plane.normal.z));
			var planeDistance = plane.distance;

			float r = extents.x * abs.x + extents.y * abs.y + extents.z * abs.z;
			float s = planeNormal.x * center.x + planeNormal.y * center.y + planeNormal.z * center.z;

			if (s + r < -planeDistance - frustumPadding)
			{
				return IntersectionType.Disjoint;
			}

			intersecting |= (s - r <= -planeDistance);
		}

		return intersecting ? IntersectionType.Intersects : IntersectionType.Contains;
	}

	/// <summary>
	/// 分别判断一组boxes是否在视锥范围内，只要用于bounding box不经常发生变化，但是摄像机的视锥位置经常发生变化的情况，如四叉树等
	/// </summary>
	/// <param name="planes"></param>
	/// <param name="boxes"></param>
	/// <param name="boxCount"></param>
	public static void CullBoxes(Plane[] planes,CullingBox[] boxes, int boxCount)
	{
		
		var planeNormal0 = planes[0].normal;
		var planeNormal1 = planes[1].normal;
		var planeNormal2 = planes[2].normal;
		var planeNormal3 = planes[3].normal;
		var planeNormal4 = planes[4].normal;
		var planeNormal5 = planes[5].normal;

		var abs0 = new Vector3(Mathf.Abs(planeNormal0.x), Mathf.Abs(planeNormal0.y), Mathf.Abs(planeNormal0.z));
		var abs1 = new Vector3(Mathf.Abs(planeNormal1.x), Mathf.Abs(planeNormal1.y), Mathf.Abs(planeNormal1.z));
		var abs2 = new Vector3(Mathf.Abs(planeNormal2.x), Mathf.Abs(planeNormal2.y), Mathf.Abs(planeNormal2.z));
		var abs3 = new Vector3(Mathf.Abs(planeNormal3.x), Mathf.Abs(planeNormal3.y), Mathf.Abs(planeNormal3.z));
		var abs4 = new Vector3(Mathf.Abs(planeNormal4.x), Mathf.Abs(planeNormal4.y), Mathf.Abs(planeNormal4.z));
		var abs5 = new Vector3(Mathf.Abs(planeNormal5.x), Mathf.Abs(planeNormal5.y), Mathf.Abs(planeNormal5.z));

	
		var planeDistance0 = planes[0].distance;
		var planeDistance1 = planes[1].distance;
		var planeDistance2 = planes[2].distance;
		var planeDistance3 = planes[3].distance;
		var planeDistance4 = planes[4].distance;
		var planeDistance5 = planes[5].distance;

		for (int bi = 0; bi < boxCount; bi++)
		{
			var box = boxes[bi];
			var center = box.BoxCenter;
			var extents = box.BoxExtents;

			bool outOfFrustum = false;

			outOfFrustum = outOfFrustum || (
				               (extents.x * abs0.x + extents.y * abs0.y + extents.z * abs0.z) +
				               (planeNormal0.x * center.x + planeNormal0.y * center.y + planeNormal0.z * center.z)) <
			               -planeDistance0;

			outOfFrustum = outOfFrustum || (
				               (extents.x * abs1.x + extents.y * abs1.y + extents.z * abs1.z) +
				               (planeNormal1.x * center.x + planeNormal1.y * center.y + planeNormal1.z * center.z)) <
			               -planeDistance1;

			outOfFrustum = outOfFrustum || (
				               (extents.x * abs2.x + extents.y * abs2.y + extents.z * abs2.z) +
				               (planeNormal2.x * center.x + planeNormal2.y * center.y + planeNormal2.z * center.z)) <
			               -planeDistance2;

			outOfFrustum = outOfFrustum || (
				               (extents.x * abs3.x + extents.y * abs3.y + extents.z * abs3.z) +
				               (planeNormal3.x * center.x + planeNormal3.y * center.y + planeNormal3.z * center.z)) <
			               -planeDistance3;

			outOfFrustum = outOfFrustum || (
				               (extents.x * abs4.x + extents.y * abs4.y + extents.z * abs4.z) +
				               (planeNormal4.x * center.x + planeNormal4.y * center.y + planeNormal4.z * center.z)) <
			               -planeDistance4;

			outOfFrustum = outOfFrustum || (
				               (extents.x * abs5.x + extents.y * abs5.y + extents.z * abs5.z) +
				               (planeNormal5.x * center.x + planeNormal5.y * center.y + planeNormal5.z * center.z)) <
			               -planeDistance5;

			boxes[bi].IsInFrustum = !outOfFrustum;
		}
	}

	/// <summary>
	///  oriented bounding box 和 frustum 是否相交
	/// </summary>
	/// <param name="planes">frustum的六个面</param>
	/// <param name="box">box.center需要是在世界坐标系统的坐标</param>
	/// <param name="right">box本地坐标的right方向值（等价于Transform.right)</param>
	/// <param name="up">box本地坐标的up方向值（等价于Transform.up)</param>
	/// <param name="forward">box本地坐标的forward方向值（等价于Transform.forward)</param>
	/// <returns>true: 相交</returns>
	public static bool IntersectsOrientedBox(Plane[] planes, ref Bounds box, ref Vector3 right, ref Vector3 up,
		ref Vector3 forward, float frustumPadding = 0)
	{
		IntersectionType type =
			GetOrientedBoxIntersection(planes, ref box, ref right, ref up, ref forward, frustumPadding);
		return type != IntersectionType.Disjoint;
	}

	/// <summary>
	/// oriented bounding box和frustum的位置关系.
	/// </summary>
	/// <param name="planes">frustum的六个面</param>
	/// <param name="box">box.center需要是在世界坐标系统的坐标</param>
	/// <param name="right">box本地坐标的right方向值（等价于Transform.right)</param>
	/// <param name="up">box本地坐标的up方向值（等价于Transform.up)</param>
	/// <param name="forward">box本地坐标的forward方向值（等价于Transform.forward)</param>
	/// <returns></returns>
	public static IntersectionType GetOrientedBoxIntersection(Plane[] planes, ref Bounds box, ref Vector3 right,
		ref Vector3 up, ref Vector3 forward, float frustumPadding = 0)
	{
		var center = box.center;
		var extents = box.extents;

		var intersecting = false;

		for (int i = 0; i < planes.Length; i++)
		{
			var planeNormal = planes[i].normal;
			var planeDistance = planes[i].distance;

			float r =
				extents.x * Mathf.Abs(Vector3.Dot(planeNormal, right)) +
				extents.y * Mathf.Abs(Vector3.Dot(planeNormal, up)) +
				extents.z * Mathf.Abs(Vector3.Dot(planeNormal, forward));

			float s = planeNormal.x * center.x + planeNormal.y * center.y + planeNormal.z * center.z;

			if (s + r < -planeDistance - frustumPadding)
			{
				return IntersectionType.Disjoint;
			}

			intersecting |= (s - r <= -planeDistance);
		}

		return intersecting ? IntersectionType.Intersects : IntersectionType.Contains;
	}

	/// <summary>
	/// Sphere 是否和frustum相交
	/// </summary>
	/// <param name="planes">frustum 的6个面</param>
	/// <param name="center">sphere 的世界坐标</param>
	/// <param name="radius">sphere 的半径</param>
	/// <param name="frustumPadding">摄像机视锥的往里padding</param>
	/// <returns>true : 相交</returns>
	public static bool IntersectsSphere(Plane[] planes, ref Vector3 center, float radius, float frustumPadding = 0)
	{
		IntersectionType type = GetSphereIntersection(planes, ref center, radius, frustumPadding);
		return type != IntersectionType.Disjoint;
	}

	/// <summary>
	/// Sphere 是否和frustum相交
	/// </summary>
	/// <param name="planes">frustum 的6个面</param>
	/// <param name="BoundingSphere">sphere</param>
	/// <param name="frustumPadding">摄像机视锥的往里padding</param>
	/// <returns>true : 相交</returns>
	public static bool IntersectsSphere(Plane[] planes, ref BoundingSphere sphere, float frustumPadding = 0)
	{
		return IntersectsSphere(planes, ref sphere.position, sphere.radius, frustumPadding);
	}

	/// <summary>
	/// 分别判断一组sphere是否在视锥范围内，只要用于bounding box不经常发生变化，但是摄像机的视锥位置经常发生变化的情况，如四叉树等
	/// </summary>
	/// <param name="planes">摄像机的视锥6个平面</param>
	/// <param name="spheres">待判断的一组sphere</param>
	/// <param name="sphereCount"></param>
	public static void CullSpheres(Plane[] planes, CullingSphere[] spheres, int sphereCount)
	{
		var planeNormal0 = planes[0].normal;
		var planeNormal1 = planes[1].normal;
		var planeNormal2 = planes[2].normal;
		var planeNormal3 = planes[3].normal;
		var planeNormal4 = planes[4].normal;
		var planeNormal5 = planes[5].normal;

		var planeDistance0 = planes[0].distance;
		var planeDistance1 = planes[1].distance;
		var planeDistance2 = planes[2].distance;
		var planeDistance3 = planes[3].distance;
		var planeDistance4 = planes[4].distance;
		var planeDistance5 = planes[5].distance;

		for (int si = 0; si < sphereCount; si++)
		{
			var sphere = spheres[si];
			var center = sphere.SphereCenter;
			var radius = sphere.SphereRadius;

			bool outOfFrustum = false;

			outOfFrustum = outOfFrustum || (planeNormal0.x * center.x + planeNormal0.y * center.y +
			                                planeNormal0.z * center.z + planeDistance0) < -radius;
			outOfFrustum = outOfFrustum || (planeNormal1.x * center.x + planeNormal1.y * center.y +
			                                planeNormal1.z * center.z + planeDistance1) < -radius;
			outOfFrustum = outOfFrustum || (planeNormal2.x * center.x + planeNormal2.y * center.y +
			                                planeNormal2.z * center.z + planeDistance2) < -radius;
			outOfFrustum = outOfFrustum || (planeNormal3.x * center.x + planeNormal3.y * center.y +
			                                planeNormal3.z * center.z + planeDistance3) < -radius;
			outOfFrustum = outOfFrustum || (planeNormal4.x * center.x + planeNormal4.y * center.y +
			                                planeNormal4.z * center.z + planeDistance4) < -radius;
			outOfFrustum = outOfFrustum || (planeNormal5.x * center.x + planeNormal5.y * center.y +
			                                planeNormal5.z * center.z + planeDistance5) < -radius;

			spheres[si].IsInFrustum = !outOfFrustum;
		}
	}

	/// <summary>
	/// frustum和Sphere的位置关系
	/// </summary>
	/// <param name="planes">frustum 的6个面</param>
	/// <param name="center">sphere 的世界坐标</param>
	/// <param name="radius">sphere 的半径</param>
	/// <param name="frustumPadding">摄像机视锥的往里padding</param>
	/// <returns>位置关系</returns>
	public static IntersectionType GetSphereIntersection(Plane[] planes, ref Vector3 center, float radius,
		float frustumPadding = 0)
	{
		var intersecting = false;

		for (int i = 0; i < planes.Length; i++)
		{
			Plane plane = planes[i];
			var normal = plane.normal;
			var distance = plane.distance;

			float dist = normal.x * center.x + normal.y * center.y + normal.z * center.z + distance;

			if (dist < -radius - frustumPadding)
			{
				return IntersectionType.Disjoint;
			}

			intersecting |= (dist <= radius);
		}

		return intersecting ? IntersectionType.Intersects : IntersectionType.Contains;
	}

	/// <summary>
	/// sphere与camera的视锥的位置关系
	/// </summary>
	/// <param name="camera"></param>
	/// <param name="center">sphere的世界坐标</param>
	/// <param name="radius">sphere的半径</param>
	/// <param name="frustumPadding">摄像机视锥的往里padding</param>
	/// <returns></returns>
	public static IntersectionType GetSphereIntersection(Camera camera, ref Vector3 center, float radius,
		float frustumPadding = 0)
	{
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
		return GetSphereIntersection(planes, ref center, radius, frustumPadding);
	}


	/// <summary>
	/// 点pos是否在摄像机可视范围内
	/// </summary>
	/// <param name="planes"></param>
	/// <param name="point"></param>
	/// <returns>true :包含</returns>
	public static bool Contains(Plane[] planes, ref Vector3 point)
	{
		for (int i = 0; i < planes.Length; i++)
		{
			Plane plane = planes[i];
			var normal = plane.normal;
			var distance = plane.distance;

			float dist = normal.x * point.x + normal.y * point.y + normal.z * point.z + distance;

			if (dist < 0f)
			{
				return false;
			}
		}

		return true;
	}

	/// <summary>
	/// 点pos是否在摄像机可视范围内
	/// </summary>
	/// <param name="camera"></param>
	/// <param name="point"></param>
	/// <returns>true :包含</returns>
	public static bool Contains(Camera camera, ref Vector3 point)
	{
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
		return Contains(planes, ref point);
	}


	/// <summary>
	///  计算不同距离下camera corner的
	/// </summary>
	/// <param name="camera"></param>
	/// <param name="distance">distance 是camera到锥平面的垂直距离</param>
	/// <returns>order:[0] = left_bottom, [1] = right_bottom, [2] = right_up, [3] = left_up</returns>
	public static Vector3[] CameraClipPlanePoints(Camera camera, float distance)
	{
		Vector3[] points = new Vector3[4];
		Transform transform = camera.transform;
		Vector3 pos = transform.position;
		float halfFov = (camera.fieldOfView * 0.5f) * Mathf.Deg2Rad;
		float height = Mathf.Tan(halfFov) * distance;
		float width = height * camera.aspect;

		// Left bottom
		Vector3 point = pos + transform.forward * distance;
		point -= transform.right * width;
		point -= transform.up * height;
		points[0] = point;

		// Right bottom
		point = pos + transform.forward * distance;
		point += transform.right * width;
		point -= transform.up * height;
		points[1] = point;


		// Right up
		point = pos + transform.forward * distance;
		point += transform.right * width;
		point += transform.up * height;
		points[2] = point;

		// Left up
		point = pos + transform.forward * distance;
		point -= transform.right * width;
		point += transform.up * height;
		points[3] = point;


		return points;
	}

	/// <summary>
	/// 计算摄像机的8个cornor
	/// </summary>
	/// <param name="cam"></param>
	/// <param name="nearCorners"> order:[0] = left_bottom, [1] = right_bottom, [2] = right_up, [3] = left_up</param>
	/// <param name="farCorners">order:[0] = left_bottom, [1] = right_bottom, [2] = right_up, [3] = left_up</param>
	public static void GetCameraCornors(Camera cam, out Vector3[] nearCorners, out Vector3[] farCorners)
	{
		nearCorners = new Vector3[4]; //Approx'd nearplane corners
		farCorners = new Vector3[4]; //Approx'd farplane corners
		Plane[] camPlanes = GeometryUtility.CalculateFrustumPlanes(cam); //get planes from matrix
		Plane temp = camPlanes[1];
		camPlanes[1] = camPlanes[2];
		camPlanes[2] = temp; //swap [1] and [2] so the order is better for the loop

		for (int i = 0; i < 4; i++)
		{
			nearCorners[i] =
				Plane3Intersect(camPlanes[4], camPlanes[i],
					camPlanes[(i + 1) % 4]); //near corners on the created projection matrix
			farCorners[i] =
				Plane3Intersect(camPlanes[5], camPlanes[i],
					camPlanes[(i + 1) % 4]); //far corners on the created projection matrix
		}
	}

	//TODO @chenjie 临时放置，应该放在通用的Geometry中
	public static Vector3 Plane3Intersect(Plane p1, Plane p2, Plane p3)
	{
		//get the intersection point of 3 planes
		return ((-p1.distance * Vector3.Cross(p2.normal, p3.normal)) +
		        (-p2.distance * Vector3.Cross(p3.normal, p1.normal)) +
		        (-p3.distance * Vector3.Cross(p1.normal, p2.normal))) /
		       (Vector3.Dot(p1.normal, Vector3.Cross(p2.normal, p3.normal)));
	}

	//TODO @chenjie 临时放置，应该放在通用的Geometry中
	public static bool PlaneLineInersect(Plane p1, Ray ray, out Vector3 intersectPos)
	{
		intersectPos = Vector3.zero;
		float enter = 0;
		p1.Raycast(ray, out enter);
		if (enter != 0)
		{
			intersectPos = ray.GetPoint(enter);
		}

		return enter != 0;
	}

	//TODO @chenjie 临时放置，应该放在通用的Geometry中
	public static Vector3[] LinePlaneIntersectPoints(Vector3[] nearCorners, Vector3[] farCorners, Plane plane)
	{
		Vector3[] hitPoints = new Vector3[4];
		for (int i = 0; i < 4; i++)
		{
			Ray ray = new Ray(nearCorners[i], farCorners[i] - nearCorners[i]);
			CameraUtil.PlaneLineInersect(plane, ray, out hitPoints[i]);
		}

		return hitPoints;
	}

	//TODO @chenjie 临时放置，应该放在通用的Geometry中
	public static bool RayCast(this Ray ray, Ray ray1, out float distance)
	{
		distance = 0;
		float angle = Vector3.Angle(ray.direction, ray1.direction) % 360;
		if (angle >= 180)
			return false;
		return false;
	}

	//TODO @chenjie 临时放置，应该放在通用的Geometry中
	public static Vector3[] GetHitPoints(Vector3[] nearCorners, Vector3[] farCorners, Plane plane)
	{
		Vector3[] hitPoints = LinePlaneIntersectPoints(nearCorners, farCorners, plane);
		return hitPoints;
	}

	//TODO @chenjie 临时放置，应该放在通用的Geometry中
	public static Vector3[] GetHitPoints(Camera camera, Plane plane)
	{
		Vector3[] nearCorners;
		Vector3[] farCorners;
		CameraUtil.GetCameraCornors(camera, out nearCorners, out farCorners);
		Vector3[] hitPoints = GetHitPoints(nearCorners, farCorners, plane);
		return hitPoints;
	}

	//TODO @chenjie 临时放置，应该放在通用的Geometry中
	public static void GenerateQuad(Mesh mesh, Vector3[] vertices)
	{
		mesh.Clear();
		mesh.vertices = vertices;

		int[] tri = new int[6];

		tri[0] = 0;
		tri[1] = 2;
		tri[2] = 1;

		tri[3] = 0;
		tri[4] = 3;
		tri[5] = 2;

		mesh.triangles = tri;

		Vector3[] normals = new Vector3[4];

		normals[0] = -Vector3.forward;
		normals[1] = -Vector3.forward;
		normals[2] = -Vector3.forward;
		normals[3] = -Vector3.forward;

		mesh.normals = normals;

		Vector2[] uv = new Vector2[4];

		uv[0] = new Vector2(0, 0);
		uv[1] = new Vector2(1, 0);
		uv[2] = new Vector2(0, 1);
		uv[3] = new Vector2(1, 1);

		mesh.uv = uv;
	}

	//TODO @chenjie 临时放置，应该放在通用的Help中
	public static void GetMinMaxVector3(Vector3[] points, out Vector3 min, out Vector3 max)
	{
		min = Vector3.positiveInfinity;
		max = Vector3.negativeInfinity;
		foreach (Vector3 val in points)
		{
			min = Vector3.Min(min, val);
			max = Vector3.Max(max, val);
		}
	}
}
