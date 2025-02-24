using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace _Game.BuildingSystem
{
    public class BuildableObjectButton : MonoBehaviour
    {
        private PlaceableBuildingData        _data;
        private Image                       _image;
        private Button                      _button;
        
        private void Awake()
        {
            _image = GetComponent<Image>();
            _button = GetComponent<Button>();
        }
        
        public void SetData(PlaceableBuildingData data)
        {
            _data = data;
            
            if (_image != null && data.preview != null)
            {
                _image.sprite = data.preview;
            }
            
            if (_button != null)
            {
                _button.onClick.RemoveAllListeners();
                _button.onClick.AddListener(OnButtonClicked);
            }
        }
        
        private void OnButtonClicked()
        {
            if (_data != null && Mouse_BuildingManager.Instance != null)
            {
                Mouse_BuildingManager.Instance.SetBuildingAtHand(_data);
            }
        }
    }
}