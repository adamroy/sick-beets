using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

public class ButtonInputView : View
{
    public Signal OnMenuPressed = new Signal();
    public Signal OnBackPressed = new Signal();

    private void Update()
    {
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnMenuPressed.Dispatch();
        }
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            OnBackPressed.Dispatch();
        }
#elif UNITY_ANDROID
        if(Input.GetKeyDown(KeyCode.Menu))
        {
            OnMenuPressed.Dispatch();
        }
        if(Input.GetKeyDown(KeyCode.Escape)) // Esacape is the Android back button
        {
            OnBackPressed.Dispatch();
        }
#endif
    }
}
