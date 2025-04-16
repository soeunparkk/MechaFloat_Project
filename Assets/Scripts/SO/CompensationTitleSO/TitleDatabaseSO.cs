using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TitleDatabase", menuName = "Compensation/TitleDatabase")]
public class TitleDatabaseSO : ScriptableObject
{
    public List<TitleSO> titles = new List<TitleSO>();

    private Dictionary<int, TitleSO> titlesById;                

    public void Initialize()
    {
        titlesById = new Dictionary<int, TitleSO>();             

        foreach (var title in titles)                         
        {
            titlesById[title.id] = title;
        }
    }

    public TitleSO GetItemById(int id)
    {
        if (titlesById == null)                                       
        {
            Initialize();
        }

        if (titlesById.TryGetValue(id, out TitleSO title))
            return title;                                         

        return null;                                             
    }
}
