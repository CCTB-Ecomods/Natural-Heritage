using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IGameLogic 
{
    void OnRoundChange();
    void SetWorld(WorldData worldData);
    WorldData GetWorld();
    ElectionSystem GetElectionSystem();
    void AddRoundChangeEventListener(UnityAction call);
}
