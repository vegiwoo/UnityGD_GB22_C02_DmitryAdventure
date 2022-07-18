using System;

namespace DmitryAdventure
{
    /// <summary>
    /// Arguments for character event.
    /// </summary>
    public class CharacterEventArgs 
    {
        #region Сonstants, variables & properties

        public readonly CharacterType CharacterType;
        public readonly float CurrentHp;
        public readonly bool Die;

        #endregion

        #region Initializers and Deinitializer

        public CharacterEventArgs(CharacterType characterType, float currentHp)
        {
            CharacterType = characterType;
            CurrentHp = currentHp;
            Die = CurrentHp <= 0;
        }

        #endregion

        #region Functionality

        #region Event handlers

        // ...

        #endregion

        #region Other methods

        // ...

        #endregion

        #endregion
    }
}
