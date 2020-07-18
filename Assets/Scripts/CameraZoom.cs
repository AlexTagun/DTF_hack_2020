using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private Camera _camera = null;
    private float targetZoom;
    private float zoomFactor = 3f;
    [SerializeField] private float zoomLerpSpeed = 10f;
    [SerializeField] private float maxZoom = 8f;
    [SerializeField] private float minZoom = 4.5f;

    void Start()
    {
        _camera = Camera.main;
        targetZoom = _camera.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        float scroolData = Input.GetAxis("Mouse ScrollWheel");
        targetZoom -= scroolData * zoomFactor;
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, targetZoom, Time.deltaTime * zoomLerpSpeed);
    }
}
