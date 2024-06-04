using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class boss : MonoBehaviour
{
    enemy En;
    [SerializeField] private GameObject _Win;
    [SerializeField] private GameObject WinFirst;
    // Start is called before the first frame update
    void Start()
    {
        En = gameObject.AddComponent<enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (En.health <= 0)
        {
            _Win.SetActive(true);
            EventSystem.current.SetSelectedGameObject(WinFirst);
        }
    }
}
