using DmitryAdventure.Characters;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure.Props
{
    /// <summary>
    /// Represents the door entity.
    /// </summary>
    [RequireComponent(typeof(AudioIsPlaying))]
    public class Door : LockedMechanism
    {
        #region Сonstants, variables & properties
        
        private bool _isThereSecondDoor;
        
        private AudioIsPlaying _audioIsPlaying;

        /// <summary>
        /// Tolerance angle for opening door.
        /// </summary>
        private const int ToleranceAngle = 65;
        
        #endregion

        #region Monobehavior methods

        private void Awake()
        {
            _audioIsPlaying = GetComponent<AudioIsPlaying>();
        }

        protected override void Start()
        {
            base.Start();
            _isThereSecondDoor = hingeJoints.Length > 1;
        }

        #endregion

        #region Functionality

        public override void OnDiscoveryTriggerHandler(DiscoveryType discoveryType, Transform discoveryTransform, bool isObjectEnters)
        {
            base.OnDiscoveryTriggerHandler(discoveryType, discoveryTransform, isObjectEnters);
            
            var character = discoveryTransform.gameObject.GetComponent<Character>();

            switch (lockedMechanismType)
            {
                case LockedMechanismType.Key when !MechanismIsOpen:
                {
                    var key = character.FindItemInInventory(GameData.KeysKey);
                    if (key != null)
                    {
                        MechanismIsOpen = true;
                        OpenCloseMechanism(discoveryTransform, isObjectEnters);
                    }
                    else
                    {
                        _audioIsPlaying.PlaySound(SoundType.Negative);
                    }

                    break;
                }
                case LockedMechanismType.Blocked when isObjectEnters:
                    _audioIsPlaying.PlaySound(SoundType.Negative);
                    break;
                default:
                    OpenCloseMechanism(discoveryTransform, isObjectEnters);
                    break;
            }
        }
        
        #endregion

        #region Other methods

        protected override void OpenCloseMechanism(Transform discoveryTransform, bool isItemEnters)
        {
            base.OpenCloseMechanism(discoveryTransform, isItemEnters);

            var doorsTransform = DiscoveryTrigger.transform;
            var characterPosition = discoveryTransform.position;

            var targetDir = doorsTransform.position - characterPosition;
            var angle = Vector3.Angle(targetDir, doorsTransform.forward);
            var collinearity = angle < ToleranceAngle;
            
            var door001JointSpring = hingeJoints[0].spring;

            switch (isItemEnters)
            {
                case true:

                    var delta = openPosition;
                    
                    door001JointSpring.targetPosition = collinearity ? -delta : delta;
                    hingeJoints[0].spring = door001JointSpring;

                    if (_isThereSecondDoor)
                    {
                        var door002JointSpring = hingeJoints[1].spring;
                        door002JointSpring.targetPosition = collinearity ? delta : -delta;
                        hingeJoints[1].spring = door002JointSpring;
                    }
                    
                    _audioIsPlaying.PlaySound(SoundType.Positive);

                    break;
                case false:
                    foreach (var j in hingeJoints)
                    {
                        var jointSpring = j.spring;
                        jointSpring.targetPosition = closePosition;
                        j.spring = jointSpring;
                    }
                    break;
            }
        }
        
        protected override void ChangingKinematicsRigidBody()
        {
            foreach (var j in hingeJoints)
            {
                if (j.gameObject.TryGetComponent<Rigidbody>(out var rb))
                {
                    rb.isKinematic = lockedMechanismType == LockedMechanismType.Blocked || !MechanismIsOpen;
                }
            }
        }

        #endregion
    }
}