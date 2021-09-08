using System.Collections;
using UnityEngine;
using Cameo.Static;
using Cameo.Singleton;
using System;
using UnityEngine.Events;
using Cameo.Utils;

public class CustomAction : MonoBehaviour, I_CustomAction
{
    private Action _onBeginAction = delegate { };
    private Action _onEndAction = delegate { };

    public string actionName;
    public bool executeOnStart = false;
    public bool executeExternally = false;
    [Range(0, 30)]
    public float delayBeforeExecution;


    public Action OnBeginAction { private get => _onBeginAction; set => _onBeginAction += value; }
    public Action OnEndAction { private get => _onEndAction; set => _onEndAction += value; }

    private void Start()
    {
        if (!executeOnStart)
            return;
        StartCoroutine(onExecute());
    }


    public void ExecuteAction()
    {
        if (!executeExternally)
            return;
        StartCoroutine(onExecute());
    }

    private IEnumerator onExecute()
    {
        yield return new WaitForSecondsRealtime(delayBeforeExecution);
        OnBeginAction.Invoke();
        OnAction();
        OnEndAction.Invoke();
    }

    /// <summary>
    /// Override this method and put the code for the target custom action
    /// </summary>
    public virtual void OnAction() { }
}
