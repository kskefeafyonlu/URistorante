using UnityEngine;
using _Game._Scripts;

namespace _Game.BuildingSystem
{
    [CreateAssetMenu(fileName = "New Placeable Building", menuName = "Building System/Placeable Building")]
    public class PlaceableBuildingData : ScriptableObject
    {
        public string buildingName;
        public Sprite preview;
        public BuildingType buildingType = BuildingType.One;
        public GameObject buildingPrefab;
    
        public float buildingWidth = 1f;
        public float buildingHeight = 1f;
    
    
    }
}
