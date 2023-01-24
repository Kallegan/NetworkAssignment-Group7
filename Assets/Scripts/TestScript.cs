using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public void DebugHello()
    {
        Debug.Log("Hello This Is My Cool Index: " + Multiplayer.Instance.Me.Index);
    }
}
