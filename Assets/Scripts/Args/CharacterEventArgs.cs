using System;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure.Args
{
    /// <summary>
    /// Arguments for character event.
    /// </summary>
    public class CharacterEventArgs 
    {
        #region Сonstants, variables & properties

        private readonly CharacterType _characterType;
        private readonly float _currentHp;
        public readonly bool Die;

        #endregion

        #region Initializers and Deinitializer

        public CharacterEventArgs(CharacterType characterType, float currentHp)
        {
            _characterType = characterType;
            _currentHp = currentHp;
            Die = _currentHp <= 0;
        }

        #endregion
    }
}
