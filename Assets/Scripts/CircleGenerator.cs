using UnityEngine;

public class CircleGenerator : MonoBehaviour
{
    public bool isEnabled = true;
    
    public float radius = 200.0f;
    public int latitudeSegments = 25;
    public int longitudeSegments = 25;
    
    public Material material;
    
    public CameraComponent _cameraComponent;

    private Vector3[][] sphereVertices;

    private void Start()
    {
        GenerateSphere();
    }

    private float Map(float value, float start1, float stop1, float start2, float stop2)
    {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }

    private void GenerateSphere()
    {
        sphereVertices = new Vector3[latitudeSegments + 1][];

        for (int i = 0; i <= latitudeSegments; i++)
        {
            sphereVertices[i] = new Vector3[longitudeSegments + 1];
            float lat = Map(i, 0, latitudeSegments, 0, Mathf.PI);

            for (int j = 0; j <= longitudeSegments; j++)
            {
                float lon = Map(j, 0, longitudeSegments, 0, 2 * Mathf.PI);

                float x = radius * Mathf.Sin(lat) * Mathf.Cos(lon);
                float y = radius * Mathf.Sin(lat) * Mathf.Sin(lon);
                float z = radius * Mathf.Cos(lat);

                sphereVertices[i][j] = new Vector3(x, y, z);
            }
        }
    }

    private void OnPostRender()
    {
        DrawSphere();
    }

    private void OnDrawGizmos()
    {
        DrawSphere();
    }

    private void DrawSphere()
    {
        if (!isEnabled || material == null || _cameraComponent == null)
        {
            return;
        }

        var focalLength = _cameraComponent.focalLength;

        GL.PushMatrix();
        GL.Begin(GL.LINES);
        material.SetPass(0);

        for (int i = 0; i < latitudeSegments; i++)
        {
            for (int j = 0; j < longitudeSegments; j++)
            {
                var v1 = sphereVertices[i][j] * focalLength / (sphereVertices[i][j].z + focalLength);
                var v2 = sphereVertices[i + 1][j] * focalLength / (sphereVertices[i + 1][j].z + focalLength);
                var v3 = sphereVertices[i][j + 1] * focalLength / (sphereVertices[i][j + 1].z + focalLength);
                var v4 = sphereVertices[i + 1][j + 1] * focalLength / (sphereVertices[i + 1][j + 1].z + focalLength);

                GL.Color(material.color);
                
                // Draw lines
                GL.Vertex3(v1.x, v1.y, 0);
                GL.Vertex3(v2.x, v2.y, 0);

                GL.Vertex3(v1.x, v1.y, 0);
                GL.Vertex3(v3.x, v3.y, 0);
            }
        }

        GL.End();
        GL.PopMatrix();
    }
}
