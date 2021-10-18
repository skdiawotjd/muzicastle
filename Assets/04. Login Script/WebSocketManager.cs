using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class WebSocketManager : MonoBehaviour
{
    private WebSocket m_WebSocket;                                  // 서버를 위한 변수
    private int state = 0;                                          // 서버와 통신할 때 클라이언트에서 정상적으로 통신할 준비가 되었는지를 확인하기 위한 변수
    public User_Info user_info = new User_Info();                  // 서버에서 받은 user 정보를 담을 변수

    // 서버에서 받은 user data를 저장할 클래스 서버와 통신하여 새로 데이터를 받지 않는 이상 모든 씬에서는 해당 클래스의 객체에 접근하여 user data를 확인한다
     public class User_Info
    {
        // user_character_info[i]는 각 캐릭터, user_character_info[i][0] - 캐릭터 code, user_character_info[i][1] - 캐릭터 position, user_character_info[i][2] - 캐릭터 ap, user_character_info[i][3] - 캐릭터 hp
        private List<List<int>> user_character_info = new List<List<int>>();

        // 다른 스크립트에서 캐릭터 수를 확인하고자 할 때 캐릭터 수를 반환
        public int Print_User_Character_Count()
        {
            return user_character_info.Count;
        }
        // 다른 스크립트에서 특정 캐릭터의 코드를 통해 해당 코드 캐릭터의 Hp를 확인하고자 할 때 캐릭터 Hp를 반환
        public int Print_User_Character_Hp(int character_code)
        {
            return user_character_info[character_code][3];
        }
        // 다른 스크립트에서 특정 캐릭터의 코드를 통해 해당 코드 캐릭터의 Ap를 확인하고자 할 때 캐릭터 Ap를 반환
        public int Print_User_Character_Ap(int character_code)
        {
            return user_character_info[character_code][2];
        }
        // 다른 스크립트에서 특정 캐릭터의 코드를 통해 해당 코드 캐릭터의 position을 확인하고자 할 때 캐릭터 position을 반환
        public int Print_User_Character_Position(int character_code)
        {
            return user_character_info[character_code][1];
        }
        // 다른 스크립트에서 특정 캐릭터의 코드만 확인하고자 할 때 캐릭터 code를 반환
        public int Print_User_Character_Code(int character_count)
        {
            return user_character_info[character_count][0];
        }

        // 서버에서 보낸 유저의 캐릭터 정보는  {"code":"0","position":"0","ap":"10","hp":"100"} 처럼 되어있기 때문에 이를 분리하여 list에 저장
        public void Add_Character_Info(string character_info)
        {
            Debug.Log("캐릭터 정보 저장 메소드 실행");
            int index = 0;
            string tem_str = "";
            List<int> tem_list = new List<int>();

            while (index != -1) {
                // 한번 사용 후 이전 값을 지우기 위해 초기화
                tem_str = "";
                // 값이 나올 위치가 :" 2자리 뒤에 나오므로 +2
                index += 2;

                // 다음에 나오는 문자가 숫자일 경우 계속 임시 str에 추가
                while (char.IsDigit(character_info[index])) {
                    tem_str += character_info[index++];
                }

                // 추출된 값(tem_str)이 있다면 tem_list에 추가
                if (!tem_str.IsNullOrEmpty()) {
                    tem_list.Add(int.Parse(tem_str));
                }

                // 현재 index에서 다음에 나오는 :"의 위치를 찾기
                index = character_info.IndexOf(":\"", index);
            }
            // while문을 나오게 되면 한 캐릭터의 정보가 tem_list에 전부 들어가게 되어 이를 한번에 관리하기 위해 2차원 list에 저장
            user_character_info.Add(tem_list);

            for (int i = 0; i < user_character_info.Count; i++) { for (int k = 0; k < user_character_info[i].Count; k++) { Debug.Log("user_character_info[" + i + "][" + k + "] = " + user_character_info[i][k]); } }
        }

    }


    void Start()
    {
        Debug.Log("------------------Server Start------------------------");
        // 서버와 통신 준비
        m_WebSocket = new WebSocket("ws://skdiawotjd.iptime.org:11055");
        m_WebSocket.Connect();
        // 앞으로 해당 오브젝트를 통해 서버와 통신하기 위해 파괴를 막음
        DontDestroyOnLoad(gameObject);
        // 로그인을 확인하지 않은 상태이므로 시작 버튼 비활성화
        //btn_setting(state);

        // 앞으로 모든 씬에서 해당 위치를 통해 서버와 통신하게 되므로 각 씬의 이름을 통해 특정 기능을 실행
        m_WebSocket.OnMessage += (sender, e) => 
        {
            Debug.Log("서버에서 받은 데이터 " + e.Data);

            // 서버에 보낸 아이디와 서버에 저장된 아이디가 동일한 경우 1을 반환
            switch (state)
            {
                case 1 :                                               // 서버에 동일한 아이디가 있는지
                    // 서버에서 보낸 값이 1인 경우 동일한 id가 있다고 판단
                    if(e.Data[0] - '0' == 1)
                    {
                        Debug.Log("서버에서 확인 결과, 동일한 id가 있음");
                        // id확인을 완료하였으므로 서버에서 user 정보를 받는 상태로 바꾸기 위해 state를 2로 변경
                        state = 2;
                    }
                    break;
                case 2 :                                               // 서버에서 user 정보 받기 (캐릭터 존재 여부)
                    Debug.Log("서버에서 받아온 user의 정보를 저장 하기");
                    // 서버에서 받아온 값의 첫번째가 숫자가 아니라면 user 정보를 받은 것이고
                    if (!char.IsDigit(e.Data[0]))
                    {
                        Extract_Json(e.Data, state);
                    }
                    // 숫자가 맞다면 서버에서 값을 전부 보냈다는 신호를 받은 것
                    else
                    {
                        Debug.Log("서버에서 받은 신호 " + e.Data);
                        // 서버에서 user 정보를 모두 보낸 후 마지막으로 3을 보낸다.
                        // 서버에서 user 정보를 받기를 완료하였으므로 Login씬에서는 모든 통신이 완료되었다는 의미로 state를 3으로 변경
                        state = int.Parse(e.Data);
                    }
                    break;
                default:
                    Debug.Log("하는거 없음");
                    break;
            }
        };
    }


/*
// Login씬에서 시작버튼을 관리하기 위한 함수 
*/
    // 로그인을 시작하는 메소드
    public void Login_Start()
    {
        // 현재 상태는 클라이언트에서 보낸 id와 서버에 저장된 id를 확인하는 상태라는 의미로 state를 1로 변경
        state = 1;
        // 서버로 id 전송
        m_WebSocket.Send("admin");
        // 해당 id를 가진 user 정보를 가져오기
        Save_Json();

    }
    // LoginManager에서 서버와 정상적으로 통신되었는지를 확인하기 위해 state를 확인하는 메소드
    public int Login_Middle()
    {
        return state;
    }


/*
// 여러 씬에서 서버와 통신하기 위한 함수 
*/
    // 서버에 저장된 json을 임시로 클라 json에 저장하기 위해 서버에 신호("0")을 보내는 함수
    void Save_Json() {
        // 서버에 저장된 jSON의 데이터를 가져오기 위해 서버에서 구분할 수 있는 값을 전송
        Debug.Log("서버에 0 전송");
        m_WebSocket.Send("0");

    }
    // 서버에서 가져온 유저의 캐릭터 수를 확인하여 user_character_exist 변수에 저장
    void Extract_Json (string user_character_num, int cnt) {
        user_info.Add_Character_Info(user_character_num);
    }
}


