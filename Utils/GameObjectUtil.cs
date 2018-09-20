using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using  UnityEngine;
using UnityEngine.Networking;

public static class GameObjectUtil
{
       public static int GetAllGOCount(this Transform transform)
       {
              int count = 0;
              GetChildCountRecursive(transform, ref count);
              return count;
       }

       static void GetChildCountRecursive(Transform obj, ref int count)
       {
              if(obj == null)
                     return;
              count++;
              foreach (Transform child in obj)
              {
                     GetChildCountRecursive(child,ref count);
              }
       }

       public static Dictionary<T, List<string>> GetAllComponentsInfo<T>(this Transform transform) where T :Component
       {
              Dictionary<T,List<string>> results = new Dictionary<T, List<string>>();
              GetAllCompInfoRecursive(transform, results);
              return results;
       }

       static void GetAllCompInfoRecursive<T>(Transform obj, Dictionary<T, List<string>> results)where T :Component
       {
              if(obj == null)
                     return;
              T[] comps = obj.GetComponents<T>();
              foreach (var comp in comps)
              {
                     List<string> values;
                     if (results.TryGetValue(comp, out values))
                     {
                            values.Add(obj.GetFullName());
                     }
                     else
                     {
                            values = new List<string>();
                            values.Add(obj.GetFullName());
                            results.Add(comp,values);
                     }
              }

              foreach (Transform child in obj)
              {
                     GetAllCompInfoRecursive(child,results);
              }
       }

       public static string GetFullName(this Transform transform)
       {
              StringBuilder builder = new StringBuilder();
              List<string> names = new List<string>();
              GetFullNameRecursive(transform, names);
              for (int i = names.Count -1 ; i >=0; i--)
              {
                     builder.Append(names[i]);
                     if (i != 0)
                            builder.Append("/");
              }
              return builder.ToString();
       }

       static void GetFullNameRecursive(Transform obj, List<string> names)
       {
              if(obj==null)
                     return;
              names.Add(obj.name);
              GetFullNameRecursive(obj.parent,names);
       }

      /// <summary>
      ///  统计transform身上以及所有子节点的Mesh信息
      /// </summary>
      /// <param name="transform"></param>
      /// <returns>[节点名, 顶点数, 三角面数] 值为-1时表示Mesh不存在</returns>
       public static List<Tuple<string,int,int>> GetAllMeshInfo(this Transform transform)
       {
              List<Tuple<string,int,int>> results = new List<Tuple<string,int,int>> ();
              GetAllMeshInfoInCursively(transform,results);
              return results;
       }

       static void GetAllMeshInfoInCursively(Transform obj,List<Tuple<string,int,int>> results)
       {
              if(obj == null)
                     return;
              MeshFilter[] meshFilters = obj.GetComponents<MeshFilter>();
              foreach (MeshFilter filter in meshFilters)
              {
                     Mesh mesh = filter.sharedMesh;
                     int triangles = -1;
                     int verts = -1;
                     if (mesh != null)
                     {
                        triangles = mesh.triangles.Length/3;
                        verts = mesh.vertexCount;
                     }

                     results.Add(new Tuple<string, int, int>(obj.GetFullName(),verts, triangles));
              }

              foreach (Transform child in obj)
              {
                     GetAllMeshInfoInCursively(child,results);
              }
       }

       /// <summary>
       /// 统计transform身上以及所有子节点用到的shader信息
       /// </summary>
       /// <param name="transform"></param>
       /// <returns>List<ShaderInfo></returns>
       public static List<ShaderInfo> GetAllShaderInfo(this Transform transform)
       {
              List<ShaderInfo> results = new List<ShaderInfo>();
              GetAllShaderInfoInCursively(transform,results);
              return results;
       }

       static void GetAllShaderInfoInCursively(Transform obj,  List<ShaderInfo> results)
       {
              if(obj == null)
                     return;
              Renderer[] renderers = obj.GetComponents<Renderer>();
              List<Material> materials = new List<Material>();
              string objFullName = obj.GetFullName();
              foreach (Renderer renderer in renderers)
              {
                     materials.Clear();
                     materials.AddRange(renderer.sharedMaterials);
                     materials.Add(renderer.sharedMaterial);
                     foreach (Material mat in materials)
                     {
                            if (mat == null)
                                   continue;
                            string shaderName = mat.shader.name;
                            ShaderInfo elem = results.Find((x) => x.shaderName == shaderName);
                            if (elem == null)
                            {
                                   elem = new ShaderInfo(shaderName);
                                   results.Add(elem);
                            }
                            elem.AddMaterial(mat, objFullName);
                     }
              }

              foreach (Transform child in obj)
              {
                     GetAllShaderInfoInCursively(child, results);
              }
       }
}