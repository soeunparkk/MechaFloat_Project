using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Object", menuName = "ObjectDatabase/Object")]
public class ObjectSO : ScriptableObject
{
    public int id;
    public string objectName;
    public string description;
    public string objectNameEng;
    public string stageAppearanceeffect;
    public string StatusEffect;
    public ObjectType objectType;

    public string DisplayName
    {
        get { return string.IsNullOrEmpty(objectNameEng) ? objectName : objectNameEng; }
    }
}
