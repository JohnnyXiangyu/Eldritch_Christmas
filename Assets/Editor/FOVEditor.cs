using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(EnemyFOV))]
public class FOVEditor : Editor
{
    void OnSceneGUI()
    {
        EnemyFOV fov = (EnemyFOV)target;

        if (!fov.activelySearching)
        {
            return;
        }
        
        Handles.color = Color.green;
        Handles.DrawWireArc(fov.transform.position, Vector3.forward, Vector3.up, 360, fov.viewRadius);

        Vector3 viewAngleA = fov.DirectionFromAngle(-fov.viewAngle/2 + 90f + fov.angleOffset, false);
        Vector3 viewAngleB = fov.DirectionFromAngle(fov.viewAngle/2 + 90f + fov.angleOffset, false);
        Handles.color = Color.blue;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.viewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.viewRadius);
    }
}
