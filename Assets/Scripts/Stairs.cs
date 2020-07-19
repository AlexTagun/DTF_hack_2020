using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Stairs : MonoBehaviour {
    [SerializeField] [Range(0, 1)]
    private float _chance;
    [SerializeField] private Transform _endPoint;

    public bool isForMovingUp => (stairsVectorToPass.y > 0f);
    public bool isRightOriented => (stairsVectorToPass.x > 0f);

    public Vector2 stairsVectorToPass => _endPoint.transform.position - transform.position;

    public Vector3 endPointPosition => _endPoint.transform.position;

    public Vector2 getStairsPointByProgress(float inProgress) {
        Vector2 thePosition2D = transform.position;
        return thePosition2D + stairsVectorToPass * inProgress;
    }

    /*
    private bool _isRunning = false;

    public void useStairsForChild(ChildMoveComponent inChildMovement) {
        StartCoroutine(DoTransportAnimation(other.gameObject));
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(_isRunning) return;
        if(other.GetComponent<DragDropChild>().IsDraging) return;
        if (other.gameObject.tag == "Child") {
            _childsThatMayUseMe.Add(other.GetComponent<ChildMoveComponent>());

        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        _childsThatMayUseMe.Remove(other.GetComponent<ChildMoveComponent>());
    }

    private IEnumerator DoTransportAnimation(GameObject kid) {
        var endPointCollider = _endPoint.gameObject.GetComponent<BoxCollider2D>();
        _isRunning = true;
        endPointCollider.enabled = false;
        yield return kid.transform.DOMove(_endPoint.position, 2).WaitForCompletion();
        yield return new WaitForSeconds(5);
        endPointCollider.enabled = true;
        _isRunning = false;
    }


    private List<ChildMoveComponent> _childsThatMayUseMe = new List<ChildMoveComponent>();

    */
}
