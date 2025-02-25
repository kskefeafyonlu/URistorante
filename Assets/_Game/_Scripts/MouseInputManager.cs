using UnityEngine;

namespace _Game._Scripts
{
    public class MouseInputManager : MonoBehaviour
    {
        public static MouseInputManager Instance;

        private Camera _mainCamera;
        private Vector3 _mousePosition;

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

        private void Start()
        {
            _mainCamera = Camera.main;
        }



        
        public Vector3 GetMousePosition()
        {
            _mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            _mousePosition.z = 0;
            
            return _mousePosition;
        }



    
    }
}
