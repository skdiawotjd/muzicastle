using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelButton : MonoBehaviour
{
    private int code;                                       // 해당 버튼에 해당하는 캐릭터의 코드
    private int selected_character_prefab_array_location_x;   //  selected_character_prefab_array의 위치
    private int selected_character_prefab_array_location_y;   //  selected_character_prefab_array의 위치

    // 캐릭터 생성 시 삭제를 대비해 cancel button에 캐릭터 코드와 selected_character_prefab_array의 위치를 저장
    public void Input_Cancel_button_Info(int code, int location_x, int location_y)
    {
        this.code = code;
        selected_character_prefab_array_location_x = location_x;
        selected_character_prefab_array_location_y = location_y;
    }

    public int Output_Cancel_button_Character_Code()
    {
        return code;
    }
    public int Output_Cancel_button_Character_Prefab_Array_Location_X()
    {
        return selected_character_prefab_array_location_x;
    }
    public int Output_Cancel_button_Character_Prefab_Array_Location_Y()
    {
        return selected_character_prefab_array_location_y;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
