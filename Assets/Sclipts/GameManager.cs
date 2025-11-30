using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private List<ICharactor> _charactorList = new List<ICharactor>();

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        var charactors = FindObjectsByType<MonoBehaviour>(
            FindObjectsInactive.Include,     
            FindObjectsSortMode.None         
        ).OfType<ICharactor>();
        foreach (var charactor in charactors)
        {
            _charactorList.Add(charactor);
            charactor.SetupCharactor();
        }
    }

    void Update()
    {
        _charactorList.RemoveAll(c => c == null);
        foreach(ICharactor charactor in _charactorList)
        {
            charactor.UpdateCharactor();
        }
    }

    public void AddIcharactorList(ICharactor charactor)
    {
        _charactorList.Add(charactor);
    }
}
