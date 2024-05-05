# pokemonstyle
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
- IEnumerator는 어떤 컬렉션을 하나에 element마다 반복해서 접근해서 yield하는 형태로 구성이되므로
- 지금 완벽하게 이해는 안가지만.. 어떻게 활용할지는 알 것 같다.

## Property


