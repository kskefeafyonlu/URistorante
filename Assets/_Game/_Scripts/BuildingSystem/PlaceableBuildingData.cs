using UnityEngine;

namespace _Game._Scripts.BuildingSystem
{
    [CreateAssetMenu(fileName = "New Placeable Building", menuName = "Building System/Placeable Building")]
    public class PlaceableBuildingData : ScriptableObject
    {
        public string buildingName;
        public Sprite preview;
        public Sprite itemImage;
        public BuildingType buildingType = BuildingType.One;
    
        public float buildingWidth = 1f;
        public float buildingHeight = 1f;
    
    
    }
}
