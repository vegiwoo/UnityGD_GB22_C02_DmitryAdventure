using System;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure
{
    /// <summary>
    /// Represents the door entity.
    /// </summary>
    [RequireComponent(typeof(HingeJoint))]
    public class Door : MonoBehaviour
    {
        #region Сonstants, variables & properties

        [SerializeField] private string name;
        [SerializeField] private DoorTrigger doorTrigger;
        private HingeJoint _doorJoint;

        private bool _isOpen = false;
        
        
        #endregion

        #region Monobehavior methods

        private void Awake()
        {
            _doorJoint = GetComponent<HingeJoint>();
        }

        private void Start()
        {
            doorTrigger.CharacterDiscoveryNotify += OnCharacterDiscovery;
        }

        private void OnDestroy()
        {
            doorTrigger.CharacterDiscoveryNotify -= OnCharacterDiscovery;
        }

        #endregion

        #region Functionality

        #region Coroutines

        // ...

        #endregion

        #region Event handlers

        private void OnCharacterDiscovery(Vector3 charForwardDirection)
        {
            var jointSpring = _doorJoint.spring;
            
            if (charForwardDirection != Vector3.zero && !_isOpen)
            {
                var a = charForwardDirection.normalized;
                var b = transform.forward.normalized;
            
                var collinearity = Math.Abs(Vector3.Dot(a, b) - 1) < 0.00001f;
            
                if (collinearity)
                    jointSpring.targetPosition = -90;
                else
                    jointSpring.targetPosition = 90;

                _isOpen = true;
            }
            else
            {
                jointSpring.targetPosition = 0;
                _isOpen = false;
            }
            
            _doorJoint.spring = jointSpring;
        }

        #endregion

        #region Other methods

        // ...

        #endregion

        #endregion
    }
}