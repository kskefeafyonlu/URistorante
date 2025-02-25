using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.BuildingSystem
{
    public class PlaceableBuilding : MonoBehaviour
    {
        public PlaceableBuildingData data;
        public Collider2D Collider;
        
        private SpriteRenderer _spriteRenderer;
        
        private void Awake()
        {
            Collider = GetComponent<Collider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        private void Start()
        {
            if (Collider == null)
            {
                Collider = GetComponent<Collider2D>();
            }
            
            // Update the collider size based on building dimensions
            if (data != null && Collider is BoxCollider2D boxCollider)
            {
                boxCollider.size = new Vector2(data.buildingWidth, data.buildingHeight);
            }
        }
        
        public void SetBuildingFunctionality(PlaceableBuildingData buildingData)
        {
            this.data = buildingData;
            
            // Make sure we have required components
            Collider = GetComponent<Collider2D>();
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }
            
            // Update visual appearance
            if (_spriteRenderer != null && buildingData.preview != null)
            {
                _spriteRenderer.sprite = buildingData.preview;
            }
            
            // Update the collider size
            if (Collider is BoxCollider2D boxCollider)
            {
                boxCollider.size = new Vector2(buildingData.buildingWidth, buildingData.buildingHeight);
            }
            
            // Set the object's name for easier debugging
            gameObject.name = $"Building_{buildingData.buildingName}";
        }
    }
}