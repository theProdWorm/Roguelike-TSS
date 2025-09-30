using System.Collections.Generic;

namespace States
{
    public class StateMachine
    {
        private readonly Dictionary<string, State> _states;

        private State _currentState;
    
        /// <summary>
        /// Adds a state with the specified name, or replaces an existing state with that name.
        /// </summary>
        public void AddState(string stateName, State state)
        {
            _states[stateName] = state;
        }

        public void RemoveState(string stateName)
        {
            if (_states.ContainsKey(stateName))
                _states.Remove(stateName);
        }

        public void SwitchState(string stateName)
        {
            if (_currentState)
                _currentState.Exit.Invoke();
        
            _currentState = _states[stateName];
        
            _currentState.Enter.Invoke();
        }

        public void Update()
        {
            _currentState.Update.Invoke();
        }
    }
}