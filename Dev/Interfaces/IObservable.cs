using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObservable
{
    public void addObserver(IObserver obs);
    public void removeObserver(IObserver obs);
    public void notifyObservers();
}
