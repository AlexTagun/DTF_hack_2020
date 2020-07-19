using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildMoveComponent : MonoBehaviour {
    private void Awake() { }

    public void moveRight() { doMove(true); }
    public void moveLeft() { doMove(false); }
    public void useStairs() { startMovingOverStairs(); }

    public bool isPossibleToUseStairs => !usingStairs && stairsThatMayBeUsed;
    public bool isUsingStairs => (float.MinValue != _stairsMovingProgress);
    public Stairs stairsThatMayBeUsed => _stairsThatMayBeUsed;
    public Stairs usingStairs => _usingStairs;

    private void doMove(bool inMoveRight) {
        float theSpeedPerFrame = speed * Time.fixedDeltaTime;

        if (!_usingStairs) {
            float theMovingSign = inMoveRight ? 1f : -1f;
            float theMovingSpeed = theSpeedPerFrame * theMovingSign;

            Vector2 theCurrentPosition = transform.position;
            theCurrentPosition.x += theMovingSpeed;
            _rigidbody.MovePosition(theCurrentPosition);
        } else {
            float theTotalStairsDistance = _usingStairs.stairsVectorToPass.magnitude;
            float theStairsProgressChangeValue = theSpeedPerFrame / theTotalStairsDistance;
            float theStairsProgressChangeSign = (inMoveRight == _usingStairs.isRightOriented) ? 1f : -1f;

            float theStairsProgressChange = theStairsProgressChangeValue * theStairsProgressChangeSign;
            _stairsMovingProgress = Mathf.Clamp01(_stairsMovingProgress + theStairsProgressChange);

            Vector2 theStairsPosition = _usingStairs.getStairsPointByProgress(_stairsMovingProgress);
            _rigidbody.MovePosition(theStairsPosition);

            if (0f == _stairsMovingProgress || 1f == _stairsMovingProgress)
                stopMovingOverStairs();
        }
    }

    private void stopMovingOverStairs() {
        _usingStairs = null;
        _stairsMovingProgress = float.MinValue;

        setCollisionsEnable(true);
    }

    private void startMovingOverStairs() {
        if (isPossibleToUseStairs) {
            _usingStairs = _stairsThatMayBeUsed;
            _stairsMovingProgress = 0f;

            setCollisionsEnable(false);

            Vector2 theStairsPosition = _usingStairs.getStairsPointByProgress(_stairsMovingProgress);
            _rigidbody.MovePosition(theStairsPosition);
        }
    }

    void setCollisionsEnable(bool inIsEnabled) {
        _collider.enabled = inIsEnabled;
    }

    private void OnTriggerEnter2D(Collider2D inOther) {
        var theStairs = inOther.GetComponent<Stairs>();
        if (theStairs)
            _stairsThatMayBeUsed = theStairs;
    }

    private void OnTriggerExit2D(Collider2D inOther) {
        var theStairs = inOther.GetComponent<Stairs>();
        if (_stairsThatMayBeUsed == theStairs)
            _stairsThatMayBeUsed = null;
    }

    [SerializeField] private float speed = 1f;
    [SerializeField] private Rigidbody2D _rigidbody = null;
    [SerializeField] private Collider2D _collider = null;

    Stairs _stairsThatMayBeUsed = null;
    Stairs _usingStairs = null;
    float _stairsMovingProgress = float.MinValue;
}