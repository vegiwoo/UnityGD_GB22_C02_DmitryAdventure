using System.Linq;
using DmitryAdventure.Props;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure
{
    /// <summary>
    /// Represents a closed (closing) mechanism mechanism.
    /// </summary>
    public abstract class LockedMechanism : MonoBehaviour, IDiscovering
    {
        #region Сonstants, variables & properties

        [Header("Base variables and references")] 
        [SerializeField]
        protected LockedMechanismType lockedMechanismType;

        [field: SerializeField, Tooltip("Collection of types tracked by DiscoveryTrigger")]
        public DiscoveryType[] DiscoveryTypes { get; set; }

        [field:SerializeField, Tooltip("Tracking/discovery trigger")]
        public DiscoveryTrigger DiscoveryTrigger { get; set; }

        [Space] 
        [Header("Hinge joints physics settings")] 
        [SerializeField]
        protected HingeJoint[] hingeJoints;

        [SerializeField] protected float hingeJointsSpring;

        [SerializeField] protected float hingeJointsDamper;

        [SerializeField, Tooltip("Position of mechanism in degrees at full closing.")]
        protected float closePosition;

        [SerializeField, Tooltip("Position of mechanism in degrees at full opening.")]
        protected float openPosition;
        
        protected bool MechanismIsOpen;

        #endregion

        #region Monobehavior methods

        protected virtual void Start()
        {
            BasicMechanismSetting();
        }

        protected void OnEnable()
        {
            DiscoveryTrigger.DiscoveryTriggerNotify += OnDiscoveryTriggerHandler;
        }

        protected void OnDisable()
        {
            DiscoveryTrigger.DiscoveryTriggerNotify -= OnDiscoveryTriggerHandler;
        }

        #endregion

        #region Functionality

        /// <summary>
        /// Basic setting of hinge joints mechanism.
        /// </summary>
        private void BasicMechanismSetting()
        {
            DiscoveryTrigger.Init(DiscoveryTypes);

            foreach (var j in hingeJoints)
            {
                j.useSpring = true;
                var jointSpring = j.spring;
                jointSpring.spring = hingeJointsSpring;
                jointSpring.damper = hingeJointsDamper;
                jointSpring.targetPosition = closePosition;
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
            if (!DiscoveryTypes.Contains(discoveryType)) return;
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