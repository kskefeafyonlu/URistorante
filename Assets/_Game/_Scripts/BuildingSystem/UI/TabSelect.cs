using System;
using _Game._Scripts;
using UnityEngine;

namespace _Game.BuildingSystem
{
    public class BuildingTabSelect: MonoBehaviour
    {
        [SerializeField] private BuildScrollbar buildScrollbar;

        private void Start()
        {
            ChangeTab(BuildingType.One.ToString());
        }

        public void ChangeTab(string buildingType)
        {
            Debug.Log("Changing tab to: " + buildingType);
            BuildingType type = (BuildingType) Enum.Parse(typeof(BuildingType), buildingType);
            
            var buildings = BuildingsDatabase.Instance.GetBuildingsByType(type);
            
            if (buildScrollbar != null)
            {
                buildScrollbar.PopulateWithBuildings(buildings);
            }
        }
    }
}