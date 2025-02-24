using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.BuildingSystem
{
    public class BuildScrollbar: MonoBehaviour
    {
        [SerializeField] private GameObject buildingButtonPrefab;
        [SerializeField] private Transform contentContainer;

        private BuildableObjectButton _imageHolder;
        
        private List<GameObject> _instantiatedButtons = new List<GameObject>();
        private List<GameObject> _usedButtons = new List<GameObject>();
        
        public void PopulateWithBuildings(PlaceableBuildingData[] buildings)
        {
            ClearButtons();
            
            foreach (var building in buildings)
            {
                CreateBuildingButton(building);
            }
        }
        
        private void CreateBuildingButton(PlaceableBuildingData buildingData)
        {
            if (buildingButtonPrefab == null || contentContainer == null) return;
            GameObject buttonObj;
            if (_usedButtons.Count >= _instantiatedButtons.Count)
            {
                buttonObj = Instantiate(buildingButtonPrefab, contentContainer);
                _instantiatedButtons.Add(buttonObj);
                _usedButtons.Add(buttonObj);
            }
            else
            {
                buttonObj = _instantiatedButtons[_usedButtons.Count];
                buttonObj.SetActive(true);
                _usedButtons.Add(buttonObj);
            }
            
            BuildableObjectButton buildButton = buttonObj.GetComponent<BuildableObjectButton>();
            if (buildButton != null)
            {
                buildButton.SetData(buildingData);
            }
            else
            {
                Image buttonImage = buttonObj.GetComponent<Image>();
                if (buttonImage != null && buildingData.itemImage != null)
                {
                    buttonImage.sprite = buildingData.itemImage;
                }
                
                Button button = buttonObj.GetComponent<Button>();
                if (button != null)
                {
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() => SelectBuilding(buildingData));
                }
            }
        }
        
        private void SelectBuilding(PlaceableBuildingData buildingData)
        {
            if (Mouse_BuildingManager.Instance != null)
            {
                Mouse_BuildingManager.Instance.SetBuildingAtHand(buildingData);
            }
        }
        
        private void ClearButtons()
        {
            foreach (var button in _instantiatedButtons)
            {
                Destroy(button);
            }
            
            _instantiatedButtons.Clear();
        }
    }
}