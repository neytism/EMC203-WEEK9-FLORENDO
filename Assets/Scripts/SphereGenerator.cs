using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereGenerator : MonoBehaviour
{
    public bool isEnabled = true;

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

    private Vector3[] GetRing(int j)
    {
        Vector3[] ring = new Vector3[segments + 1];
        float phi = j * Mathf.PI / (rings + 1);

        for (int i = 0; i <= segments; i++)
        {
            float theta = i * 2 * Mathf.PI / segments;
            var x = center.x + radius * Mathf.Sin(phi) * Mathf.Cos(theta);
            var y = center.y + radius * Mathf.Sin(phi) * Mathf.Sin(theta);
            var z = center.z + radius * Mathf.Cos(phi);
            
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
            center = transformCenter.position ;
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

        for (int j = 0; j <= rings; j++)
        {
            var ringVectors = GetRing(j);

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
            
            if (j <= rings)
            {
                var nextRingVectors = GetRing(j + 1);

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
            }
        }

        GL.End();
        GL.PopMatrix();
    }
}
