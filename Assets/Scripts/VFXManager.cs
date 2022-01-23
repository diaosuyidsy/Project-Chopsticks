using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        EventManager.Instance.AddHandler<HotPotSpin>(_onHotPotSpin);
        EventManager.Instance.AddHandler<HotPotHotAir>(_onHotPotHotAir);
        EventManager.Instance.AddHandler<ChopsticksClash>(_onChopsticksAttack);
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
        EventManager.Instance.RemoveHandler<HotPotHotAir>(_onHotPotHotAir);
        EventManager.Instance.RemoveHandler<ChopsticksClash>(_onChopsticksAttack);
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
        
    }
    private void _onHotPotHotAir(HotPotHotAir e)
    {
        
    }
    private void _onChopsticksAttack(ChopsticksClash e)
    {
        
    }
    private void _onChopsticksDefence(ChopsticksDefence e)
    {
        
    }
    
    private void _onChopsticksGetFood(ChopsticksGetFood e)
    {
        
    }
    private void _onChopsticksNotGetFood(ChopsticksNotGetFood e)
    {
        
    }
    private void _onChopsticksBounceBack(ChopsticksBounceBack e)
    {
        
    }
    private void _onWin(Win e)
    {
        
    }
    private void _onLose(Lose e)
    {
        
    }
}
