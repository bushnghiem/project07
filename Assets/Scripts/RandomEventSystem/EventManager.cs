using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;
    public ShipHolder shipHolder;

    private void Awake()
    {
        Instance = this;
    }

    public void ExecuteOption(EventOption option)
    {
        foreach (var outcome in option.outcomes)
        {
            ApplyOutcome(outcome);
        }
    }

    void ApplyOutcome(EventOutcome outcome)
    {
        var run = RunManager.Instance.CurrentRun;

        switch (outcome.type)
        {
            case OutcomeType.GainCurrency:
                //run.currency += outcome.value;
                break;

            case OutcomeType.LoseCurrency:
                //run.currency -= outcome.value;
                break;

            case OutcomeType.HealPlayer:
                foreach (var player in shipHolder.allPlayers)
                {
                    //player.Heal(outcome.value);
                }
                break;

            case OutcomeType.DamagePlayer:
                foreach (var player in shipHolder.allPlayers)
                {
                    //player.TakeDamage(outcome.value);
                }
                break;

            case OutcomeType.StartCombat:
                run.currentEncounter = outcome.encounter;
                SceneManager.LoadScene("SpawnTestScene");
                break;

            case OutcomeType.GiveItem:
                // your inventory system here
                Debug.Log("Item given: " + outcome.item);
                break;

            case OutcomeType.Nothing:
                break;
        }
    }
}
