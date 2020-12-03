using System;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomPropertyDrawer(typeof(TransformData))]
public class TransformDataDrawer : VPropertyDrawer
{
	private const float LENGHT_SCALE_RAY = 50.0f;
	private const float RATIO_WIDTH_LABEL = 0.333333333f;
	private const float RATIO_WIDTH_FIELD = 0.666666666f;

	private SerializedProperty parent;
	private SerializedProperty position;
	private SerializedProperty rotation;
	private SerializedProperty eulerAngles;
	private SerializedProperty scale;
	private SerializedProperty showHandles;
	private Transform parentTransform;
	private bool subscribedToSceneGUI;

	/// <summary>Overrides OnGUI method.</summary>
	/// <param name="_position">Rectangle on the screen to use for the property GUI.</param>
	/// <param name="_property">The SerializedProperty to make the custom GUI for.</param>
	/// <param name="_label">The Label of this Property.</param>
	public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
	{
		float width = _position.width;
		float fieldDimension = width * RATIO_WIDTH_FIELD;
		float labelDimension = width * RATIO_WIDTH_LABEL;
		float horizontalDisplacement = labelDimension + SPACE_HORIZONTAL;

		BeginPropertyDrawing(_position, _property, _label);
		EditorGUIUtility.labelWidth = labelDimension;

		/*AddVerticalSpace();
		positionRect.width = labelDimension;
		EditorGUI.LabelField(positionRect, "Parent: ");
		positionRect.x += horizontalDisplacement;
		positionRect.width = fieldDimension;
		EditorGUI.PropertyField(positionRect, parent, new GUIContent());
		positionRect.x -= horizontalDisplacement;*/
		AddVerticalSpace();
		positionRect.width = labelDimension;
		EditorGUI.LabelField(positionRect, "Position: ");
		positionRect.x += horizontalDisplacement;
		positionRect.width = fieldDimension;
		EditorGUI.PropertyField(positionRect, position, new GUIContent());
		positionRect.x -= horizontalDisplacement;
		AddVerticalSpace();
		positionRect.width = labelDimension;
		EditorGUI.LabelField(positionRect, "Euler Rotation: ");
		positionRect.x += horizontalDisplacement;
		positionRect.width = fieldDimension;
		EditorGUI.PropertyField(positionRect, eulerAngles, new GUIContent());
		positionRect.x -= horizontalDisplacement;
		AddVerticalSpace();
		EditorGUI.LabelField(positionRect, "Rotation: ");
		positionRect.x += horizontalDisplacement;
		EditorGUI.LabelField(positionRect, rotation.quaternionValue.ToString());
		positionRect.x -= horizontalDisplacement;
		AddVerticalSpace();
		positionRect.width = labelDimension;
		EditorGUI.LabelField(positionRect, "Scale: ");
		positionRect.x += horizontalDisplacement;
		positionRect.width = fieldDimension;
		EditorGUI.PropertyField(positionRect, scale, new GUIContent());
		/*positionRect.x -= horizontalDisplacement;
		AddVerticalSpace();
		positionRect.width = labelDimension;
		EditorGUI.LabelField(positionRect, "Show Handles: ");
		positionRect.x += horizontalDisplacement;
		positionRect.width = fieldDimension;
		EditorGUI.PropertyField(positionRect, showHandles, new GUIContent());*/

		/*if(parent.objectReferenceValue != null)
		{
			parentTransform = parent.objectReferenceValue as Transform;
			Vector3 offset = position.vector3Value - parentTransform.position;
			Quaternion relativeRotation = parentTransform.rotation * rotation.quaternionValue;
			Vector3 relativeScale = Vector3.Scale(parentTransform.localScale, scale.vector3Value);

			positionRect.x -= horizontalDisplacement;
			positionRect.width = labelDimension + fieldDimension;
			AddVerticalSpace();
			EditorGUI.LabelField(positionRect, "Local Position: " + (parentTransform.position + (relativeRotation * offset)));
			AddVerticalSpace();
			EditorGUI.LabelField(positionRect, "Local Rotation: " + relativeRotation);
			AddVerticalSpace();
			EditorGUI.LabelField(positionRect, "Local Scale: " + relativeScale);
		}*/

		//OnSceneGUI(SceneView.lastActiveSceneView);

		EndPropertyDrawing(_property);
	}

	/// <summary>Enables the Editor to handle an event in the Scene view.</summary>
	/// <param name="_view">Scene's View.</param>
	private void OnSceneGUI(SceneView _view)
	{
		try
		{
			switch(Tools.current)
			{
				case Tool.View:
				break;

				case Tool.Move:
				if(position != null)
				{
					EditorGUI.BeginChangeCheck();
					Vector3 newPosition = Handles.PositionHandle(position.vector3Value, rotation.quaternionValue);
					if(EditorGUI.EndChangeCheck())
					{
						position.vector3Value = newPosition;
						position.serializedObject.ApplyModifiedProperties();
					}
				}
				break;

				case Tool.Rotate:
				if(rotation != null)
				{
					EditorGUI.BeginChangeCheck();
					Quaternion newRotation = Handles.RotationHandle(rotation.quaternionValue, position.vector3Value);
					if(EditorGUI.EndChangeCheck())
					{
						rotation.quaternionValue = newRotation;
						eulerAngles.vector3Value = newRotation.eulerAngles;
						eulerAngles.serializedObject.ApplyModifiedProperties();
						rotation.serializedObject.ApplyModifiedProperties();
					}
				}
				break;

				case Tool.Scale:
				if(scale != null)
				{
					EditorGUI.BeginChangeCheck();
					Vector3 newScale = Handles.ScaleHandle(scale.vector3Value, position.vector3Value, rotation.quaternionValue, scale.vector3Value.GetAverage() * LENGHT_SCALE_RAY);
					if(EditorGUI.EndChangeCheck())
					{
						scale.vector3Value = newScale;
						scale.serializedObject.ApplyModifiedProperties();
					}
				}
				break;
			}
		}
		catch(Exception exception)
		{
			Debug.LogWarning("[TransformDataDrawer] Catched Exception when trying to reload SceneGUI: " + exception.Message);
			/*subscribedToSceneGUI = false;
			SceneView.onSceneGUIDelegate -= OnSceneGUI;*/
		}	

	}

	/// <summary>Override this method to specify how tall the GUI for this field is in pixels.</summary>
	/// <param name="_property">The SerializedProperty to make the custom GUI for.</param>
	/// <param name="_label">The label of this property.</param>
	public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
	{
		/*if(!subscribedToSceneGUI)
		{
			SceneView.onSceneGUIDelegate += OnSceneGUI;
			subscribedToSceneGUI = true;
		}
		//SceneView.onSceneGUIDelegate += OnSceneGUI;*/

		//parent = _property.FindPropertyRelative("_parent");
		position = _property.FindPropertyRelative("_position");
		rotation = _property.FindPropertyRelative("_rotation");
		eulerAngles = _property.FindPropertyRelative("_eulerAngles");
		scale = _property.FindPropertyRelative("_scale");
		//showHandles = _property.FindPropertyRelative("showHandles");

		return SPACE_VERTICAL * 6.0f;
	}
}
}