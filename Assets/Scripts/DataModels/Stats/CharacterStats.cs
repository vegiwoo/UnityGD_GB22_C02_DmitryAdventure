using UnityEngine;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure.Stats
{
    public class CharacterStats : ScriptableObject
    {
        [field: Header("General stats")]
        [field: SerializeField, Tooltip("Character name")] private string CharacterName { get; set; }
        [field: SerializeField,Tooltip("Max hit points"), Range(50,150)] public float MaxHp { get; set; }

        [field: Header("Movement stats")]
        [field: SerializeField, Tooltip("Character base movement speed"), Range(1, 5)]
        public float BaseMoveSpeed { get; set; }

        [field:SerializeField,Tooltip("Character base  rotation speed"),Range(1,5)] 
        public float BaseRotationSpeed { get; set; }

        [field:SerializeField, Tooltip("Character attention radius"), Range(10, 30)]
        public float AttentionRadius { get; set; }

        public float RotationAngleDelta => BaseRotationSpeed * 2;
    }
}