using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DmitryAdventure
{
    public class AimPointer : MonoBehaviour
    {
        [SerializeField] private Transform aim;
        [SerializeField] private Camera gameCamera;
        private GameObject hero;
        private float yEuler;

        private void Awake()
        {
            hero = transform.parent.gameObject;
        }

        private void LateUpdate()
        {
            var ray = gameCamera.ScreenPointToRay(Input.mousePosition);
            var plane = new Plane(-Vector3.forward, Vector3.zero);
            plane.Raycast(ray, out var distance);
            var point = ray.GetPoint(distance);
            aim.position = point;
        }
    }

}

