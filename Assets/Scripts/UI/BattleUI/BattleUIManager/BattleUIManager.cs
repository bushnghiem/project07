using UnityEngine;

public class BattleUIManager : MonoBehaviour
{
    public static BattleUIManager Instance;

    [SerializeField]
    private BattleManager battleManager;

    [SerializeField]
    private ItemTargetingController itemTargetingController;

    public static System.Action OnUIScreenChanged;

    public enum UIScreen
    {
        HUD,
        Fleet,
        CombatLog
    }

    [Header("Roots")]
    [SerializeField] private GameObject hudRoot;
    [SerializeField] private GameObject fleetRoot;
    [SerializeField] private GameObject combatLogRoot;

    private UIScreen currentScreen;

    public UIScreen CurrentScreen => currentScreen;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetScreen(UIScreen.HUD);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleFleet();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            ToggleCombatLog();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetScreen(UIScreen.HUD);
        }
    }

    public void ToggleFleet()
    {
        if (currentScreen == UIScreen.Fleet)
        {
            SetScreen(UIScreen.HUD);
        }
        else
        {
            SetScreen(UIScreen.Fleet);
        }
    }

    public void ToggleCombatLog()
    {
        if (currentScreen == UIScreen.CombatLog)
        {
            SetScreen(UIScreen.HUD);
        }
        else
        {
            SetScreen(UIScreen.CombatLog);
        }
    }

    public void SetScreen(UIScreen screen)
    {
        currentScreen = screen;

        fleetRoot.SetActive(screen == UIScreen.Fleet);

        combatLogRoot.SetActive(screen == UIScreen.CombatLog);

        if (screen != UIScreen.HUD)
        {
            CancelPendingPlayerAction();
        }

        OnUIScreenChanged?.Invoke();
    }

    public bool IsOverlayOpen()
    {
        return currentScreen != UIScreen.HUD;
    }

    private void CancelPendingPlayerAction()
    {
        if (battleManager == null)
            return;

        if (battleManager.currentUnit is Player player)
        {
            player.clickAndFlingComponent
                  .CancelPendingAction();
        }

        if (itemTargetingController != null)
        {
            itemTargetingController.CancelTargeting();
        }
    }
}