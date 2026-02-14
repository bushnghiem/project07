using UnityEngine;
using UnityEngine.UI;

public class PlayerActions : MonoBehaviour
{

    public Button moveButton;
    public Button attackButton;
    public Button itemButton;
    public Button endTurnButton;
    public GameObject panel;

    private Unit currentUnit;

    private TurnAction moveAction = new MoveAction();
    private TurnAction shootAction = new ShootAction();
    private TurnAction itemAction = new ItemAction();

    private void OnEnable()
    {
        TurnEvent.OnPlayerTurnStart += HandlePlayerTurnStart;
        TurnEvent.OnPlayerTurnEnd += HandlePlayerTurnEnd;
    }

    private void OnDisable()
    {
        TurnEvent.OnPlayerTurnStart -= HandlePlayerTurnStart;
        TurnEvent.OnPlayerTurnEnd -= HandlePlayerTurnEnd;
    }

    private void Awake()
    {
        moveButton.onClick.AddListener(() => Execute(moveAction));
        attackButton.onClick.AddListener(() => Execute(shootAction));
        itemButton.onClick.AddListener(() => Execute(itemAction));
        endTurnButton.onClick.AddListener(() => TurnEvent.OnPlayerTurnEnd?.Invoke(currentUnit));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandlePlayerTurnStart(Unit unit)
    {
        Debug.Log("Show UI");
        ShowActions(unit);
    }

    public void HandlePlayerTurnEnd(Unit unit)
    {
        Debug.Log("Hide UI");
        currentUnit = unit;
        Hide();
    }

    public void ShowActions(Unit unit)
    {
        currentUnit = unit;
        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }

    private void Execute(TurnAction action)
    {
        action.Execute(currentUnit);
    }
}
