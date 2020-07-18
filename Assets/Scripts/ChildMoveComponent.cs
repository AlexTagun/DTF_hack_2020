using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildMoveComponent : MonoBehaviour {
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D _rigidbody;
    private void Update() {
        var curPos = new Vector2(transform.position.x, transform.position.y);
        curPos.x += speed + Time.deltaTime;
        _rigidbody.MovePosition(curPos);
    }
}