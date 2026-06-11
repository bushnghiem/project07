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

    [SerializeField]
    private BattleManager battleManager;

    private Unit currentUnit;

    private TurnAction moveAction = new MoveAction();
    private TurnAction shootAction = new ShootAction();

    [SerializeField]
    private ItemTargetingController targetingController;

    private TurnAction itemAction;

    private void OnEnable()
    {
        TurnEvent.OnUnitTurnStart += HandleTurnChanged;
        TurnEvent.OnUnitContinueTurn += HandleTurnChanged;
        TurnEvent.OnUnitTurnEnd += HandleTurnChanged;
        TurnEvent.OnUnitActionResolved += HandleTurnChanged;

        BattleManager.OnBattlePhaseChanged += RefreshVisibility;

        BattleUIManager.OnUIScreenChanged += RefreshVisibility;
    }

    private void OnDisable()
    {
        TurnEvent.OnUnitTurnStart -= HandleTurnChanged;
        TurnEvent.OnUnitContinueTurn -= HandleTurnChanged;
        TurnEvent.OnUnitTurnEnd -= HandleTurnChanged;
        TurnEvent.OnUnitActionResolved -= HandleTurnChanged;

        BattleManager.OnBattlePhaseChanged += RefreshVisibility;

        BattleUIManager.OnUIScreenChanged -= RefreshVisibility;
    }

    private void Start()
    {
        if (battleManager == null)
        {
            battleManager =
                FindFirstObjectByType<BattleManager>();
        }

        if (targetingController == null)
        {
            targetingController =
                FindFirstObjectByType<ItemTargetingController>();
        }

        if (targetingController == null)
        {
            Debug.LogError(
                "No ItemTargetingController found in scene");
            return;
        }

        itemAction =
            new ItemAction(targetingController);

        moveButton.onClick.AddListener(
            () => Execute(moveAction));

        attackButton.onClick.AddListener(
            () => Execute(shootAction));

        itemButton.onClick.AddListener(
            () => Execute(itemAction));

        endTurnButton.onClick.AddListener(
            () => currentUnit?.EndTurn());

        RefreshVisibility();
    }

    private void HandleTurnChanged(Unit unit)
    {
        RefreshVisibility();
    }

    private void RefreshVisibility()
    {

        if (battleManager == null)
            return;

        Debug.Log(battleManager.CurrentPhase);

        bool overlayOpen =
            BattleUIManager.Instance != null &&
            BattleUIManager.Instance.IsOverlayOpen();

        bool shouldShow =
            battleManager.CurrentPhase ==
            BattlePhase.WaitingForInput
            &&
            !overlayOpen;

        playerActionPanel.SetActive(
            shouldShow
        );

        barPanel.SetActive(
            shouldShow
        );

        if (shouldShow)
        {
            currentUnit =
                battleManager.currentUnit;
        }
    }

    private void Execute(TurnAction action)
    {
        if (currentUnit == null)
            return;

        if (BattleUIManager.Instance != null &&
            BattleUIManager.Instance.IsOverlayOpen())
        {
            return;
        }

        action.Execute(currentUnit);
    }
}