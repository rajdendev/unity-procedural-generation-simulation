using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField] private float zoomSensitivity = 1;
    [SerializeField, Min(1)] private float minCamSize = 1;
    [SerializeField, Min(1)] private float maxCamSize = 70;

    private Camera cam;
    private Vector3 dragOrigin;

    private void Start() {
        cam = Camera.main;
    }

    private void Update() {
        if (Input.mouseScrollDelta.y != 0) {
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - Input.mouseScrollDelta.y * zoomSensitivity, minCamSize, maxCamSize);
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(2)) {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0) || Input.GetMouseButton(2)) {
            cam.transform.position += dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
