using System.Linq;
using DmitryAdventure.Props;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure
{
    /// <summary>
    /// Represents a closed (closing) mechanism mechanism.
    /// </summary>
    public abstract class LockedMechanism : MonoBehaviour
    {
        #region Сonstants, variables & properties

        [Header("Base variables and references")] [SerializeField]
        protected LockedMechanismType lockedMechanismType;

        [SerializeField, Tooltip("An array of object types to discover.")]
        protected DiscoveryType[] discoveryTypes;

        [SerializeField, Tooltip("Discovery trigger listening for state change")]
        protected DiscoveryTrigger discoveryTrigger;

        [Space] [Header("Hinge joints physics settings")] [SerializeField]
        protected HingeJoint[] hingeJoints;

        [SerializeField] protected float hingeJointsSpring;

        [SerializeField] protected float hingeJointsDamper;

        [SerializeField, Tooltip("Position of mechanism in degrees at full closing.")]
        protected float closeMechanismTargetPosition;

        [SerializeField, Tooltip("Position of mechanism in degrees at full opening.")]
        protected float openMechanismTargetPosition;
        
        protected bool MechanismIsOpen;

        #endregion

        #region Monobehavior methods

        protected virtual void Start()
        {
            BasicMechanismSetting();
        }

        protected void OnEnable()
        {
            discoveryTrigger.DiscoveryTriggerNotify += OnDiscoveryTriggerHandler;
        }

        protected void OnDisable()
        {
            discoveryTrigger.DiscoveryTriggerNotify -= OnDiscoveryTriggerHandler;
        }

        #endregion

        #region Functionality

        /// <summary>
        /// Basic setting of hinge joints mechanism.
        /// </summary>
        private void BasicMechanismSetting()
        {
            discoveryTrigger.Init(discoveryTypes);

            foreach (var j in hingeJoints)
            {
                j.useSpring = true;
                var jointSpring = j.spring;
                jointSpring.spring = hingeJointsSpring;
                jointSpring.damper = hingeJointsDamper;
                jointSpring.targetPosition = closeMechanismTargetPosition;
                j.spring = jointSpring;
            }

            MechanismIsOpen = lockedMechanismType == LockedMechanismType.None;
        }

        /// <summary>
        /// Trigger signal event handler.
        /// </summary>
        /// <param name="discoveryType">Type of object found on trigger.</param>
        /// <param name="discoveryTransform">Transform of object found on trigger.</param>
        /// <param name="isObjectEnters">Object entered trigger (true) or exited it (false).</param>
        protected virtual void OnDiscoveryTriggerHandler(DiscoveryType discoveryType, Transform discoveryTransform,
            bool isObjectEnters)
        {
            if (!discoveryTypes.Contains(discoveryType)) return;
        }

        /// <summary>
        /// Opens or closes mechanism.
        /// </summary>
        /// <param name="discoveryTransform">Transform component of game object that hit trigger.</param>
        /// <param name="isItemEnters">An object enters (true) or exits (false) a trigger.</param>
        protected virtual void OpenCloseMechanism(Transform discoveryTransform, bool isItemEnters)
        {
            if(hingeJoints.Length == 0) return;
        }

        #endregion
    }
}