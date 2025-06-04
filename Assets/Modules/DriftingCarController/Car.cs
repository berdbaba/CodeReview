using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField]
    private CarStats carStats;
    public CarStats CarStats => carStats;

}
