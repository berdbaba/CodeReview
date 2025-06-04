using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionContainer", menuName = "Containers/ActionContainer/ActionContainer")]
public class ActionContainer : ScriptableObject
{
    public Action action;

    public void FireAction()
    {
        action?.Invoke();
    }

    public ActionContainer Clone()
    {
        return CreateInstance<ActionContainer>();
    }
}