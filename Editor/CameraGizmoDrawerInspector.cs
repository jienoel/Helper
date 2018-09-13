using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraGizmoDrawer))]
public class CameraGizmoDrawerInspector : Editor
{
	private CameraGizmoDrawer m_drawer;
	private Color m_color = Color.magenta;
	private int m_xScreen = 0;
	private int m_zScreen = 0;
	private Vector3 cameraOriginPos;
	private void OnEnable()
	{
		m_drawer = (CameraGizmoDrawer) target;
		if(m_drawer.camera!=null)
		cameraOriginPos = m_drawer.camera.transform.position;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		GUILayout.BeginHorizontal();
		m_color = EditorGUILayout.ColorField("显示颜色", m_color);
		if (GUILayout.Button("pin"))
		{
			m_drawer.Pin(m_color);
		}

		if (GUILayout.Button("clear"))
		{
			m_drawer.ClearPins();
		}
		GUILayout.EndHorizontal();
		m_xScreen = EditorGUILayout.IntSlider("横像移动屏幕", m_xScreen, -15, 15);
		m_zScreen = EditorGUILayout.IntSlider("纵向移动屏幕", m_zScreen, -15, 15);
		
		GUILayout.BeginHorizontal();
		if (m_drawer.camera!=null && GUILayout.Button("移动摄像机"))
		{
			Vector3 spin = m_drawer.GetHitPointSpin();
			Vector3 pos = m_drawer.transform.position;
			pos.x += m_xScreen * spin.x;
			pos.z += m_zScreen * spin.z;
			m_drawer.camera.transform.position = pos;
		}

		if (m_drawer.camera!=null && GUILayout.Button("还原摄像机"))
		{
			m_drawer.camera.transform.position = cameraOriginPos;
		}
		
		if (m_drawer.camera!=null && GUILayout.Button("保存为默认摄像机位置"))
		{
			cameraOriginPos = m_drawer.camera.transform.position;
		}

		GUILayout.EndHorizontal();

		if (GUILayout.Button("Log HitPoints"))
		{
			m_drawer.LogHitPoints();
		}

		CalculateTerrainScreenCount();

		CalculateRenderScreenCount();
	}


	private Terrain m_terrain;
	//计算当前可移动的屏数
	void CalculateTerrainScreenCount()
	{
		GUILayout.BeginHorizontal();
		m_terrain = EditorGUILayout.ObjectField("Terrain",m_terrain,typeof(Terrain)) as Terrain;
		if (m_terrain!=null && m_terrain.terrainData!=null && GUILayout.Button("计算屏幕数"))
		{
			Vector3 size = m_terrain.terrainData.size;
			Vector3 spin = m_drawer.GetHitPointSpin();
			Debug.LogFormat("{0} 的地形尺寸可以横向显示{1}屏, 纵向显示{2}屏 , terrainsize:{3}, spin:{4}", m_terrain.name,size.x/spin.x, size.z/spin.z,size,spin);
		}
		GUILayout.EndHorizontal();
	}

	private GameObject renderObject;

	void CalculateRenderScreenCount()
	{
		GUILayout.BeginHorizontal();
		renderObject = EditorGUILayout.ObjectField("Renderer",renderObject,typeof(GameObject)) as GameObject;
		if (renderObject!=null && GUILayout.Button("计算屏幕数"))
		{
			Renderer render = renderObject.GetComponent<Renderer>();
			if (render != null)
			{
				Vector3 size = render.bounds.extents*2;
				Vector3 spin = m_drawer.GetHitPointSpin();
				Debug.LogFormat("{0} 的地形尺寸可以横向显示{1}屏, 纵向显示{2}屏 , Meshsize:{3}, spin:{4}", renderObject.name,size.x/spin.x, size.z/spin.z,size,spin);
			}
		}
		GUILayout.EndHorizontal();
	}

	void CheckIsInCameraView()
	{
		
	}

}
