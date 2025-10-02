using System.Collections.Generic;
using System.Linq;
using Actors;
using Actors.Enemies;
using UnityEngine;

namespace Generation
{
    public class SetSpawner : MonoBehaviour
    {
        [SerializeField] private List<EnemySet> _possibleSets;
		[SerializeField] private CircleCollider2D _spawnArea;

        private List<Enemy> _livingEnemies;
        
        public void SpawnRandomSet()
        {
            int index = Random.Range(0, _possibleSets.Count);
            var set = _possibleSets[index];

            foreach (var enemy in set.Enemies)
            {
                var pos = _spawnArea.offset + Random.insideUnitCircle * _spawnArea.radius;
                
                // Attempt to avoid overlapping spawns
                // bool overlapping = true;
                // int attempts = 0;
                // while (overlapping && attempts++ < 10)
                // {
                //     var hits = Physics2D.OverlapCircleAll(pos,
                //         enemy.GetComponent<Collider2D>().bounds.extents.magnitude);
                //
                //     overlapping = hits.Any(hit => hit.CompareTag("Enemy"));
                // }
                
                var enemyInstance = Instantiate(enemy, pos, Quaternion.identity, transform);
                enemyInstance.OnDeath.AddListener(OnEnemyDeath);
            }
        }

        private void OnEnemyDeath(Entity enemy)
        {
            
        }
    }
}
