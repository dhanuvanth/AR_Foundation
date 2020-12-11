using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using System;

public class ARTapToSpawn : MonoBehaviour
{
    public GameObject placementIndicator;
    public GameObject spawnerPrefab;

    public ARSessionOrigin arOrigin;
    private Pose placementPose;
    private bool poseIsValid = false;

    // Update is called once per frame
    void Update()
    {
        DetectPose();
        DetectPlane();
        if(poseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            ObjectSpawn();
        }
    }

    private void ObjectSpawn()
    {
        Instantiate(spawnerPrefab, placementPose.position, placementPose.rotation);
    }

    private void DetectPlane()
    {
        if (poseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void DetectPose()
    {
        var screenPoints = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        arOrigin.Raycast(screenPoints, hits, TrackableType.All);

        poseIsValid = hits.Count > 0;
        if (poseIsValid)
        {
            placementPose = hits[0].pose;

            var cameraForward = Camera.main.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z);
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
            
        }
    }
}
