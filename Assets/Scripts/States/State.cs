using UnityEngine;
using UnityEngine.Events;

namespace States
{
    public class State : ScriptableObject
    {
        [SerializeField] public UnityEvent Enter;
        [SerializeField] public UnityEvent Exit;
        [SerializeField] public UnityEvent Update;
    
        public static implicit operator bool(State state) => state != null;
    }
}