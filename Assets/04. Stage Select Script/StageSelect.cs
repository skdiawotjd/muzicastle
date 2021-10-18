using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// 각 스테이지마다 저장된 적의 정보를 CharacterSelect -> Main Battle 씬으로 전달하기 위한 스크립트

public class StageSelect : MonoBehaviour
{
    // 캐릭터의 정보를 저장할 구조체
    public struct EnemyCharacter_info {
        private float hp;
        private float ap;
        private RuntimeAnimatorController ani_controller;

        public void InputData(float hp, float ap, RuntimeAnimatorController ani_controller) { this.hp = hp; this.ap = ap; this.ani_controller = ani_controller; }
        public float Print_Hp() { return hp; }
        public float Print_Ap() { return ap; }
        public RuntimeAnimatorController Print_RuntimeAnimatorController() { return ani_controller; }
    }
    // 각 스테이지를 구분할 배열
    private string[] stage_level = new string[6];
    // 적 캐릭터 배치도
    private int[,] enemy_charac_layout = new int[3, 2];
    // 각 스테이지의 적 데이터를 저장할 배열
    public EnemyCharacter_info[] enemy_charac_info = new EnemyCharacter_info[6];
    // 적 캐릭터 Animator Controller를 담을 변수
    public RuntimeAnimatorController ani_controller_1;
    public RuntimeAnimatorController ani_controller_2;

    public Text selected_stage_text;

    void Start() {
        // 임시로 스태이지 구분을 위한 배열 값 입력
        for (int i = 0; i < 6; i++) {
            stage_level[i] = "Stage 1-" + (i + 1).ToString();
        }
        // 적 캐릭터 배치도를 0으로 초기화
        for (int i = 0; i < 3; i++) {
            for (int k = 0; k < 2; k++) {
                enemy_charac_layout[i, k] = 0;
            }
        }

        DontDestroyOnLoad(gameObject);          // 다른 씬에 데이터를 저장시키기 위해 현재 TemEnemy 오브젝트 파괴를 방지
    }

    // Update is called once per frame
    void Update()
    {
        
    } 
    public void Stage_Select(GameObject gameObject) {       // 특정 스테이지 버튼을 클릭 시 
        selected_stage_text.text = gameObject.name;       // 현재 선택된 스테이지 이름을 SelectedStage오브젝트에 표시
        
        // 우선 여기에서 스테이지 이름으로 구분하여 데이터를 하나씩 넣어야겠다
        if (gameObject.name == "Stage 1-1") {
            enemy_charac_layout[0, 0] = 1;
            enemy_charac_info[0].InputData(100.0f, 10.0f, ani_controller_1);
        }
        else if (gameObject.name == "Stage 1-2") {
            enemy_charac_layout[0, 0] = 1; enemy_charac_layout[0, 1] = 1;
            enemy_charac_info[0].InputData(100.0f, 10.0f, ani_controller_1); enemy_charac_info[1].InputData(100.0f, 10.0f, ani_controller_2);
        }
        else if (gameObject.name == "Stage 1-3") {
            enemy_charac_layout[0, 0] = 1; enemy_charac_layout[0, 1] = 1; enemy_charac_layout[1, 0] = 1;
            enemy_charac_info[0].InputData(100.0f, 10.0f, ani_controller_1); enemy_charac_info[1].InputData(100.0f, 10.0f, ani_controller_2); enemy_charac_info[2].InputData(100.0f, 10.0f, ani_controller_1);
        }
        else if (gameObject.name == "Stage 1-4") {
            enemy_charac_layout[0, 0] = 1; enemy_charac_layout[0, 1] = 1; enemy_charac_layout[1, 0] = 1; enemy_charac_layout[1, 1] = 1;
            enemy_charac_info[0].InputData(100.0f, 10.0f, ani_controller_1); enemy_charac_info[1].InputData(100.0f, 10.0f, ani_controller_2); enemy_charac_info[2].InputData(100.0f, 10.0f, ani_controller_1); enemy_charac_info[3].InputData(100.0f, 10.0f, ani_controller_2);
        }
        else if (gameObject.name == "Stage 1-5") {
            enemy_charac_layout[0, 0] = 1; enemy_charac_layout[0, 1] = 1; enemy_charac_layout[1, 0] = 1; enemy_charac_layout[1, 1] = 1; enemy_charac_layout[2, 0] = 1;
            enemy_charac_info[0].InputData(100.0f, 10.0f, ani_controller_1); enemy_charac_info[1].InputData(100.0f, 10.0f, ani_controller_2); enemy_charac_info[2].InputData(100.0f, 10.0f, ani_controller_1); enemy_charac_info[3].InputData(100.0f, 10.0f, ani_controller_2); enemy_charac_info[4].InputData(100.0f, 10.0f, ani_controller_1);
        }
        else if (gameObject.name == "Stage 1-6") {
            enemy_charac_layout[0, 0] = 1; enemy_charac_layout[0, 1] = 1; enemy_charac_layout[1, 0] = 1; enemy_charac_layout[1, 1] = 1; enemy_charac_layout[2, 0] = 1; enemy_charac_layout[2, 1] = 1;
            enemy_charac_info[0].InputData(100.0f, 10.0f, ani_controller_1); enemy_charac_info[1].InputData(100.0f, 10.0f, ani_controller_2); enemy_charac_info[2].InputData(100.0f, 10.0f, ani_controller_1); enemy_charac_info[3].InputData(100.0f, 10.0f, ani_controller_2); enemy_charac_info[4].InputData(100.0f, 10.0f, ani_controller_1); enemy_charac_info[5].InputData(100.0f, 10.0f, ani_controller_2);
        }
    }
    public (int, int[,]) Transfor_Enemy_Character_Num_Layout() {                  // 다른 곳에서 캐릭터의 배치도와 수를 가져오기 위한 메소드
        int num = 0;
        for (int i = 0; i < 3; i++) {
            for (int k = 0; k < 2; k++) {
                if (enemy_charac_layout[i, k] == 0) {
                    num++;
                }
            }
        }
        return (num, enemy_charac_layout);
    }
    public void Scene_Move() {
        SceneManager.LoadScene("Character Select");      // 메인 전투 씬 로드
    }
}

// 문제점? 고민사항
/*
적 정보를 DB같은 데에서 가져와 저장해야 해서 사실상 전부 바꿔야한다.
 



추가사항
적 정보를 파일 등의 형태로 저장하여 그 내용을 불러오는 방식으로 변경
*/
