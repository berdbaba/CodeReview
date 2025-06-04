using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ActionContainerTransform", menuName = "Containers/ActionContainer/ActionContainerTransform")]
public class ActionContainerTransform : ScriptableObject
{
    public Action<Transform> action;

    public void FireAction(Transform argument)
    {
        action?.Invoke(argument);
    }
}