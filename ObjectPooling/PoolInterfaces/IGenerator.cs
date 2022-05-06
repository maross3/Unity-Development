using System.Collections.Generic;

namespace ElectroMag._DevPooling
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