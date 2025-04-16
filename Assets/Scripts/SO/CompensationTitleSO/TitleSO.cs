using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Compensation_Title", menuName = "Compensation/Title")]
public class TitleSO : ScriptableObject
{
    public int id;
    public string titleName;
    public Sprite title;
}
