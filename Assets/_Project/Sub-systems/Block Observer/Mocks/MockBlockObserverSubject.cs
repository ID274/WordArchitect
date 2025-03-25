using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
public class MockBlockObserverSubject : BaseObserverSubject<IBlockObserver>, IBlockObserverSubject
{
    // this is a mock implementation of the BaseObserverSubject for the sake of Unit Testing the observer behaviour
    public void AddObserver(IBlockObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IBlockObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers(char character, Color color)
    {
        foreach (IBlockObserver observer in observers)
        {
            observer.OnBlockSpawn(character, color);
        }
    }
}

#endif
