# pokemonstyle

![pokemon](https://github.com/Jor20230107/pokemonstyle_20240427/assets/122156670/f70597bd-7e7e-44b5-8ee8-124727f49867)


2024-05-05

개인적인 관심으로 게임개발을 하고싶었다.   
어떻게 시작할지 고민이었는데, 이미 경험이 있던 게임업계의 친구에게 물어보니,   
유명하고 잘 알고 있는게임을 카피개발하면서 시작하면 많은 걸 배울 수 있다고 조언해줬다.   
그래서 내가 게임시스템을 잘 이해하고 있던 포켓몬스터 게임을 COPY 개발하려고 했다.   
다행히 유튜브에 관련 자료가 있어 편하게 따라하면서 작업했다.   
아래 유튜브 자료의 ~13강까지의 내용이 반영된 레파지토리이다.   
https://youtu.be/_Pm16a18zy8?si=qU_AQRTAsOyS_Uo1

애초에 목적이 해당 강의를 다 따라한다(X) 게임개발할 때 UNITY와 C#에서 쓰는 기본적인 기능들을 이해한다(O) 였기 때문에,   
현시점에서 잠시 STOP하고 여태까지 개발한 사항을 정리하면서 복기해본다.

# UNITY C# SCRIPT
## Unity object에 컴포넌트 더하기
- 기본적으로 내가 작성한 스크립트가 unity로 만든 게임에서 상호작용하고 실행되기 위해서는
- unity object에 스크립트를 컴포넌트로 추가해야한다
- 또한 스크립트에서는 public 변수만 원래 unity inspector에 노출되어서 접근할 수 있는데,(직렬화)
- - 직렬화는 개체의 상태를 나중에 저장, 전송 또는 재구성할 수 있는 형식으로 변환하는 프로세스입니다. 유니티에서 직렬화는 게임 상태를 저장 및 로드하거나 에디터와 런타임 간에 데이터를 전송하는 데 사용됩니다.   (출처 : https://www.ibatstudio.com/%EC%9C%A0%EB%8B%88%ED%8B%B0-serializefield-%EB%9E%80-%EB%AC%B4%EC%97%87%EC%9D%B4%EA%B3%A0-%EC%99%9C-%EC%82%AC%EC%9A%A9%ED%95%98%EB%8A%94%EA%B0%80/)
- [SerializeField]를 앞에 붙이면 private 변수도 직렬화 할 수 있게 된다.
- 반대로 스크립트 안에서 transform과 같은 unity inspector의 값도 받아오기도, 반대로 변경할 수도 있다.(ex. 키보드로 움직이게 할때)
- 
## public class BattleSystem : MonoBehavior
- unity에서 c# 스크립트를 생성하면 디폴트로 위 클래스가 생성이 된다.   
- start와 update 메소드로 구성되며,
- start는 해당 객체가 실행될때 실행되고, 이후 update는 frame단위로 실행된다.
- - frame 단위보다 더 짧은단위로 실행하고싶다면?  
  - IEnumerator를 생성하고
  - StartCoroutine으로 실행한다.
- 저 :이 C#에서 상속을 의미하는 것인가? -> 찾아보니 맞네
- MonoBehavior를 상속하지않고 일반 클래스로 작성해서 활용하는 것도 당연히 가능하다.

## Coroutine, IEnumerator
- MonoBehavior 상속클래스의 update 메소드보다 더 짧은단위로 실행하고자 할때 활용
- - coroutine : Coroutines in Unity allow for executing code over several frames, which is useful for gradual movements or animations
- 일반적인 메소드에서 coroutine을 실행시키려면
- - StartCoroutine메소드로 실행시킨다
  - coroutine 안에서 다른 coroutine으로 넘어가려면, 다시 StartCoroutine으로 실행해야 넘어간다.
  - coroutine 안에서 다른 coroutine을 실행하고 (돌아와서) 나머지 구문도 실행해야하면 yield return으로 호출한다
  - - yield return : statement allows the coroutine to yield control back to Unity until the next frame.
  - coroutine 안에서 다른 메소드를 실행시킬 때는 그냥 호출하면된다.
- IEnumerator는 어떤 컬렉션을 하나에 element마다 반복해서 접근해서 yield하는 형태로 구성이되므로 원래는 반복문의 형태로 구현되어야 할듯하다.(python에서 Enumerate처럼)
- 지금 완벽하게 이해는 안가지만.. 어떻게 활용할지는 알 것 같다.

## Property
- Property는 클래스의 멤버 변수를 외부에서 접근해서 읽기 및 쓰기를 할 수 있도록 도와주는 것
- python의 dataclass를 생각하면 쉬운데,
- 그래서 멤버변수를 선언할때 property로 활용하고자 한다면, 변수선언 시에 get; set;을 통해 읽기, 쓰기를 기능을 추가한다.
- 생성자 만들때, java에서 this.x =x 뭐 이런거랑 비슷한 역할인듯.

## Subsribe event
- 게임에서는 이벤트의 흐름에따라 여러 컴포넌트들이 호출되고 실행되게 되는데, '상호참조'가 발생할 경우 문제가 될 수 있다
- - 어떤 문제? 무한반복? 아무튼 그래서
- 상위 개체에서 event를 구독하고 뿌려주는 방법을 채택하게 된다
- monobehaviour를 상속한 상위 컨트롤러에서, 하위 개체를 직렬화로 가져오고 start 메소드에서 하위 개체의 이벤트 시에 메소드를 실행하도록 작성한다.
- (하위는 그냥 상위에서 실행당하면 되는 것)
- 아래와 같이 구현
- 하위개체.어떤event += 실행할 메소드; 
- 그리고 start 메소드 밖에서 실행할 메소드를 구현
- 이때 GameState를 enum으로 같이 관리해주면 event 구현에 도움이 된다. (왜?)
