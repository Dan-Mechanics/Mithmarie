using System.Collections.Generic;
using UnityEngine;

namespace Mithmarie
{
    public class GreedyWorldMesh : IWorldMeshStrategy
    {
        private Dictionary<Vector3Int, HashSet<Vector3Int>> chunks;
        private bool Has(Vector3Int blockPos)
        {
            Vector3Int chunkPos = Utils.GetChunkPos(blockPos, World.CHUNK_SIZE);
            if (!chunks.ContainsKey(chunkPos))
                return false;

            return chunks[chunkPos].Contains(blockPos);
        }

        public List<Mesh> GenerateAsChunks(Dictionary<Vector3Int, HashSet<Vector3Int>> chunks)
        {
            this.chunks = chunks;
            List<Mesh> meshes = new List<Mesh>();

            int counter = 1;
            foreach (HashSet<Vector3Int> blocks in chunks.Values)
            {
                List<Vector3> verts = new List<Vector3>();
                List<int> tris = new List<int>();
                List<Vector2> uvs = new List<Vector2>();
                Mesh mesh = new Mesh();
                mesh.name = $"chunk_{counter}_greedy";
                counter++;

                MeshingUtils.GenerateGreedyMesh(blocks, Has, verts, tris, uvs);

                mesh.vertices = verts.ToArray();
                mesh.triangles = tris.ToArray();
                mesh.uv = uvs.ToArray();
                mesh.RecalculateNormals();

                meshes.Add(mesh);
            }

            return meshes;
        }

        public Mesh GenerateMesh(HashSet<Vector3Int> blocks)
        {
            List<Vector3> verts = new List<Vector3>();
            List<int> tris = new List<int>();
            List<Vector2> uvs = new List<Vector2>();
            Mesh mesh = new Mesh();
            mesh.name = "greedy_level";

            MeshingUtils.GenerateGreedyMesh(blocks, blocks.Contains, verts, tris, uvs);

            mesh.vertices = verts.ToArray();
            mesh.triangles = tris.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.RecalculateNormals();

            return mesh;
        }

        public static MeshData _GenerateMesh(HashSet<Vector3Int> blocks)
        {
            List<Vector3> verts = new List<Vector3>();
            List<int> tris = new List<int>();
            List<Vector2> uvs = new List<Vector2>();
            //Mesh mesh = new Mesh();
            //mesh.name = "greedy_level";

            MeshingUtils.GenerateGreedyMesh(blocks, blocks.Contains, verts, tris, uvs);

            //mesh.vertices = verts.ToArray();
            //mesh.triangles = tris.ToArray();
            //mesh.uv = uvs.ToArray();
            //mesh.RecalculateNormals();

            return new MeshData() { name = "greedy_level", verts = verts, tris = tris, uvs = uvs };
        }
    }
}