using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;
using System;

public class PickupTileController : TileController
{
    public HexHelper.Tile tile;
    private Light sphereLight;
    private bool active = false;
    private Vector3 activeVector;
    private Vector3 inactiveVector;
    public override void Start() {
        sphereLight = gameObject.GetComponent<Light>();
        sphereLight.intensity = 5;
        sphereLight.shadows = LightShadows.Soft;
        sphereLight.color = Color.yellow;
        inactiveVector = transform.position;
        activeVector = new Vector3(inactiveVector.x, inactiveVector.y + 5, inactiveVector.z);

        if (tile != null) {
            if (tile.actions.Length != 0) {
                BuildActionPillar();
            }
        }
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
        sphereLight.color = Color.blue;
        GameObject ariadneString = GameObject.Find("String");
        if (ariadneString) {
            ariadneString.GetComponent<StringController>().IncreaseLength(60);
        }
        if (tile.actions.Length != 0) {
            GameObject ariadne = GameObject.Find("Ariadne");
            ariadne.GetComponent<AriadneController>().AddActions(tile.actions);
        }
    }

    public override void Deactivate() {
        active = false;
        sphereLight.color = Color.yellow;
        GameObject ariadneString = GameObject.Find("String");
        if (ariadneString) {
            ariadneString.GetComponent<StringController>().DecreaseLength(60);
        }
        if (tile.actions.Length != 0) {
            GameObject ariadne = GameObject.Find("Ariadne");
            ariadne.GetComponent<AriadneController>().RemoveActions(tile.actions);
        }
    }

    private void BuildActionPillar() {
        inactiveVector.y = 2.5f;
        transform.position = inactiveVector;

        string tileName = $"[{tile.coordinates[0]},{tile.coordinates[1]},{tile.coordinates[2]}]";
        string actionName = $"[{String.Join(",", tile.actions)}]";
        GameObject actionPillar = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        actionPillar.transform.position = new Vector3(
            transform.position.x,
            0.5f,
            transform.position.z
        );
        actionPillar.transform.localScale = new Vector3(0.5f, 2, 0.5f);
        actionPillar.name = $"{tileName}:{actionName}";
        
        ObiCollider collider = actionPillar.AddComponent(typeof(ObiCollider)) as ObiCollider;
        collider.Thickness = 0.2f;
        collider.CollisionMaterial = new ObiCollisionMaterial();
    }
}
