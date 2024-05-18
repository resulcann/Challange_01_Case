using Ali.Helper;
using Lean.Pool;
using UnityEngine;
public class PoolManager : LocalSingleton<PoolManager>
{
    [SerializeField] private LeanGameObjectPool _cellPool;
    
    public GameObject SpawnCell()
    {
        GameObject result = null;
        _cellPool.TrySpawn(ref result);
        return result;
    }
    
    public void DeSpawnCell(GameObject cell) { _cellPool.Despawn(cell); }
}