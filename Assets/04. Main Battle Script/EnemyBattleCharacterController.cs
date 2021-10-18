using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleCharacterController : MonoBehaviour
{
    // 초기에 미리 설정해야할 값들
    public EnemyBattleCharacter EnemyBattleCharacterPrefab;   // 생성할 적 AI
    private float[,] charac_posi = new float[2, 3]; // 캐릭터의 포지션 좌표
    private float[] area_offset = new float[3];     // Generate Area의 offset Y값
    private float[] area_size = new float[3];       // Generate Area의 size Y값
    // 이전 씬에서 캐릭터 정보를 담을 변수
    //CharacterSelect charac_select;                      // 이전 씬에서 남은 오브젝트를 담을 변수
    private int[,] charac_layout = new int[3, 2];       // 캐릭터 배치도
    private int charac_num = 0;                         // 캐릭터의 수
    private int charac_exist = 0;                      // 한 라인에서 캐릭터의 수
    private int charac_info_count = 0;                  // 현재 생성할 캐릭터의 배열 변수

    void Start()
    {
        /*// 전투캐릭터 있는지 값
        charac[0, 0] = 0; charac[0, 1] = 0; charac[1, 0] = 0; charac[1, 1] = 0; charac[2, 0] = 1; charac[2, 1] = 0;
        // 전투캐릭터의 수
        charac_num = 4;
        // 전투캐릭터 체력/공격력 값
        charac_stat[0, 0] = 230.0f; charac_stat[0, 1] = 10.0f; charac_stat[1, 0] = 150.0f; charac_stat[1, 1] = 10.0f; charac_stat[2, 0] = 200.0f; charac_stat[2, 1] = 10.0f;
        charac_stat[3, 0] = 100.0f; charac_stat[3, 1] = 10.0f; charac_stat[4, 0] = 100.0f; charac_stat[4, 1] = 10.0f; charac_stat[5, 0] = 100.0f; charac_stat[5, 1] = 10.0f;
        // 전투캐릭터의 공격 범위 값
        chatac_ran[0] = 10.0f;
        */

        // 전투캐릭터 위치 좌표 값
        charac_posi[0, 0] = -1.1f; charac_posi[0, 1] = 0.0f; charac_posi[0, 2] = 1.1f;
        charac_posi[1, 0] = 1.0f; charac_posi[1, 1] = 3.3f; charac_posi[1, 2] = 5.6f;
        // Generate Area의 offset Y값
        area_offset[0] = -0.5f; area_offset[1] = -4.5f; area_offset[2] = -8.5f;
        // Generate Area의 size Y값
        area_size[0] = 8.0f; area_size[1] = 16.0f; area_size[2] = 24.0f;

        /*int zerozero = 0;                               // 전/중앙에 캐릭터가 없는지를 확인
        for (int i = 0; i < 3; i++) {
            for (int k = 0; k < 2; k++) {
                if (charac[i, k] == 1) { two++; }
            }
            if (two == 0) {
                zerozero++;
            }
            else if (two == 2) { // 한 라인에 캐릭 둘
                //CreateBattleCharacter(new Vector3(charac_posi[0, two - 2], charac_posi[1, i - zerozero], -1.0f), charac_stat[0, 0], charac_stat[0, 1], chatac_ran[0]);
                //CreateBattleCharacter(new Vector3(charac_posi[0, two], charac_posi[1, i - zerozero], -1.0f), charac_stat[0, 0], charac_stat[0, 1], chatac_ran[0]);
                CreateBattleCharacter(new Vector3(charac_posi[0, two - 2], charac_posi[1, i - zerozero], -1.0f), charac_stat[charar_num_cout, 0], charac_stat[charar_num_cout++, 1], area_offset[i], area_size[i]);
                CreateBattleCharacter(new Vector3(charac_posi[0, two], charac_posi[1, i - zerozero], -1.0f), charac_stat[charar_num_cout, 0], charac_stat[charar_num_cout++, 1], area_offset[i], area_size[i]);
                line_charac++;
            }
            else if (two == 1) {  // 한 라인에 캐릭 하나
                //CreateBattleCharacter(new Vector3(charac_posi[0, two], charac_posi[1, i - zerozero], -1.0f), charac_stat[0, 0], charac_stat[0, 1], chatac_ran[0]);
                CreateBattleCharacter(new Vector3(charac_posi[0, two], charac_posi[1, i - zerozero], -1.0f), charac_stat[charar_num_cout, 0], charac_stat[charar_num_cout++, 1], area_offset[i], area_size[i]);
                line_charac++;
            }
            two = 0;
        }*/

        // 이전 씬에서 캐릭터들의 정보를 받아와 필요한 곳에 입력하는 과정이 필요
        StageSelect enemy_charac_select = GameObject.Find("SelectedStage").GetComponent<StageSelect>();
        // 캐릭터 배치도와 수를 가져옴 
        (charac_num, charac_layout) = enemy_charac_select.Transfor_Enemy_Character_Num_Layout();

        // 캐릭터 생성
        for (int line_count = 0; line_count < 3; line_count++)
        {
            for (int k = 0; k < 2; k++)
            {
                if (charac_layout[line_count, k] == 1)
                {    // 캐릭터 배치도의 수가 1이면
                    charac_exist++;                         // 라인의 캐릭터 수 증가
                }
            }
            // 현재 라인의 캐릭터의 수에 따라 캐릭터 생성
            if (charac_exist == 1)
            {                        // 한명이라면
                CreateBattleCharacter(new Vector3(charac_posi[0, charac_exist], charac_posi[1, line_count], -1.0f),                                     // 캐릭터 생성 좌표
                    enemy_charac_select.enemy_charac_info[charac_info_count].Print_Hp(), enemy_charac_select.enemy_charac_info[charac_info_count].Print_Ap(),                   // 캐릭터 체력, 공격력
                    area_offset[line_count], area_size[line_count], enemy_charac_select.enemy_charac_info[charac_info_count].Print_RuntimeAnimatorController());    // 캐릭터 콜라이더 offset, size, 애니메이터 컨트롤러
            }
            else if (charac_exist == 2)
            {                   // 두명이라면
                CreateBattleCharacter(new Vector3(charac_posi[0, charac_exist], charac_posi[1, line_count], -1.0f), enemy_charac_select.enemy_charac_info[charac_info_count].Print_Hp(), enemy_charac_select.enemy_charac_info[charac_info_count].Print_Ap(), area_offset[line_count], area_size[line_count], enemy_charac_select.enemy_charac_info[charac_info_count].Print_RuntimeAnimatorController());
                CreateBattleCharacter(new Vector3(charac_posi[0, charac_exist - 2], charac_posi[1, line_count], -1.0f), enemy_charac_select.enemy_charac_info[charac_info_count].Print_Hp(), enemy_charac_select.enemy_charac_info[charac_info_count].Print_Ap(), area_offset[line_count], area_size[line_count], enemy_charac_select.enemy_charac_info[charac_info_count].Print_RuntimeAnimatorController());
            }
            charac_exist = 0;                               // 라인 캐릭터 수 초기화
        }

        Destroy(GameObject.Find("SelectedStage"));    // 이전 씬에서 살아남은 오브젝트 파괴
    }

    void Update() {
        // Generate Area의 좌표를 옮기게 되면 전투캐릭터는 localposition을 사용하므로 마치 전투캐릭터가 움직이는 것처렴 표현
    }

    // 캐릭터 생성
    private void CreateBattleCharacter(Vector3 generatelocation, float hp, float ap, float coll_offset, float coll_size, RuntimeAnimatorController ani_controller) {
        charac_info_count++;                        // 중가시켜 1캐릭터 생성했다는 것을알림
        // 전투캐릭터 프리팹 생성
        EnemyBattleCharacter enemybattlecharacter = Instantiate(EnemyBattleCharacterPrefab, generatelocation, Quaternion.identity);
        enemybattlecharacter.transform.parent = gameObject.transform;                        // 생성한 전투캐릭터 프리팹을 Generate Area의 자식 오브젝트로 변경
        enemybattlecharacter.transform.localPosition = enemybattlecharacter.transform.position;   // 생성한 전투캐릭터 프리팹의 position을 localposition으로 변경
        enemybattlecharacter.tag = "2";                                                      // 생성한 전투캐릭터 프리팹의 tag를 1로 설정(아군이라 판단)

        //생성한 적의 능력치 설정
        enemybattlecharacter.GetComponent<EnemyBattleCharacter>().Setup(hp, ap, new Vector3(generatelocation.x, generatelocation.y + -2.35f, -1.0f), coll_offset, coll_size, ani_controller);
    }

    void UpdateBattleCharacterPosition()
    {
        // 전투캐릭터 생성 시 도착 포지션 설정
        // 추후 전투캐릭터의 도착 포지션 설정
    }
    // Gnerate Area의 콜라이터가 다른 콜라이터와 충돌 시 상대방을 만났다고 가정
    // 이라고 생각했지만 실제로는 자식 오브젝트의 콜라이더 충돌은 부모 오브젝트로 전달되므로 어떤 자식 오브젝트이든 충돌하게 되면 해당 메소드가 호출
    /*void OnTriggerEnter2D(Collider2D collision) {    // 충돌 시
        //walk_speed = 0.0f;                      // 우선 걸음을 멈추고
        Debug.Log(gameObject.name + "에서 " + collision.gameObject.name + "와 출돌을 감지");
        if (collision.CompareTag("Enemy")) {         // 부딛힌 오브젝트의 Tag가 Enemy인 경우(즉 적이랑 충돌 시)
            Debug.Log(gameObject.name + "에서 충돌한 오브젝트의 태그는 Enemy");
            // 공격 모션 시작
            for (int i = 0; i < transform.childCount; i++) {
                transform.GetChild(i).GetComponent<BattleCharacter>().Attack_Start_Animation();
                transform.GetChild(i).GetComponent<BattleCharacter>().Attack(collision);
                
            }
        }
        else{
            Debug.Log(gameObject.name + "에서 충돌한 오브젝트의 태그는 Enemy가 아니다");
        }
    }
    void OnTriggerExit2D(Collider2D coll) {    // Generate Area의 콜라이더가 다른 콜라이더와 충돌을 끝냈을 시
        walk_speed = 0.1f;
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).GetComponent<BattleCharacter>().Attack_End_Animation();
        }
    }*/
}
