using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderGenerator : MonoBehaviour
{
    public bool isEnabled = true;
    
    public int numberOfSides = 6;
    public float cylinderHeight;
    public float cylinderRadius;
    
    public Transform transformCenter;

    public Material cylinderMaterial;
    
    private Vector3 center;
    
    public CameraComponent _cameraComponent;

    public Vector3[] GetCirclePoints(float z)
    {
        Vector3[] points = new Vector3[numberOfSides];
        for (int i = 0; i < numberOfSides; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfSides;
            points[i] = new Vector3(center.x + cylinderRadius * Mathf.Cos(angle), center.y + cylinderRadius * Mathf.Sin(angle), z);
        }
        return points;
    }

    private void OnPostRender()
    {
        DrawLines();
    }

    private void OnDrawGizmos()
    {
        DrawLines();
    }

    private void DrawLines()
    {
        if (!isEnabled || cylinderMaterial == null || _cameraComponent == null)
        {
            return;
        }
        
        if (transformCenter != null)
        {
            center = transformCenter.position;
        }

        var focalLength = _cameraComponent.focalLength;
        
        GL.PushMatrix();
        GL.Begin(GL.LINES);
        cylinderMaterial.SetPass(0);

        var topCircle = GetCirclePoints(center.z - cylinderHeight * .5f);
        var bottomCircle = GetCirclePoints(center.z + cylinderHeight * .5f);

        var topScale = focalLength / ((center.z - cylinderHeight * .5f) + focalLength);
        var bottomScale =  focalLength/((center.z + cylinderHeight * .5f) + focalLength);

        for (int i = 0; i < topCircle.Length; i++) 
        {
            GL.Color(cylinderMaterial.color);
            var point1 = topCircle[i] * topScale;
            GL.Vertex3(point1.x, point1.y, 0);
            var point2 = topCircle[(i + 1)% topCircle.Length] * topScale;
            GL.Vertex3(point2.x, point2.y, 0);
        }

        for (int i = 0; i < bottomCircle.Length; i++)
        {
            GL.Color(cylinderMaterial.color);
            var point1 = bottomCircle[i] * bottomScale;
            GL.Vertex3(point1.x, point1.y, 0);
            var point2 = bottomCircle[(i + 1) % bottomCircle.Length] * bottomScale;
            GL.Vertex3(point2.x, point2.y, 0);
        }

        for (int i = 0; i < bottomCircle.Length; i++)
        {
            GL.Color(cylinderMaterial.color);
            var point1 = topCircle[i] * topScale;
            GL.Vertex3(point1.x, point1.y, 0);
            var point2 = bottomCircle[i] * bottomScale;
            GL.Vertex3(point2.x, point2.y, 0);
        }

        GL.End();
        GL.PopMatrix();
    }
}
