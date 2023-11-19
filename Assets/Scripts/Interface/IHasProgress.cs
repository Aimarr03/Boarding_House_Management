using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasProgress
{
    public event System.Action<float> progressOccur;
    public event System.Action<bool> HoldingOccured;
}
