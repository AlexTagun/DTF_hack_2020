using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropChild : MonoBehaviour
{
    [SerializeField] private HingeJoint2D _hingeJoint;
    [SerializeField] private Rigidbody2D _rigidbody;
    private Camera _camera = null;
    private CameraMovement _cameraMovement;
    private bool _isMouseDown = false;
    private Vector2 CurPosition => new Vector2(transform.position.x, transform.position.y);
    private Vector2 MousePosition => GetMousePosition();
    private Vector2 _mouseDelta;

    void Start()
    {
        _hingeJoint.connectedBody = GameObject.Find("hand").GetComponent<Rigidbody2D>();
        _camera = Camera.main;
        _cameraMovement = _camera.GetComponent<CameraMovement>();
        _hingeJoint.enabled = false;
    }
    private Vector2 GetMousePosition()
    {
        Vector2 mousePos = Input.mousePosition;
        mousePos = _camera.ScreenToWorldPoint(mousePos);
        return new Vector2(mousePos.x, mousePos.y);
    }


    private void OnMouseDown()
    {
        _isMouseDown = true;
        _hingeJoint.enabled = true;
        _mouseDelta = CurPosition - MousePosition;
    }

    private void OnMouseUp()
    {
        _isMouseDown = false;
        _hingeJoint.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isMouseDown)
        {
            Vector3 coordinateChildOnScreen = _camera.WorldToViewportPoint(transform.position);
            if (coordinateChildOnScreen.x < 0.2f)
            {
                float speed = (0.2f - coordinateChildOnScreen.x) / 0.2f;
                _cameraMovement.MoveCameraWhenDragChild(CameraMovement.DirectionMovement.Left, speed);
            }
            if (coordinateChildOnScreen.x > 0.8)
            {
                float speed = (coordinateChildOnScreen.x - 0.8f) / 0.2f;
                _cameraMovement.MoveCameraWhenDragChild(CameraMovement.DirectionMovement.Right, speed);
            }
            if (coordinateChildOnScreen.y < 0.2f)
            {
                float speed = (0.2f - coordinateChildOnScreen.y) / 0.2f;
                _cameraMovement.MoveCameraWhenDragChild(CameraMovement.DirectionMovement.Down, speed);
            }
            if (coordinateChildOnScreen.y > 0.8)
            {
                float speed = (coordinateChildOnScreen.y - 0.8f) / 0.2f;
                _cameraMovement.MoveCameraWhenDragChild(CameraMovement.DirectionMovement.Up, speed);
            }

        }
    }

}
