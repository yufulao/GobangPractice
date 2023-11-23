public interface IGameState
{
    void OnInit(GamingFsmManager fsmManager){}
    
    void OnUpdate(GamingFsmManager fsmManager){}
    
    void OnClear(GamingFsmManager fsmManager){}
}
