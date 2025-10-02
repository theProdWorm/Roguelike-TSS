using UnityEngine;
using Weapons.Attacks;

namespace Actors.Enemies.DarkFairy
{
    public class DarkFairy : Enemy
    {
        private static readonly int SPELL_COOLDOWN = Animator.StringToHash("spellCooldown");
        private static readonly int TELEPORT_COOLDOWN = Animator.StringToHash("teleportCooldown");
        private static readonly int SHIELD_COOLDOWN = Animator.StringToHash("shieldCooldown");
        private static readonly int MELEE_COOLDOWN = Animator.StringToHash("meleeCooldown");

        [SerializeField] private Attack _meleePrefab;
        [SerializeField] private Attack _spellPrefab;
        
        [SerializeField] private float _spellDamage = 20f;
        [SerializeField] private float _meleeDamage = 10f;

        private Player[] _players;
        
        private float _teleportCooldown;
        private float _spellCooldown;
        private float _shieldCooldown;
        private float _meleeCooldown;
        
        private float _teleportTimer;
        private float _spellTimer;
        private float _shieldTimer;
        private float _meleeTimer;

        protected override void Awake()
        {
            base.Awake();
            
            _teleportCooldown = _animator.GetFloat(TELEPORT_COOLDOWN);
            _spellCooldown = _animator.GetFloat(SPELL_COOLDOWN);
            _shieldCooldown = _animator.GetFloat(SHIELD_COOLDOWN);
            _meleeCooldown = _animator.GetFloat(MELEE_COOLDOWN);
            
            _teleportTimer = _teleportCooldown;
            _spellTimer = _spellCooldown;
            _shieldTimer = _shieldCooldown;
            _meleeTimer = _meleeCooldown;
        }
        
        private void Start()
        {
            _players = FindObjectsByType<Player>(FindObjectsSortMode.None);
        }
        
        private void Update()
        {
            // Reduce timers and update animator parameters
            if (_teleportTimer > 0)
                _animator.SetFloat(TELEPORT_COOLDOWN, _teleportTimer -= Time.deltaTime);
            if (_spellTimer > 0)
                _animator.SetFloat(SPELL_COOLDOWN, _spellTimer -= Time.deltaTime);
            if (_shieldTimer > 0)
                _animator.SetFloat(SHIELD_COOLDOWN, _shieldTimer -= Time.deltaTime);
            if (_meleeTimer > 0)
                _animator.SetFloat(MELEE_COOLDOWN, _meleeTimer -= Time.deltaTime);
        }

        public void SetMoveDir(Vector2 dir) => _rigidbody.linearVelocity = _moveSpeed * dir;

        public void Melee()
        {
            Player targetPlayer = _players[0];
            float shortestDistance = Vector2.Distance(targetPlayer.transform.position, transform.position);

            for (int i = 1; i < _players.Length; i++)
            {
                var player = _players[i];
                
                var distance = Vector2.Distance(player.transform.position, transform.position);
                if (distance >= shortestDistance)
                    continue;

                shortestDistance = distance;
                targetPlayer = player;
            }

            Vector2 attackPos = Vector2.MoveTowards(transform.position, targetPlayer.transform.position, 1f);
            var attackInstance = Instantiate(_meleePrefab, attackPos, Quaternion.identity);
            attackInstance.Initialize(_meleeDamage, 0.05f, "Player");
        }

        public void CastSpell()
        {
            foreach (var player in _players)
            {
                var attackInstance = Instantiate(_spellPrefab, player.transform.position, Quaternion.identity);
                attackInstance.Initialize(_spellDamage, Mathf.Infinity, "Player"); // Attack handles its own lifetime
            }
        }

        public void ResetSpellCooldown()
        {
            _spellTimer = _spellCooldown;
            _animator.SetFloat(SPELL_COOLDOWN, _spellCooldown);
        }
        
        public void ResetTeleportCooldown()
        {
            _teleportTimer = _teleportCooldown;
            _animator.SetFloat(TELEPORT_COOLDOWN, _teleportCooldown);
        }
        
        public void ResetShieldCooldown()
        {
            _shieldTimer = _shieldCooldown;
            _animator.SetFloat(SHIELD_COOLDOWN, _shieldCooldown);
        }
        
        public void ResetMeleeCooldown()
        {
            _meleeTimer = _meleeCooldown;
            _animator.SetFloat(MELEE_COOLDOWN, _meleeCooldown);
        }
    }
}