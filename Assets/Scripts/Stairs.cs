using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Stairs : MonoBehaviour {
    [SerializeField] [Range(0, 1)]
    private float _chance;
    [SerializeField] private Transform _endPoint;

    private bool _isRunning = false;

    private void OnTriggerEnter2D(Collider2D other) {
        if(_isRunning) return;
        if(other.GetComponent<DragDropChild>().IsDraging) return;
        var randValue = Random.value;
        if(_chance <= randValue) return;
        if (other.gameObject.tag == "Child") {
            StartCoroutine(DoTransportAnimation(other.gameObject));
        }
    }

    private IEnumerator DoTransportAnimation(GameObject kid) {
        var endPointCollider = _endPoint.gameObject.GetComponent<BoxCollider2D>();
        _isRunning = true;
        endPointCollider.enabled = false;
        kid.transform.position = _endPoint.position;
        yield return new WaitForSeconds(5);
        endPointCollider.enabled = true;
        _isRunning = false;
    }

}