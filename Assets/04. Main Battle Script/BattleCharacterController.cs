using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Main Battle 씬에서 자신의 전투캐릭터를 생성하기 위한 스크립트

public class BattleCharacterController : MonoBehaviour
{
    // 초기에 미리 설정해야할 값들
    //public BattleCharacter BattleCharacterPrefab;   // 생성할 적 AI
    //private float[,] charac_posi = new float[2, 3]; // 캐릭터의 포지션 좌표
    //private float[] area_offset = new float[3];     // Generate Area의 offset Y값
    //private float[] area_size = new float[3];       // Generate Area의 size Y값
    // 이전 씬에서 캐릭터 정보를 담을 변수
    //CharacterSelect charac_select;                      // 이전 씬에서 남은 오브젝트를 담을 변수
    //private int[,] charac_layout = new int[3, 2];       // 캐릭터 배치도
    //private int charac_num = 0;                         // 캐릭터의 수
    //private int  charac_exist = 0;                      // 한 라인에서 캐릭터의 수
    //private int charac_info_count = 0;                  // 현재 생성할 캐릭터의 배열 변수

    //
    //private List<int> selected_character = new List<int>(); // Character Select씬에서 선택된 캐릭터 정보를 담을 변수
    //

    private WebSocketManager web_socket_manager;                                            // CharacterSelectManager스크립트의 web_socket_manager를 옮겨 사용할 변수
    private BattleCharacter[,] battle_character_prefab_array = new BattleCharacter[3, 2];   // CharacterSelectManager스크립트의 selected_character_prefab_array를 옮겨 사용할 변수

