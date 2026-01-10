using System.Collections.Generic;
using UnityEngine;

namespace Mithmarie
{
    public class CulledWorldMesh : IWorldMeshStrategy
    {
        private Dictionary<Vector3Int, HashSet<Vector3Int>> chunks;

        private bool Has(Vector3Int blockPos)
        {
            Vector3Int chunkPos = Utils.GetChunkPos(blockPos, World.CHUNK_SIZE);
            if (!chunks.ContainsKey(chunkPos))
                return false;

            return chunks[chunkPos].Contains(blockPos);
        }

        public List<MeshData> GenerateAsChunks(Dictionary<Vector3Int, HashSet<Vector3Int>> chunks)
        {
            this.chunks = chunks;
            List<MeshData> meshes = new List<MeshData>();

            int counter = 1;
            foreach (HashSet<Vector3Int> blocks in chunks.Values)
            {
                List<Vector3> verts = new List<Vector3>();
                List<int> tris = new List<int>();
                List<Vector2> uvs = new List<Vector2>();

                MeshingUtils.GenerateGreedyMesh(blocks, Has, verts, tris, uvs);

                meshes.Add(new MeshData() { name = $"chunk_{counter}_greedy", verts = verts, tris = tris, uvs = uvs });
                counter++;
            }

            return meshes;
        }

        public MeshData GenerateMesh(HashSet<Vector3Int> blocks)
        {
            List<Vector3> verts = new List<Vector3>();
            List<int> tris = new List<int>();
            List<Vector2> uvs = new List<Vector2>();

            MeshingUtils.GenerateCulledMesh(blocks, blocks.Contains, verts, tris, uvs);

            return new MeshData() { name = "greedy_level", verts = verts, tris = tris, uvs = uvs };
        }
    }
}