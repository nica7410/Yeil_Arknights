using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SomethingStart : MonoBehaviour
{
    private string sceneName;   //현재 객체의 이름을 받아와 객체의 이름과 동일한 씬을 로드한다.
                                //씬들 이름을 바꿔야 함

    public GridLayoutGroup Squads;
    // Start is called before the first frame update
    void Start()
    {        
        sceneName = this.name;
    }

    public void CombatScene()
    {
        //SceneManager.LoadScene(sceneName);
        SceneManager.LoadScene("SampleScene");//1-1메인
    }

    public void OperationSquadSelectPadding()//스테이지 메뉴에서 작전 개시 버튼을 눌렀을 때 패딩 값
    {
        Squads.padding.left = 30;
    }
    public void SquadSelectPadding()//전체 메뉴에서 스쿼드 선택시 패딩 값
    {
        Squads.padding.left = 120;
    }
}
