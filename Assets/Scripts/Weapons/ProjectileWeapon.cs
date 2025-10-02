using UnityEngine;
using UnityEngine.Splines;
using Weapons.Attacks;

namespace Weapons
{
    public class ProjectileWeapon : Weapon
    {
        [SerializeField] private float _projectileSpeed;
        [SerializeField] private SplineContainer _trajectories;
        
        private int _currentTrajectoryIndex;
        
        protected override Attack PerformAttack()
        {
            var attack = (ProjectileAttack) base.PerformAttack();
            attack.Initialize(_trajectories.Splines[_currentTrajectoryIndex], _projectileSpeed, Damage, 1f, _allyTag);

            _currentTrajectoryIndex++;
            _currentTrajectoryIndex %= _trajectories.Splines.Count;

            return attack;
        }
    }
}