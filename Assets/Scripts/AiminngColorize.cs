using UnityEngine;
using UnityEngine.UI;

namespace DmitryAdventure
{
    /// <summary>
    /// Responsible for illumination of sight.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class AiminngColorize : MonoBehaviour
    {
        #region Variables & constants
        public Image image;
        #endregion

        #region Monobehavior methods
        private void Start()
        {
            image = GetComponent<Image>();
        }
        #endregion

        #region Funcationality
        public void SetColor(Color32 newColor)
        {
            if (image.color == newColor) return;
            image.color = newColor;
            
        }
        #endregion
    }
}