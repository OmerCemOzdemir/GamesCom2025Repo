using UnityEngine;

public class ElevatorControl : MonoBehaviour
{
    [SerializeField] private GameObject elevator;

    [Header("Only change in parent prefab")]
    [SerializeField] private Sprite toggleOn;
    [SerializeField] private Sprite toggleOff;
    private bool toggleElevator = true;
    private bool toggleSprite = true;

    private void OnEnable()
    {
        Elevator.onElevatorDone += CheckElevatorState;
    }

    private void OnDisable()
    {
        Elevator.onElevatorDone -= CheckElevatorState;
    }

    public void ToggleElevator()
    {
        if (toggleElevator)
        {
            elevator.GetComponent<Elevator>().ToggleElevatorWithOutPlayer();
            ToggleSprite();
            toggleElevator = false;
        }

    }

    private void CheckElevatorState()
    {
        toggleElevator = true;
    }

    private void ToggleSprite()
    {
        if (toggleSprite)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = toggleOn;
            toggleSprite = false;
        }
        else
        {
            GetComponentInChildren<SpriteRenderer>().sprite = toggleOff;
            toggleSprite = true;

        }

    }


}
