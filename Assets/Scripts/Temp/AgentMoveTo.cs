using System;
using System.Collections;
using DmitryAdventure;
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
    private Coroutine attackCoroutine;

    private bool _isMovingForward;

    private const int StartIndex = 0;
    private int LastIndex { get; set; }

    private bool _agentOnOffMeshLink;

    private DiscoveryTrigger _discoveryTrigger;

    private Vector3 _aimPoint;

    private const float MinAttackDestination = 4.0f;
    private const float MaxAttackDestination = 15.0f;
    
    private void Start()  
    {
        agent  = GetComponent<NavMeshAgent>();
        _discoveryTrigger = GetComponentInChildren<DiscoveryTrigger>();

        _currentGoal = 0;
        _isMovingForward = true;
        _agentOnOffMeshLink = false;

        LastIndex = goals.Length - 1;

        _discoveryTrigger.DiscoveryTriggerNotify += OnDiscoveryTriggerNotify;
        
        SwitchState(AgentState.Patrol);
    }

    private void OnDiscoveryTriggerNotify(DiscoveryType discoveryType, Transform discoveryTransform, bool entry)
    {
        Debug.Log("Trigger!");
        
        if (discoveryType != DiscoveryType.Player) return;
        
        _aimPoint = discoveryTransform.position;
        SwitchState(AgentState.Attack);
    }

    private void Update()
    {
        _agentOnOffMeshLink = agent.isOnOffMeshLink switch
        {
            true when !_agentOnOffMeshLink => true,
            false when _agentOnOffMeshLink => false,
            _ => _agentOnOffMeshLink
        };
    }

    private void OnDestroy()
    {
        _discoveryTrigger.DiscoveryTriggerNotify -= OnDiscoveryTriggerNotify;
    }

    private IEnumerator PatrolCoroutine()
    {
        Debug.Log("To patrol +");
        
        while (true)
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

            if (State == AgentState.Patrol)
            {
                yield return null;
            }
            else
            {
                Debug.Log("To patrol -");
                SwitchState(AgentState.Attack);
                patrolCoroutine = null;
                yield break;
            }
        }
    }

    private IEnumerator AttackCoroutine()
    {
        Debug.Log("To Attack +");
        
        while (State == AgentState.Attack && _aimPoint != Vector3.zero)
        {
            var distanceToTarget = Vector3.Distance(agent.transform.position, _aimPoint);
            
            if  (distanceToTarget is < MaxAttackDestination and > MinAttackDestination)
            {
                agent.SetDestination(_aimPoint);
                yield return null;
            }
            else
            {
                Debug.Log("To Attack -");
                SwitchState(AgentState.Patrol);
                attackCoroutine = null;
                yield break;
            }
        }
    }
    
    private void SwitchState(AgentState newState)
    {
        State = newState;
        
        switch (State)
        {
            case AgentState.Patrol:
                patrolCoroutine = StartCoroutine(PatrolCoroutine());
                _aimPoint = Vector3.zero;
                break;
            case AgentState.Attack:
                attackCoroutine = StartCoroutine(AttackCoroutine());
                break;
        }
    }
}