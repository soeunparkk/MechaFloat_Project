using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkinDatabase", menuName = "Compensation/SkinDatabase")]
public class SkinDatabaseSO : ScriptableObject
{
    public List<SkinSO> skins = new List<SkinSO>();          

    private Dictionary<int, SkinSO> skinsById;                      

    public void Initialize()
    {
        skinsById = new Dictionary<int, SkinSO>();

        foreach (var skin in skins)                         
        {
            skinsById[skin.id] = skin;
        }
    }

    public SkinSO GetItemById(int id)
    {
        if (skinsById == null)                                     
        {
            Initialize();
        }

        if (skinsById.TryGetValue(id, out SkinSO skin))
            return skin;                                            

        return null;                                              
    }
}
