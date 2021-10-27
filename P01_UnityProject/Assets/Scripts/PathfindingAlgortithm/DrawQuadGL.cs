using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IA_sim
{
    public class DrawQuadGL : MonoBehaviour
    {
        public Material material;
        Mesh mesh;
        MeshRenderer meshRenderer;
        MeshFilter meshFilter;


        private List<int[]> intList;
        public struct QuadGL
        {

            public Color color;

            public Vector3 v1;
            public Vector3 v2;
            public Vector3 v3;
            public Vector3 v4;

            public QuadGL(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Color color)
            {
                this.v1 = v1;
                this.v2 = v2;
                this.v3 = v3;
                this.v4 = v4;

                this.color = color;
            }
        }

        private QuadGL GenerateQuad(int[] pos, Color color)
        {
            Vector3 topRight = new Vector3(pos[0] + 0.5f, 0.1f, pos[1] + 0.5f);
            Vector3 topLeft = new Vector3(pos[0] - 0.5f, 0.1f, pos[1] + 0.5f);
            Vector3 bottomRight = new Vector3(pos[0] + 0.5f, 0.1f, pos[1] - 0.5f);
            Vector3 bottomLeft = new Vector3(pos[0] - .5f, 0.1f, pos[1] - .5f);

            QuadGL output = new QuadGL(topRight, topLeft, bottomRight, bottomLeft, color);

            return output;
        }

        private void Start()
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.material = material;
            mesh = new Mesh();
            meshFilter.mesh = mesh;
        }

        public void DrawGLQuads(List<int[]> intList, Color color)
        {
            this.intList = intList;

            GL.PushMatrix();
            GL.Begin(GL.QUADS);
            GL.Color(color);
            for (int i = 0; i < intList.Count; i++)
            {
                QuadGL currentQuad = GenerateQuad(intList[i], color);
                GL.Vertex(currentQuad.v1);
                GL.Vertex(currentQuad.v2);
                GL.Vertex(currentQuad.v3);
                GL.Vertex(currentQuad.v4);
            }

            GL.End();
            GL.PopMatrix();
        }
    }
}
