using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ShaderInfo
{
       public string shaderName;
       public Dictionary<string, List<string>> matGO = new Dictionary<string, List<string>>();

       public ShaderInfo(string shaderName)
       {
              this.shaderName = shaderName;
       }

       public void AddMaterial(Material material, string gameObjectName = null)
       {
              string materialName = material.name;
              List<string> objs = null;
              if (!matGO.TryGetValue(materialName, out objs))
              {
                   objs = new List<string>();
                   matGO.Add(materialName,objs);
              }

              if (!string.IsNullOrEmpty(gameObjectName)&&!objs.Contains(gameObjectName))
              {
                     objs.Add(gameObjectName);
              }
       }

       public void RemoveMaterial(Material material)
       {
              string materialName = material.name;
              if (matGO.ContainsKey(materialName))
              {
                     matGO.Remove(materialName);
              }
       }

       public override string ToString()
       {
              StringBuilder builder = new StringBuilder();
              builder.AppendFormat("shader:{0} ", shaderName);
              if (matGO.Count > 0)
                     builder.Append("[");
              foreach (var pair in matGO)
              {
                     builder.AppendFormat(" (Material:{0} -- ", pair.Key);
                     for (int i = 0, count = pair.Value.Count; i < count; i++)
                     {
                            if (i != count - 1)
                            {
                                   builder.AppendFormat("{0} , ", pair.Value[i]);
                            }
                            else
                            {
                                   builder.Append(pair.Value[i]);
                            }
                     }

                     builder.Append(" ) ");
              }
              if (matGO.Count > 0)
                     builder.Append("]");
              return builder.ToString();
       }
}