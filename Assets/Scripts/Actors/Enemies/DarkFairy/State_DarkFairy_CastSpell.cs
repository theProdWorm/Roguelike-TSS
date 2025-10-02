using System.Linq;
using UnityEngine;

namespace Actors.Enemies.DarkFairy
{
    public class State_DarkFairy_CastSpell : StateMachineBehaviour
    {
        private static readonly int IDLE = Animator.StringToHash("idle");
        private static readonly int PLAYBACK_DIR = Animator.StringToHash("playbackDir");

        [SerializeField] private float _castDuration;
        [SerializeField] private float _castsPerSecond;
        
        private DarkFairy _darkFairy;

        private bool _castingStarted;
        
        private float _remainingDuration;
        private float _timeUntilNextCast;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!_darkFairy)
                _darkFairy = animator.GetComponent<DarkFairy>();
            
            _remainingDuration = _castDuration;
            _timeUntilNextCast = 0;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!_castingStarted)
            {
                float animatorTime = animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime;
                if (animatorTime < 1) 
                    return;
                
                animator.SetFloat(PLAYBACK_DIR, 0);
                _castingStarted = true;
            }
            
            if (_remainingDuration > 0)
            {
                Cast();
                
                _remainingDuration -= Time.deltaTime;
                if (_remainingDuration <= 0)
                    animator.SetFloat(PLAYBACK_DIR, -1);
            }
            else
            {
                PlayExitAnimation(animator, stateInfo);
            }
        }

        private void Cast()
        {
            if (_timeUntilNextCast > 0)
            {
                _timeUntilNextCast -= Time.deltaTime;
                return;
            }
            
            _darkFairy.CastSpell();

            _timeUntilNextCast = 1 / _castsPerSecond;
        }

        private void PlayExitAnimation(Animator animator, AnimatorStateInfo stateInfo)
        {
            bool done = stateInfo.normalizedTime <= 0;

            if (!done) 
                return;
            
            animator.SetBool(IDLE, true);
            animator.SetFloat(PLAYBACK_DIR, 1);
            _darkFairy.ResetSpellCooldown();
        }
    }
}