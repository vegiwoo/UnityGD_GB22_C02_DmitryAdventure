using System;
using System.Linq;
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

        [SerializeField] private string objectName;
        /// <summary>
        /// An array of object types to discover.
        /// </summary>
        public DiscoveryType [] discoveryTypes;
        private DiscoveryTrigger _trigger;
        private HingeJoint _doorJoint;

        [field: SerializeField, Tooltip("Determines if door is locked")] private bool isLocked;
        
        private bool _isOpen;
        
        #endregion

        #region Monobehavior methods

        private void Awake()
        {
            _trigger = GetComponentInChildren<DiscoveryTrigger>();
            if (_trigger == null)
            {
                var parent = transform.parent;
                _trigger = parent.GetComponentInChildren<DiscoveryTrigger>();
            }

            _trigger.DiscoverableTypes = discoveryTypes;
            _doorJoint = GetComponent<HingeJoint>();
        }

        private void Start()
        {
            _trigger.DiscoveryTriggerNotify += OnTriggerNotify;
        }

        private void OnDestroy()
        {
            _trigger.DiscoveryTriggerNotify += OnTriggerNotify;
        }

        #endregion

        #region Functionality
        #region Coroutines
        // ...
        #endregion

        #region Event handlers

        private void OnTriggerNotify(DiscoveryType discoveryType, Transform discoveryTransform, bool entry)
        {
            if (!discoveryTypes.Contains(discoveryType)) return;

            // if (isLocked)
            // {
            //     
            // }
            // else
            // {
            //     
            // }

            var jointSpring = _doorJoint.spring;
            var forward = discoveryTransform.forward;
            
            switch (entry)
            {
                case true when forward != Vector3.zero && !_isOpen:
                {
                    var a = forward.normalized;
                    var b = transform.forward.normalized;
            
                    var collinearity = Math.Abs(Vector3.Dot(a, b) - 1) < 0.00001f;

                    if (collinearity)
                    {
                        jointSpring.targetPosition = -90;
                    }
                    else
                    {
                        jointSpring.targetPosition = 90;
                    }
                    
                    _isOpen = true;
                    break;
                }
                case false when discoveryTransform.forward != Vector3.zero && _isOpen:
                    jointSpring.targetPosition = 0;
                    _isOpen = false;
                    break;
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