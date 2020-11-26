using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DoorController : MonoBehaviour
{
    public string tileName;
    public (float, float) hexCoordinates;
    public int doorNumber;

    private bool open = false;
    void Start() {
        gameObject.name = $"{tileName}:|{doorNumber}|";
        (float tileX, float tileY) = hexCoordinates;
        ((float x1, float y1), (float x2, float y2)) = HexHelper.getWallCoordinates(
            tileX,
            tileY,
            doorNumber,
            HexHelper.HEX_SIZE
        );
        HexHelper.Wall wall = new HexHelper.Wall();
        wall.x = ((x1 + x2) / 2f);
        wall.y = ((y1 + y2) / 2f);

        float rotation = HexHelper.WALL_ANGLES[(doorNumber - 1) % 3];
        gameObject.transform.position = new Vector3(wall.x, 4, wall.y);
        gameObject.transform.rotation = Quaternion.Euler(0, rotation, 0);

        BoxCollider collider = gameObject.GetComponent<BoxCollider>();
        collider.size = new Vector3(HexHelper.HEX_SIZE, wall.z, 3);

        GameObject door1 = gameObject.transform.GetChild(0).gameObject;
        GameObject door2 = gameObject.transform.GetChild(1).gameObject;

        float doorOffset = (HexHelper.HEX_SIZE / 4f) + 0.02f;
        door1.transform.localPosition = new Vector3(
            doorOffset,
            0,
            0
        );
        door2.transform.localPosition = new Vector3(
            (doorOffset * -1),
            0,
            0
        );

        door1.transform.localScale = new Vector3(
            (HexHelper.HEX_SIZE / 2f) - 0.02f,
            wall.z,
            0.5f
        );
        door2.transform.localScale = new Vector3(
            (HexHelper.HEX_SIZE / 2f) - 0.02f,
            wall.z,
            0.5f
        );
    }

    void Update() {
        if (open && transform.position.y > -11) {
            float step = 1.75f * Time.deltaTime;
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y - step,
                transform.position.z
            );
        }
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.name == "Ariadne") {
            GameObject ariadne = GameObject.Find("Ariadne");
            if (ariadne.GetComponent<AriadneController>().GetActions().Contains("unlock")) {
                open = true;
            }
        }
    }
}
