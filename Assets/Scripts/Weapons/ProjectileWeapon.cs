using UnityEngine;
using UnityEngine.Splines;
using Weapons.Attacks;

namespace Weapons
{
    public class ProjectileWeapon : Weapon
    {
        [SerializeField] private float _projectileSpeed;
        [SerializeField] private SplineContainer _trajectories;
        
        [Tooltip("If checked, picks a random trajectory for every attack.")]
        public bool RandomizeTrajectory;
        
        private int _currentTrajectoryIndex;
        
        protected override Attack PerformAttack()
        {
            var attack = (ProjectileAttack) base.PerformAttack();
            
            if (RandomizeTrajectory)
            {
                _currentTrajectoryIndex = Random.Range(0, _trajectories.Splines.Count);
            }
            attack.Initialize(_trajectories.Splines[_currentTrajectoryIndex], _projectileSpeed, Damage, _allyTag);

            if (!RandomizeTrajectory)
            {
                _currentTrajectoryIndex++;
                _currentTrajectoryIndex %= _trajectories.Splines.Count;
            }
            
            return attack;
        }
    }
}