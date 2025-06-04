using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ActionContainerFloat", menuName = "Containers/ActionContainer/ActionContainerFloat")]
public class ActionContainerFloat : ScriptableObject
{
    public Action<float> action;

    public void FireAction(float argument)
    {
        action?.Invoke(argument);
    }
}