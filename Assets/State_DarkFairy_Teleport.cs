using Actors;
using UnityEngine;

public class State_DarkFairy_Teleport : StateMachineBehaviour
{
    [SerializeField] private float _maxDistance;
    
    private Vector3 _teleportPosition;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var players = FindObjectsByType<Player>(FindObjectsSortMode.None);
        int index = Random.Range(0, players.Length);
        
        Transform teleportTarget = players[index].transform;
        
        var distanceVector = teleportTarget.position - animator.transform.position;
        float distance = distanceVector.magnitude;

        if (distance > _maxDistance)
            distanceVector = distanceVector.normalized * _maxDistance;
        
        _teleportPosition = animator.transform.position + distanceVector;
    }
    
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.position = _teleportPosition;
    }
}
