using UnityEngine;

public class BattleScreenMessageController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BattleManager battleManager;

    [Header("Message Colors")]
    [SerializeField]
    private Color playerTurnColor =
        new Color(0.2f, 0.85f, 1f);

    [SerializeField]
    private Color enemyTurnColor =
        new Color(1f, 0.4f, 0.2f);

    [SerializeField]
    private Color physicsColor =
        new Color(1f, 0.8f, 0.25f);

    [SerializeField]
    private Color roundColor =
        Color.white;

    [SerializeField]
    private Color completionColor =
        new Color(0.3f, 1f, 0.6f);

    [Header("Message Settings")]
    [SerializeField] private float typingSpeed = 45f;
    [SerializeField] private float holdTime = 1.5f;
    [SerializeField] private float fadeInTime = 0.2f;
    [SerializeField] private float fadeOutTime = 0.5f;

    private Unit lastUnit;
    private int lastRound = -1;
    private BattlePhase? lastPhase;

    private void Awake()
    {
        if (battleManager == null)
        {
            battleManager =
                FindFirstObjectByType<BattleManager>();
        }
    }

    private void OnEnable()
    {
        BattleManager.OnBattlePhaseChanged += HandlePhaseChanged;
    }

    private void OnDisable()
    {
        BattleManager.OnBattlePhaseChanged -= HandlePhaseChanged;
    }

    private void HandlePhaseChanged()
    {
        if (battleManager == null)
            return;

        Unit currentUnit = battleManager.currentUnit;

        BattlePhase phase = battleManager.CurrentPhase;

        int round = battleManager.CurrentRound;

        bool newRound = round != lastRound;

        // --------------------------------
        // New round notification
        // --------------------------------

        if (newRound)
        {
            ShowMessage(
                $"ROUND {round:00}",
                roundColor,
                10
            );

            lastRound = round;
        }

        switch (phase)
        {
            case BattlePhase.WaitingForInput:

                if (currentUnit == null)
                    break;

                if (currentUnit.IsPlayerControllable)
                {
                    if (newRound || currentUnit != lastUnit)
                    {
                        ShowMessage(
                            $"PLAYER TURN\n{GetUnitName(currentUnit)}",
                            playerTurnColor,
                            8
                        );
                    }
                    else if (lastPhase == BattlePhase.ResolvingAction)
                    {
                        ShowPlayerActionComplete(currentUnit);
                    }
                }

                break;

            case BattlePhase.EnemyTurn:

                if (currentUnit == null)
                    break;

                if (newRound || currentUnit != lastUnit)
                {
                    ShowMessage(
                        $"ENEMY TURN\n{GetUnitName(currentUnit)}",
                        enemyTurnColor,
                        8
                    );
                }
                else if (lastPhase == BattlePhase.ResolvingAction)
                {
                    ShowMessage(
                        "ENEMY ACTION COMPLETE",
                        completionColor,
                        5
                    );
                }

                break;

            case BattlePhase.ResolvingAction:

                ShowMessage(
                    "PHYSICS\nIN PROGRESS",
                    physicsColor,
                    9
                );

                break;
        }

        lastUnit = currentUnit;
        lastPhase = phase;
    }

    private void ShowPlayerActionComplete(Unit unit)
    {
        if (unit is UnitBase unitBase)
        {
            ShowMessage(
                $"ACTION COMPLETE\n{unitBase.CurrentAP} AP REMAINING",
                completionColor,
                5
            );
        }
    }

    private string GetUnitName(Unit unit)
    {
        if (unit is UnitBase unitBase)
            return unitBase.DisplayName;

        Component component = unit as Component;

        if (component != null)
            return component.gameObject.name;

        return "UNKNOWN UNIT";
    }

    private void ShowMessage(
        string message,
        Color color,
        int priority)
    {
        if (ScreenMessageController.Instance == null)
            return;

        ScreenMessageController.Instance.ShowMessage(
            message,
            color,
            typingSpeed,
            holdTime,
            priority,
            fadeInTime,
            fadeOutTime
        );
    }
}
