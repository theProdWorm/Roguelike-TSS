using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pathfinding
{
    public class Pathfinder : MonoBehaviour
    {
        public static bool FindPath(NavigationArea navArea, Vector3 startPos, Vector3 endPos, out List<Node> path)
        {
            path = new List<Node>();

            Node start = navArea.FindClosestNode(startPos);
            Node end = navArea.FindClosestNode(endPos);

            var graph = navArea.Graph;
            
            List<Node> openSet = new();
            List<Node> closedSet = new();
            Dictionary<Node, float> gCosts = new();
            Dictionary<Node, float> fCosts = new();
            Dictionary<Node, Node> cameFrom = new();

            openSet.Add(start);
            gCosts[start] = Vector2.Distance(start.Position, startPos);
            fCosts[start] = Vector2.Distance(start.Position, end.Position);
            
            while (openSet.Count > 0)
            {
                Node node = openSet.Aggregate((current, next) => fCosts[current] < fCosts[next] ? current : next);

                if (node == end)
                {
                    path = ReconstructPath(node, cameFrom);
                    return true;
                }
                
                List<Node> neighbours = graph[node];

                foreach (var neighbour in neighbours)
                {
                    if (closedSet.Contains(neighbour))
                        continue;

                    float g = gCosts[node] + Vector2.Distance(neighbour.Position, node.Position);
                    
                    if (gCosts.ContainsKey(neighbour) && gCosts[neighbour] <= g)
                        continue;
                    
                    gCosts[neighbour] = g;
                    
                    float h = Vector2.Distance(neighbour.Position, end.Position);
                    fCosts[neighbour] = g + h;
                    
                    cameFrom[neighbour] = node;
                    
                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }

                openSet.Remove(node);
                closedSet.Add(node);
            }

            return false;
        }
        
        private static List<Node> ReconstructPath(Node end, Dictionary<Node, Node> cameFrom)
        {
            List<Node> path = new() { end };

            while (cameFrom.ContainsKey(path[^1]))
                path.Add(cameFrom[path[^1]]);

            path.Reverse();
            return path;
        }
    }
}
