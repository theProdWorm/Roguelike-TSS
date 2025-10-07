using UnityEngine;
using UnityEngine.Tilemaps;

namespace Graphics
{
    public class WallSpriteSorter : MonoBehaviour
    {
        private static readonly Color VISIBLE = new(1, 1, 1, 1f);
        private static readonly Color INVISIBLE = new(1, 1, 1, 0f);
    
        [SerializeField] private Tilemap _wallTilemap;
        [SerializeField] private Transform _player;

        private void Update()
        {
            BoundsInt bounds = _wallTilemap.cellBounds;

            foreach (Vector3Int pos in bounds.allPositionsWithin)
            {
                if (!_wallTilemap.HasTile(pos))
                    continue;
            
                Vector3 tileWorldPosition = _wallTilemap.GetCellCenterWorld(pos);
            
                bool abovePlayer = _player.transform.position.y < tileWorldPosition.y;
            
                Color visibility = abovePlayer ? INVISIBLE : VISIBLE;
                _wallTilemap.SetColor(pos, visibility);
            }
        }
    }
}
