using UnityEngine;

namespace Helpers
{
    public class SingletonByName : MonoBehaviour
    {
        private static SingletonByName _instance;
        
        [SerializeField] private bool _favorSelf;
        
        private void Awake()
        {
            if (!_instance)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                if (_favorSelf)
                {
                    Destroy(_instance.gameObject);
                    
                    _instance = this;
                    DontDestroyOnLoad(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}