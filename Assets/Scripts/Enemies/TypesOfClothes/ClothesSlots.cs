using UnityEngine;

public enum BodySlot {Head, Torso, Legs, Accessories} ;

[CreateAssetMenu(menuName = "ScriptableObjects/ClothesSlots")]
public class ClothesSlots : ScriptableObject
{
    public BodySlot slot; //slot to where the object is going
    public string name; // the name of the object
    public GameObject model; //the model itself
}
