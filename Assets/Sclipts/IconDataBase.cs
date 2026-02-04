using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataBase/Icon")]
public class IconDataBase : ScriptableObject
{
    [SerializeField] private ElementIconData[] _elementIconDatas;
    [SerializeField] private TypeIconData[] _typeIconDatas;

    private Dictionary<MagicElement, Sprite> _elementIconDict;
    private Dictionary<MagicType, Sprite> _typeIconDict;


    private void OnEnable()
    {
        CreateElementDictionary();
        CreateTypeDictionary();
    }

    private void CreateElementDictionary()
    {
        _elementIconDict = new Dictionary<MagicElement, Sprite>();

        foreach (var data in _elementIconDatas)
        {
            if (_elementIconDict.ContainsKey(data.MagicElement)) continue;

            _elementIconDict.Add(data.MagicElement, data.Icon);
        }
    }

    private void CreateTypeDictionary()
    {
        _typeIconDict = new Dictionary<MagicType, Sprite>();

        foreach (var data in _typeIconDatas)
        {
            if (_typeIconDict.ContainsKey(data.Type)) continue;

            _typeIconDict.Add(data.Type, data.Icon);
        }
    }


    public Sprite GetElementIcon(MagicElement element)
    {
        if (_elementIconDict == null)
        {
            CreateElementDictionary();
        }

        if (_elementIconDict.TryGetValue(element, out Sprite icon))
        {
            return icon;
        }

        return null;
    }

    public Sprite GetTypeIcon(MagicType type)
    {
        if (_typeIconDict == null)
        {
            CreateTypeDictionary();
        }

        if (_typeIconDict.TryGetValue(type, out Sprite icon))
        {
            return icon;
        }

        return null;
    }
}

[Serializable]
public class ElementIconData
{
    [SerializeField] private MagicElement _magicElement;
    [SerializeField] private Sprite _icon;

    public MagicElement MagicElement => _magicElement;
    public Sprite Icon => _icon;
}

[Serializable]
public class TypeIconData
{
    [SerializeField] private MagicType _type;
    [SerializeField] private Sprite _icon;
    public MagicType Type => _type;
    public Sprite Icon => _icon;
}
