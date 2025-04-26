using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialDatabase", menuName = "Tutorial/Database")]
public class TutorialDatabaseSO : ScriptableObject
{
    public List<TutorialSO> tutorials = new List<TutorialSO>();

    private Dictionary<int, TutorialSO> tutorialsById;

    public void Initialize()
    {
        tutorialsById = new Dictionary<int, TutorialSO>();

        foreach (var tutorial in tutorials)
        {
            tutorialsById[tutorial.id] = tutorial;
        }
    }

    // ID로 아이템 찾기
    public TutorialSO GetItemById(int id)
    {
        if (tutorialsById == null)
        {
            Initialize();
        }

        if (tutorialsById.TryGetValue(id, out TutorialSO tutorial))
            return tutorial;

        return null;
    }
}
