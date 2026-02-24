using UnityEngine;
using UnityEngine.UI;

public class PlayerActions : MonoBehaviour
{

    public Button moveButton;
    public Button attackButton;
    public Button itemButton;
    public Button endTurnButton;
    public GameObject playerActionPanel;
    public GameObject barPanel;

    private Unit currentUnit;

    private TurnAction moveAction = new MoveAction();
    private TurnAction shootAction = new ShootAction();
    private TurnAction itemAction = new ItemAction();

    private void OnEnable()
    {
        TurnEvent.OnUnitTurnStart += HandleUnitTurnStart;
        TurnEvent.OnUnitTurnEnd += HandleUnitTurnEnd;
    }

    private void OnDisable()
    {
        TurnEvent.OnUnitTurnStart -= HandleUnitTurnStart;
        TurnEvent.OnUnitTurnEnd -= HandleUnitTurnEnd;
    }

    private void Awake()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Debug.Log("Added button clicks");
        moveButton.onClick.AddListener(() => Execute(moveAction));
        attackButton.onClick.AddListener(() => Execute(shootAction));
        itemButton.onClick.AddListener(() => Execute(itemAction));
        endTurnButton.onClick.AddListener(() => currentUnit.EndTurn());
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleUnitTurnStart(Unit unit)
    {
        if (unit.IsPlayerControllable)
        {
            //Debug.Log("Is Player");
            ShowActions(unit);
        }
    }

    public void HandleUnitTurnEnd(Unit unit)
    {
        if (unit.IsPlayerControllable)
        {
            //Debug.Log("Is Player");
            Hide();
        }
    }

    public void ShowActions(Unit unit)
    {
        //Debug.Log("Show UI");
        currentUnit = unit;
        playerActionPanel.SetActive(true);
        barPanel.SetActive(true);
    }

    public void Hide()
    {
        //Debug.Log("Hide UI");
        playerActionPanel.SetActive(false);
        barPanel.SetActive(false);
    }

    private void Execute(TurnAction action)
    {
        //Debug.Log(action);
        action.Execute(currentUnit);
    }
}
