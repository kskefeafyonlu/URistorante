using System;
using _Game;
using _Game.BuildingSystem;
using UnityEngine;
using UnityEngine.Serialization;

public class Mouse_BuildingManager : MonoBehaviour
{
    public static Mouse_BuildingManager Instance;
    
    public bool isEnabled = true;

    public PlaceableBuilding buidlingAtHand;
    public GameObject placeableBuildingPrefab;

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


    private void Update()
    {
        
    }
    
    private void TryPlaceBuilding()
    {
        if (buidlingAtHand == null)
        {
            Debug.LogError("No building at hand");
            return;
        }
        
        Vector3 position = WorldGridSystem.Instance.GetPositionOnGrid(MouseInputManager.Instance.GetMousePosition());
        
        if (CanPlaceBuilding(position))
        {
            PlaceBuilding(position);
        }
    }
    
    private bool CanPlaceBuilding(Vector3 position)
    {
        if (buidlingAtHand == null)
        {
            Debug.LogError("No building at hand");
            return false;
        }

        Collider2D[] colliders = Physics2D.OverlapBoxAll(position, new Vector2(buidlingAtHand.data.buildingWidth, buidlingAtHand.data.buildingHeight), 0);
        foreach (var col in colliders)
        {
            if (col != buidlingAtHand.Collider)
            {
                Debug.LogError("Cannot place building here, another building is in the way");
                return false;
            }
        }

        return true;
    }
    
    
    private void PlaceBuilding(Vector3 position)
    {
        if (buidlingAtHand == null)
        {
            Debug.LogError("No building at hand");
            return;
        }
        
        GameObject newBuildingObject = Instantiate(placeableBuildingPrefab, position, Quaternion.identity);
        newBuildingObject.GetComponent<PlaceableBuilding>().SetBuildingFunctionality(buidlingAtHand.data);
        WorldGridSystem.Instance.currentBuildings.Add(newBuildingObject.GetComponent<PlaceableBuilding>());
    }
    
    public void SetBuildingAtHand(PlaceableBuildingData buildingData)
    {
        if (buildingData == null) return;
        
        // Instantiate a new building at the mouse position
        if (placeableBuildingPrefab != null)
        {
            // Destroy existing building at hand if there is one
            if (buidlingAtHand != null)
            {
                Destroy(buidlingAtHand.gameObject);
            }
            
            // Create new building
            Vector3 position = WorldGridSystem.Instance.GetPositionOnGrid(MouseInputManager.Instance.GetMousePosition());
            GameObject newBuildingObject = Instantiate(placeableBuildingPrefab, position, Quaternion.identity);
            buidlingAtHand = newBuildingObject.GetComponent<PlaceableBuilding>();
            
            if (buidlingAtHand != null)
            {
                buidlingAtHand.SetBuildingFunctionality(buildingData);
            }
        }
    }
}
