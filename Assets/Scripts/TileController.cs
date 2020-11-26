using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileController : MonoBehaviour
{
    private Light sphereLight;
    private bool active;
    private Vector3 activeVector;
    private Vector3 inactiveVector;
    public abstract void Start();

    public abstract void Update();

    public abstract void Activate();

    public abstract void Deactivate();
}
