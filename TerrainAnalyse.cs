using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Terrain))]
public class TerrainAnalyse : Editor
{

	[MenuItem("Tools/Terrian/Size")]
	static void GetTerrainSize()
	{
		GameObject obj = Selection.activeGameObject;
		if (obj == null)
			return;
		Terrain terrian = obj.GetComponent<Terrain>();
		if (terrian != null)
		{
			Debug.Log(obj.name + "  terrian size:" + terrian.terrainData.size);
		}
	}

	[MenuItem("GameObject/GetChildCount")]
	static void GetChildCount()
	{
		GameObject obj = Selection.activeGameObject;
		if (obj == null)
			return;
		Debug.Log(obj.name + ":" + obj.transform.GetAllGOCount());
	}

	[MenuItem("GameObject/GetChildBorder")]
	static void GetChildBorder()
	{
		GameObject obj = Selection.activeGameObject;
		if(obj == null)
			return;
		Vector3 min= Vector3.positiveInfinity;
		Vector3 max = Vector3.negativeInfinity;
		foreach (Transform trans in obj.transform)
		{
			max = Vector3.Max(max, trans.position);
			min = Vector3.Min(min, trans.position);
		}
		Debug.Log("Min:"+min+"  max:"+max);
	}


	Terrain  m_terrian;

	private void OnEnable()
	{
		m_terrian = (Terrain) target;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		if (m_terrian.terrainData != null && GUILayout.Button("log terrain size"))
		{
			Debug.Log(m_terrian.name + "  terrian size:" + m_terrian.terrainData.size);
		}

	}
}
