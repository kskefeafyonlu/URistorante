using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace _Game.BuildingSystem
{
    public class WorldGridSystem : MonoBehaviour
    {
        // Singleton implementation
        public static WorldGridSystem Instance { get; private set; }
        
        private float _maxGridDimension = 4f;
        private float _minGridDimension = 0.125f;
        
        public float gridWidth = 1f; //default 1
        public float gridHeight = 1f; //default 1
        
        public TextMeshProUGUI gridWidthText;
        
        public List<PlaceableBuilding> currentBuildings = new();
        
        private void Awake()
        {
            // Correct singleton implementation
            if (Instance == null)
            {
                Instance = this;
                // Uncomment if you want it to persist between scenes
                // DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Debug.LogWarning("Found duplicate WorldGridSystem - destroying duplicate");
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            if (gridWidthText != null)
            {
                gridWidthText.text = gridWidth.ToString();
            }
            else
            {
                Debug.LogWarning("gridWidthText reference is null in WorldGridSystem");
            }
        }
        
        public Vector3 GetPositionOnGrid(Vector3 rawPosition)
        {
            Vector3 pointerPosition = new Vector3();
            pointerPosition.x = Mathf.Floor(rawPosition.x / gridWidth) * gridWidth + gridWidth / 2;
            pointerPosition.y = Mathf.Floor(rawPosition.y / gridHeight) * gridHeight + gridHeight / 2;
        
            return pointerPosition;
        }
    
        public void MultiplyGridDimension(float d)
        {
            if (gridWidth * d > _maxGridDimension || gridWidth * d < _minGridDimension)
            {
                return;
            }
            gridWidth *= d;
            gridHeight *= d;
            
            if (gridWidthText != null)
            {
                gridWidthText.text = gridWidth.ToString();
            }
        }
    }
}