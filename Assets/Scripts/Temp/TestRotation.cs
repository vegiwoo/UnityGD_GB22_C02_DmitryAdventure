using JetBrains.Annotations;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure
{
    public class TestRotation : MonoBehaviour
    {
        private bool isRotateObjectsInClass;
        private float rotationAngle;
 
        // Update is called once per frame
        private void Update()
        {
            if (!isRotateObjectsInClass) return;
            
            var t = transform;
            t.RotateAround(t.position, t.up, rotationAngle * Time.deltaTime);

        }

        public void Init(bool isRotateObjects, float? angle, [CanBeNull] Transform parent)
        {
            isRotateObjectsInClass = isRotateObjects;
            rotationAngle = angle ?? 0;

            if (parent != null)
            {
                transform.parent = parent;
            }
        }
    }
}
