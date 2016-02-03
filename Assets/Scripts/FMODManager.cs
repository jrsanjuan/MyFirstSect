using UnityEngine;
using System.Collections.Generic;

public class FMODManager : MonoBehaviour 
{
    #region CONSTANTS
    
    public enum Sounds
    {
        NONE,
        Cat,
        Chicken,
        Dog,
        DoorWindowOpen,
        DoorWindowClose,
        Environment,
        GameOver,
        Grandma,
        HandsOn,
        LeaveObject,
        MainMenu,
        Monks,
        OffCandle,
        OnCandle,
        PickObject,
        PlayerStep,
        Radio,
        Request,        
        RequestFail,
        RequestSuccess,
        Teletubies,
        WashingMachine,
    }
    
    public enum Parameter
    {
        NONE,
        Time,
        Monks,
    }
    
    
    #endregion
    

    #region FIELDS
    
    private static string _eventsPath = "event:/";
    
    private Dictionary<string, FMOD.Studio.EventInstance> _runningSounds = null;
    
    #endregion


    #region SINGLETON
    
    private static FMODManager _instance = null;
    
    public static FMODManager SINGLETON { get { return _instance; } }
    
    #endregion


    #region UNITY_METHODS
    
    /// <summary>
    /// Awake del componente
    /// </summary>
    private void Awake()
    {
        _instance = this;
        
        _runningSounds = new Dictionary<string, FMOD.Studio.EventInstance>();
    }
    
    #endregion
    
    
    #region CUSTOM_METHODS
    
    public void PlayOneShot(Sounds soundEventName)
    {
        if(soundEventName!=Sounds.NONE)
            FMODUnity.RuntimeManager.PlayOneShot(_eventsPath + soundEventName.ToString());
    }
    
    public void PlaySound(Sounds soundEventName, Parameter parameterName = Parameter.NONE, float parameterValue = 0.0f)
    {
        if(soundEventName == Sounds.NONE)
            return;
        
        if(_runningSounds.ContainsKey(soundEventName.ToString()))
        {
            Debug.LogWarning("[FMODManager::PlaySound] El sonido '"+soundEventName.ToString()+"' ya se esta reproduciendo");
            return;
        }
        
        FMOD.Studio.EventInstance fmodEvent = FMODUnity.RuntimeManager.CreateInstance(_eventsPath+soundEventName.ToString());
        fmodEvent.start();
        
        _runningSounds.Add(soundEventName.ToString(), fmodEvent);
        
        if(parameterName!=Parameter.NONE)
        {
            FMOD.Studio.ParameterInstance parameter;
            fmodEvent.getParameter(parameterName.ToString(), out parameter);
            parameter.setValue(parameterValue);
        }
    }
    
    public void ChangeParameterValue(Sounds soundEventName, Parameter parameterName, float parameterValue)
    {
        if(soundEventName == Sounds.NONE)
            return;
        
        if(!_runningSounds.ContainsKey(soundEventName.ToString()))
        {
            Debug.LogWarning("[FMODManager::ChangeParameterValue] El sonido '"+soundEventName.ToString()+"' no se esta reproduciendo reproduciendo");
            return;
        }
        
        FMOD.Studio.EventInstance fmodEvent = _runningSounds[soundEventName.ToString()];
        FMOD.Studio.ParameterInstance parameter;
        fmodEvent.getParameter(parameterName.ToString(), out parameter);
        parameter.setValue(parameterValue);
    }
    
    public void StopSound(Sounds soundEventName)
    {
        if(soundEventName == Sounds.NONE)
            return;
        
        if(!_runningSounds.ContainsKey(soundEventName.ToString()))
        {
            Debug.LogWarning("[FMODManager::StopSound] El sonido '"+soundEventName.ToString()+"' no se esta reproduciendo reproduciendo");
            return;
        }
        
        FMOD.Studio.EventInstance fmodEvent = _runningSounds[soundEventName.ToString()];
        fmodEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        
        _runningSounds.Remove(soundEventName.ToString());
    }
    
    public void StopAllSounds()
    {
        foreach(var pair in _runningSounds)
        {
            pair.Value.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);            
        }
        
        _runningSounds.Clear();
    }
    
    #endregion

}
