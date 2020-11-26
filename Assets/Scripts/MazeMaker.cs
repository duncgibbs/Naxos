using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Obi;

public class MazeMaker : MonoBehaviour
{
    public TextAsset jsonFile;
    public GameObject naxosLightPrefab = null;
    public GameObject wallPrefab = null;
    public GameObject doorPrefab = null;
 
    void Start()
    {
        HexHelper.Puzzle puzzle = JsonUtility.FromJson<HexHelper.Puzzle>(jsonFile.text);
 
        foreach (HexHelper.Tile tile in puzzle.tiles)
        {
            string tileName = $"[{tile.coordinates[0]},{tile.coordinates[1]},{tile.coordinates[2]}]";
            (float tileX, float tileY) = HexHelper.hexToPixel(tile.coordinates[0], tile.coordinates[1], tile.coordinates[2]);
            
            GenerateTiles(tile, tileX, tileY, tileName);

            GenerateWalls(tile.walls, tileX, tileY, tileName);

            if (tile.doors != null) {
                GenerateDoors(tile.doors, tileX, tileY, tileName);
            }
        }
    }

    void GenerateTiles(HexHelper.Tile tile, float tileX, float tileY, string tileName) {
        if (
            tile.coordinates[0] != 0 ||
            tile.coordinates[1] != 0 ||
            tile.coordinates[0] != 0
        ) {
            GameObject sphere = Instantiate(naxosLightPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            sphere.name = tileName;
            sphere.transform.position = new Vector3(tileX, 0, tileY);
            if (tile.type == "pickup") {
                PickupTileController controller = sphere.AddComponent(typeof(PickupTileController)) as PickupTileController;
                controller.tile = tile;
            } else if (tile.type == "end") {
                EndTileController controller = sphere.AddComponent(typeof(EndTileController)) as EndTileController;
                // controller.tile = tile;
            } else {
                EmptyTileController controller = sphere.AddComponent(typeof(EmptyTileController)) as EmptyTileController;
                // controller.tile = tile;
            }
        }
    }

    void GenerateWalls(int[] walls, float tileX, float tileY, string tileName) {
        foreach (int wallNumber in walls) {
            ((float x1, float y1), (float x2, float y2)) = HexHelper.getWallCoordinates(
                tileX,
                tileY,
                wallNumber,
                HexHelper.HEX_SIZE
            );
            HexHelper.Wall wall = new HexHelper.Wall();
            wall.x = ((x1 + x2) / 2f);
            wall.y = ((y1 + y2) / 2f);
            GameObject wallObject = Instantiate(wallPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            wallObject.name = $"{tileName}:{wallNumber}";
            wallObject.transform.position = new Vector3(wall.x, 4, wall.y);
            wallObject.transform.localScale = new Vector3(
                HexHelper.HEX_SIZE,
                wall.z,
                0.5f
            );
            float rotation = HexHelper.WALL_ANGLES[(wallNumber - 1) % 3];
            wallObject.transform.rotation = Quaternion.Euler(0, rotation, 0);
        }
    }

    void GenerateDoors(int[] doors, float tileX, float tileY, string tileName) {
        foreach (int doorNumber in doors) {
            GameObject door = Instantiate(doorPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            DoorController controller = door.GetComponent<DoorController>();
            controller.tileName = tileName;
            controller.hexCoordinates = (tileX, tileY);
            controller.doorNumber = doorNumber;
        }
    }
}
