using System.Collections.Generic;
using UnityEngine;

namespace ElectroMag._DevPooling
{
    public interface IObjectPool
    {
        abstract void ApplyValues(int numPool, GameObject prefab);
        abstract int NumObjectsToPool { set; get; }
        abstract GameObject ToPoolObject { set; get; }
        abstract void Initialize(); 

        abstract void Act(Vector3 pos);

        abstract void Sleep();
        abstract void CreatePool(GameObject prefab);

        abstract void AddObject(GameObject prefab, int amtToAdd);
    }
}
