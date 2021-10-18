using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Main Battle 씬에서 자신의 전투캐릭터를 설정하는 스크립트

public class BattleCharacter : MonoBehaviour
{
    // 싱글톤 접근용 프로퍼티
    public static BattleCharacter instance { get { if (m_instance == null) { m_instance = FindObjectOfType<BattleCharacter>(); } return m_instance; } }
    private static BattleCharacter m_instance;

    public float Hp {
        get { return hp; }
        set { hp -= value; }
    }
    // 충돌한 캐릭터 개수
    private int crash_charac = 0;
    // 체력바 이미지
    public Image hp_img;
    // 스탯
    private float hp = 0.0f;
    private float hp_max = 0.0f;
    private float ap = 0.0f;
    // 좌표
    private Vector3 endpos = new Vector3(0, 12, -1); // 도착 위치 (초기에 0값을 어케 넣지?)
    // 움직임
    private float walk_speed = 0.02f;               // 이동 속도
    Animator animator;                              // 걷기/공격 애니메이션

    // 전투캐릭터 스탯 설정
    public void Setup(float ap, float hp, RuntimeAnimatorController ani_controller) {
        // 전투캐릭터의 스탯을 옮김
        this.hp = hp;
        hp_max = hp;
        this.ap = ap;
        //this.endpos = endpos;

/*        // 전투캐릭터의 공격범위 설정
        gameObject.AddComponent<BoxCollider2D>().isTrigger = true;
        BoxCollider2D setting = gameObject.GetComponent<BoxCollider2D>();
        setting.offset = new Vector2(0.0f, coll_offset);
        setting.size = new Vector2(6.35f, coll_size);*/

        // 자식 오브젝트 태그 변경
        transform.GetChild(1).gameObject.tag = "AllyAttackedRange";

        // 전투캐릭터의 움직임
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = ani_controller;        //해당 캐릭터에 맞는 애니메이션 컨트롤러를 설정
    }
    void Update() {
        if (animator.GetInteger("state") == 0) {    // animator.GetInteger("state") == 0이면 이동 애니메이션이 실행되므로
            Move();
        }
    }
    // 캐릭터 공격 범위 설정
    void ChangeCollider(Vector2 off, Vector2 size)
    {
        gameObject.AddComponent<BoxCollider2D>().isTrigger = true;
        BoxCollider2D setting = gameObject.GetComponent<BoxCollider2D>();
        setting.offset = off;
        setting.size = size;
    }
    // 도착 좌표를 변경
    void ChangeEndPos(Vector3 end)
    {
        endpos = end;
    }
    // 캐릭터 움직이기
    void Move() {
        transform.position = Vector3.MoveTowards(transform.position, endpos, walk_speed);// 동일? transform.Translate(Vector3.up * walk_speed);
    }
    // 캐릭터 체력 표시
    void UpdateHpBar() {
        hp_img.fillAmount = hp / hp_max;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("EnemyAttackedRange")) {   // 자신의 공격범위와 상대방의 몸이 닿은 경우
            walk_speed = 0.0f;                      // 우선 걸음을 멈추고
            crash_charac++;                         // 충돌한 캐릭터의 수를 증가
            Attack_Start_Animation();               // 공격 애니메이션 실행
            if (collision) {                        // NullReferenceException: Object reference not set to an instance of an object오류를 막기 위해
                StartCoroutine(Attack(collision.transform.parent.GetComponent<Collider2D>()));  // 상대캐릭터의 RangeReach의 콜라이더와 충돌한 것이므로 그 부모 오브젝트의 콜라이더를 매개변수로 사용
            }
        }
        else if (collision.CompareTag("Enemy")) {   // 자신의 공격범위와 상대방의 성이 닿은 경우
            walk_speed = 0.0f;                      // 우선 걸음을 멈추고
            crash_charac++;                         // 충돌한 캐릭터의 수를 증가
            Attack_Start_Animation();               // 공격 애니메이션 실행
            if (collision) {
                StartCoroutine(Attack(collision));
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision) {            // Generate Area의 콜라이더가 다른 콜라이더와 충돌을 끝냈을 시
        if (collision.CompareTag("EnemyAttackedRange")) {    // 자신의 공격범위와 상대방의 몸의 충돌이 끝난 경우
            crash_charac--;                                 // 상대방 캐릭터가 죽었다는 의미이므로 충돌된 캐릭터 수를 감소

            if (crash_charac == 0) {                // 충돌한 캐릭터의 수가 0일 경우를 확인하여 충돌된 캐릭터가 모두 죽어야 이동을 시작할 수 있게 한다
                walk_speed = 0.02f;
                Attack_End_Animation();
            }
        }
    }
    public void Attack_Start_Animation() {
        animator.SetInteger("state", 1);    // 이동 애니메이션을 공격 애니메이션으로 변경
    }
    public void Attack_End_Animation() {
        animator.SetInteger("state", 0);    // 공격 애니메이션을 이동 애니메이션으로 변경
    }
    // 공격당하여 자신의 체력 감소
    public void Attacked(float ap) {                  // 상대 전투캐릭터 프리팹에서 공격하여 해당 메소드를 실행해 체력을 감소
        hp -= ap;
        //Debug.Log(gameObject.name + "의 hp가 " + hp + "가 됨");
        //Debug.Log(gameObject.name + "가 맞음");
        UpdateHpBar();
        if (hp <= 0.0f) {
            Destroy(gameObject);
            //Debug.Log(gameObject.name + "(이)가 사망");
        }
    }
    // 공격 코루틴 함수
    private IEnumerator Attack(Collider2D collision) {     // 전투캐릭터의 공격속도를 정하기 위해 코루틴 함수 사용
        while (collision) { // 상대방 콜라이더가 아직 존재하면(상대방 전투캐릭터가 아직 살아 있다면)
                            //Debug.Log(gameObject.name + "(이)가 " + collision.gameObject.name + "(을)를 공격하여 " + ap + "의 데미지를 입힘");

            if (collision.CompareTag("2")) {
                collision.gameObject.GetComponent<EnemyBattleCharacter>().Attacked(ap);             // Attacked함수 실행
            }
            else if (collision.CompareTag("Enemy")) {
                collision.gameObject.GetComponent<Castle>().Attacked(ap);             // Attacked함수 실행
            }

            yield return new WaitForSeconds(1.0f);                                              // 1.0초 대기
        }
    }
}

// 문제점? 고민사항
/*
Character_info 배열을 그냥 6개로 설정해 버렸다
~~~CharacterController에서 접근하기 위해 Character_info 구조체가 public으로 선언 되어있다
AddCharacter_Info 메서드가 깔끔하지 않다

 
*/
