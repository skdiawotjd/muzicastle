using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPUM_Prefabs : MonoBehaviour
{
    public float _version;
    public SPUM_SpriteList _spriteOBj;
    public bool EditChk;
    public string _code;
    public Animator _anim;

    public bool _horse;
    public string _horseString;

    /**/
    // 캐릭터 정보
    private int code = 0;
    private float hp = 0.0f;
    private float hp_max = 0.0f;
    private float ap = 0.0f;

    // 좌표
    private Vector3 endpos = new Vector3(0, 12, -1); // 도착 위치 (초기에 0값을 어케 넣지?)
    // 움직임
    private float walk_speed = 0.02f;               // 이동 속도

    public void Setup(float ap, float hp, int code)
    {
        this.code = code;
        // 전투캐릭터의 스탯을 저장
        this.hp = hp;
        hp_max = hp;
        this.ap = ap;
    }

    public int Code()
    {
        return code;
    }
    /**/

    public void PlayAnimation (int num)
    {
        switch(num)
        {
            case 0: //Idle
            _anim.SetFloat("RunState",0f);
            break;

            case 1: //Run
            _anim.SetFloat("RunState",0.5f);
            break;

            case 2: //Death
            _anim.SetTrigger("Die");
            _anim.SetBool("EditChk",EditChk);
            break;

            case 3: //Stun
            _anim.SetFloat("RunState",1.0f);
            break;

            case 4: //Attack Sword
            _anim.SetTrigger("Attack");
            _anim.SetFloat("AttackState",0.0f);
            _anim.SetFloat("NormalState",0.0f);
            break;

            case 5: //Attack Bow
            _anim.SetTrigger("Attack");
            _anim.SetFloat("AttackState",0.0f);
            _anim.SetFloat("NormalState",0.5f);
            break;

            case 6: //Attack Magic
            _anim.SetTrigger("Attack");
            _anim.SetFloat("AttackState",0.0f);
            _anim.SetFloat("NormalState",1.0f);
            break;

            case 7: //Skill Sword
            _anim.SetTrigger("Attack");
            _anim.SetFloat("AttackState",1.0f);
            _anim.SetFloat("SkillState",0.0f);
            break;

            case 8: //Skill Bow
            _anim.SetTrigger("Attack");
            _anim.SetFloat("AttackState",1.0f);
            _anim.SetFloat("SkillState",0.5f);
            break;

            case 9: //Skill Magic
            _anim.SetTrigger("Attack");
            _anim.SetFloat("AttackState",1.0f);
            _anim.SetFloat("SkillState",1.0f);
            break;
        }
    }
}
