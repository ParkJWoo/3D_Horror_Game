using UnityEngine;


public class UICondition : MonoBehaviour
{
    public Condition stamina;

    void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }
}
