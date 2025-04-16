using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Compensation_Skin", menuName = "Compensation/Skin")]
public class SkinSO : ScriptableObject
{
    public int id;
    public string skinName;
    public Mesh skin; // 임시로 메쉬로 지정 (스킨을 어떤식으로 주느냐에 따라 수정될 예정)
}
