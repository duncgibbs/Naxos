using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyTileController : TileController
{
    private Light sphereLight;
    private bool active = false;
    private Vector3 activeVector;
    private Vector3 inactiveVector;
    public override void Start() {
        sphereLight = gameObject.GetComponent<Light>();
        sphereLight.intensity = 5;
        sphereLight.shadows = LightShadows.Soft;
        sphereLight.enabled = false;
        inactiveVector = transform.position;
        activeVector = new Vector3(inactiveVector.x, inactiveVector.y + 5, inactiveVector.z);
    }

    public override void Update() {
        float step = 1 * Time.deltaTime;
        if (active) {
            transform.position = Vector3.MoveTowards(transform.position, activeVector, step);
        } else {
            transform.position = Vector3.MoveTowards(transform.position, inactiveVector, step);
        }
    }

    public override void Activate() {
        active = true;
        sphereLight.enabled = true;
    }

    public override void Deactivate() {
        active = false;
        sphereLight.enabled = false;
    }
}
