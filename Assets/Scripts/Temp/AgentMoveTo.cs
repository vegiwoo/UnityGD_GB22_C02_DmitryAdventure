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

    [field: SerializeField] private bool IsCircularRoute { get; set; }

    private int _currentGoal;

    private AgentState State { get; set; }

    private Coroutine patrolCoroutine;

    private bool _isMovingForward;

    private int StartIndex { get; } = 0;
    private int LastIndex { get; set; }
    
    private void Start()  
    {
        agent  = GetComponent<NavMeshAgent>();
        _currentGoal = 0;
        State = AgentState.Patrol;
        _isMovingForward = true;

        LastIndex = goals.Length - 1;
        
        patrolCoroutine = StartCoroutine(PatrolCoroutine());
    }

    private IEnumerator PatrolCoroutine()
    {
        while (State == AgentState.Patrol)
        {
            if (_currentGoal >= 0 && _currentGoal <= goals.Length)
            {
                agent.SetDestination(goals[_currentGoal].position);

                // change point 
                if (Math.Abs(transform.position.x - goals[_currentGoal].position.x) < 0.1f && 
                    Math.Abs(transform.position.z - goals[_currentGoal].position.z) < 0.1f)
                {

                    if (_isMovingForward)
                    {
                        if (_currentGoal != LastIndex)
                        {
                            _currentGoal++;
                        }
                        else
                        {
                            if (IsCircularRoute)
                            {
                                _currentGoal = StartIndex;
                            }
                            else
                            {
                                _isMovingForward = !_isMovingForward;
                                _currentGoal--;
                            }
                        }
                    }
                    else
                    {
                        if (_currentGoal != StartIndex)
                        {
                            _currentGoal--;
                        }
                        else
                        {
                            _isMovingForward = !_isMovingForward;
                            _currentGoal++;
                        }
                    }
                   
                }
            }
            else
            {
                Debug.LogErrorFormat($"Индекса {_currentGoal} нет в массиве!");
            }
            
            yield return null;
        }
    }
}
