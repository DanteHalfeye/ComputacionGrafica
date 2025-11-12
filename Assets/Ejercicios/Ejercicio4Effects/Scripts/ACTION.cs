using UnityEngine;
using UnityEngine.Events;

public class ACTION : MonoBehaviour
{
    public UnityEvent _OnAction;

    public void DoAction()
    {
        print("ACTION");
        _OnAction?.Invoke();
    }
}
