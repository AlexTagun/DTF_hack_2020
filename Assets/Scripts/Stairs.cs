using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour {
    [SerializeField] private Transform _endPoint;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Child") {
            StartCoroutine(DoTransportAnimation(other.gameObject));
        }
    }

    private IEnumerator DoTransportAnimation(GameObject kid) {
        kid.transform.position = _endPoint.position;
        yield return null;
    }

}