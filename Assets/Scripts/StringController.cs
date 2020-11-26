using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;

public class StringController : MonoBehaviour {

    ObiRopeCursor cursor;
    ObiRope rope;
    private int length = 60;

    void Start () {
        cursor = GetComponentInChildren<ObiRopeCursor>();
        rope = cursor.GetComponent<ObiRope>();
        cursor.ChangeLength(length);
    }

    public void IncreaseLength(int addLength) {
        // cursor.cursorMu = 1f;
        Debug.Log($"Increasing Length from {length} to {length + addLength}");
        length += addLength;
        cursor.ChangeLength(length);
    }

    public void DecreaseLength(int subLength) {
        Debug.Log($"Decreasing Length from {length} to {length - subLength}");
        length -= subLength;
        cursor.ChangeLength(length);
    }
}