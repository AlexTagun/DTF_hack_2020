using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildMoveComponent : MonoBehaviour {
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D _rigidbody;

    private MoveState _state;

    private void Awake() {
        MoveRight();
    }

    private enum MoveState {
        Right,
        Left,
        Idle
    }

    public void Stop() {
        _state = MoveState.Idle;
    }

    public void MoveLeft() {
        _state = MoveState.Left;
        
    }

    public void MoveRight() {
        _state = MoveState.Right;
    }
    private void Update() {
        if(_state == MoveState.Idle) return;
        
        var curPos = new Vector2(transform.position.x, transform.position.y);
        curPos.x += speed * Time.deltaTime * (_state == MoveState.Left ? -1 : 1);
        _rigidbody.MovePosition(curPos);
        var curVelocity = _rigidbody.velocity;
        curVelocity.x = speed * (_state == MoveState.Left ? -1 : 1);
        _rigidbody.velocity = curVelocity;
    }
}