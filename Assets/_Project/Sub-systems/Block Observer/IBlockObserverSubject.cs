using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlockObserverSubject
{
    // 3 methods may be pushing it if trying to follow the Interface Segregation Principle, but in this case we ALWAYS want 
    // to add, remove, and notify observers, so we can use this interface as is as they always need to be implemented together.
    void AddObserver(IBlockObserver observer);
    void RemoveObserver(IBlockObserver observer);
    void NotifyObservers(char character, Color color);
}