using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<ICharactor> _charactorList = new List<ICharactor>();
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
        foreach(ICharactor charactor in _charactorList)
        {
            charactor.UpdateCharactor();
        }
    }
}
