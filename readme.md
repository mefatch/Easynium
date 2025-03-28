# Easynium
Easynium은 2시간 정도 간단히 만들어본 비전공자를 위한 웹제어 언어입니다. 가볍게 쓸 용도로 개발하였습니다.

[Release Download](https://github.com/mefatch/Easynium/releases/tag/Release) 왼쪽 링크를 클릭하시면 Release 탭으로 가서 다운로드가 가능합니다.

# Grammar
우선 Easynium 대부분의 문법은 ```<명령어> <인자1> <인자2> ...```의 구조를 갖고 있습니다. 각 블럭은 띄어쓰기로 구분됩니다. 예를 들면
```
input 바보 on id: InputIdBox
wait 3000
click 400 500
(...)
```
와 같은 방식입니다. 다만 만약 한 블럭임에도 띄어쓰기가 들어갈 경우, 작은따옴표 ```'묶을 문자열'```로 묶어주시면 됩니다. 예를 들면
```
input '나는 띄어쓰기가 좋아' on xpath: '//item[text()="hello world"]'
```
처럼 쓰시면 됩니다. 만약 문자열 속 ```'```가 들어가야 한다면 ,```\'```를 쓰시면 됩니다. 보통 키보드 [Enter] 키 위에 있습니다.

또한 프로그램은 Tab키나 스페이스바 등으로 들여쓰기를 해도 잘 작동합니다. Tab키를 하면 자동으로 띄어쓰기 4번이 삽입되니 ```if```문이나 ```loop``` 문을 사용할 때 활용하시면 좋습니다.

세부적인 명령어와 문법은 아래에서 자세히 알려드리겠습니다.
<br>

## 브라우저 실행, 종료
브라우저는 한 번에 하나의 브라우저만 제어할 수 있습니다. 이미 아래의 ```chrome``` 명령어로 크롬 제어가 시작되었는데, 이후 또다시 ```chrome```나 ```edge``` 명령어를 실행하시면 안 됩니다.

```quit```로 닫고 다시 여시거나, ```tab``` 명령어로 다른 탭을 열어 제어하셔야 합니다. 추후 여러 브라우저를 제어할 수 있도록 개발도 고려 중입니다.
### chrome, edge
```
chrome <profile_number>
```
```
edge <profile_number>
```
제어할 브라우저를 실행합니다. \<profile_number\>는 몇 번째 브라우저 계정으로 열지를 의미합니다. 비워두면 계정 없이 열립니다.

각 브라우저의 계정과 Profile 번호를 보고 싶으시다면, Easynium 창에서 **Chrome Profile** 또는 **Edge Profile**을 누르시면 목록과 번호를 보여줍니다.

예를 들어 example@gmail.com로 로그인된 Profile 3 크롬창을 제어하고 싶다면, ```chrome 3```으로 시작하시면 됩니다.

<br>

* **단, 브라우저 계정의 세션은 하나만 열려있을 수 있습니다. 이미 열려있는 계정 창을 새로 열어 제어하려고 시도할 경우, 프로그램이 중단될 수 있습니다.**
* **이를 방지하기 위해 제어할 크롬 계정 창은 미리 닫고 활용하시기 바랍니다.**

***

### quit 구문
```
quit
```
제어 중인 브라우저를 종료합니다. 다시 제어하려면 위의 ```chrome```, ```edge``` 명령어를 활용해야 합니다.

<br>

## 논리 제어
### loop 구문
```
loop <number>
    <number> 번만큼 반복할 코드는 여기에..
end
```
```
loop forever
    무한 반복할 코드는 여기에..
end
```
특정 코드들을 반복할 수 있는 제어문입니다. 깜빡하고 break 구문을 구현하지 않았습니다. 추후 추가하겠습니다. Tab키나 스페이스바로 들여쓰기를 활용할 수 있으며, 필수는 아닙니다.

***
### if 구문
```
if <element_type>: <web_element> <operator> <값>
    처리할 코드는 여기에..
end
```
```
if <value1> <operator> <value2>
    처리할 코드는 여기에..
end
```

<br>

웹 요소 비교 ```if```구문 예시:
```
if xpath: '//*[@id="shortcutArea"]/ul/li[1]/a/span[2]' != '메일'
    처리할 코드는 여기에..
end
```
웹의 특정 요소의 값과 비교하여 참이면 수행하는 코드입니다. '' 는 띄어쓰기가 있을 때 저렇게 묶어주면 됩니다. 위의 예시에서는 띄어쓰기가 없으니 사실
```
if xpath: //*[@id="shortcutArea"]/ul/li[1]/a/span[2] != 메일
    처리할 코드는 여기에..
end
```
이렇게 써도 무방합니다. 웹요소의 경우,
* 웹 타입 \<element_type\>은 ```xpath:```, ```id:```, ```css:```, ```class:```를 사용할 수 있습니다.
* 비교 연산자 \<operator\>은 ```=```, ```!=```를 사용할 수 있습니다.
 
<br>

반면 앞에 \<element_type\>을 지정하지 않은 경우, 일반 ```if``` 구문으로 처리합니다.
```
if $var1 >= 10
    msg '와 10보다 크네유'
end
```
웹 속 값의 비교가 아닌 일반 ```if``` 구문의 경우,
* 비교 연산자 \<operator\>은 ```=```, ```!=```, ```>```, ```<```, ```>=```, ```<=```를 사용할 수 있습니다.
* 참고로 ```>```, ```<```, ```>=```, ```<=```로 비교하면 \<value1\>, \<value2\>는 자동으로 숫자로 변환됩니다.
* ```>```, ```<```, ```>=```, ```<=``` 비교에서 숫자로 변환 불가능하면 ```if```문은 ```false```로 처리됩니다.

<br>

## 일반 제어
### wait 구문
```
wait <number>
```
```
wait HH:MM:SS <tick>
```
\<number\> 밀리초 동안 대기하거나, HH:MM:SS 까지 대기하는 명령어입니다. 만약 3초 동안 기다리고 싶다면 ```wait 3000```을 하시면 됩니다.

```wait HH:MM:SS <tick>```는 특정 시간까지 기다리는 명령어입니다. 해당 시간에 도달하였는지 \<tick\> 밀리초마다 확인합니다. \<tick\>이 입력되지 않으면 기본적으로 20밀리초마다 체크합니다. ```wait 22:00:00```이나, ```wait 13:30:00 13```과 같이 활용할 수 있습니다.

***

### refresh 구문
```
refresh
```
현재 제어 중인 브라우저 창을 새로고침합니다.

***

### go 구문
```
go <url>
```
현재 제어 중인 브라우저 창에서 특정 URL 주소로 이동합니다. ```go https://google.com``` 처럼 활용하실 수 있으며, http://, https:// 는 필수로 붙여주셔야 합니다.

***

### click 구문
```
click <element-type>: <element>
```
```
click <X> <Y>
```
click 명령어는 두 가지 방식으로 활용하실 수 있습니다. 제어 중인 웹 브라우저의 특정 요소를 클릭하거나, 화면의 특정 좌표를 마우스 제어로 클릭할 수 있습니다. 예를 들어 ```click xpath //*[@id="shortcutArea"]/ul/li[1]/a/span[2]```를 하면 해당 웹페이지의 저 xpath 요소를 클릭하며, ```click 400 500```을 하면 화면의 (400, 500)을 클릭합니다. 마우스의 현재 좌표는 Easynium이 UI상에 실시간으로 보여주니 참고하시면 될 것 같습니다.

클릭할 웹 요소에 띄어쓰기가 들어갈 경우 위에도 설명한 것처럼 ```click xpath: '//*[@id="shortcut Area"]/ul/li[1]/a/span[2]'``` 와 같이 작은 따옴표로 묶어주시면 됩니다.
* 웹 타입 \<element_type\>은 ```xpath:```, ```id:```, ```css:```, ```class:```를 사용할 수 있습니다.

***

### input 구문
```
inpnut <input-string> on <element-type>: <element>
```
```
input <input-string>
```
input 명령어도 두 가지 방식으로 쓰일 수 있습니다. 첫 번째는 웹 요소를 지정하여 입력하는 것입니다. ```input '피자 만드는 법' on xpath: '//*[@id="example xpath"]/ul/input'``` 처럼 쓰면 해당 xpath 요소에 ```피자 만드는 법```을 입력하게 되며, ```input '피자 만드는 법'```이라고만 하면 키보드 제어로 그냥 해당 단어를 입력합니다.
* 웹 타입 \<element_type\>:은 ```xpath:```, ```id:```, ```css:```, ```class:```를 사용할 수 있습니다.

이 구문의 예시는 띄어쓰기가 있기에 작은따옴표로 묶었을 뿐, ```input 피자```도 당연히 가능합니다.

***

### clip 구문
```
clip <복사할_텍스트>
```
```clip```은 클립보드에 텍스트를 넣는 명령어입니다. ```clip 피자```나, ```clip '피자 만드는 법'```과 같이 활용 가능합니다. 추후 ```ctrl v``` 등을 통해 붙여넣을 수 있습니다.

***

### tab 구문
```
tab add
```
```
tab last
```
```
tab <number>
```
```
tab <number> close
```
```tab```은 웹제어 시 탭을 만들거나 닫고, 이동할 수 있는 명령어입니다. 좀 많지만 뜯어보면 매우 간단합니다.
* ```tab add```의 경우 새 탭을 만듭니다.
* ```tab last```의 경우 마지막 탭으로 이동합니다. 새 탭은 마지막에 생기므로, 새 탭을 만들고 이동할 때도 쓸 수 있습니다.
* ```tab <number>```의 경우 \<numer\>번째 탭으로 이동합니다.
* ```tab <number> close```의 경우 \<numer\>번째 탭을 닫습니다.

***

### msg 구문
```
msg <문자열>
```
\<문자열\>을 메시지박스로 띄웁니다. 띄어쓰기가 있으면 작은따옴표 ```'```로 묶어주면 됩니다.
ex) ```msg '안녕하세요 반갑습니다!'```

