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
    private bool _isMouseDown = false;
    private Vector2 CurPosition => new Vector2(transform.position.x, transform.position.y);
    private Vector2 MousePosition => GetMousePosition();
    private Vector2 _mouseDelta;

    void Start()
    {
        _camera = Camera.main;
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
        if (!_isMouseDown) return;
        //_rigidbody.MovePosition(MousePosition + _mouseDelta);
    }

}
