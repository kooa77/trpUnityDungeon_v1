using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGameScene : MonoBehaviour
{
    public MainGameUI GameUI;
    public TileMap _TileMap;

	void Start ()
    {
        Init();		
	}
	
	void Update ()
    {
        MessageSystem.Instance.ProcessMessage();
    }

    void Init()
    {
        _TileMap.Init();
        GameManager.Instance.SetMap(_TileMap);

        Character player = CreateCharacter("Player", "character01");
        Character monster = CreateCharacter("Monster", "character02");
        player.BecomeViewer();
    }

    Character CreateCharacter(string fileName, string resourceName)
    {
        string filePath = "Prefabs/CharacterFrame/Character";
        GameObject charPrefabs = Resources.Load<GameObject>(filePath);
        GameObject charGameObject = GameObject.Instantiate(charPrefabs);
        charGameObject.transform.SetParent(_TileMap.transform);
        charGameObject.transform.localPosition = Vector3.zero;

        Character character = charGameObject.GetComponent<Player>();
        switch(fileName)
        {
            case "Player":
                character = charGameObject.AddComponent<Player>();
                break;
            case "Monster":
                character = charGameObject.AddComponent<Monster>();
                break;
        }
        character.Init(resourceName);

        CreateGameSlider(character, true, Vector3.zero);
        CreateGameSlider(character, false, new Vector3(0.0f, 0.4f, 0.0f));

        return character;
    }

    void CreateGameSlider(Character character, bool isHPGuage, Vector3 position)
    {
        Slider slider = GameUI.CreateSlider(isHPGuage);
        character.LinkGameGuage(slider, position, isHPGuage);
    }
}
