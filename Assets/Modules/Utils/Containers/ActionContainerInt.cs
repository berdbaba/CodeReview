using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ActionContainerInt", menuName = "Containers/ActionContainer/ActionContainerInt")]
public class ActionContainerInt : ScriptableObject
{
    public Action<int> action;

    public void FireAction(int argument)
    {
        action?.Invoke(argument);
    }
}