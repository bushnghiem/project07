using UnityEngine;

public interface IActionParticipant : ICameraTarget
{
    bool IsActionComplete { get; }

    void BeginActionTracking(ActionContext context);
}
