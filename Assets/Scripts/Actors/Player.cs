using System.Collections.Generic;
using System.Linq;
using Interaction;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Weapons;

namespace Actors
{
    public class Player : Entity
    {
        private static readonly int MOVING = Animator.StringToHash("moving");
        private static readonly int DELTA_MOVING = Animator.StringToHash("deltaMoving");
        private static readonly int DIR_X = Animator.StringToHash("dirX");
        private static readonly int DIR_Y = Animator.StringToHash("dirY");
        private static readonly int DIE = Animator.StringToHash("die");
        
        private static readonly Color TRANSPARENT = new(0, 0, 0, 0); 
        
        [SerializeField] private Animator _animator;
        [SerializeField] private PlayerCamera _camera;
        
        [Header("Visualization")]
        [SerializeField] private Transform _weaponPositionTransform;
        
        [Tooltip("Applied for animation direction.")]
        [SerializeField] private float _horizontalDeadZone;
        [Tooltip("Applied for animation direction.")]
        [SerializeField] private float _verticalDeadZone;
        
        [Header("Weapons")]
        [SerializeField] private float _weaponPositionOffset;
        [SerializeField] private List<Image> _weaponsUI;
        
        private PlayerInput _input;
        
        private readonly List<GameObject> _interactablesInRange = new();
        private GameObject _targetInteractable;
        
        private Weapon[] _weapons;
        private Weapon _equippedWeapon;
        private int _equippedWeaponIndex = -1;
        
        private Vector2 _moveInput;
        private Vector2 _aimInput;
        
        private Vector2 _lookDir;
        
        private bool _attacking;
        
        protected override void Awake()
        {
            base.Awake();
            
            _weapons = new Weapon[_weaponsUI.Count];
            
            _input = GetComponent<PlayerInput>();
        }
    
        private void Update()
        {
            _animator.SetBool(DELTA_MOVING, _animator.GetBool(MOVING));
            _animator.SetBool(MOVING, _moveInput.magnitude > 0);
        
            _rigidbody.linearVelocity = _moveInput * MoveSpeed;
            
            if (_aimInput.magnitude >= 0.05f)
                SetLookDirection(_aimInput);
            else if (_moveInput.magnitude >= 0.05f)
                SetLookDirection(_moveInput);
            
            SetTargetInteractable();

            if (_equippedWeapon)
                _equippedWeapon.Attacking = _attacking;
        }

        private void SetLookDirection(Vector2 dir)
        {
            if (dir.magnitude < 0.05f)
                return;
            
            int dirX = Mathf.Abs(dir.x) <= _horizontalDeadZone ? 0 : dir.x > 0 ? 1 : -1;
            int dirY = Mathf.Abs(dir.y) <= _verticalDeadZone ? 0 : dir.y > 0 ? 1 : -1;
        
            _animator.SetInteger(DIR_X, dirX);
            _animator.SetInteger(DIR_Y, dirY);

            dir.Normalize();
            
            _lookDir = dir;
            
            _weaponPositionTransform.right = dir.normalized;
            _weaponPositionTransform.localPosition = dir.normalized * _weaponPositionOffset;

            float rot = Mathf.Deg2Rad * _weaponPositionTransform.rotation.eulerAngles.z;
            bool flip = Mathf.Cos(rot) < 0;
            foreach (var weapon in _weapons)
            {
                if (weapon)
                    weapon.FlipSprite(flip);
            }
        }

        protected override void Die()
        {
            if (_dead)
                return;
            
            base.Die();

            _animator.SetTrigger(DIE);
            
            _input.enabled = false;
        }
        
        private void SetTargetInteractable()
        {
            GameObject target = null;
            float targetDot = -10;
        
            foreach (var interactable in _interactablesInRange)
            {
                if (interactable.CompareTag("Untagged"))
                    continue;
                
                Vector2 dir = (interactable.transform.position - transform.position).normalized;
                float dot = Vector2.Dot(dir, _lookDir);

                if (dot <= targetDot)
                    continue;
            
                target = interactable;
                targetDot = dot;
            }

            _targetInteractable = target;
        }

        public void PickUp(GameObject referenceObject)
        {
            bool isWeapon = referenceObject.TryGetComponent(out Weapon weapon);
        
            if (isWeapon)
                PickUp(weapon);
        }

