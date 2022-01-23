using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        EventManager.Instance.AddHandler<HotPotSpin>(_onHotPotSpin);
        EventManager.Instance.AddHandler<ChopsticksClash>(_onChopsticksAttack);
        EventManager.Instance.AddHandler<ChopsticksGetFood>(_onChopsticksGetFood);
        EventManager.Instance.AddHandler<ChopsticksNotGetFood>(_onChopsticksNotGetFood);
        EventManager.Instance.AddHandler<End>(_onEnd);
        EventManager.Instance.AddHandler<ChopsticksNotGetFood>(_onChopsticksNotGetFood);
    }
    
    private void OnDestroy()
    {
        EventManager.Instance.RemoveHandler<HotPotSpin>(_onHotPotSpin);
        EventManager.Instance.RemoveHandler<ChopsticksClash>(_onChopsticksAttack);
        EventManager.Instance.RemoveHandler<ChopsticksGetFood>(_onChopsticksGetFood);
        EventManager.Instance.RemoveHandler<ChopsticksNotGetFood>(_onChopsticksNotGetFood);
        EventManager.Instance.RemoveHandler<End>(_onEnd);
        EventManager.Instance.RemoveHandler<ChopsticksNotGetFood>(_onChopsticksNotGetFood);
    }

    private void _onHotPotSpin(HotPotSpin e)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Sounds/HotPotSPIN1");
    }
    
    private void _onChopsticksAttack(ChopsticksClash e)
    {
        if (e.AttType == ChopsticksClash.AttackType.AttackOnAttack)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Sounds/ChopstickBladeRanA");
        }
        else if(e.AttType == ChopsticksClash.AttackType.AttackOnDefend)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Sounds/ChopstickBladeRanB");
        }
        else if(e.AttType == ChopsticksClash.AttackType.AttackOnIdle)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Sounds/CHOPSTICKBLADE");
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Sounds/PerfectShiled");
        }
        
    }
   
    private void _onChopsticksGetFood(ChopsticksGetFood e)
    {
        if (e.Score > 0)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Sounds/Score");
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Sounds/PunishSound");
        }
    }
    private void _onChopsticksNotGetFood(ChopsticksNotGetFood e)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Sounds/ChopStciksNull");
    }
    private void _onEnd(End e)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Music/EndMusic");
    }
}
