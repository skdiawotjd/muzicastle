using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour
{
    private int character_code = 0;                              // 캐릭터 정보 저장할 변수
    private CharacterSelectManager character_select_manager;    // SelectManager 오브젝트 담을 변수
    public Image character_image;
    private Button my_self;                                     // 캐릭터 선택 여부를 확인하여 버튼을 클릭할 수 있는지를 변경하기 위해 버튼을 저장하는 변수

    void Start()
    {
        // 캐릭터 버튼이 클릭될 시 CharacterSelectManager에게 자신이 어떤 캐릭터인지 알리기 위해 미리 CharacterSelectManager를 저장
        character_select_manager = GameObject.Find("SelectManager").GetComponent<CharacterSelectManager>();
        // 자기 자신을 변경하기 위해 변수에 저장
        my_self = gameObject.GetComponent<Button>();
    }

    // 버튼 생성 시 각 버튼에 맞는 캐릭터 이미지를 넣기
    public void Initialize_Image ()
    {
        if (character_code == 0)
        {
            character_image.sprite = Resources.Load("sword_0", typeof(Sprite)) as Sprite;
            Debug.Log("생성된 버튼의 캐릭터 이미지 sword_0");
        }
        else if (character_code == 1)
        {
            character_image.sprite = Resources.Load("twin swords_0", typeof(Sprite)) as Sprite;
            Debug.Log("생성된 버튼의 캐릭터 이미지 twin sword_0");
        }
        else if (character_code == 2)
        {
            character_image.sprite = Resources.Load("enemy_0", typeof(Sprite)) as Sprite;
            Debug.Log("생성된 버튼의 캐릭터 이미지 twin sword_0");
        }
    }

    // CharacterSelectManager에서 버튼 생성시 캐릭터 정보를 넘겨받는 메소드
    public void Input_Character_Code(int code)
    {
        // CharacterSelectManager에서 버튼 생성 시 자신이 어떤 캐릭터의 버튼인지 정보를 줌
        character_code = code;
        Debug.Log("생성된 버튼의 캐릭터 코드 = " + character_code);
    }
    // 버튼 클릭 시 CharacterSelectManager로 자신이 어떤 캐릭터인지 정보를 넘겨주는 메소드
    public void Output_Character_Code()
    {
        // CharacterSelectManager스크립트의 selected_character에 user가 선택한 캐릭터의 정보를 전송
        character_select_manager.Create_Character(character_code, character_image.sprite);
        Debug.Log("캐릭터 코드 " + character_code + "가 있는 버튼이 클릭됨");
        // 해당 버튼이 다시 눌리지 않게 버튼 비활성화
        Button_state(false);
    }
    public void Button_state(bool clickable)
    {
        my_self.interactable = clickable;
    }
}

// 선택된 캐릭터를 해제하면 버튼 활성화