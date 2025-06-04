using System;
using UnityEngine;
[CreateAssetMenu(fileName = "ActionContainerBool", menuName = "Containers/ActionContainer/ActionContainerBool")]
public class ActionContainerBool : ScriptableObject
{
    public Action<bool> action;

    public void FireAction(bool argument)
    {
        action?.Invoke(argument);
    }

    public ActionContainerBool Clone()
    {
        return CreateInstance<ActionContainerBool>();
    }
}
