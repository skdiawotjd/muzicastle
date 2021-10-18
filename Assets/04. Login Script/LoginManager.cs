using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public Button start_btn;
    public WebSocketManager websocket;
    private int state = 1;

    private SPUM_Prefabs spum_prefab;
    private GameObject g_o;
    void Start()
    {
        Debug.Log("------------------Login Scene Start------------------------");
        // 로그인 버튼 비활성화
        start_btn.gameObject.SetActive(false);

        /*
        */
        //Create();
        /*
        */
    }
    void Create()
    {
        /*
        */
        /*g_o = Instantiate(Resources.Load("SPUM/SPUM_Units/Unit000")) as GameObject;
        g_o.transform.localScale = new Vector3(4, 4, 1);
        g_o.transform.localPosition = Vector3.zero;
        spum_prefab = g_o.GetComponent<SPUM_Prefabs>();
        spum_prefab.PlayAnimation(4);*/
        /*
        */
    }
    // 매 순간마다 현재 상태를 확인하여 특정 메소드 실행
    void Update()
    {
        //Debug.Log("Login state 확인 : " + state);
        switch(state)
        {
            case 0:
                //Debug.Log("완료");
                break;
            case 1:
                Login();
                break;
            case 2:
                Login_Check();
                break;
            case 3:
                Login_State();
                break;
        }
    }

    // WebSocketManager 오브젝트로 id를 넘겨주는 메소드
    void Login()
    {
        // 서버로 아이디를 전송
        Debug.Log("서버로 adminid 전송");
        
        websocket.Login_Start();
        state = 2;
    }
    //  WebSocketManager 오브젝트의 state를 가져오는 메소드
    void Login_Check()
    {
        if (websocket.Login_Middle() == 3)
        {
            state = 3;
        }
        Debug.Log("update된 state " + state);
    }
    // 버튼의 상태를 변경하는 메소드
    public void Login_State()
    {
        Debug.Log("버튼 활성화");
        start_btn.gameObject.SetActive(true);
        // Update문에서 더 이상 어떤 메소드도 실행하지 못하게 state를 0으로 변경
        state = 0;
    }
    // start_button을 클릭 시 Stage Select 씬으로 넘어가는 메소드
    public void Scene_Move()
    {
        // 캐릭터 선택 씬 로드
        Debug.Log("Stage Select씬을 로드");
        SceneManager.LoadScene("Stage Select");
    }

}
