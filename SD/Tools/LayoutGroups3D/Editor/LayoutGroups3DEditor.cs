using UnityEditor;
using UnityEngine;

namespace SD.Tools
{
    [CustomEditor(typeof(LayoutGroups3D))]
    public class LayoutGroups3DEditor : Editor
    {
        SerializedProperty startPositionOffsetProp;

        SerializedProperty alignmentTypeProp;

        SerializedProperty spacingProp;
        SerializedProperty spacingOtherAxisProp;
        SerializedProperty spacingOtherAxis2Prop;
        SerializedProperty radialRadiusProp;
        SerializedProperty maxArcAngleProp;
        SerializedProperty startAngleOffsetProp;
        SerializedProperty constraintCountProp;
        SerializedProperty secondaryConstraintCountProp;
        SerializedProperty alignToRadiusProp;
        SerializedProperty useSameSpacingProp;

        SerializedProperty linearAxisProp;
        SerializedProperty radialAxisProp;
        SerializedProperty primaryGridAxisProp;
        SerializedProperty primaryAxisProp;
        SerializedProperty secondaryAxisProp;

        SerializedProperty xAxisDirProp;
        SerializedProperty yAxisDirProp;
        SerializedProperty zAxisDirProp;


        SerializedProperty showDebugLinesProp;
        SerializedProperty debugColorProp;

        private void OnEnable()
        {
            startPositionOffsetProp = serializedObject.FindProperty("startPositionOffset");

            alignmentTypeProp = serializedObject.FindProperty("alignmentType");

            linearAxisProp = serializedObject.FindProperty("linearAxis");
            radialAxisProp = serializedObject.FindProperty("radialAxis");
            primaryGridAxisProp = serializedObject.FindProperty("primaryGridAxis");
            primaryAxisProp = serializedObject.FindProperty("primaryAxis");
            secondaryAxisProp = serializedObject.FindProperty("secondaryAxis");

            spacingProp = serializedObject.FindProperty("spacing");
            spacingOtherAxisProp = serializedObject.FindProperty("spacingOtherAxis");
            spacingOtherAxis2Prop = serializedObject.FindProperty("spacingOtherAxis2");
            radialRadiusProp = serializedObject.FindProperty("radialRadius");
            maxArcAngleProp = serializedObject.FindProperty("maxArcAngle");
            startAngleOffsetProp = serializedObject.FindProperty("startAngleOffset");
            constraintCountProp = serializedObject.FindProperty("constraintCount");
            secondaryConstraintCountProp = serializedObject.FindProperty("secondaryConstraintCount");
            alignToRadiusProp = serializedObject.FindProperty("alignToRadius");
            useSameSpacingProp = serializedObject.FindProperty("useSameSpacing");

            xAxisDirProp = serializedObject.FindProperty("xAxisDirection");
            yAxisDirProp = serializedObject.FindProperty("yAxisDirection");
            zAxisDirProp = serializedObject.FindProperty("zAxisDirection");


            showDebugLinesProp = serializedObject.FindProperty("showDebugLines");
            debugColorProp = serializedObject.FindProperty("debugColor");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(alignmentTypeProp, new GUIContent("Alignment Type"));

            LayoutGroups3D.AlignType alignType = (LayoutGroups3D.AlignType)alignmentTypeProp.enumValueIndex;
            LayoutGroups3D.Axis linearAxis = (LayoutGroups3D.Axis)linearAxisProp.enumValueIndex;

            if (alignType == LayoutGroups3D.AlignType.Radial)
            {
                EditorGUILayout.PropertyField(radialAxisProp, new GUIContent("Radial Axis"));
                EditorGUILayout.PropertyField(radialRadiusProp, new GUIContent("Radial Radius"));
                EditorGUILayout.PropertyField(maxArcAngleProp, new GUIContent("Max Arc Angle"));
                EditorGUILayout.PropertyField(startAngleOffsetProp, new GUIContent("Start Angle Offset"));
                EditorGUILayout.PropertyField(alignToRadiusProp, new GUIContent("Align To Radius"));
                EditorGUILayout.PropertyField(showDebugLinesProp, new GUIContent("Show Debug Lines"));
                bool debug = showDebugLinesProp.boolValue;
                if (debug)
                {
                    EditorGUILayout.PropertyField(debugColorProp, new GUIContent("Debug Color"));
                }
            }
            else if (alignType == LayoutGroups3D.AlignType.Linear)
            {
                EditorGUILayout.PropertyField(startPositionOffsetProp, new GUIContent("Start Position Offset"));
                EditorGUILayout.PropertyField(linearAxisProp, new GUIContent("Linear Axis"));
                switch (linearAxis)
                {
                    case LayoutGroups3D.Axis.X:
                        EditorGUILayout.PropertyField(xAxisDirProp, new GUIContent("X Axis Direction"));
                        break;
                    case LayoutGroups3D.Axis.Y:
                        EditorGUILayout.PropertyField(yAxisDirProp, new GUIContent("Y Axis Direction"));
                        break;
                    case LayoutGroups3D.Axis.Z:
                        EditorGUILayout.PropertyField(zAxisDirProp, new GUIContent("Z Axis Direction"));
                        break;
                    default: break;
                }

                EditorGUILayout.PropertyField(spacingProp, new GUIContent("Spacing"));
            }
            else if (alignType == LayoutGroups3D.AlignType.Grid)
            {
                EditorGUILayout.PropertyField(startPositionOffsetProp, new GUIContent("Start Position Offset"));
                EditorGUILayout.PropertyField(primaryGridAxisProp, new GUIContent("Primary Grid Axis"));
                EditorGUILayout.PropertyField(constraintCountProp, new GUIContent("Constraint Count"));

                EditorGUILayout.PropertyField(useSameSpacingProp, new GUIContent("Use Same Spacing"));
                bool useSameSpacingValue = useSameSpacingProp.boolValue;

                EditorGUILayout.PropertyField(spacingProp, new GUIContent("Spacing Selected Axis"));
                if (!useSameSpacingValue)
                    EditorGUILayout.PropertyField(spacingOtherAxisProp, new GUIContent("Spacing Other Axis"));
            }
            else if (alignType == LayoutGroups3D.AlignType.Euclidean)
            {
                EditorGUILayout.PropertyField(startPositionOffsetProp, new GUIContent("Start Position Offset"));

                EditorGUILayout.PropertyField(primaryAxisProp, new GUIContent("Primary Axis"));
                EditorGUILayout.PropertyField(secondaryAxisProp, new GUIContent("Secondary Axis"));

                EditorGUILayout.PropertyField(constraintCountProp, new GUIContent("Constraint Count"));
                EditorGUILayout.PropertyField(secondaryConstraintCountProp, new GUIContent("Secondary Constraint Count"));

                EditorGUILayout.PropertyField(useSameSpacingProp, new GUIContent("Use Same Spacing"));
                bool useSameSpacingValue = useSameSpacingProp.boolValue;

                EditorGUILayout.PropertyField(spacingProp, new GUIContent("Spacing Selected Axis"));
                if (!useSameSpacingValue)
                {
                    EditorGUILayout.PropertyField(spacingOtherAxisProp, new GUIContent("Spacing Other Axis"));
                    EditorGUILayout.PropertyField(spacingOtherAxis2Prop, new GUIContent("Spacing Other Axis2"));
                }
            }

            serializedObject.ApplyModifiedProperties();

            LayoutGroups3D aligner = (LayoutGroups3D)target;
            aligner.Align();
        }
    }
}
