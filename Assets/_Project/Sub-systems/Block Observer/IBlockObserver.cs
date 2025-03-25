using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlockObserver
{
    IBlockObserverSubject GrabBlockObserverSubject();
    void OnBlockSpawn(char character, Color color);
}
