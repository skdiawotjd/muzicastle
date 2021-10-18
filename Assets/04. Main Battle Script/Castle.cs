using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    private float hp = 0.0f;
    
    void Start() {
        hp = 100.0f;// 시작 시 성의 체력을 따로 설정해야 한다.
    }
    public void Attacked(float ap) {                  // 상대 전투캐릭터 프리팹에서 공격하여 해당 메소드를 실행해 체력을 감소
        hp -= ap;
        Debug.Log("성의 체력 : " + hp);

        if (hp <= 0.0f) {
            Destroy(gameObject);
        }
    }
}
