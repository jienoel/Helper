using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

	[MenuItem("Tools/Model/ModelTool To File")]
	static void InitToFile()
	{
		GameObject go = Selection.activeGameObject;
		if (go == null)
		{
			Debug.LogError("没有选中目标GameObject");
			return;
		}
		StringBuilder builder = new StringBuilder();
		builder.AppendLine("Child Count:"+go.transform.GetAllGOCount().ToString());
		List<Tuple<string, int, int>> meshInfos = go.transform.GetAllMeshInfo();
		builder.AppendLine(meshInfos.IEnumerableToString((Tuple<string, int, int> value) => { 
			return string.Format("{0} , verts:{1}, trianles:{2}",value.item1, value.item2, value.item3);
		}));
		List<ShaderInfo> shaderinfos = go.transform.GetAllShaderInfo();
		builder.AppendLine(string.Format("shaders:{0} \r\n {1}", shaderinfos.Count.ToString(), shaderinfos.IEnumerableToString(
			(ShaderInfo value) => { return value.shaderName + "  Mat Count:"+value.matGO.Count.ToString();})));
		builder.AppendLine(shaderinfos.IEnumerableToString());
		string path = EditorUtility.SaveFilePanel("", Application.dataPath, go.name, "txt");
		if (!string.IsNullOrEmpty(path))
		{
			File.WriteAllText(path,builder.ToString());
		}
	}

	[MenuItem("Tools/Model/ModelAnalyseWin")]
	static void OnWindowCreate()
	{
		
	}
}
