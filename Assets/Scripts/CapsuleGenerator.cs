using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleGenerator : MonoBehaviour
{
    public bool isEnabled = true;

    public float capsuleLength;
    public float radius;

    public Transform transformCenter;

    public Material material;

    private Vector3 center;

    public CameraComponent _cameraComponent;

    public int segments = 10;
    public int rings = 10;

    private void OnPostRender()
    {
        DrawLines();
    }

    private void OnDrawGizmos()
    {
        DrawLines();
    }

    private Vector3[] GetRing(int j, float offset)
    {
        Vector3[] ring = new Vector3[segments + 1];
        float phi = j * Mathf.PI / (rings - 1) - Mathf.PI / 2; 

        for (int i = 0; i <= segments; i++)
        {
            float theta = i * 2 * Mathf.PI / segments;
            var x = center.x + radius * Mathf.Cos(phi) * Mathf.Cos(theta);
            var y = center.y + radius * Mathf.Cos(phi) * Mathf.Sin(theta);
            var z = center.z + radius * Mathf.Sin(phi) + offset;

            ring[i] = new Vector3(x, y, z);
        }

        return ring;
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

        if (rings < 0)
        {
            rings = 0;
        }

        if (segments < 0)
        {
            segments = 0;
        }

        var focalLength = _cameraComponent.focalLength;

        GL.PushMatrix();
        GL.Begin(GL.LINES);
        material.SetPass(0);

        Vector3[] topLastRing = null;
        for (int j = rings / 2; j <= rings; j++)
        {
            if (j ==rings / 2)
            {
                topLastRing =  DrawRing(j, capsuleLength / 2);
            }
            
            DrawRing(j, capsuleLength / 2);
        }

        Vector3[] bottomFirstRing = null;
        for (int j = 0; j < rings / 2; j++)
        {
            if (j == (rings / 2)-1)
            {
                bottomFirstRing = DrawRing(j, -capsuleLength / 2);
            }
            
            DrawRing(j, -capsuleLength / 2);
        }

        if (topLastRing != null && bottomFirstRing != null)
        {
            for (int i = 0; i < topLastRing.Length; i++)
            {
                GL.Color(material.color);
                var scale1 = focalLength / ((topLastRing[i].z - center.z) + focalLength); 
                var point1 = topLastRing[i] * scale1;
                GL.Vertex3(point1.x, point1.y, 0);
                var scale2 = focalLength / ((bottomFirstRing[i].z - center.z) + focalLength); 
                var point2 = bottomFirstRing[i] * scale2;
                GL.Vertex3(point2.x, point2.y, 0);
            }
        }
        
        
        GL.End();
        GL.PopMatrix();
    }


    private Vector3[] DrawRing(int j, float offset)
    {
        
        var focalLength = _cameraComponent.focalLength;
        var ringVectors = GetRing(j, offset);

        for (int i = 0; i < ringVectors.Length; i++)
        {
            GL.Color(material.color);
            var scale1 = focalLength / ((ringVectors[i].z - center.z) + focalLength); 
            var point1 = ringVectors[i] * scale1;
            GL.Vertex3(point1.x, point1.y, 0);
            var scale2 = focalLength / ((ringVectors[(i + 1) % ringVectors.Length].z - center.z) + focalLength); 
            var point2 = ringVectors[(i + 1) % ringVectors.Length] * scale2;
            GL.Vertex3(point2.x, point2.y, 0);
        }
        
        var nextRingVectors = GetRing(j + 1, offset);
        
        for (int i = 0; i < ringVectors.Length; i++)
        {
            GL.Color(material.color);
            var scale1 = focalLength / ((ringVectors[i].z - center.z) + focalLength);
            var point1 = ringVectors[i] * scale1;
            GL.Vertex3(point1.x, point1.y, 0);
            var scale2 = focalLength / ((nextRingVectors[i].z - center.z) + focalLength);
            var point2 = nextRingVectors[i] * scale2;
            GL.Vertex3(point2.x, point2.y, 0);
        }
    
        return ringVectors;
    }
}