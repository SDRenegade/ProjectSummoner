using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPersistentData
{
    void LoadData(GamePersistentData gamePersistentData);

    void SaveData(ref GamePersistentData gamePersistentData);
}
