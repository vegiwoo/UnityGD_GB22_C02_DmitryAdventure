using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure
{
    public class SomethingWithCounter
    {
        private int counter = 0;

        public void Increase()
        {
            counter++;
        }
        
    }

    public enum TestScriptMode
    {
        IsRotateGoInParent, IsRotateGoInSelf, NoGo
    }
    
    /// <summary>
    /// Create a short description of entity!
    /// </summary>
    public class TestScript : MonoBehaviour
    {
        #region Сonstants, variables & properties

        [SerializeField] 
        private GameObject rotationObjectPrefab;

        [SerializeField] private int count;
        
        [field:SerializeField, ReadonlyField] 
        private int NumberOfObjectsCreated { get; set; }
        
        private const int GenerationRadius = 50;
        private const float RotationAngle = 10f;

        [SerializeField] private TestScriptMode mode;

        private List<GameObject> gameObjects;
        private List<SomethingWithCounter> noGameObjects;

        #endregion

        #region Monobehavior methods

        private void Start()
        {
            NumberOfObjectsCreated = 0;

            switch (mode)
            {
                case TestScriptMode.IsRotateGoInParent:
                    gameObjects = new List<GameObject>(1000);
                    StartCoroutine(CreateObjectsWithSelfRotationCoroutine());
                    break;
                case TestScriptMode.IsRotateGoInSelf:
                    gameObjects = new List<GameObject>(1000);
                    StartCoroutine(CreateObjectsWithParentRotationCoroutine());
                    StartCoroutine(ParentRotationCoroutine());
                    break;
                case TestScriptMode.NoGo:
                    noGameObjects = new List<SomethingWithCounter>(1000);
                    StartCoroutine(NoGameObjectCoroutine());
                    break;
            }
        }

        private void Update()
        {
            // while loop in Update method
            // var count = 10000;
            // while (count != 0)
            // {
            //     count--;
            //     Debug.Log(count);
            // }
        }

        #endregion

        #region Functionality

        private IEnumerator CreateObjectsWithSelfRotationCoroutine()
        {
           
            while (NumberOfObjectsCreated < count)
            {
                var newFigure = CreateObject();

                if (newFigure.TryGetComponent<TestRotation>(out var rotation))
                {
                    rotation.Init(true, RotationAngle, transform);
                }

                NumberOfObjectsCreated++;
                
                yield return null;
            }
        }

        private IEnumerator CreateObjectsWithParentRotationCoroutine()
        {
            while (NumberOfObjectsCreated < count)
            {
                var newFigure = CreateObject();

                if (newFigure.TryGetComponent<TestRotation>(out var rotation))
                {
                    rotation.Init(false, RotationAngle, null);
                }

                NumberOfObjectsCreated++;
                
                gameObjects.Add(newFigure);
                
                yield return null;
            }
        }

        private IEnumerator ParentRotationCoroutine()
        {
            while (true)
            {
                foreach (var t in gameObjects.Select(go => go.transform))
                {
                    t.RotateAround(t.position, t.up, RotationAngle * Time.deltaTime);
                }
                yield return null;
            }
        }

        private IEnumerator NoGameObjectCoroutine()
        {
            while (NumberOfObjectsCreated < count)
            {
                var counter = new SomethingWithCounter();
                noGameObjects.Add(counter);
                NumberOfObjectsCreated++;
                
                foreach (var obj in noGameObjects)
                {
                    obj.Increase();
                }
                
                yield return null;
            }
        }

        private GameObject CreateObject()
        {
            var t = transform;
            var newFigure = Instantiate(rotationObjectPrefab, t.position, t.rotation);
            newFigure.transform.localPosition = Random.insideUnitSphere * GenerationRadius;
            return newFigure;
        }
        
        #endregion
    }
}