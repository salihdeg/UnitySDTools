using UnityEngine;

namespace SD.Tools
{
    public class LayoutGroup3D : MonoBehaviour
    {
        #region ENUMS

        public enum Axis
        {
            X, Y, Z
        }

        public enum RadialAxis
        {
            Y, Z
        }

        public enum GridAxis
        {
            X, Y
        }

        public enum AlignType
        {
            Linear, Grid, Radial, Euclidean
        }

        public enum XAxisDirection
        {
            Right, Left
        }

        public enum YAxisDirection
        {
            Up, Down
        }

        public enum ZAxisDirection
        {
            Front, Back
        }

        #endregion

        public AlignType alignmentType = AlignType.Linear;

        public Vector3 startPositionOffset = Vector3.zero;

        public Axis linearAxis;
        public RadialAxis radialAxis;
        public GridAxis primaryGridAxis;
        public Axis primaryAxis;
        public Axis secondaryAxis;

        public XAxisDirection xAxisDirection = XAxisDirection.Right;
        public YAxisDirection yAxisDirection = YAxisDirection.Up;
        public ZAxisDirection zAxisDirection = ZAxisDirection.Front;

        public float spacing;
        public float spacingOtherAxis;
        public float spacingOtherAxis2;
        public float radialRadius;
        public float maxArcAngle = 360f;
        public float startAngleOffset = 0f;
        [Min(2)] public int constraintCount;
        [Min(2)] public int secondaryConstraintCount;
        public bool alignToRadius = false;
        public bool useSameSpacing = true;

        public bool showDebugLines = false;
        public Color debugColor = Color.red;

        public void Align()
        {
            // List all children
            Transform[] children = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                children[i] = transform.GetChild(i);
            }

            Vector3 startPos = startPositionOffset;
            float currentPosition = startAngleOffset;
            int gridSelectedAxisCounter = 0;

            int euclidPrimaryCount = 0;
            int euclidSecondaryCount = 1;

            if (useSameSpacing)
            {
                spacingOtherAxis = spacing;
                spacingOtherAxis2 = spacing;
            }

