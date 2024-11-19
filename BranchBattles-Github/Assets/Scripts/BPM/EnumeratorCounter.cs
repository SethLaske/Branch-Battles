using System;

public class CoroutineCounter{
    
    private int totalCoroutines = 0;
    
    private int currentCoroutinesFinished = 0;
    
    private Action onFinished = null;

    public CoroutineCounter(int argTotalCoroutines, Action argOnFinished)
    {
        totalCoroutines = argTotalCoroutines;
        currentCoroutinesFinished = 0;
        onFinished = argOnFinished;
    }

    public void OnCoroutineFinished()
    {
        currentCoroutinesFinished++;
        if (currentCoroutinesFinished == totalCoroutines)
        {
            onFinished?.Invoke();
        }
    }
}
