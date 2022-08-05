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

        protected override void OnDiscoveryTriggerHandler(DiscoveryType discoveryType, Transform discoveryTransform, bool isObjectEnters)
        {
            base.OnDiscoveryTriggerHandler(discoveryType, discoveryTransform, isObjectEnters);
            
            var character = discoveryTransform.gameObject.GetComponent<Character>();

            if (lockedMechanismType == LockedMechanismType.Key && !MechanismIsOpen)
            {
                var key = character.FindItemInInventory(GameData.KeysKey);
                if (key != null)
                {
                    OpenCloseMechanism(discoveryTransform, isObjectEnters);
                }
                else
                {
                    _audioIsPlaying.PlaySound(SoundType.Negative);
                }
            }
            else
            {
                OpenCloseMechanism(discoveryTransform, isObjectEnters);
            }
        }
        

        #endregion

        #region Other methods

        protected override void OpenCloseMechanism(Transform discoveryTransform, bool isItemEnters)
        {
            base.OpenCloseMechanism(discoveryTransform, isItemEnters);

            var doorsTransform = discoveryTrigger.transform;
            var characterPosition = discoveryTransform.position;

            var targetDir = doorsTransform.position - characterPosition;
            var angle = Vector3.Angle(targetDir, doorsTransform.forward);
            var collinearity = angle < ToleranceAngle;
            
            var door001JointSpring = hingeJoints[0].spring;

            switch (isItemEnters)
            {
                case true:

                    var delta = openMechanismTargetPosition;
                    
                    door001JointSpring.targetPosition = collinearity ? -delta : delta;
                    hingeJoints[0].spring = door001JointSpring;

                    if (_isThereSecondDoor)
                    {
                        var door002JointSpring = hingeJoints[1].spring;
                        door002JointSpring.targetPosition = collinearity ? delta : -delta;
                        hingeJoints[1].spring = door002JointSpring;
                    }
                    
                    _audioIsPlaying.PlaySound(SoundType.Positive);
                    MechanismIsOpen = true;
                    
                    break;
                case false:
                    foreach (var j in hingeJoints)
                    {
                        var jointSpring = j.spring;
                        jointSpring.targetPosition = closeMechanismTargetPosition;
                        j.spring = jointSpring;
                    }
                    MechanismIsOpen = false;
                    break;
            }
        }

        #endregion
    }
}