using System.Collections.Generic;
using Actors.Enemies;
using UnityEngine;

namespace Generation
{
    [CreateAssetMenu(fileName = "New Wave", menuName = "Spawning/Wave")]
    public class EnemySet : ScriptableObject
    {
        public List<Enemy> Enemies;
    }
}