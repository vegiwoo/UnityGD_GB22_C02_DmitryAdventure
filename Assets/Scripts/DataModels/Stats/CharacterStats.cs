using UnityEngine;

namespace DmitryAdventure
{
    public class CharacterStats : ScriptableObject
    {
        [Header("General stats of characters")]
        [SerializeField,Tooltip("Character name")] private string characterName;
        /// <summary>
        /// Character name.
        /// </summary>
        public string CharacterName => characterName;
        
        [SerializeField,Tooltip("Max hit points"), Range(50,150)] private float maxHp;
        /// <summary>
        /// Max hit points .
        /// </summary>
        public float MaxHp => maxHp;
        
        [Header("Movement stats")]
        [SerializeField,Tooltip("Character base movement speed"), Range(1,5)] private float baseMovementSpeed;
        /// <summary>
        /// Character base movement speed.
        /// </summary>
        public float BaseMovementSpeed => baseMovementSpeed;
        
        [SerializeField,Tooltip("Character base  rotation speed"),Range(1,5)] private float baseRotationSpeed;
        /// <summary>
        /// Character base  rotation speed.
        /// </summary>
        public float BaseRotationSpeed => baseRotationSpeed;
        
        [SerializeField,Tooltip("Character attention radius"), Range(10,30)] private float attentionRadius;
        /// <summary>
        /// Character attention radius.
        /// </summary>
        public float AttentionRadius => attentionRadius;

        public float RotationAngleDelta => baseRotationSpeed * 2;
    }
}

