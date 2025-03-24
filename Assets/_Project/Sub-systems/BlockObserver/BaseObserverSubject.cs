using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseObserverSubject<T> : MonoBehaviour
{
    protected List<T> observers = new List<T>();
}