<br>

## 변수 처리
### var 구문
```
var <variable_name> <variable_value>
```
```
var <varuable_name> <element_type>: <web_element>
```
var은 변수를 만들고 수정하는 코드입니다. ```var var1 '피자 만드는 법'```으로 쓸 수도 있고, 웹 제어 중이라면 ```var var1 xpath: '//*[@id="shortcut Area"]/ul/li[1]/a/span[2]'```로 웹페이지 내 값을 가져올 수도 있습니다.
* 웹 타입 \<element_type\>:은 ```xpath:```, ```id:```, ```css:```, ```class:```를 사용할 수 있습니다.

추후 사용은 코드 내에서 $var1 과 같이 할 수 있습니다. ex) ```input $var1```

<br>

## 키보드 제어
사실 ```input```도 키보드 제어는 가능하지만, 웹 요소와 키보드를 둘 다 제어하여 일단 일반 제어 쪽에 두었습니다.
### enter 구문
```
enter
```
엔터키를 누릅니다.

***

### ctrl 구문
```
ctrl <key>
```
ctrl+\<key\>를 키보드로 누릅니다. 현재 ```ctrl```+```a```, ```b```, ```c```, ```s```, ```v```가 가능하며, 추후 모든 알파벳으로 확대할 계획입니다.
