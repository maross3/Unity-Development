using System.Collections.Generic;

namespace ObjectPooling
{
    public interface IGenerator
    {
        abstract void Initialize();
        abstract void UpdateInspectorValues();
        abstract string generatorName { set; get; }
        abstract int GetPoolsCount();
        abstract IObjectPool GetPool(string str);
        abstract void SleepPools();

    }
}