using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HexHelper {
    [System.Serializable]
    public class Tile
    {
        public int[] coordinates;
        public string type;
        public int movement;
        public string[] actions;
        public int[] walls;
        public int[] doors;
    }

    [System.Serializable]
    public class Wall
    {
        public float x;
        public float y;
        public float z = 11;
    }

    [System.Serializable]
    public class Puzzle
    {
        public Tile[] tiles;
    }

    public static int HEX_SIZE = 9;
    public static int[] WALL_ANGLES = {-30,90,30};

    private static int HEX_X_OFFSET = 0;
    private static int HEX_Y_OFFSET = 0;

    public static (float, float) hexToPixel(int hexX, int hexY, int hexZ)
    {
        float pixelX = (Mathf.Sqrt(3) * HEX_SIZE * ((hexZ / 2f) + hexX)) + HEX_X_OFFSET;
        float pixelY = (1.5f * HEX_SIZE * hexZ) + HEX_Y_OFFSET;

        return (pixelX, pixelY);
    }

    public static ((float, float), (float, float)) getWallCoordinates(float hexX, float hexY, int wallNumber, int hexSize)
    {
        float x1 = hexX + (hexSize * Mathf.Sin(1.0472f * ((wallNumber + 2) * -1f)));
        float y1 = hexY + (hexSize * Mathf.Cos(1.0472f * ((wallNumber + 2) * -1f)));
        float x2 = hexX + (hexSize * Mathf.Sin(1.0472f * ((wallNumber + 3) * -1f)));
        float y2 = hexY + (hexSize * Mathf.Cos(1.0472f * ((wallNumber + 3) * -1f)));
        return ((x1, y1), (x2, y2));
    }
}
