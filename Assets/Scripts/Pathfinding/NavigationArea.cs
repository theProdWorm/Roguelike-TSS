using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Pathfinding
{
    [RequireComponent(typeof(Tilemap))]
    public class NavigationArea : MonoBehaviour
    {
        [SerializeField] private int _resolution = 3;
        [SerializeField] private bool _drawGizmos;
        [SerializeField] private LayerMask _wallLayer;
        
        public Dictionary<Node, List<Node>> Graph;
        
        [HideInInspector]
        [SerializeField] private Node[] _nodes;

        [HideInInspector]
        [SerializeField] private Vector2 _worldOffset;
        [HideInInspector]
        [SerializeField] private int _xSize, _ySize;
        
        private Node _closestNode;

        private Tilemap _tilemap;
        
        private void Start()
        {
            _tilemap = GetComponent<Tilemap>();
            
            GenerateNodes();
            
            if (Graph == null || Graph.Count == 0)
                Debug.LogWarning("Graph is empty");
        }

        public Node FindClosestNode(Vector2 position)
        {
            float smallestDistance = Mathf.Infinity;
            int smallestIndex = -1;
            for (int i = 0; i < _nodes.Length; i++)
            {
                if (_nodes[i] is null)
                    continue;
                
                float distance = Vector2.Distance(position, _nodes[i].Position);
                if (distance < smallestDistance)
                {
                    smallestDistance = distance;
                    smallestIndex = i;
                }
            }
            
            return _nodes[smallestIndex];
        }
        
        public void GenerateNodes()
        {
            Graph = new();
            if (!_tilemap)
                _tilemap = GetComponent<Tilemap>();
            
            var bounds = _tilemap.cellBounds;
            _xSize = bounds.size.x * _resolution;
            _ySize = bounds.size.y * _resolution;
            
            _nodes = new Node[_xSize * _ySize];

            _worldOffset = new Vector2(bounds.xMin - 0.5f, bounds.yMin - 0.5f)
                + new Vector2(2f / _resolution, 2f / _resolution);
            
            for (int i = 0; i < _nodes.Length; i++)
            {
                int x = i % _xSize;
                int y = i / _xSize;

                int xIndex = x / _resolution + bounds.xMin;
                int yIndex = y / _resolution + bounds.yMin;
                var tile = _tilemap.GetTile(new Vector3Int(xIndex, yIndex, 0));
                if (!tile)
                    continue;

                float xPos = x / (float) _resolution;
                float yPos = y / (float) _resolution;
                
                Vector2 pos = new Vector2(xPos, yPos) + _worldOffset;
                bool blocked = Physics2D.OverlapPoint(pos, _wallLayer);

                _nodes[i] = new Node() { Position = pos, Blocked = blocked };
            }
            
            GenerateNeighbours();
        }

        private void GenerateNeighbours()
        {
            for (int i = 0; i < _nodes.Length; i++)
            {
                if (_nodes[i] is null)
                    continue;
                
                Graph[_nodes[i]] = new List<Node>();
                
                int x = i % _xSize;
                int y = i / _xSize;

                if (x > 0)
                {
                    Node leftNeighbour = _nodes[i - 1];
                    if (leftNeighbour is { Blocked: false })
                        Graph[_nodes[i]].Add(leftNeighbour);
                }

                if (x < _xSize - 1)
                {
                    Node rightNeighbour = _nodes[i + 1];
                    if (rightNeighbour is { Blocked: false })
                        Graph[_nodes[i]].Add(rightNeighbour);
                }
                
                if (y > 0)
                {
                    Node upNeighbour = _nodes[i - _xSize];
                    if (upNeighbour is { Blocked: false })
                        Graph[_nodes[i]].Add(upNeighbour);
                }
                
                if (y < _ySize - 1)
                {
                    Node downNeighbour = _nodes[i + _xSize];
                    if (downNeighbour is { Blocked: false })
                        Graph[_nodes[i]].Add(downNeighbour);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (!_drawGizmos)
                return;
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(_worldOffset, Vector3.one * 0.2f);

            Gizmos.color = Color.white;
            
            foreach (var node in _nodes.Where(n => n != null))
            {
                Gizmos.color = node.Blocked ? Color.red : node == _closestNode ? Color.yellow : Color.white;
                
                Gizmos.DrawWireSphere(node.Position, 0.05f);
            }
        }
    }
}