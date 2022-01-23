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
        EventManager.Instance.AddHandler<ChopsticksAttack>(_onChopsticksAttack);
        EventManager.Instance.AddHandler<ChopsticksDefence>(_onChopsticksDefence);
        EventManager.Instance.AddHandler<ChopsticksGetFood>(_onChopsticksGetFood);
        EventManager.Instance.AddHandler<ChopsticksNotGetFood>(_onChopsticksNotGetFood);
        EventManager.Instance.AddHandler<ChopsticksBounceBack>(_onChopsticksBounceBack);
        EventManager.Instance.AddHandler<Win>(_onWin);
        EventManager.Instance.AddHandler<Lose>(_onLose);
        EventManager.Instance.AddHandler<ChopsticksNotGetFood>(_onChopsticksNotGetFood);
    }
    
    private void OnDestroy()
    {
        EventManager.Instance.RemoveHandler<HotPotSpin>(_onHotPotSpin);
        EventManager.Instance.RemoveHandler<ChopsticksAttack>(_onChopsticksAttack);
        EventManager.Instance.RemoveHandler<ChopsticksDefence>(_onChopsticksDefence);
        EventManager.Instance.RemoveHandler<ChopsticksGetFood>(_onChopsticksGetFood);
        EventManager.Instance.RemoveHandler<ChopsticksNotGetFood>(_onChopsticksNotGetFood);
        EventManager.Instance.RemoveHandler<ChopsticksBounceBack>(_onChopsticksBounceBack);
        EventManager.Instance.RemoveHandler<Win>(_onWin);
        EventManager.Instance.RemoveHandler<Lose>(_onLose);
        EventManager.Instance.RemoveHandler<ChopsticksNotGetFood>(_onChopsticksNotGetFood);
    }

    private void _onHotPotSpin(HotPotSpin e)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/HotPotSPIN1");
    }
    
    private void _onChopsticksAttack(ChopsticksAttack e)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/CHOPSTICKBLADE");
    }
    private void _onChopsticksDefence(ChopsticksDefence e)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/ChopstickBladeRanA");
    }
    
    private void _onChopsticksGetFood(ChopsticksGetFood e)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Score");
    }
    private void _onChopsticksNotGetFood(ChopsticksNotGetFood e)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/ChopStciksNull");
    }
    private void _onChopsticksBounceBack(ChopsticksBounceBack e)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/ChopstickBladeRanB");
    }
    private void _onWin(Win e)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Win");
    }
    private void _onLose(Lose e)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Lose");
    }
}
