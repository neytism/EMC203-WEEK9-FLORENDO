using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidGenerator : MonoBehaviour
{
    public bool isEnabled = true;
    
    public float pyramidZLength;
    public float pyramidXLength;
    public float pyramidHeight;
    
    public Transform transformCenter;

    public Material material;
    
    private Vector3 center;
    
    public CameraComponent _cameraComponent;



    public Vector3 GetApexPoint()
    {
        var halfHeight = pyramidHeight * .5f;

        return new Vector3(center.x, center.y + halfHeight, center.z);
    }
    
    public Vector3[] GetFrontLine()
    {
        var halfZ = pyramidZLength * .5f;
        var halfX = pyramidXLength * .5f;
        var halfHeight = pyramidHeight * .5f;

        return new[] {
            new Vector3(center.x - halfX, center.y - halfHeight, -halfZ),
            new Vector3(center.x + halfX, center.y - halfHeight, -halfZ),
        };
    }

    public Vector3[] GetBackLine()
    {
        var halfZ = pyramidZLength * .5f;
        var halfX = pyramidXLength * .5f;
        var halfHeight = pyramidHeight * .5f;

        return new[] {
            new Vector3(center.x - halfX, center.y - halfHeight, halfZ),
            new Vector3(center.x + halfX, center.y - halfHeight, halfZ),
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
        var frontLineVectors = GetFrontLine();
        var frontScale = focalLength / ((center.z - pyramidZLength * .5f) + focalLength);
        for (int i = 0; i < frontLineVectors.Length; i++) 
        {


            GL.Color(material.color);
            var point1 = frontLineVectors[i] * frontScale;
            GL.Vertex3(point1.x, point1.y, 0);
            var point2 = frontLineVectors[(i + 1)% frontLineVectors.Length] * frontScale;
            GL.Vertex3(point2.x, point2.y, 0);

        }

        
        var backLineVectors = GetBackLine();
        var backScale =  focalLength/((center.z + pyramidZLength * .5f) + focalLength);
        for (int i = 0; i < backLineVectors.Length; i++)
        {


            GL.Color(material.color);
            var point1 = backLineVectors[i] * backScale;
            GL.Vertex3(point1.x, point1.y, 0);
            var point2 = backLineVectors[(i + 1) % frontLineVectors.Length] * backScale;
            GL.Vertex3(point2.x, point2.y, 0);

        }

        for (int i = 0; i < backLineVectors.Length; i++)
        {
            
            GL.Color(material.color);
            var point1 = frontLineVectors[i] * frontScale;
            GL.Vertex3(point1.x, point1.y, 0);
            var point2 = backLineVectors[i] * backScale;
            GL.Vertex3(point2.x, point2.y, 0);
        }
        
        for (int i = 0; i < backLineVectors.Length; i++)
        {
            
            GL.Color(material.color);
            var point1 = backLineVectors[i] * backScale;
            GL.Vertex3(point1.x, point1.y, 0);
            var point2 = GetApexPoint();
            GL.Vertex3(point2.x, point2.y, 0);
        }

        
        for (int i = 0; i < frontLineVectors.Length; i++)
        {
            
            GL.Color(material.color);
            var point1 = frontLineVectors[i] * frontScale;
            GL.Vertex3(point1.x, point1.y, 0);
            var point2 = GetApexPoint();
            GL.Vertex3(point2.x, point2.y, 0);
        }

        GL.End();
        GL.PopMatrix();
    }




}
