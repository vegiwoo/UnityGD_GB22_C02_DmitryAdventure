using System.Linq;
using DmitryAdventure.Characters;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure.Props
{

    
    /// <summary>
    /// Represents the door entity.
    /// </summary>
    [RequireComponent(typeof(AudioIsPlaying))]
    public class Door : MonoBehaviour
    {
        #region Сonstants, variables & properties

        [SerializeField] 
        private LockedDoorType lockedDoorType;
        
        [SerializeField, Tooltip("An array of object types to discover.")]
        public DiscoveryType[] discoveryTypes;

        [SerializeField] 
        private HingeJoint[] hingeJoints;
        private bool _isThereSecondDoor;

        [SerializeField] private float doorSpring;
        [SerializeField] private float doorDamper;
        [SerializeField] private float doorTargetPosition;
        [SerializeField] private float doorBreakForce;
        private float openDoorTargetPosition;
        
        private DiscoveryTrigger _discoveryTrigger;
        private AudioIsPlaying _audioIsPlaying;

        /// <summary>
        /// Tolerance angle for opening door.
        /// </summary>
        private const int ToleranceAngle = 65;
        
        private bool _isOpen;
        
        #endregion

        #region Monobehavior methods

        private void Awake()
        {
            _discoveryTrigger = GetComponentInChildren<DiscoveryTrigger>();
            _audioIsPlaying = GetComponent<AudioIsPlaying>();
        }

        private void Start()
        {
            doorSpring = 180.0f;
            doorDamper = 90.0f;
            doorTargetPosition = 0.0f;
            openDoorTargetPosition = 90.0f;
            doorBreakForce = 0.0f;

            _isThereSecondDoor = hingeJoints.Length > 1;

            BasicDoorSetting();

            _discoveryTrigger.DiscoverableTypes = discoveryTypes;
            _isOpen = lockedDoorType == LockedDoorType.None; 
        }

        private void OnEnable()
        {
            _discoveryTrigger.DiscoveryTriggerNotify += OnDiscoveryTriggerNotify;
        }

        private void OnDisable()
        {
            _discoveryTrigger.DiscoveryTriggerNotify -= OnDiscoveryTriggerNotify;
        }

        #endregion

        #region Functionality

        private void BasicDoorSetting()
        {
            foreach (var j in hingeJoints)
            {
                j.useSpring = true;
                var jointSpring = j.spring;
                jointSpring.spring = doorSpring;
                jointSpring.damper = doorDamper;
                jointSpring.targetPosition = doorTargetPosition;
                j.spring = jointSpring;
            }
        }
        
        private void OnDiscoveryTriggerNotify(DiscoveryType discoveryType, Transform discoveryTransform, bool isCharacterEnters)
        {
            if (!discoveryTypes.Contains(discoveryType)) return;

            var character = discoveryTransform.gameObject.GetComponent<Character>();

            if (lockedDoorType == LockedDoorType.Key && !_isOpen)
            {
                var key = character.FindItemInInventory(GameData.KeysKey);
                if (key != null)
                {
                    OpenCloseDoor(discoveryTransform, isCharacterEnters);
                }
                else
                {
                    _audioIsPlaying.PlaySound(SoundType.Negative);
                }
            }
            else
            {
                OpenCloseDoor(discoveryTransform, isCharacterEnters);
            }
        }

        #endregion

        #region Other methods

        private void OpenCloseDoor(Transform discoveryTransform, bool isItemEnters)
        {
            if(hingeJoints.Length == 0) return;

            var doorsTransform = _discoveryTrigger.transform;
            var characterPosition = discoveryTransform.position;

            var targetDir = doorsTransform.position - characterPosition;
            var angle = Vector3.Angle(targetDir, doorsTransform.forward);
            var collinearity = angle < ToleranceAngle;
            
            var door001JointSpring = hingeJoints[0].spring;

            switch (isItemEnters)
            {
                case true:
                   
                    door001JointSpring.targetPosition = collinearity ? -openDoorTargetPosition : openDoorTargetPosition;
                    hingeJoints[0].spring = door001JointSpring;

                    if (_isThereSecondDoor)
                    {
                        var door002JointSpring = hingeJoints[1].spring;
                        door002JointSpring.targetPosition = collinearity ? openDoorTargetPosition : -openDoorTargetPosition;
                        hingeJoints[1].spring = door002JointSpring;
                    }
                    
                    _audioIsPlaying.PlaySound(SoundType.Positive);
                    
                    break;
                case false:
                    foreach (var j in hingeJoints)
                    {
                        var jointSpring = j.spring;
                        jointSpring.targetPosition = doorTargetPosition;
                        j.spring = jointSpring;
                    }
                    break;
            }
        }

        #endregion
    }
}