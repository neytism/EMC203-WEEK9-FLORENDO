using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
    public bool isEnabled = true;
    
    public float cubeSideLength;

    public Transform transformCenter;

    public Material material;
    
    private Vector3 center;
    
    public CameraComponent _cameraComponent;

    

    public Vector3[] GetFrontSquare()
    {
        var halfLength = cubeSideLength * .5f;

        return new[] { 
            new Vector3(center.x + halfLength, center.y + halfLength, -halfLength),
            new Vector3(center.x - halfLength, center.y + halfLength, -halfLength),
            new Vector3(center.x - halfLength, center.y - halfLength, -halfLength),
            new Vector3(center.x + halfLength, center.y - halfLength, -halfLength),
        };
    }

    public Vector3[] GetBackSquare()
    {
        var halfLength = cubeSideLength * .5f;

        return new[] {
            new Vector3(center.x + halfLength, center.y + halfLength, halfLength),
            new Vector3(center.x - halfLength, center.y + halfLength, halfLength),
            new Vector3(center.x - halfLength, center.y - halfLength, halfLength),
            new Vector3(center.x + halfLength, center.y - halfLength, halfLength),
        };
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

        if (!isEnabled || material == null || _cameraComponent == null)
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
        material.SetPass(0);
        var squareVectors = GetFrontSquare();
        var frontScale = focalLength / ((center.z - cubeSideLength * .5f) + focalLength);
        for (int i = 0; i < squareVectors.Length; i++) 
        {


            GL.Color(material.color);
            var point1 = squareVectors[i] * frontScale;
            GL.Vertex3(point1.x, point1.y, 0);
            var point2 = squareVectors[(i + 1)% squareVectors.Length] * frontScale;
            GL.Vertex3(point2.x, point2.y, 0);

        }

        
        var backsquareVectors = GetBackSquare();
        var backScale =  focalLength/((center.z + cubeSideLength * .5f) + focalLength);
        for (int i = 0; i < backsquareVectors.Length; i++)
        {


            GL.Color(material.color);
            var point1 = backsquareVectors[i] * backScale;
            GL.Vertex3(point1.x, point1.y, 0);
            var point2 = backsquareVectors[(i + 1) % squareVectors.Length] * backScale;
            GL.Vertex3(point2.x, point2.y, 0);

        }

        for (int i = 0; i < backsquareVectors.Length; i++)
        {


            GL.Color(material.color);
            var point1 = squareVectors[i] * frontScale;
            GL.Vertex3(point1.x, point1.y, 0);
            var point2 = backsquareVectors[i] * backScale;
            GL.Vertex3(point2.x, point2.y, 0);

        }


        GL.End();
        GL.PopMatrix();
    }




}
