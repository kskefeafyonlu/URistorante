using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.BuildingSystem
{
    public class PlaceableBuilding : MonoBehaviour
    {
        public PlaceableBuildingData data;
        public Collider2D Collider;
        
        private void Start()
        {
            Collider = GetComponent<Collider2D>();
        }
        
        
        public void SetBuildingFunctionality(PlaceableBuildingData data)
        {
            this.data = data;
            Collider = GetComponent<Collider2D>();

        }
    }
}