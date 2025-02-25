using System;
using _Game;
using _Game.BuildingSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Mouse_BuildingManager : MonoBehaviour
{
    public static Mouse_BuildingManager Instance;
    
    public bool isEnabled = true;

    public PlaceableBuilding buildingAtHand;
    public GameObject placeableBuildingPrefab;
    
    // Track building movement
    private Vector3 _lastPosition;
    private bool _isDragging = false;

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
        if (!isEnabled || buildingAtHand == null) return;
        
        // Move building with mouse
        Vector3 mousePos = MouseInputManager.Instance.GetMousePosition();
        Vector3 gridPos = WorldGridSystem.Instance.GetPositionOnGrid(mousePos);
        
        // Only update position if it changed (optimization)
        if (gridPos != _lastPosition)
        {
            buildingAtHand.transform.position = gridPos;
            _lastPosition = gridPos;
        }
        
        // Check for left mouse button to place building
        if (Input.GetMouseButtonDown(0))
        {
            TryPlaceBuilding();
        }
        
        // Check for right mouse button to cancel placement
        if (Input.GetMouseButtonDown(1))
        {
            CancelPlacement();
        }
    }
    
    private void CancelPlacement()
    {
        if (buildingAtHand != null)
        {
            Destroy(buildingAtHand.gameObject);
            buildingAtHand = null;
        }
    }
    
    private void TryPlaceBuilding()
    {
        if (buildingAtHand == null)
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
        if (buildingAtHand == null)
        {
            Debug.LogError("No building at hand");
            return false;
        }

        Collider2D[] colliders = Physics2D.OverlapBoxAll(position, new Vector2(buildingAtHand.data.buildingWidth, buildingAtHand.data.buildingHeight), 0);
        foreach (var col in colliders)
        {
            if (col != buildingAtHand.Collider)
            {
                Debug.LogError("Cannot place building here, another building is in the way");
                return false;
            }
        }

        return true;
    }
    
    private void PlaceBuilding(Vector3 position)
    {
        if (buildingAtHand == null)
        {
            Debug.LogError("No building at hand");
            return;
        }
        
        // Store the data for future placement
        PlaceableBuildingData buildingData = buildingAtHand.data;
        
        // Create the actual building (permanent placement)
        GameObject newBuildingObject = Instantiate(placeableBuildingPrefab, position, Quaternion.identity);
        newBuildingObject.GetComponent<PlaceableBuilding>().SetBuildingFunctionality(buildingData);
        WorldGridSystem.Instance.currentBuildings.Add(newBuildingObject.GetComponent<PlaceableBuilding>());
        
        // Get another copy of the same building to continue placing
        SetBuildingAtHand(buildingData);
    }
    
    public void SetBuildingAtHand(PlaceableBuildingData buildingData)
    {
        if (buildingData == null)
        {
            Debug.LogError("Attempted to set null building data");
            return;
        }
        
        Debug.Log($"Setting building at hand: {buildingData.buildingName}");
        
        // Use the prefab from the building data rather than requiring a global prefab
        GameObject prefabToUse = null;
        
        // First check if we have a valid prefab in the building data
        if (buildingData.buildingPrefab != null)
        {
            prefabToUse = buildingData.buildingPrefab;
            Debug.Log($"Using building prefab from data: {prefabToUse.name}");
        }
        // Fall back to the global prefab if available
        else if (placeableBuildingPrefab != null)
        {
            prefabToUse = placeableBuildingPrefab;
            Debug.Log($"Using global building prefab: {prefabToUse.name}");
        }
        else
        {
            Debug.LogError("No valid building prefab found! Please assign either:\n" +
                          "1. A prefab to the buildingPrefab field in your PlaceableBuildingData asset, or\n" +
                          "2. A global prefab to the placeableBuildingPrefab field in Mouse_BuildingManager");
            return;
        }
        
        try
        {
            // Check if the prefab has the required component
            PlaceableBuilding prefabComponent = prefabToUse.GetComponent<PlaceableBuilding>();
            if (prefabComponent == null)
            {
                Debug.LogError($"The prefab {prefabToUse.name} doesn't have a PlaceableBuilding component!");
                return;
            }
            
            // Destroy existing building at hand if there is one
            if (buildingAtHand != null)
            {
                Debug.Log("Destroying existing building at hand");
                Destroy(buildingAtHand.gameObject);
                buildingAtHand = null;
            }
            
            // Ensure MouseInputManager and WorldGridSystem exist
            if (MouseInputManager.Instance == null)
            {
                Debug.LogError("MouseInputManager.Instance is null!");
                return;
            }
            
            if (WorldGridSystem.Instance == null)
            {
                Debug.LogError("WorldGridSystem.Instance is null!");
                return;
            }
            
            // Create new building
            Vector3 position = WorldGridSystem.Instance.GetPositionOnGrid(MouseInputManager.Instance.GetMousePosition());
            Debug.Log($"Creating new building at position: {position}");
            
            GameObject newBuildingObject = Instantiate(prefabToUse, position, Quaternion.identity);
            Debug.Log($"New building object created: {newBuildingObject.name}");
            
            // Make sure we're getting the component
            buildingAtHand = newBuildingObject.GetComponent<PlaceableBuilding>();
            
            if (buildingAtHand == null)
            {
                Debug.LogError("Failed to get PlaceableBuilding component from instantiated object!");
                Destroy(newBuildingObject);
                return;
            }
            
            _lastPosition = position;
            
            // Set data and make it semi-transparent to indicate it's a preview
            buildingAtHand.SetBuildingFunctionality(buildingData);
            
            // Make the preview semi-transparent
            SpriteRenderer spriteRenderer = buildingAtHand.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                Color previewColor = spriteRenderer.color;
                previewColor.a = 0.7f; // 70% opacity
                spriteRenderer.color = previewColor;
            }
            else
            {
                Debug.LogWarning("No SpriteRenderer found on building preview");
            }
            placeableBuildingPrefab = prefabToUse;
            
            Debug.Log($"Successfully created preview for: {buildingData.buildingName}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Exception in SetBuildingAtHand: {e.Message}\n{e.StackTrace}");
        }
    }
}