/*
앞으로 서버와 통신을 하기 위한 스크립트
모든 씬에서 해당 스크립트를 사용하므로 scene_name과 state를 통하여 구분


User_Info 클래스
서버에서 가져온 사용자의 정보를 list의 형태로 저장 후 값 변경 전까지 서버와 통신하지 않고 해당 값을 이용
Add_Character_Exist - 캐릭터 존재 여부(str 형태)를 분리하여 list에 저장
Add_Character_Info  - 캐릭터 정보(str 형태)를 분리하여 2차원 list에 저장


여러 곳에서 사용될 수 있는 메서드
Start()         - 서버에서 데이터를 받은 경우 scene_name을 통해 다른 코드를 실행
Update()        - state를 확인하여 통신 가능한 상태인 경우 서버로 데이터를 전송
Scene_Rename()  - 현재 씬의 이름을 확인하기 위해 현재 씬의 이름을 저장
Save_Json()     - 서버에서 유저의 캐릭터 존재 여부를 받기 위해 특정 신호("0")을 전송
Extract_Json()  - 서버에서 받은 캐릭터 존재 여부가 string이므로 이를 구분하여 list에 저장


Login씬에서 사용하는 메서드
btn_setting()   - 시작 버튼 조작
Login()         - 서버로 사용자 id 전송
Scene_Move()    - 다음 씬 이동


특정 씬에서 사용하는 state 상태
Login씬 - Update [ 0(대기), 1(로그인), 2(사용자 정보) ], Start [ 10(로그인 상태 수신), 20(캐릭터 정보 수신) ]

캐릭터 자체 정보를 저장하는 JSON 열기



WebSocket 하는 방법
https://coderzero.tistory.com/entry/%EC%9C%A0%EB%8B%88%ED%8B%B0-%EB%A7%A4%EB%89%B4%EC%96%BC-Nodejs-%EC%84%9C%EB%B2%84%EC%99%80-%EC%9C%A0%EB%8B%88%ED%8B%B0-%ED%81%B4%EB%9D%BC%EC%9D%B4%EC%96%B8%ED%8A%B8%EA%B0%84%EC%9D%98-socketio-%EC%9B%B9%EC%86%8C%EC%BC%93Websocket-%ED%86%B5%EC%8B%A0
WebSocket의 try catch문
https://timeboxstory.tistory.com/69
WebSocket외에 다른 방법
https://meetup.toast.com/posts/112
json 파일을 읽는 방법
https://gupu.tistory.com/74

json 사용법
json 배열을 문자열로 변환 - JSON.stringify(user_data.character)
json의 요소 수 - Object.keys(user_data).length


추가 사항
서버 연결 시 try catch문 사용해서 버그 방지
*/
