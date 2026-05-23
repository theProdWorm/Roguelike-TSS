using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Pathfinding
{
    [RequireComponent(typeof(Tilemap))]
    public class NavigationArea : MonoBehaviour
    {
        [SerializeField] private int _resolution = 3;
        [SerializeField] private bool _drawGizmos;
        
        public Dictionary<Node, List<Node>> Graph;
        
        [HideInInspector]
        [SerializeField] private Node[] _nodes;

        private Vector2 _worldOffset;
        private int _xSize, _ySize;

        private void Start()
        {
            if (Graph != null) 
                return;
            
            if (_nodes == null)
                GenerateNodes();
            else
                GenerateNeighbours();
        }

        public Node FindClosestNode(Vector2 position)
        {
            Vector2 offsetPosition = position + _worldOffset;

            float x = offsetPosition.x / _resolution;
            float y = offsetPosition.y / _resolution;

            int preIndex = Mathf.RoundToInt(x + y * _xSize);
            Node node = _nodes[preIndex];

            while (node.Blocked)
            {
                var neighbours = Graph[node];
                foreach (var neighbour in neighbours)
                {
                    node = neighbour;
                    if (!node.Blocked)
                        return node;
                }
            }

            return node;
        }
        
        public void GenerateNodes()
        {
            Graph = new();

            var bounds = GetComponent<Tilemap>().cellBounds;
            _xSize = bounds.size.x * _resolution;
            _ySize = bounds.size.y * _resolution;
            
            _nodes = new Node[_xSize * _ySize];

            _worldOffset = new Vector2(bounds.xMin - 0.5f, bounds.yMin - 0.5f)
                + new Vector2(2f / _resolution, 2f / _resolution);
            
            for (int i = 0; i < _nodes.Length; i++)
            {
                int x = i % _xSize;
                int y = i / _xSize;

                float xPos = x / (float) _resolution;
                float yPos = y / (float) _resolution;
                
                Vector2 pos = new Vector2(xPos, yPos) + _worldOffset;
                bool blocked = Physics2D.OverlapPoint(pos);

                _nodes[i] = new Node() { Position = pos, Blocked = blocked };
            }
            
            GenerateNeighbours();
        }

        private void GenerateNeighbours()
        {
            for (int i = 0; i < _nodes.Length; i++)
            {
                Graph[_nodes[i]] = new List<Node>();
                
                int x = i % _xSize;
                int y = i / _xSize;

                if (x > 0)
                {
                    Node leftNeighbour = _nodes[i - 1];
                    if (!leftNeighbour.Blocked)
                        Graph[_nodes[i]].Add(leftNeighbour);
                }

                if (x < _xSize - 1)
                {
                    Node rightNeighbour = _nodes[i + 1];
                    if (!rightNeighbour.Blocked)
                        Graph[_nodes[i]].Add(rightNeighbour);
                }
                
                if (y > 0)
                {
                    Node upNeighbour = _nodes[i - _xSize];
                    if (!upNeighbour.Blocked)
                        Graph[_nodes[i]].Add(upNeighbour);
                }
                
                if (y < _ySize - 1)
                {
                    Node downNeighbour = _nodes[i + _xSize];
                    if (!downNeighbour.Blocked)
                        Graph[_nodes[i]].Add(downNeighbour);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (!_drawGizmos)
                return;
            
            foreach (var node in _nodes)
            {
                Gizmos.DrawWireSphere(node.Position, 0.05f);
            }
        }
    }
}