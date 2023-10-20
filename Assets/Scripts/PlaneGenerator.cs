using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneGenerator : MonoBehaviour
{
    public bool isEnabled = true;
    
    public float planeZLength;
    public float planeXLength;
    
    public Transform transformCenter;

    public Material material;
    
    private Vector3 center;
    
    public CameraComponent _cameraComponent;

    

    public Vector3[] GetFrontLine()
    {
        var halfZ = planeZLength * .5f;
        var halfX = planeXLength * .5f;

        return new[] {
            new Vector3(center.x - halfX, center.y, -halfZ),
            new Vector3(center.x + halfX, center.y, -halfZ),
        };
    }

    public Vector3[] GetBackLine()
    {
        var halfZ = planeZLength * .5f;
        var halfX = planeXLength * .5f;

        return new[] {
            new Vector3(center.x - halfX, center.y, halfZ),
            new Vector3(center.x + halfX, center.y, halfZ),
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
        var squareVectors = GetFrontLine();
        var frontScale = focalLength / ((center.z - planeZLength * .5f) + focalLength);
        for (int i = 0; i < squareVectors.Length; i++) 
        {


            GL.Color(material.color);
            var point1 = squareVectors[i] * frontScale;
            GL.Vertex3(point1.x, point1.y, 0);
            var point2 = squareVectors[(i + 1)% squareVectors.Length] * frontScale;
            GL.Vertex3(point2.x, point2.y, 0);

        }

        
        var backsquareVectors = GetBackLine();
        var backScale =  focalLength/((center.z + planeZLength * .5f) + focalLength);
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
