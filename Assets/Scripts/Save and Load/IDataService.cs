using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataService
{
    public void Save(GameData data, bool overwrite = true);
    public GameData Load(string name);
    public void Delete(string name);
    public void DeleteAll();
    public IEnumerable<string> ListSaves();
}
