using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;
using System.Linq;

public class HexPathFinder : MonoBehaviour
{
    private ObiSolver solver;
    private Vector3[] activeHexes = {};
    // Start is called before the first frame update
    void Start()
    {
        solver = gameObject.GetComponent<ObiSolver>();
        InvokeRepeating("GetHexList", 2.0f, 1.0f);
    }

    // Update is called once per frame
    void GetHexList()
    {
        if (solver == null) { return; }
        HashSet<Vector3> uniqueHexes = new HashSet<Vector3>();
        foreach (Vector3 position in solver.positions) {
            Vector3 hexCoord = pixelToHex(position.x, position.z);
            uniqueHexes.Add(hexCoord);
        }
        
        ActivateHexes(uniqueHexes.ToArray());
    }

    void ActivateHexes(Vector3[] newHexes) {
        foreach (Vector3 hex in activeHexes.Except(newHexes)) {
            string hexId = $"[{hex.x},{hex.y},{hex.z}]";
            GameObject tileLight = GameObject.Find(hexId);
            if (tileLight) {
                tileLight.GetComponent<TileController>().Deactivate();
            }
        }

        foreach (Vector3 hex in newHexes.Except(activeHexes)) {
            string hexId = $"[{hex.x},{hex.y},{hex.z}]";
            GameObject tileLight = GameObject.Find(hexId);
            if (tileLight) {
                tileLight.GetComponent<TileController>().Activate();
            }
        }

        activeHexes = newHexes;
    }

    public Vector3 pixelToHex(float pixX, float pixY) {
        float hexX = (Mathf.Sqrt(3)/3f * pixX  -  1f/3f * pixY) / 9f;
        float hexZ = (2f/3f * pixY) / 9f;
        return roundToHex(
            hexX,
            (hexX * -1f) - hexZ,
            hexZ
        );
    }

    private Vector3 roundToHex(float hexX, float hexY, float hexZ) {
        float roundX = Mathf.Round(hexX);
        float roundY = Mathf.Round(hexY);
        float roundZ = Mathf.Round(hexZ);

        float xDiff = Mathf.Abs(roundX - hexX);
        float yDiff = Mathf.Abs(roundY - hexY);
        float zDiff = Mathf.Abs(roundZ - hexZ);

        if (xDiff > yDiff && xDiff > zDiff) {
            roundX = (roundY * -1f) - roundZ;
        } else if (yDiff > zDiff) {
            roundY = (roundX * -1f) - roundZ;
        } else {
            roundZ = (roundX * -1f) - roundY;
        }

        return new Vector3(roundX, roundY, roundZ);
    }
}
