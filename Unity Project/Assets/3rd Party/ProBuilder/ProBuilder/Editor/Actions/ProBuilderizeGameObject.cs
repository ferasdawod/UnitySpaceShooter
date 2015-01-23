using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using ProBuilder2.Common;

namespace ProBuilder2.Actions
{
public class ProBuilderizeMesh : Editor
{
	[MenuItem("Tools/" + pb_Constant.PRODUCT_NAME + "/Actions/ProBuilderize Selection")]
	public static void init()
	{
		foreach(Transform t in Selection.transforms)
		{
			if(t.GetComponent<MeshFilter>())
			{
				pb_Object pb = ProBuilderize(t);
				if(pb.GetComponent<MeshCollider>())	
					DestroyImmediate(pb.GetComponent<MeshCollider>());
			}
		}
	}

	public static pb_Object ProBuilderize(Transform t)
	{
		Mesh m = t.GetComponent<MeshFilter>().sharedMesh;

		pb_Face[] faces = new pb_Face[m.triangles.Length/3];
		int f = 0;
		for(int n = 0; n < m.subMeshCount; n++)
		{
			for(int i = 0; i < m.triangles.Length; i+=3)
			{
				faces[f] = new pb_Face(
					new int[3] {
						m.triangles[i+0],
						m.triangles[i+1],
						m.triangles[i+2]
						},
					t.GetComponent<MeshRenderer>().sharedMaterials[n],
					new pb_UV(),
					0,
					Color.white
				);
				f++;
			}
		}

		t.gameObject.SetActive(false);
		pb_Object pb = ProBuilder.CreateObjectWithVerticesFaces(m.vertices, faces);
		pb.SetName("FrankenMesh");
		pb_Editor_Utility.SetEntityType(ProBuilder.EntityType.Detail, pb.gameObject);
		
		GameObject go = pb.gameObject;

		go.transform.position = t.position;
		go.transform.localRotation = t.localRotation;
		go.transform.localScale = t.localScale;
		pb.FreezeScaleTransform();
		return pb;
	}
}
}