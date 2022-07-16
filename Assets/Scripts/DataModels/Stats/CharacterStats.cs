using UnityEngine;

namespace DmitryAdventure
{
    public class CharacterStats : ScriptableObject
    {
        [Header("General stats of characters")]
        [Tooltip("Character name")] public string chatacterName;
        [Tooltip("Max hit points"), Range(50,150)] public float maxHp;
        [Header("Movement stats")]
        [Tooltip("Character base movement speed"), Range(1,5)] public float baseMovementSpeed;
        [Tooltip("Character base  rotation speed"),Range(1,5)] public float baseRotationSpeed;
        [Tooltip("Character attention radius"), Range(10,30)] public float attentionRadius;
        public float RotationAngleDelta => baseRotationSpeed * 2;
    }
}