        private void PickUp(Weapon newWeapon)
        {
            int index = _equippedWeaponIndex;

            // Find first empty slot
            for (int i = 0; i < _weapons.Length; i++)
            {
                if (_weapons[i] != null) 
                    continue;
            
                index = i;
                break;
            }

            // If true, current equipped weapon should be replaced
            if (index == _equippedWeaponIndex)
            {
                var weapon = _weapons[_equippedWeaponIndex];
                weapon.SetEquipper(null);   // So it gets unequipped
                Drop(_equippedWeaponIndex); // and dropped
            }

            newWeapon.SetEquipper(this); // Equip new weapon
            
            // Update references
            _weapons[index] = newWeapon;
            
            OnWeaponIndexChanged(index);
            
            _weaponsUI[index].sprite = newWeapon.GetComponent<SpriteRenderer>().sprite;
        
            // Weapon follows player
            newWeapon.transform.SetParent(_weaponPositionTransform);
            newWeapon.transform.localPosition = Vector3.zero;
            newWeapon.transform.localRotation = Quaternion.identity;
        }
    
        private void Drop(int index)
        {
            var weapon = _weapons[index];
            _weapons[index] = null;
            
            _weaponsUI[index].sprite = null;
            _weaponsUI[index].color = TRANSPARENT;

            weapon.Attacking = false; // Prevents undesirable attacks upon enabling the weapon again
            weapon.SetEquipper(null);
            
            var throwable = weapon.GetComponent<Throwable>();
            throwable.Throw(_lookDir, 10f);
        
            _equippedWeapon = null;
            _equippedWeaponIndex = -1;

            for (int i = 0; i < _weapons.Length; i++)
            {
                if (!_weapons[i])
                    continue;

                _equippedWeaponIndex = i;
                _equippedWeapon = _weapons[i];
                break;
            }
        }
    
        private void OnWeaponIndexChanged(int targetIndex)
        {
            int deltaEquippedWeaponIndex = _equippedWeaponIndex;
            _equippedWeaponIndex = targetIndex;
            
            int numWeapons = _weapons.Count(weapon => weapon);
            _equippedWeaponIndex = Mathf.Abs(_equippedWeaponIndex % numWeapons);
            
            _equippedWeapon = _weapons[_equippedWeaponIndex];

            if (deltaEquippedWeaponIndex >= 0)
            {
                var previousWeapon = _weapons[deltaEquippedWeaponIndex];
                previousWeapon.Attacking = false;
                previousWeapon.SetVisible(false);
            }

            _equippedWeapon.SetVisible(true);

            for (int i = 0; i < _weapons.Length; i++)
            {
                if (!_weapons[i])
                    continue;
                
                var spriteColor = _weapons[i].GetSpriteColor();

                if (i != _equippedWeaponIndex)
                    spriteColor *= Color.gray;

                _weaponsUI[i].color = spriteColor;
            }
        }

        #region INPUT
    
        public void OnMove(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
        }

        public void OnAimController(InputAction.CallbackContext context)
        {
            _aimInput = context.ReadValue<Vector2>();
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            // Perform dash
        }
        
        public void OnNextWeapon(InputAction.CallbackContext context)
        {
            if (!context.performed || _weapons.All(x => !x))
                return;
        
            OnWeaponIndexChanged(_equippedWeaponIndex + 1);
        }

        public void OnPreviousWeapon(InputAction.CallbackContext context)
        {
            if (!context.performed || _weapons.All(x => !x))
                return;
        
            OnWeaponIndexChanged(_equippedWeaponIndex - 1);
        }
    
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (!context.performed || !_targetInteractable)
                return;
            
            var interactable = _targetInteractable.GetComponent<IInteractable>();
            interactable.Interact(this);
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.started)
                _attacking = true;
            else if (context.canceled)
                _attacking = false;
        }

        public void OnDrop(InputAction.CallbackContext context)
        {
            if (!context.performed || !_equippedWeapon)
                return;
        
            Drop(_equippedWeaponIndex);
        }
    
        #endregion INPUT
    
        #region COLLISION
        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            if (!otherCollider.CompareTag("Interactable"))
                return;
        
            _interactablesInRange.Add(otherCollider.gameObject);
        }

        private void OnTriggerExit2D(Collider2D otherCollider)
        {
            if (!otherCollider.CompareTag("Interactable"))
                return;
        
            _interactablesInRange.Remove(otherCollider.gameObject);
        
            if (_targetInteractable == otherCollider.gameObject)
                _targetInteractable = null;
        }
        #endregion COLLISION
    }
}