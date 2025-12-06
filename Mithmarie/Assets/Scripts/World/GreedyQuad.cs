using System.Collections.Generic;
using UnityEngine;

namespace Mithmarie
{
    /// <summary>
    /// This struct is heavily dependant
    /// on context to be made into a mesh.
    /// </summary>
    public struct GreedyQuad
    {
        public int minX;
        public int maxX;
        public int minY;
        public int maxY;

        private readonly Vector2Int origin;
    
        public GreedyQuad(Vector2Int origin, HashSet<Vector2Int> facesRemaining)
        {
            this.origin = origin;
            minX = origin.x;
            minY = origin.y;
            maxX = minX + 1;
            maxY = minY + 1;
    
            facesRemaining.Remove(origin);
        }
    
        public void ExpandRight(HashSet<Vector2Int> facesRemaining)
        {
            Vector2Int head = origin;
            head.x++;
    
            while (facesRemaining.Contains(head))
            {
                facesRemaining.Remove(head);
                head.x++;
            }
    
            maxX = head.x;
        }
    
        public void ExpandLeft(HashSet<Vector2Int> facesRemaining)
        {
            Vector2Int head = origin;
            head.x--;
    
            while (facesRemaining.Contains(head))
            {
                facesRemaining.Remove(head);
                head.x--;
            }
    
            minX = head.x + 1;
        }
    
        public void ExpandUp(HashSet<Vector2Int> facesLeft)
        {
            Queue<Vector2Int> clipped = new Queue<Vector2Int>();
            Vector2Int head = origin;
            head.y++;
    
            while (ContainsRow(head.y, facesLeft, clipped))
            {
                while(clipped.Count > 0)
                {
                    facesLeft.Remove(clipped.Dequeue());
                }
    
                head.y++;
            }
    
            maxY = head.y;
        }
        
        public void ExpandDown(HashSet<Vector2Int> facesLeft)
        {
            Queue<Vector2Int> clipped = new Queue<Vector2Int>();
            Vector2Int head = origin;
            head.y--;
    
            while (ContainsRow(head.y, facesLeft, clipped))
            {
                while (clipped.Count > 0)
                {
                    facesLeft.Remove(clipped.Dequeue());
                }
    
                head.y--;
            }
    
            minY = head.y + 1;
        }
        
        private bool ContainsRow(int y, HashSet<Vector2Int> facesLeft, Queue<Vector2Int> clipped)
        {
            Vector2Int head = new Vector2Int(0, y);
            for (int x = minX; x < maxX; x++)
            {
                head.x = x;
                clipped.Enqueue(head);
                if (!facesLeft.Contains(head))
                    return false;
            }
    
            return true;
        }
    }
}