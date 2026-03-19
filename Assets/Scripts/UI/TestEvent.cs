using UnityEngine;

public class TestEvent : MonoBehaviour
{
    public EventUI eventUI;
    public EventData testEvent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        eventUI.ShowEvent(testEvent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
