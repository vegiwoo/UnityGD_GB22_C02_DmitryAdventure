using UnityEngine;

namespace DmitryAdventure
{
    public class AimPointer : MonoBehaviour
    {
        [SerializeField] private Transform aim;
        [SerializeField] private Camera gameCamera;

        /// <summary>
        /// Текущая позиция прицела.
        /// </summary>
        public Vector3 PointerCurrentPosition { get; private set; }
        
        private void LateUpdate()
        {
            var ray = gameCamera.ScreenPointToRay (Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit, 30)) return;
            PointerCurrentPosition = new Vector3(hit.point.x, hit.point.y + 1, hit.point.z);
            aim.position = PointerCurrentPosition;
        }
    }
}

