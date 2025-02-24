using System.Collections.Generic;
using System.Linq;
using _Game.BuildingSystem;
using _Game.General._Project._Scripts.General.ClassBases;
using UnityEngine;

namespace _Game._Scripts
{
    public enum BuildingType
    {
        One,
        Two,
        Three
    }
    
    public class BuildingsDatabase: MonoSingleton<BuildingsDatabase>
    {
        [SerializeField] private List<PlaceableBuildingData> buildingDataAssets;
        
        private HashSet<PlaceableBuildingData> _buildingDatas;
        
        private Dictionary<BuildingType, List<PlaceableBuildingData>> _buildingsByType;
        
        protected override void Awake()
        {
            base.Awake();
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            _buildingDatas   = new HashSet<PlaceableBuildingData>();
            _buildingsByType = new Dictionary<BuildingType, List<PlaceableBuildingData>>();
            
            // Initialize dictionary for each building type
            foreach (BuildingType type in System.Enum.GetValues(typeof(BuildingType)))
            {
                _buildingsByType[type] = new List<PlaceableBuildingData>();
            }
            
            // Load buildings from assets if provided
            if (buildingDataAssets != null && buildingDataAssets.Count > 0)
            {
                foreach (var building in buildingDataAssets)
                {
                    RegisterBuilding(building);
                }
            }
        }
        public PlaceableBuildingData[] GetAllBuildings()
        {
            return _buildingDatas.ToArray();
        }
        
        public PlaceableBuildingData[] GetBuildingsByType(BuildingType type)
        {
            if (_buildingsByType.TryGetValue(type, out var buildings))
            {
                return buildings.ToArray();
            }
            
            return new PlaceableBuildingData[0];
        }
        
        public void RegisterBuilding(PlaceableBuildingData buildingData)
        {
            if (buildingData == null) return;
            
            _buildingDatas.Add(buildingData);
            
            // You'll need to add a buildingType field to PlaceableBuildingData
            // For now, let's assume we're using the first type as default
            BuildingType type = BuildingType.One;
            
            if (!_buildingsByType.ContainsKey(type))
            {
                _buildingsByType[type] = new List<PlaceableBuildingData>();
            }
            
            _buildingsByType[type].Add(buildingData);
        }
        
        public void UnregisterBuilding(PlaceableBuildingData buildingData)
        {
            if (buildingData == null) return;
            
            _buildingDatas.Remove(buildingData);
            
            // Remove from type dictionary
            foreach (var typeList in _buildingsByType.Values)
            {
                typeList.Remove(buildingData);
            }
        }
    }
}