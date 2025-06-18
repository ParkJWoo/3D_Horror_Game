using UnityEngine;

public static class Constants
{
    //씬이 시작되면 나오는 텍스트. 게임의 인트로
    public static readonly string[] introText =
    {
        "여기는 어디지...", "...젠장...또 그 꿈인가...", "몇 번을 꿔도 익숙해지지 않는다. 점점 꿈인지 현실인지 구분이 어려워지고 있다.",
        "꿈에서 깨어나는 방법은 단 한가지. 이 집을 벗어나는 것 밖에는 없다.", "일단 주위를 둘러보자.", 
        "방이 너무 어둡다. 빛을 밝힐만한 게 있는지 찾아봐야겠다."
    };
    // 손전등을 얻으면 사용법을 알려주는 가이드
    public const string getFlashlight = "손전등을 얻었다. F를 눌러 켜보자.";
    // 속삭임 트리거가 작동되면 출력할 텍스트
    public const string whisperGuiedText = "소리가 들려올 때 마다 머리가 깨질 듯이 아파온다... 빨리... 그 사진을 찾아야 해... 그렇지 않으면...";
    //사진과 상호작용을 하면 출력할 텍스트
    public static readonly string[] picture =
    {
        "이 사진을 볼 때면 항상 애틋한 마음이 든다.", "동생이 보고싶다."
    };
    //소비 아이템을 얻으면 출력할 텍스트
    public const string getItem = "사용할 수 있을 것 같다. Tab을 눌러 확인해보자.";


}