            foreach (Transform child in children)
            {
                child.localPosition = startPos;
                switch (alignmentType)
                {
                    case AlignType.Linear:
                        switch (linearAxis)
                        {
                            case Axis.X:
                                switch (xAxisDirection)
                                {
                                    case XAxisDirection.Right:
                                        startPos.x += spacing;
                                        break;
                                    case XAxisDirection.Left:
                                        startPos.x -= spacing;
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case Axis.Y:
                                switch (yAxisDirection)
                                {
                                    case YAxisDirection.Up:
                                        startPos.y += spacing;
                                        break;
                                    case YAxisDirection.Down:
                                        startPos.y -= spacing;
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case Axis.Z:
                                switch (zAxisDirection)
                                {
                                    case ZAxisDirection.Front:
                                        startPos.z += spacing;
                                        break;
                                    case ZAxisDirection.Back:
                                        startPos.z -= spacing;
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            default: break;
                        }
                        break;
                    case AlignType.Grid:
                        gridSelectedAxisCounter++;

                        switch (primaryGridAxis)
                        {
                            case GridAxis.X:
                                if (gridSelectedAxisCounter >= constraintCount)
                                {
                                    startPos.y += spacingOtherAxis;
                                    startPos.x = startPositionOffset.x;
                                    gridSelectedAxisCounter = 0;
                                }
                                else
                                    startPos.x += spacing;
                                break;
                            case GridAxis.Y:
                                if (gridSelectedAxisCounter >= constraintCount)
                                {
                                    startPos.x += spacingOtherAxis;
                                    startPos.y = startPositionOffset.y;
                                    gridSelectedAxisCounter = 0;
                                }
                                else
                                    startPos.y += spacing;
                                break;
                            default: break;
                        }
                        break;
                    case AlignType.Radial:
                        float angle = currentPosition * Mathf.Deg2Rad;
                        switch (radialAxis)
                        {
                            case RadialAxis.Y:
                                startPos.y = Mathf.Cos(angle) * radialRadius;
                                startPos.x = Mathf.Sin(angle) * radialRadius;
                                break;
                            case RadialAxis.Z:
                                startPos.z = Mathf.Cos(angle) * radialRadius;
                                startPos.x = Mathf.Sin(angle) * radialRadius;
                                break;
                            default: break;
                        }

                        currentPosition += maxArcAngle / children.Length;
                        child.localPosition = startPos;

                        if (alignToRadius)
                        {
                            float angleInDegrees = Mathf.Atan2(startPos.y, startPos.x) * Mathf.Rad2Deg;
                            child.localRotation = Quaternion.Euler(0f, 0f, angleInDegrees);
                        }
                        else
                        {
                            child.localRotation = Quaternion.Euler(0f, 0f, 0f);
                        }
                        break;
                    case AlignType.Euclidean:
                        euclidPrimaryCount++;
                        child.localRotation = Quaternion.Euler(0f, 0f, 0f);
                        switch (primaryAxis)
                        {
                            case Axis.X:
                                switch (secondaryAxis)
                                {
                                    case Axis.Y:
                                        if (euclidPrimaryCount < constraintCount)
                                            startPos.x += spacing;
                                        else
                                        {
                                            startPos.x = startPositionOffset.x;
                                            startPos.y += spacingOtherAxis;
                                            euclidSecondaryCount++;
                                            euclidPrimaryCount = 0;
                                        }

                                        if (euclidSecondaryCount > secondaryConstraintCount)
                                        {
                                            startPos.x = startPositionOffset.x;
                                            startPos.y = startPositionOffset.y;
                                            startPos.z += spacingOtherAxis2;
                                            euclidSecondaryCount = 1;
                                        }
                                        break;
                                    case Axis.Z:
                                        if (euclidPrimaryCount < constraintCount)
                                            startPos.x += spacing;
                                        else
                                        {
                                            startPos.x = startPositionOffset.x;
                                            startPos.z += spacingOtherAxis;
                                            euclidSecondaryCount++;
                                            euclidPrimaryCount = 0;
                                        }

                                        if (euclidSecondaryCount > secondaryConstraintCount)
                                        {
                                            startPos.x = startPositionOffset.x;
                                            startPos.z = startPositionOffset.z;
                                            startPos.y += spacingOtherAxis2;
                                            euclidSecondaryCount = 1;
                                        }
                                        break;
                                    default: break;
                                }
                                break;
                            case Axis.Y:
                                switch (secondaryAxis)
                                {
                                    case Axis.X:
                                        if (euclidPrimaryCount < constraintCount)
                                            startPos.y += spacing;
                                        else
                                        {
                                            startPos.y = startPositionOffset.y;
                                            startPos.x += spacingOtherAxis;
                                            euclidSecondaryCount++;
                                            euclidPrimaryCount = 0;
                                        }

                                        if (euclidSecondaryCount > secondaryConstraintCount)
                                        {
                                            startPos.y = startPositionOffset.y;
                                            startPos.x = startPositionOffset.x;
                                            startPos.z += spacingOtherAxis2;
                                            euclidSecondaryCount = 1;
                                        }
                                        break;
                                    case Axis.Z:
                                        if (euclidPrimaryCount < constraintCount)
                                            startPos.y += spacing;
                                        else
                                        {
                                            startPos.y = startPositionOffset.y;
                                            startPos.z += spacingOtherAxis;
                                            euclidSecondaryCount++;
                                            euclidPrimaryCount = 0;
                                        }

                                        if (euclidSecondaryCount > secondaryConstraintCount)
                                        {
                                            startPos.y = startPositionOffset.y;
                                            startPos.z = startPositionOffset.z;
                                            startPos.x += spacingOtherAxis2;
                                            euclidSecondaryCount = 1;
                                        }
                                        break;
                                    default: break;
                                }
                                break;
                            case Axis.Z:
                                switch (secondaryAxis)
                                {
                                    case Axis.X:
                                        if (euclidPrimaryCount < constraintCount)
                                            startPos.z += spacing;
                                        else
                                        {
                                            startPos.z = startPositionOffset.z;
                                            startPos.x += spacingOtherAxis;
                                            euclidSecondaryCount++;
                                            euclidPrimaryCount = 0;
                                        }

                                        if (euclidSecondaryCount > secondaryConstraintCount)
                                        {
                                            startPos.z = startPositionOffset.z;
                                            startPos.x = startPositionOffset.x;
                                            startPos.y += spacingOtherAxis2;
                                            euclidSecondaryCount = 1;
                                        }
                                        break;
                                    case Axis.Y:
                                        if (euclidPrimaryCount < constraintCount)
                                            startPos.z += spacing;
                                        else
                                        {
                                            startPos.z = startPositionOffset.z;
                                            startPos.y += spacingOtherAxis;
                                            euclidSecondaryCount++;
                                            euclidPrimaryCount = 0;
                                        }

                                        if (euclidSecondaryCount > secondaryConstraintCount)
                                        {
                                            startPos.z = startPositionOffset.z;
                                            startPos.y = startPositionOffset.y;
                                            startPos.x += spacingOtherAxis2;
                                            euclidSecondaryCount = 1;
                                        }
                                        break;
                                    default: break;
                                }
                                break;
                            default: break;
                        }
                        break;
                    default: break;
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (showDebugLines)
            {
                foreach (Transform child in transform)
                    Debug.DrawLine(transform.position, child.position, debugColor);
            }
        }
    }
}
