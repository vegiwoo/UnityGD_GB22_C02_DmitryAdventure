using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum AgentState
{
    Patrol, Attack
}


public class AgentMoveTo : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform[] goals;
    private int _currentGoal;

    private bool _isMovingForward;

    private AgentState State { get; set; }

    private Coroutine patrolCoroutine;
    private void Start()  
    {
        agent  = GetComponent<NavMeshAgent>();
        _currentGoal = 0;
        State = AgentState.Patrol;
        _isMovingForward = true;
        
        patrolCoroutine = StartCoroutine(MovingCoroutine());
    }
    
    private IEnumerator MovingCoroutine()
    {
        while (State == AgentState.Patrol)
        {
            var tPosition = transform.position;
            var pPosition = goals[_currentGoal].position;
            
            agent.destination = pPosition;
            
            if (Math.Abs(tPosition.x - pPosition.x) < 0.1f && 
                Math.Abs(tPosition.z - pPosition.z) < 0.1f)
            {

                _isMovingForward = _currentGoal != goals.Length;
                
                
                
                if (_isMovingForward)
                {
                    _isMovingForward = _currentGoal != goals.Length;
                    _currentGoal = _currentGoal != goals.Length ? _currentGoal++ : _currentGoal--;
                }
                else
                {
                    _isMovingForward = _currentGoal == 0;
                    _currentGoal = _currentGoal == 0 ? _currentGoal-- : _currentGoal++;
                }
            }
            
            yield return null;
        }
    }
    
    private IEnumerator ChangeWaypoint()
    {

        yield break;
    }
}
