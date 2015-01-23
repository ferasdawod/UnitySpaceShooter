using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using ProBuilder2.EditorEnum;

public class ProBuilderMenuItems : EditorWindow
{
	const int SECTION = 15;

	const int EDITOR_SELECTION = 45;

	// todo - Consolidate PRODUCT_NAME to pb_Constant
	#if PROTOTYPE
	const string PRODUCT_NAME = "Prototype";
	#else
	const string PRODUCT_NAME = "ProBuilder";
	#endif

#region WINDOW

	[MenuItem("Tools/" + PRODUCT_NAME + "/" + PRODUCT_NAME + " Window", false, SECTION + 0)]
	public static pb_Editor OpenEditorWindow()
	{
		if(EditorPrefs.HasKey(pb_Constant.pbDefaultOpenInDockableWindow) && !EditorPrefs.GetBool(pb_Constant.pbDefaultOpenInDockableWindow))
			return (pb_Editor)EditorWindow.GetWindow(typeof(pb_Editor), true, PRODUCT_NAME, true);			// open as floating window
		else
			return (pb_Editor)EditorWindow.GetWindow(typeof(pb_Editor), false, PRODUCT_NAME, true);			// open as dockable window
	}

	#if !PROTOTYPE
	#if UNITY_STANDALONE_OSX
	[MenuItem("Tools/ProBuilder/Texture Window _j", false, SECTION + 1)]
	#else
	[MenuItem("Tools/ProBuilder/Texture Window", false, SECTION + 1)]
	#endif
	public static void OpenTextureWindow()
	{
		pb_Editor.instance.SetEditLevel(EditLevel.Texture);
	}
	#endif

	[MenuItem("Tools/" + PRODUCT_NAME + "/Shape Window %#k", false, SECTION + 2)]
	public static void ShapeMenu()
	{
		EditorWindow.GetWindow(typeof(pb_Geometry_Interface), true, "Shape Menu", true);
	}

	[MenuItem("Tools/" + PRODUCT_NAME + "/Vertex Colors Window", false, SECTION + 3)]
	public static void Init()
	{
		bool openInDockableWindow = !pb_Preferences_Internal.GetBool(pb_Constant.pbDefaultOpenInDockableWindow);
		EditorWindow.GetWindow<pb_VertexColorInterface>(openInDockableWindow, "Vertex Colors", true);
	}

	public static void ForceCloseEditor()
	{
		EditorWindow.GetWindow<pb_Editor>().Close();
	}
#endregion

#region ProBuilder/Edit

	#if UNITY_STANDALONE_OSX // unity can't figure out how to implement single char shortcuts.  this breaks uppercase input on windows unity 4+, but not mac
	[MenuItem("Tools/ProBuilder/Texture Window _j", true, SECTION + 1)]
	[MenuItem("Tools/" + PRODUCT_NAME + "/Editor/Toggle Edit Level _g", true, EDITOR_SELECTION + 0)]
	[MenuItem("Tools/" + PRODUCT_NAME + "/Editor/Toggle Selection Mode _h", true, EDITOR_SELECTION + 1)]
	[MenuItem("Tools/" + PRODUCT_NAME + "/Editor/Toggle Handle Pivot _p", true, EDITOR_SELECTION + 1)]
	public static bool ValidateToggleSelectMode()
	{
		EditorWindow window = EditorWindow.focusedWindow;
		return window != null && (window.GetType() == typeof(SceneView) || window.GetType() == typeof(pb_Editor));
	}
	#endif

	#if UNITY_STANDALONE_OSX
	[MenuItem("Tools/" + PRODUCT_NAME + "/Editor/Toggle Edit Level _g", false, EDITOR_SELECTION + 0)]
	#else
	[MenuItem("Tools/" + PRODUCT_NAME + "/Editor/Toggle Edit Level", false, EDITOR_SELECTION + 0)]
	#endif
	public static void ToggleEditLevel()
	{
		pb_Editor.instance.ToggleEditLevel();
		switch(pb_Editor.instance.editLevel)
		{
			case EditLevel.Top:
				pb_Editor_Utility.ShowNotification("Top Level Editing");
				break;

			case EditLevel.Geometry:
				pb_Editor_Utility.ShowNotification("Geometry Editing");
				break;
		}
	}

	#if UNITY_STANDALONE_OSX
	[MenuItem("Tools/" + PRODUCT_NAME + "/Editor/Toggle Selection Mode _h", false, EDITOR_SELECTION + 1)]
	#else
	[MenuItem("Tools/" + PRODUCT_NAME + "/Editor/Toggle Selection Mode", false, EDITOR_SELECTION + 1)]
	#endif
	public static void ToggleSelectMode()
	{
		pb_Editor.instance.ToggleSelectionMode();
		switch(pb_Editor.instance.selectionMode)
		{
			case SelectMode.Face:
				pb_Editor_Utility.ShowNotification("Editing Faces");
				break;

			case SelectMode.Vertex:
				pb_Editor_Utility.ShowNotification("Editing Vertices");
				break;

			case SelectMode.Edge:
				pb_Editor_Utility.ShowNotification("Editing Edges\n(Beta!)");
				break;
		}
	}

	#if UNITY_STANDALONE_OSX
	[MenuItem("Tools/" + PRODUCT_NAME + "/Editor/Toggle Handle Pivot _p", false, EDITOR_SELECTION + 1)]
	#else
	[MenuItem("Tools/" + PRODUCT_NAME + "/Editor/Toggle Handle Pivot", false, EDITOR_SELECTION + 1)]
	#endif
	public static void ToggleHandleAlignment()
	{
		pb_Editor.instance.ToggleHandleAlignment();		
		pb_Editor_Utility.ShowNotification("Handle Alignment: " + ((HandleAlignment)pb_Editor.instance.handleAlignment).ToString());
	}

	[MenuItem("Tools/" + PRODUCT_NAME + "/Editor/Lightmap Settings Window", false, EDITOR_SELECTION + 2)]
	public static void LightmapWindowInit()
	{
		pb_Lightmap_Editor.Init(pb_Editor.instance);
	}

	[MenuItem("Tools/" + pb_Constant.PRODUCT_NAME + "/Editor/Invert Face Selection %#i", false, EDITOR_SELECTION + 3)]
	public static void InvertFaceSelection()
	{
		foreach(pb_Object pb in pbUtil.GetComponents<pb_Object>(Selection.transforms))
		{
			List<pb_Face> unselectedFaces = new List<pb_Face>();
			foreach(pb_Face face in pb.faces)
			{
				if(!pb.selected_faces.Contains(face))
					unselectedFaces.Add(face);
			}

			pb_Editor.instance.SetSelectedFaces(pb, unselectedFaces.ToArray());
			pb_Editor.instance.UpdateSelection();
			SceneView.RepaintAll();
		}
	} 
#endregion	
}