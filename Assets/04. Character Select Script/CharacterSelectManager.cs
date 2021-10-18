using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class CharacterSelectManager : MonoBehaviour
{
    private WebSocketManager web_socket_manager;                                        // 다른 씬에서 파괴되지 않은 오브젝트이므로 미리 받아오기 위한 변수
    public ScrollRect scrollRect;                                                       // 캐릭터 버튼 생성 시 parent-child 관계를 설정하기 위해 스크롤 뷰를 담을 변수
    public CharacterButton character_btn;                                               // 사용자가 가지고 있는 캐릭터의 수에 따라 버튼을 생성하기 때문에 생성 시 사용될 button 프리팹
    private int position_yobu = 0;                                                      // 전/중/후방에 캐릭터가 1마리 들어갔는지 2마리 들어갔는지 확인하기 위한 변수

    //private BattleCharacter[,] selected_character_prefab_array = new BattleCharacter[3,2];      // 선택된 캐릭터 프리팹을 수정할 수 있기 때문에 미리 담아놓을 변수
    public BattleCharacter battle_character_prefab;                                             // 캐릭터 생성 시 사용할 프리팹 변수
    public GameObject ally_character_area;                                                      // Main Battle 씬에서 사용하기 위해 파괴를 방지하기 위해 사용할 변수
    private RuntimeAnimatorController ani_controller;                                           // 캐릭터 생성 시 해당 캐릭터에 맞는 애니메이션 컨트롤러를 전달하기 위한 변수
    public Button cancel_button;
    private SPUM_Prefabs[,] selected_character_prefab_array = new SPUM_Prefabs[3, 2];
    public GameObject cencel_group;


    void Start()
    {
        Debug.Log("------------------Character Select Scene Start------------------------");
        // WebSocket 오브젝트의 WebSocketManager 스크립트에 있는 함수에 접근하기 위해 미리 불러옴
        web_socket_manager = GameObject.Find("WebSocket").GetComponent<WebSocketManager>();
        // Main Battle씬에게 특정 정보를 전달하기 위해 우선 파괴 방지
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(ally_character_area);

        for (int i = 0; i < web_socket_manager.user_info.Print_User_Character_Count(); i++)
        {
            Debug.Log(web_socket_manager.user_info.Print_User_Character_Count()+ "개 중 " + (i + 1) + "번 캐릭터 버튼 생성");
            Create_Button(i);
        }
    }

    // 캐릭터 버튼을 생성하는 메소드
    private void Create_Button(int i)
    {
        // 버튼 프리팹 생성
        CharacterButton char_btn = Instantiate(character_btn, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        // web_socket_manager에서 i번 째 캐릭터의 캐릭터 코드를 받아와 CharacterButton에게 넘겨줌
        char_btn.Input_Character_Code(web_socket_manager.user_info.Print_User_Character_Code(i));
        // 생성된 버튼을 리스트뷰 안에 넣기 위해 parent-child 관계 설정
        char_btn.transform.SetParent(scrollRect.content);
        // 생성된 버튼의 scale을 1로 설정
        char_btn.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        // 생성된 버튼의 이름을 캐릭터 코드로 변경
        char_btn.name = web_socket_manager.user_info.Print_User_Character_Code(i).ToString();
        // 생성된 버튼의 이미지를 초기화
        char_btn.Initialize_Image();
    }

    // 선택된 캐릭터를 전/중/후방에 따라 배치
    public void Create_Character(int character_code, Sprite selected_image)
    {
        // char_posi는 전 - 0 중 - 1 후 - 2를 가지는 이유로 캐릭터 생성 시 selected_character_prefab_array[char_posi]로 사용할 변수
        int char_posi = web_socket_manager.user_info.Print_User_Character_Position(character_code);
        float x = 0; float y = 0; int one_two = 0;
        // 배치하려는 캐릭터의 전/중/후방을 확인
        switch (char_posi)
        {
            // 캐릭터 position이 전방인 경우 x: 58.4
            case 0:
                Debug.Log("전방 캐릭터의");
                x = 1.6f;
                one_two = (position_yobu += 1) % 10;
                break;
            // 캐릭터 position이 중앙인 경우 x: -1
            case 1:
                Debug.Log("중앙 캐릭터의");
                x = -0.1f;
                one_two = ((position_yobu += 10) / 10) % 10;
                break;
            // 캐릭터 position이 후방인 경우 x: -58
            case 2:
                Debug.Log("후방 캐릭터의");
                x = -1.8f;
                one_two = (((position_yobu += 100) / 10) / 10) % 10;
                break;
        }
        // 캐릭터가 1마리인 경우
        if (one_two == 1)
        {
            y = 2.5f;
        }
        // 캐릭터가 2마리인 경우
        else
        {
            y = 1.72f;
            // 기존 캐릭터의 위치를 변경해야 하므로 기존 캐릭터의 y좌표를 3.3f로 변경
            Debug.Log("기존 캐릭터 y좌표 변경, posi 값 ");
            Debug.Log("selected_character_prefab_array[" + char_posi + ", " + (one_two - 2) + "]");
            selected_character_prefab_array[char_posi, one_two - 2].transform.localPosition = new Vector3(x, 3.3f, -2.0f);
        }
        Debug.Log("x좌표: " + x + " y좌표: " + y + "를 기준으로 " + (one_two - 1) + "번 째 캐릭 생성");

        // 선택된 캐릭터 프리팹 생성
        //selected_character_prefab_array[char_posi, one_two - 1] = Instantiate(battle_character_prefab, ally_character_area.transform);

        // 캐릭터 코드와 프리팹 이름이 동일하므로 캐릭터 코드를 이용해 프리팹 생성
        GameObject game_object = Instantiate(Resources.Load("SPUM/SPUM_Units/" + character_code.ToString()), ally_character_area.transform) as GameObject;
        game_object.transform.localScale = new Vector3(-2, 2, 1);
        //game_object.transform.localPosition = Vector3.zero;
        selected_character_prefab_array[char_posi, one_two - 1] = game_object.GetComponent<SPUM_Prefabs>();
        //selected_character_prefab_array[char_posi, one_two - 1].PlayAnimation(1);
        //spum_prefab.PlayAnimation(4);


        Debug.Log("selected_character_prefab_array[" + char_posi + ", " + (one_two - 1) + "]");
        Debug.Log("selected_character_prefab_array[0,0]" + selected_character_prefab_array[0, 0]);
        // 생성한 캐릭터의 tag를 1로 설정(아군이라 판단)
        selected_character_prefab_array[char_posi, one_two - 1].tag = "1";
        // 생성한 캐릭터의 기본 정보 입력
        switch(character_code)
        {
            case 0:
                ani_controller = Resources.Load("Player Battle Character") as RuntimeAnimatorController;
                break;
            case 1:
                ani_controller = Resources.Load("twin swords") as RuntimeAnimatorController;
                break;
            case 2:
                ani_controller = Resources.Load("Enemy Battle Character") as RuntimeAnimatorController;
                break;
        }
        Debug.Log("해당 캐릭터의 공격력 " + web_socket_manager.user_info.Print_User_Character_Ap(character_code));
        Debug.Log("해당 캐릭터의 체력 " + web_socket_manager.user_info.Print_User_Character_Hp(character_code));
        //selected_character_prefab_array[char_posi, one_two - 1].Setup(web_socket_manager.user_info.Print_User_Character_Ap(character_code), web_socket_manager.user_info.Print_User_Character_Hp(character_code), ani_controller);

        selected_character_prefab_array[char_posi, one_two - 1].Setup(web_socket_manager.user_info.Print_User_Character_Ap(character_code), web_socket_manager.user_info.Print_User_Character_Hp(character_code), character_code);

        // 생성한 캐릭터의 현재 좌표 설정
        selected_character_prefab_array[char_posi, one_two - 1].transform.localPosition = new Vector3(x, y, -2.0f);

        // 취소 버튼에 캐릭터 코드, array의 위치 전송
        string location = char_posi.ToString() + (one_two - 1).ToString();
        cencel_group.transform.Find(location).GetComponent<CancelButton>().Input_Cancel_button_Info(character_code, char_posi, one_two - 1);
    }

    // 캐릭터 취소 버튼을 클릭 시 AllyCharacterArea에 생성된 캐릭터를 삭제 후 해당 캐릭터 버튼을 다시 활성화
    public void Cancel_Character_Select()
    {
        // 클릭된 버튼의 gameobject 가져오기
        GameObject asd = EventSystem.current.currentSelectedGameObject;

        // cancel button에 저장된 selected_character_prefab_array의 좌표를 가져오기
        int x = asd.GetComponent<CancelButton>().Output_Cancel_button_Character_Prefab_Array_Location_X();
        int y = asd.GetComponent<CancelButton>().Output_Cancel_button_Character_Prefab_Array_Location_Y();
        int count = asd.GetComponent<CancelButton>().Output_Cancel_button_Character_Code();

        // AllyCharacterArea에 생성된 캐릭터 프리팹 삭제
        Destroy(selected_character_prefab_array[x, y].gameObject);

        // cancel button에 저장된 캐릭터 코드를 이용해 character button을 찾아내 버튼을 활성화
        scrollRect.content.GetChild(count).GetComponent<CharacterButton>().Button_state(true);

        // 삭제할 캐릭터의 전/중/후를 확인하여 position_yobu를 수정
        switch (web_socket_manager.user_info.Print_User_Character_Position(count))
        {
            case 0:
                position_yobu -= 1; break;
            case 1:
                position_yobu -= 10; break;
            case 2:
                position_yobu -= 100; break;
        }

        // 전/중/후 중 2캐릭을 선택했는데 앞의 캐릭을 삭제 시 뒤의 캐릭을 앞으로 변경






        /*// 캐릭터 취소 버튼을 클릭 시 selected_character_prefab_array[x, y]를 확인하기 위해 클릭된 버튼의 이름을 확인
        int xa = 0; int ya = 0;
        switch (int.Parse(asd.name))
        {
            case 0:
                xa = 0; ya = 0; break;
            case 1:
                xa = 0; ya = 1; break;
            case 10:
                xa = 1; ya = 0; break;
            case 11:
                xa = 1; ya = 1; break;
            case 20:
                xa = 2; ya = 0; break;
            case 21:
                xa = 2; ya = 1; break;
        }
        // AllyCharacterArea에 생성된 캐릭터 프리팹 삭제
        Destroy(selected_character_prefab_array[xa, ya].gameObject);
        // character button의 이름이 캐릭터 코드와 일치하므로 취소 버튼을 클릭 시 해당 버튼에 등록된 코드를 통해 character button를 찾아 버튼 활성화
        scrollRect.content.GetChild(count).GetComponent<CharacterButton>().Button_state(true);
        // 삭제할 캐릭터의 전/중/후를 확인하여 position_yobu를 수정
        switch (web_socket_manager.user_info.Print_User_Character_Position(count))
        {
            case 0:
                position_yobu -= 1; break;
            case 1:
                position_yobu -= 10; break;
            case 2:
                position_yobu -= 100; break;
        }*/
        // 전/중/후 중 2캐릭을 선택했는데 앞의 캐릭을 삭제 시 뒤의 캐릭을 앞으로 변경

    }

    // Login씬에서 씬을 전환하기 위한 함수
    public void Scene_Move()
    {
        // 캐릭터 선택 씬 로드
        Debug.Log("Main Battle씬을 로드");
        SceneManager.LoadScene("Main Battle");
    }


/*
 * Main Battle씬에서 해당 스크립트에 저장된 변수를 재이용하기 위한 메소드들
 */
    public WebSocketManager Move_Websocket()
    {
        return web_socket_manager;
    }
    public SPUM_Prefabs[,] Move_Battlecharacter()
    {
        return selected_character_prefab_array;
    }
}
/*
Arrangement_Character
    position_yobu 첫번 째 자리는 전방 캐릭터 수, posi = (position_yobu + 1) % 10; 시 position_yobu의 자리수와 관계없이 첫번 째 자리수가 나와 전방에 몇개의 캐릭터가 들어갔는지를 확인
    position_yobu 두번 째 자리는 중앙 캐릭터 수, posi = ((position_yobu + 10) / 10) % 10; 시 position_yobu의 첫번 째 자리수를 제외하여 두번 째 자리수가 나와 중앙에 몇개의 캐릭터가 들어갔는지를 확인
    position_yobu 세번 째 자리는 후방 캐릭터 수, posi = (((position_yobu + 100) / 10) / 10) % 10; 시 position_yobu의 두번 째 자리수를 제외하여 세번 째 자리수가 나와 후방에 몇개의 캐릭터가 들어갔는지를 확인

selected_character_prefab_array
    ㅁㅁㅁ 2,0 1,0 0,0
    ㅁㅁㅁ 2,1 1,1 0,1
    후중전
    방앙방
    

 
*/
