using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    public bool Enabled { get; set; }
    public void EnterState();
    public void Hovering();
    public void OnClick();
    public void ExitState();
}
