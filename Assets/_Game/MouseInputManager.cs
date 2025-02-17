using UnityEngine;


public class MouseInputManager : MonoBehaviour
{
    public GameObject indicatorObjectPrefab;
    public GameObject indicatorObject;

    private Camera _mainCamera;
    private Vector3 _mousePosition;

    void Start()
    {
        _mainCamera = Camera.main;
        if (indicatorObject == null)
        {
            indicatorObject = Instantiate(indicatorObjectPrefab);
        }

    }


    void Update()
    {
        _mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        _mousePosition.z = 0;
        indicatorObject.transform.position = GetPositionOnGrid(_mousePosition);
    }


    public float gridWidth = .5f;
    public float gridHeight = .5f;
    
    
    private Vector3 GetPositionOnGrid(Vector3 rawPosition)
    {
        Vector3 pointerPosition = new Vector3();
        pointerPosition.x = Mathf.Floor(rawPosition.x / gridWidth) * gridWidth + gridWidth / 2;
        pointerPosition.y = Mathf.Floor(rawPosition.y / gridHeight) * gridHeight + gridHeight / 2;
        
        return pointerPosition;
    }
    
}
