using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace _Game.BuildingSystem
{
    public class WorldGridSystem : MonoBehaviour
    {
        public static WorldGridSystem Instance;
        
        
        private float _maxGridDimension = 4f;
        private float _minGridDimension = 0.125f;
        
        public float gridWidth = 1f; //default 1
        public float gridHeight = 1f; //default 1
        
        public TextMeshProUGUI gridWidthText;
        
        
        public List<PlaceableBuilding> currentBuildings = new List<PlaceableBuilding>();
    
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }
        
        private void Start()
        {
            gridWidthText.text = gridWidth.ToString();
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
            
            gridWidthText.text = gridWidth.ToString();
        }
    }
    
    
    
}
