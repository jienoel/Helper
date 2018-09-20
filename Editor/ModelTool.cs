using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEditor;

public class ModelTool : EditorWindow {

	[MenuItem("Tools/Model/ModelTool")]
	static void Init()
	{
		GameObject go = Selection.activeGameObject;
		if (go == null)
		{
			Debug.LogError("没有选中目标GameObject");
			return;
		}
		
		Debug.Log(go.transform.GetAllGOCount());
		List<Tuple<string, int, int>> meshInfos = go.transform.GetAllMeshInfo();
		Debug.Log(meshInfos.IEnumerableToString((Tuple<string, int, int> value) => { 
			return string.Format("{0} , verts:{1}, trianles:{2}",value.item1, value.item2, value.item3);
		}));
		List<ShaderInfo> shaderinfos = go.transform.GetAllShaderInfo();
		Debug.Log(string.Format("shaders:{0} \r\n {1}", shaderinfos.Count.ToString(), shaderinfos.IEnumerableToString(
			(ShaderInfo value) => { return value.shaderName + "  Mat Count:"+value.matGO.Count.ToString();})));
		Debug.Log(shaderinfos.IEnumerableToString());
	}

	[MenuItem("Tools/Model/ModelAnalyseWin")]
	static void OnWindowCreate()
	{
		
	}
}
