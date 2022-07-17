using UnityEngine;

namespace DmitryAdventure
{
    public class CharacterStats : ScriptableObject
    {
        [Header("General stats of characters")]
        [SerializeField,Tooltip("Character name")] private string chatacterName;
        public string ChatacterName => chatacterName;
        
        [SerializeField,Tooltip("Max hit points"), Range(50,150)] private float maxHp;
        public float MaxHP => maxHp;
        
        [Header("Movement stats")]
        [SerializeField,Tooltip("Character base movement speed"), Range(1,5)] private float baseMovementSpeed;
        public float BaseMovementSpeed => baseMovementSpeed;
        
        [SerializeField,Tooltip("Character base  rotation speed"),Range(1,5)] private float baseRotationSpeed;
        public float BaseRotationSpeed => baseRotationSpeed;
        
        [SerializeField,Tooltip("Character attention radius"), Range(10,30)] private float attentionRadius;
        public float AttentionRadius => attentionRadius;

        public float RotationAngleDelta => baseRotationSpeed * 2;
    }
}