    void Start() {
        /*        // 전투캐릭터 위치 좌표 값
                charac_posi[0, 0] = -1.1f; charac_posi[0, 1] = 0.0f; charac_posi[0, 2] = 1.1f;
                charac_posi[1, 0] = 0.0f; charac_posi[1, 1] = -2.3f; charac_posi[1, 2] = -4.6f;
                // Generate Area의 offset Y값
                area_offset[0] = 0.5f; area_offset[1] = 4.5f; area_offset[2] = 8.5f;
                // Generate Area의 size Y값
                area_size[0] = 8.0f; area_size[1] = 16.0f; area_size[2] = 24.0f;


                // 이전 씬에서 캐릭터들의 정보를 받아와 필요한 곳에 입력하는 과정이 필요
                CharacterSelect charac_select = GameObject.Find("CharacterSelect").GetComponent<CharacterSelect>();
                // 캐릭터 배치도와 수를 가져옴 
                (charac_num, charac_layout) = charac_select.Transfor_Character_Num_Layout();

                // 캐릭터 생성
                for (int line_count = 0; line_count < 3; line_count++) {
                    for (int k = 0; k < 2; k++) {
                        if (charac_layout[line_count, k] == 1) {    // 캐릭터 배치도의 수가 1이면
                            charac_exist++;                         // 라인의 캐릭터 수 증가
                        }
                    }
                    // 현재 라인의 캐릭터의 수에 따라 캐릭터 생성
                    if (charac_exist == 1) {                        // 한명이라면
                        CreateBattleCharacter(new Vector3(charac_posi[0, charac_exist], charac_posi[1, line_count], -1.0f),                                     // 캐릭터 생성 좌표
                            charac_select.charac_info[charac_info_count].Print_Hp(), charac_select.charac_info[charac_info_count].Print_Ap(),                   // 캐릭터 체력, 공격력
                            area_offset[line_count], area_size[line_count], charac_select.charac_info[charac_info_count].Print_RuntimeAnimatorController());    // 캐릭터 콜라이더 offset, size, 애니메이터 컨트롤러
                    }
                    else if (charac_exist == 2) {                   // 두명이라면
                        CreateBattleCharacter(new Vector3(charac_posi[0, charac_exist], charac_posi[1, line_count], -1.0f), charac_select.charac_info[charac_info_count].Print_Hp(), charac_select.charac_info[charac_info_count].Print_Ap(), area_offset[line_count], area_size[line_count], charac_select.charac_info[charac_info_count].Print_RuntimeAnimatorController());
                        CreateBattleCharacter(new Vector3(charac_posi[0, charac_exist - 2], charac_posi[1, line_count], -1.0f), charac_select.charac_info[charac_info_count].Print_Hp(), charac_select.charac_info[charac_info_count].Print_Ap(), area_offset[line_count], area_size[line_count], charac_select.charac_info[charac_info_count].Print_RuntimeAnimatorController());
                    }
                    charac_exist = 0;                               // 라인 캐릭터 수 초기화
                }

                Destroy(GameObject.Find("CharacterSelect"));    // 이전 씬에서 살아남은 오브젝트 파괴

                // CharacterSelectManager씬에 저장된 user가 선택한 캐릭터 정보를 가져오기
        *//*        CharacterSelectManager character_select_manager = GameObject.Find("SelectManager").GetComponent<CharacterSelectManager>();
                selected_character = character_select_manager.Selected_Character();
                for (int i = 0; i < selected_character.Count; i++) { Debug.Log("selected_character[" + i + "] = " + selected_character[i]); }*//*


                // SelectManager 오브젝트를 이용하였으므로 파괴
                Destroy(GameObject.Find("SelectManager"));*/

        // Character Select씬에서 필요한 것들을 옮기기
        Move_Characterselectmanager_Data();

    }
    // CharacterSelectManager스크립트에 저장된 여러 값을 가져오기 위한 함수
    private void Move_Characterselectmanager_Data()
    {
        /*// CharacterSelectManager스크립트에 저장된 아래 두 변수를 가져오기 위한 변수
        CharacterSelectManager character_select_manager;
        // Character Select씬에서 살아있는 SelectManager오브젝트를 사용하기 위해 CharacterSelectManager스크립트가 있는 character_select_manager를 생성한다
        character_select_manager = GameObject.Find("SelectManager").GetComponent<CharacterSelectManager>();
        // Character Select씬에서 살아있는 SelectManager오브젝트에 있는 CharacterSelectManager스크립트에 저장된 web_socket_manager를 옮겨온다
        web_socket_manager = character_select_manager.Move_Websocket();
        battle_character_prefab_array = character_select_manager.Move_Battlecharacter();
        // SelectManager 오브젝트는 이제 필요가 없으므로 파괴
        Destroy(GameObject.Find("SelectManager"));*/
    }
    void Update() {
        
    }

/*    // 캐릭터 생성(생성위치, 체력, 공격력, 콜라이더 offset, 콜라이더 size)
    private void CreateBattleCharacter(Vector3 generatelocation, float hp, float ap, float coll_offset, float coll_size, RuntimeAnimatorController ani_controller) {
        charac_info_count++;                        // 중가시켜 1캐릭터 생성했다는 것을알림
        // 전투캐릭터 프리팹 생성
        BattleCharacter battlecharacter = Instantiate(BattleCharacterPrefab, generatelocation, Quaternion.identity);
        battlecharacter.transform.parent = gameObject.transform;                        // 생성한 전투캐릭터 프리팹을 Generate Area의 자식 오브젝트로 변경
        battlecharacter.transform.localPosition = battlecharacter.transform.position;   // 생성한 전투캐릭터 프리팹의 position을 localposition으로 변경
        battlecharacter.tag = "1";                                                      // 생성한 전투캐릭터 프리팹의 tag를 1로 설정(아군이라 판단)

        //생성한 적의 능력치 설정
        //battlecharacter.GetComponent<BattleCharacter>().Setup(hp, ap, new Vector3(generatelocation.x, generatelocation.y + 18.0f, -1.0f), coll_offset, coll_size, ani_controller);
    }

    void UpdateBattleCharacterPosition() {
        // 전투캐릭터 생성 시 도착 포지션 설정
        // 추후 전투캐릭터의 도착 포지션 설정
    }

    void AddBattleCharacter() {

    }*/
}

// 문제점? 고민사항
/*
Character_info 배열을 그냥 6개로 설정해 버렸다
~~~CharacterController에서 접근하기 위해 Character_info 구조체가 public으로 선언 되어있다
AddCharacter_Info 메서드가 깔끔하지 않다

 
*/