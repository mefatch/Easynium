using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Text.RegularExpressions;
using OpenQA.Selenium.Interactions;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;
using WebDriverManager;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using OpenQA.Selenium.Edge;
using WindowsInput.Native;
using WindowsInput;
using System.Runtime.InteropServices;
using System.Collections;
using Newtonsoft.Json.Linq;

namespace Easynium
{
    public partial class Form1 : Form
    {
        //None Form 이동해주기
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();



        //None Form 크기 조정
        private const int HTLEFT = 10;
        private const int HTRIGHT = 11;
        private const int HTTOP = 12;
        private const int HTTOPLEFT = 13;
        private const int HTTOPRIGHT = 14;
        private const int HTBOTTOM = 15;
        private const int HTBOTTOMLEFT = 16;
        private const int HTBOTTOMRIGHT = 17;

        private const int WM_NCHITTEST = 0x84;
        private const int WM_SETCURSOR = 0x20;

        private const int RESIZE_HANDLE_SIZE = 10;


        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_NCHITTEST)
            {
                Point pos = PointToClient(new Point(m.LParam.ToInt32()));
                if (pos.X < RESIZE_HANDLE_SIZE && pos.Y < RESIZE_HANDLE_SIZE)
                {
                    m.Result = (IntPtr)HTTOPLEFT;
                    return;
                }
                else if (pos.X < RESIZE_HANDLE_SIZE && pos.Y > ClientSize.Height - RESIZE_HANDLE_SIZE)
                {
                    m.Result = (IntPtr)HTBOTTOMLEFT;
                    return;
                }
                else if (pos.X > ClientSize.Width - RESIZE_HANDLE_SIZE && pos.Y < RESIZE_HANDLE_SIZE)
                {
                    m.Result = (IntPtr)HTTOPRIGHT;
                    return;
                }
                else if (pos.X > ClientSize.Width - RESIZE_HANDLE_SIZE && pos.Y > ClientSize.Height - RESIZE_HANDLE_SIZE)
                {
                    m.Result = (IntPtr)HTBOTTOMRIGHT;
                    return;
                }
                else if (pos.X < RESIZE_HANDLE_SIZE)
                {
                    m.Result = (IntPtr)HTLEFT;
                    return;
                }
                else if (pos.X > ClientSize.Width - RESIZE_HANDLE_SIZE)
                {
                    m.Result = (IntPtr)HTRIGHT;
                    return;
                }
                else if (pos.Y < RESIZE_HANDLE_SIZE)
                {
                    m.Result = (IntPtr)HTTOP;
                    return;
                }
                else if (pos.Y > ClientSize.Height - RESIZE_HANDLE_SIZE)
                {
                    m.Result = (IntPtr)HTBOTTOM;
                    return;
                }
            }

            base.WndProc(ref m);
        }



        //===== 마우스 클릭 재현 =====
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, IntPtr dwExtraInfo);

        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;






        Thread Engine;
        bool IsSeleniumStarted = false;

        //Selenum_Chrome
        ChromeDriverService chrome_driverService = null;
        ChromeOptions chrome_options = null;

        EdgeDriverService edgeDriverService = null;
        EdgeOptions edgeOptions = null;
        //ChromeDriver driver = null;

        private IWebDriver driver = null;
        InputSimulator simulator = new InputSimulator();

        bool isClosing = false;
        bool Running = false;

        Thread MouseXY;
        bool MouseTrack = true;

        HashSet<string> class_element = new HashSet<string>
        {
            "xpath",
            "id",
            "class",
            "css"
        };
        Dictionary<string, object> vars = new Dictionary<string, object>();


        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            if (File.Exists("Logo.png"))
                Logo.Image = System.Drawing.Image.FromFile("Logo.png");

            if(File.Exists("prev.txt"))
            {
                string file = File.ReadAllText("prev.txt");
                if(File.Exists(file))
                {
                    FilePath.Text = file;
                    Code.Text = File.ReadAllText(file);
                }
            }

            MouseXY = new Thread(MouseTracker);
            MouseXY.SetApartmentState(ApartmentState.STA);
            MouseXY.Start();
        }

        private void x_Click(object sender, EventArgs e)
        {
            isClosing = true;
            System.Windows.Forms.Application.Exit();
        }

        private void __Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void 실행하기_Click(object sender, EventArgs e)
        {
            if(String.Equals(실행하기.Text, "Run"))
            {
                isClosing = false;
                Engine = new Thread(() => Interpreter(Code.Text));
                Engine.SetApartmentState(ApartmentState.STA);
                Engine.Start();
                실행하기.Text = "Stop";
            }
            else
            {
                isClosing = true;
                sublogo.Text = ": Canceling...";
                try
                {
                    if (driver != null)
                        driver.Quit();
                    IsSeleniumStarted = false;
                }
                catch { }

                while (Running)
                {
                    Thread.Sleep(100);
                }
                sublogo.Text = ": Easynium";

                실행하기.Text = "Run";
            }
        }

        public void MouseTracker()
        {
            Point mousePosition;

            while (MouseTrack)
            {
                mousePosition = Cursor.Position;
                XY.Text = $"X: {mousePosition.X}, Y: {mousePosition.Y}";

                Thread.Sleep(100);
            }
        }

        static List<string> SyntaxSplit(string input)
        {
            List<string> result = new List<string>();
            Regex regex = new Regex(@"\G(?:\s*'(?<quoted>(?:[^']|'')*)'|(?<unquoted>\S+))\s*");
            MatchCollection matches = regex.Matches(input);

            foreach (Match match in matches)
            {
                if (match.Groups["quoted"].Success)
                {
                    result.Add(match.Groups["quoted"].Value);
                }
                else if (match.Groups["unquoted"].Success)
                {
                    result.Add(match.Groups["unquoted"].Value);
                }
            }

            return result;
        }

        static void ShowStringListInMessageBox(List<string> stringList)
        {
            // 문자열 리스트를 하나의 문자열로 결합
            string message = string.Join(Environment.NewLine, stringList);

            // 메시지 박스 출력
            MessageBox.Show(message, "String List");
        }

        static string ExtractSubstring(string input, string start, string end)
        {
            // 유효성 검사
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(start) || string.IsNullOrEmpty(end))
            {
                return input;
            }

            int startIndex = input.IndexOf(start);
            if (startIndex == -1)
            {
                return input;
                //throw new ArgumentException("시작 문자열을 찾을 수 없습니다.");
            }

            startIndex += start.Length; // 시작 문자열 끝으로 이동
            int endIndex = input.IndexOf(end, startIndex);
            if (endIndex == -1)
            {
                return input;
                //throw new ArgumentException("끝 문자열을 찾을 수 없습니다.");
            }

            // 부분 문자열 추출
            return input.Substring(startIndex, endIndex - startIndex);
        }

        static void ShowProfileInfo(string localStatePath, string browser)
        {
            if (!File.Exists(localStatePath))
            {
                MessageBox.Show("Local State 파일이 존재하지 않습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Local State 파일 읽기
                string localStateContent = File.ReadAllText(localStatePath);

                // JSON 파싱
                var localStateJson = JObject.Parse(localStateContent);

                // 프로필 정보 추출
                var profiles = localStateJson["profile"]?["info_cache"];
                if (profiles == null)
                {
                    MessageBox.Show("프로필 정보를 찾을 수 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 메시지 내용 구성
                string message = $"{browser} 프로필 정보:\n\n";
                foreach (var profile in profiles.Children<JProperty>())
                {
                    string profileName = profile.Name; // 프로필 폴더 이름 (예: Profile 1, Default)
                    string displayName = profile.Value.SelectToken("name")?.ToString() ?? "Unknown"; // 표시 이름
                    string email = profile.Value.SelectToken("user_name")?.ToString() ?? "Unknown"; // 이메일

                    message += $"프로필: {profileName} | {displayName}\n";
                    message += $"이메일: {email}\n\n";
                }

                // 메시지 박스 출력
                MessageBox.Show(message, $"{browser} 프로필 정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void Interpreter(string code)
        {
            Running = true;
            sublogo.Text = ": Running...";

            string[] lines = code.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder builder; //변수치환용 $변수명
            string placeholder;//변수치환용 $변수명

            // 결과 출력
            for (int i = 0; i < lines.Length && !isClosing; i++)
            {
                string line = lines[i].TrimStart();

                foreach (var pair in vars)
                {
                    placeholder = $"${pair.Key}"; // $key1 형태의 문자열
                    builder = new StringBuilder(line);
                    builder.Replace(placeholder, Convert.ToString(pair.Value));
                    line = builder.ToString();
                }

                //MessageBox.Show(line);

                
                if (String.IsNullOrEmpty(line))
                {
                    continue;
                }

                var blocks = SyntaxSplit(line);

                if (String.Equals(blocks[0], "chrome"))
                {
                    //if(IsSelenium -- false)
                    IsSeleniumStarted = true;

                    try
                    {
                        if (driver != null)
                            driver.Quit();
                    }
                    catch { }

                    chrome_driverService = null;
                    chrome_options = null;
                    driver = null;


                    //크롬 드라이버 자동 업데이트
                    new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);

                    chrome_driverService = ChromeDriverService.CreateDefaultService();
                    chrome_driverService.HideCommandPromptWindow = true;

                    chrome_options = new ChromeOptions();
                    chrome_options.AddArgument("start-maximized");
                    chrome_options.AddArgument("disable-gpu");

                    //알림창 안 뜨게
                    chrome_options.UnhandledPromptBehavior = UnhandledPromptBehavior.Ignore;
                    //options.AddArgument("--disable-popup-blocking");
                    //options.AddArgument("--disable-infobars");
                    //options.AddArgument("--disable-notifications");

                    //자동화 탐지 방지
                    chrome_options.AddArgument("--disable-blink-features=AutomationControlled");

                    // 자동화 표시 제거
                    chrome_options.AddLocalStatePreference("excludeSwitches", new string[] { "enable-automation" });

                    // 자동화 확장 기능 사용 안 함
                    chrome_options.AddLocalStatePreference("useAutomationExtension", false);


                    if (blocks.Count >= 2)
                    {
                        string ProfilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\AppData\Local\Google\Chrome\User Data";

                        if (String.Equals(blocks[1], "1"))
                        {
                            if (Directory.GetDirectories(ProfilePath, "Profile *", SearchOption.TopDirectoryOnly).Any())
                            {
                                if (Directory.Exists($@"{ProfilePath}\Profile 1"))
                                {
                                    chrome_options.AddArgument($@"--user-data-dir={ProfilePath}");
                                    chrome_options.AddArgument($"--profile-directory=Profile 1");
                                }
                            }
                            else
                            {
                                if (Directory.Exists($@"{ProfilePath}\Default"))
                                {
                                    chrome_options.AddArgument($@"--user-data-dir={ProfilePath}");
                                    chrome_options.AddArgument($"--profile-directory=Default");
                                }
                            }
                        }
                        else
                        {
                            if(Directory.Exists($@"{ProfilePath}\Profile {blocks[1]}"))
                            {
                                chrome_options.AddArgument($@"--user-data-dir={ProfilePath}");
                                chrome_options.AddArgument($"--profile-directory=Profile {blocks[1]}");
                            }
                        }
                    }


                    driver = new ChromeDriver(chrome_driverService, chrome_options);
                    ((IJavaScriptExecutor)driver).ExecuteScript("Object.defineProperty(navigator, 'webdriver', {get: () => undefined})");


                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
                }
                else if (String.Equals(blocks[0], "edge"))
                {
                    try
                    {
                        if (driver != null)
                            driver.Quit();
                    }
                    catch { }

                    IsSeleniumStarted = true;

                    // 엣지 드라이버 자동 업데이트 (브라우저 버전과 일치)
                    new DriverManager().SetUpDriver(new EdgeConfig(), VersionResolveStrategy.MatchingBrowser);

                    // 드라이버 서비스 설정
                    edgeDriverService = EdgeDriverService.CreateDefaultService();
                    edgeDriverService.HideCommandPromptWindow = true; // 콘솔 창 숨기기

                    // 엣지 옵션 최적화
                    edgeOptions = new EdgeOptions();
                    edgeOptions.AddArgument("start-maximized");            // 창 최대화
                    edgeOptions.AddArgument("disable-gpu");               // GPU 비활성화 (리소스 절약)
                    edgeOptions.AddArgument("--disable-popup-blocking");  // 팝업 차단
                    edgeOptions.AddArgument("--disable-notifications");   // 알림 비활성화
                    edgeOptions.AddArgument("--disable-extensions");      // 확장 프로그램 비활성화
                    edgeOptions.AddArgument("--no-sandbox");              // 샌드박스 모드 해제 (성능 향상)
                    edgeOptions.AddArgument("--disable-dev-shm-usage");   // 메모리 공유 비활성화 (리소스 절약)
                    edgeOptions.AddArgument("--remote-allow-origins=*");  // CORS 관련 오류 방지


                    if (blocks.Count >= 2)
                    {
                        string ProfilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\AppData\Local\Microsoft\Edge\User Data";

                        if (String.Equals(blocks[1], "1"))
                        {
                            if (Directory.GetDirectories(ProfilePath, "Profile *", SearchOption.TopDirectoryOnly).Any())
                            {
                                if (Directory.Exists($@"{ProfilePath}\Profile 1"))
                                {
                                    edgeOptions.AddArgument($@"--user-data-dir={ProfilePath}");
                                    edgeOptions.AddArgument($"--profile-directory=Profile 1");
                                }
                            }
                            else
                            {
                                if (Directory.Exists($@"{ProfilePath}\Default"))
                                {
                                    edgeOptions.AddArgument($@"--user-data-dir={ProfilePath}");
                                    edgeOptions.AddArgument($"--profile-directory=Default");
                                }
                            }
                        }
                        else
                        {
                            if (Directory.Exists($@"{ProfilePath}\Profile {blocks[1]}"))
                            {
                                edgeOptions.AddArgument($@"--user-data-dir={ProfilePath}");
                                edgeOptions.AddArgument($"--profile-directory=Profile {blocks[1]}");
                            }
                        }
                    }



                    // 자동화 탐지 방지
                    edgeOptions.AddArgument("--disable-blink-features=AutomationControlled");
                    edgeOptions.AddExcludedArgument("enable-automation");
                    edgeOptions.AddAdditionalOption("useAutomationExtension", false);

                    // 드라이버 초기화
                    driver = new EdgeDriver(edgeDriverService, edgeOptions);

                    // webdriver 속성 숨기기 (JS에서 탐지 우회)
                    ((IJavaScriptExecutor)driver).ExecuteScript(
                        "Object.defineProperty(navigator, 'webdriver', {get: () => undefined})");

                    // 대기 시간 설정 (페이지 로드 & 암시적 대기)
                    //driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
                }

                else if (String.Equals(blocks[0], "loop"))
                {
                    int stack = 0;
                    string loopcode = "";
                    string[] temp;
                    for (int j = i + 1; j < lines.Length; j++)
                    {
                        temp = lines[j].TrimStart().Split();

                        if (String.Equals(temp[0], "end"))
                        {
                            if (stack > 0)
                            {
                                stack -= 1;
                            }
                            else
                            {
                                i = j;
                                break;
                            }
                        }
                        else if (String.Equals(temp[0], "if"))
                        {
                            stack += 1;
                        }
                        else if (String.Equals(temp[0], "loop"))
                        {
                            stack += 1;
                        }

                        loopcode = loopcode + Environment.NewLine + lines[j];
                    }

                    if (String.Equals(blocks[1], "forever"))
                    {
                        while (true)
                        {
                            Interpreter(loopcode);
                        }
                    }
                    else
                    {
                        int cnt = Convert.ToInt32(blocks[1]);

                        for (int j = 0; j < Convert.ToInt32(blocks[1]); j++)
                        {
                            Interpreter(loopcode);
                        }
                    }
                }

                else if (String.Equals(blocks[0], "if"))
                {
                    if (IsSeleniumStarted)
                    {
                        bool iftrue = false;
                        int type = DetermineSelectorType(blocks[1]);

                        if (blocks.Count >= 4)
                        {
                            if (String.Equals(blocks[2], "is"))
                            {
                                try
                                {
                                    if (type == 0) //xpath
                                    {
                                        if (String.Equals(driver.FindElement(By.XPath(blocks[1])).Text, blocks[3]))
                                        {
                                            iftrue = true;
                                        }
                                    }
                                    else if (type == 1) //id
                                    {
                                        if (String.Equals(driver.FindElement(By.Id(blocks[1])), blocks[3]))
                                        {
                                            iftrue = true;
                                        }
                                    }
                                    else if (type == 2) //xpath
                                    {
                                        if (String.Equals(driver.FindElement(By.CssSelector(blocks[1])), blocks[3]))
                                        {
                                            iftrue = true;
                                        }
                                    }
                                    else if (type == 3) //xpath
                                    {
                                        if (String.Equals(driver.FindElement(By.ClassName(blocks[1])), blocks[3]))
                                        {
                                            iftrue = true;
                                        }
                                    }
                                }
                                catch { }

                                if (iftrue)
                                {
                                    int stack = 0;
                                    string ifcode = "";
                                    string[] temp;
                                    for (int j = i + 1; j < lines.Length; j++)
                                    {
                                        temp = lines[j].TrimStart().Split();

                                        if (String.Equals(temp[0], "end"))
                                        {
                                            if (stack > 0)
                                            {
                                                stack -= 1;
                                            }
                                            else
                                            {
                                                i = j;
                                                break;
                                            }
                                        }
                                        else if (String.Equals(temp[0], "if"))
                                        {
                                            stack += 1;
                                        }
                                        else if (String.Equals(temp[0], "loop"))
                                        {
                                            stack += 1;
                                        }

                                        ifcode = ifcode + Environment.NewLine + lines[j];
                                    }

                                    Interpreter(ifcode);
                                }
                            }
                        }
                    }
                }

                else if (String.Equals(blocks[0], "wait"))
                {
                    int waittime = 0;
                    if (int.TryParse(blocks[1], out waittime))
                    {
                        if(waittime > 10000)
                        {
                            int chunkSize = 1000; // 1000ms (1초) 단위로 나눔

                            while (waittime > 0 && !isClosing)
                            {
                                int sleepTime = Math.Min(chunkSize, waittime); // 남은 시간과 chunkSize 중 작은 값
                                Thread.Sleep(sleepTime); // 해당 시간만큼 대기
                                waittime -= sleepTime; // 남은 시간 감소
                            }
                        }
                        else
                        {
                            Thread.Sleep(Convert.ToInt32(blocks[1]));
                        }
                    }
                    else
                    {
                        if (TimeSpan.TryParse(blocks[1], out TimeSpan inputTime))
                        {
                            // 현재 시간 가져오기
                            DateTime now = DateTime.Now;
                            TimeSpan currentTime = now.TimeOfDay;

                            // 입력된 시간과 현재 시간을 비교
                            int comparison = inputTime.CompareTo(currentTime);
                            int checkgap = 20;
                            if (blocks.Count > 2)
                            {
                                checkgap = Convert.ToInt32(blocks[2]);
                            }

                            while (inputTime.CompareTo(DateTime.Now.TimeOfDay) > 0 && !isClosing)
                            {
                                Thread.Sleep(checkgap);
                            }
                        }
                    }
                }

                else if (String.Equals(blocks[0], "refresh"))
                {
                    driver.Navigate().Refresh();
                }

                else if (String.Equals(blocks[0], "go"))
                {
                    if (IsSeleniumStarted)
                    {
                        string url = line.Replace("go ", "").TrimStart();
                        driver.Navigate().GoToUrl(url);
                    }
                }

                else if (String.Equals(blocks[0], "click"))
                {
                    if (class_element.Contains(blocks[1]))
                    {
                        if (IsSeleniumStarted)
                        {
                            if (String.Equals(blocks[1], "xpath"))
                            {
                                try
                                {
                                    driver.FindElement(By.XPath(blocks[2])).Click();
                                }
                                catch { }
                            }
                            else if (String.Equals(blocks[1], "id"))
                            {
                                try
                                {
                                    driver.FindElement(By.Id(blocks[2])).Click();
                                }
                                catch { }
                            }
                            else if (String.Equals(blocks[1], "css"))
                            {
                                try
                                {
                                    driver.FindElement(By.CssSelector(blocks[2])).Click();
                                }
                                catch { }
                            }
                            else if (String.Equals(blocks[1], "class"))
                            {
                                try
                                {
                                    driver.FindElement(By.ClassName(blocks[2])).Click();
                                }
                                catch { }
                            }
                        }
                    }
                    else
                    {
                        if(blocks.Count >= 3)
                        {
                            int x = Convert.ToInt32(blocks[1]);
                            int y = Convert.ToInt32(blocks[2]);
                            SetCursorPos(x, y);

                            // 마우스 좌클릭 이벤트 발생
                            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, IntPtr.Zero);
                            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, IntPtr.Zero);
                        }
                    }
                }

                else if (String.Equals(blocks[0], "input"))
                {
                    if (blocks.Count >= 5)
                    {
                        if (IsSeleniumStarted)
                        {
                            if (String.Equals(blocks[2], "on"))
                            {
                                if (String.Equals(blocks[3], "xpath"))
                                {
                                    try
                                    {
                                        driver.FindElement(By.XPath(blocks[4])).SendKeys(blocks[1]);
                                    }
                                    catch { }
                                }
                                else if (String.Equals(blocks[3], "id"))
                                {
                                    try
                                    {
                                        driver.FindElement(By.Id(blocks[4])).SendKeys(blocks[1]);
                                    }
                                    catch { }
                                }
                                else if (String.Equals(blocks[3], "css"))
                                {
                                    try
                                    {
                                        driver.FindElement(By.CssSelector(blocks[4])).SendKeys(blocks[1]);
                                    }
                                    catch { }
                                }
                                else if (String.Equals(blocks[3], "class"))
                                {
                                    try
                                    {
                                        driver.FindElement(By.ClassName(blocks[4])).SendKeys(blocks[1]);
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                    else if (blocks.Count >= 2)
                    {
                        simulator.Keyboard.TextEntry(blocks[1]);
                    }
                }

                else if (String.Equals(blocks[0], "clip"))
                {
                    Clipboard.SetText(blocks[1]);
                }

                else if (String.Equals(blocks[0], "var"))
                {
                    if (blocks.Count >= 3)
                    {
                        if (class_element.Contains(blocks[2]))
                        {
                            if (String.Equals(blocks[2], "xpath"))
                            {
                                try
                                {
                                    vars[blocks[1]] = driver.FindElement(By.XPath(blocks[3])).Text;
                                }
                                catch { }
                            }
                            else if (String.Equals(blocks[2], "id"))
                            {
                                try
                                {
                                    vars[blocks[1]] = driver.FindElement(By.Id(blocks[3])).Text;
                                }
                                catch { }
                            }
                            else if (String.Equals(blocks[2], "css"))
                            {
                                try
                                {
                                    vars[blocks[1]] = driver.FindElement(By.CssSelector(blocks[3])).Text;
                                }
                                catch { }
                            }
                            else if (String.Equals(blocks[2], "class"))
                            {
                                try
                                {
                                    vars[blocks[1]] = driver.FindElement(By.ClassName(blocks[3])).Text;
                                }
                                catch { }
                            }
                        }
                        else
                        {
                            vars[blocks[1]] = blocks[2];
                        }
                    }
                    else
                    {
                        MessageBox.Show("[" + blocks[1] + "]" + Environment.NewLine + Convert.ToString(vars[blocks[1]]));
                    }
                }

                else if (String.Equals(blocks[0], "cut"))
                {
                    if (vars.ContainsKey(blocks[1]))
                    {
                        vars[blocks[1]] = ExtractSubstring(Convert.ToString(vars[blocks[1]]), blocks[2], blocks[3]);
                    }
                }

                else if (String.Equals(blocks[0], "tab"))
                {
                    if (String.Equals(blocks[1], "add"))
                    {
                        ((IJavaScriptExecutor)driver).ExecuteScript("window.open();");
                    }

                    else if (String.Equals(blocks[1], "last"))
                    {
                        var allTabs = driver.WindowHandles;
                        var newTab = allTabs[allTabs.Count - 1];
                        driver.SwitchTo().Window(newTab);
                    }
                    else
                    {
                        var allTabs = driver.WindowHandles;

                        if (blocks.Count >= 3)
                        {
                            if (String.Equals(blocks[2], "close"))
                            {
                                //var originalTab = driver.CurrentWindowHandle;
                                //if(originalTab == allTabs[Convert.ToInt32(blocks[1])])
                                {
                                    driver.SwitchTo().Window(allTabs[Convert.ToInt32(blocks[1])]); // 두 번째 탭 (0부터 시작)
                                    driver.Close();
                                }
                            }
                        }
                        else
                        {
                            driver.SwitchTo().Window(allTabs[Convert.ToInt32(blocks[1])]);
                        }
                    }
                }

                else if (String.Equals(blocks[0], "enter"))
                {
                    //simulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_V);
                    simulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                }

                else if (String.Equals(blocks[0], "msg"))
                {
                    MessageBox.Show(blocks[1]);
                }

                else if (String.Equals(blocks[0], "ctrl"))
                {
                    if (String.Equals(blocks[1], "a"))
                    {
                        simulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_A);
                    }
                    else if (String.Equals(blocks[1], "c"))
                    {
                        simulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_C);
                    }
                    else if (String.Equals(blocks[1], "v"))
                    {
                        simulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_V);
                    }
                }

                else if (String.Equals(blocks[0], "quit"))
                {
                    try
                    {
                        if (driver != null)
                            driver.Quit();
                        IsSeleniumStarted = false;
                    }
                    catch { }
                }
            }

            Running = false;
        }

        private void Logo_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void Code_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Tab)
            {
                int selectionStart = Code.SelectionStart;
                Code.Text = Code.Text.Insert(selectionStart, "    ");
                Code.SelectionStart = selectionStart + 4;
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == System.Windows.Forms.Keys.S)
            {
                // 특정 버튼 클릭 이벤트 호출
                저장하기.PerformClick();
                e.SuppressKeyPress = true; // 기본 Ctrl+S 동작을 억제
            }
        }

        private void 불러오기_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                string fileContent = File.ReadAllText(filePath);

                Code.Text = fileContent;
                FilePath.Text = filePath;
            }
        }

        private void 저장하기_Click(object sender, EventArgs e)
        {
            if (String.Equals(FilePath.Text, "File path will be here"))
            {
                다른이름저장.PerformClick();
            }
            else
            {
                File.WriteAllText(FilePath.Text, Code.Text);
            }
        }

        private void 다른이름저장_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;
            if (String.Equals(FilePath.Text, "File path will be here") == false)
            {
                saveFileDialog.FileName = Path.GetFileName(FilePath.Text);
            }

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                string fileContent = Code.Text;

                File.WriteAllText(filePath, fileContent);
                FilePath.Text = filePath;
            }
        }

        private void MaxBtn_Click(object sender, EventArgs e)
        {
            if(String.Equals(MaxBtn.Text, "1"))
            {
                this.WindowState = FormWindowState.Maximized;
                MaxBtn.Text = "2";
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                MaxBtn.Text = "1";
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            isClosing = true;
            MouseTrack = false;
            sublogo.Text = ": Closing...";
            try
            {
                if(driver != null)
                {
                    driver.Quit();
                }
            }
            catch { }

            File.WriteAllText("prev.txt", FilePath.Text);
        }

        //============== 직접 타입 판단 ================
        static int DetermineSelectorType(string selector)
        {
            if (IsXPath(selector))
            {
                return 0; // "XPath";
            }
            else if (IsCssSelector(selector))
            {
                return 1;// "CSS Selector";
            }
            else if (IsId(selector))
            {
                return 2;// "ID";
            }
            else if (IsClass(selector))
            {
                return 3;// "Class";
            }
            else
            {
                return -1; // "알 수 없음";
            }
        }

        static bool IsXPath(string selector)
        {
            // XPath는 슬래시('/') 또는 더블 슬래시('//')로 시작
            return selector.StartsWith("/") || selector.StartsWith("//");
        }

        static bool IsCssSelector(string selector)
        {
            // CSS Selector는 태그 이름, 클래스, ID, 속성 등의 조합으로 구성
            return Regex.IsMatch(selector, @"^[a-zA-Z#\.

\[]");
        }

        static bool IsId(string selector)
        {
            // ID는 해시('#')로 시작
            return selector.StartsWith("#") && !selector.Contains(" ");
        }

        static bool IsClass(string selector)
        {
            // Class는 점('.')으로 시작
            return selector.StartsWith(".");
        }

        private void 프로필_Click(object sender, EventArgs e)
        {
            string ProfilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\AppData\Local\Google\Chrome\User Data\Local State";
            ShowProfileInfo(ProfilePath, "Chrome");
        }

        private void 엣지프로필_Click(object sender, EventArgs e)
        {
            string ProfilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\AppData\Local\Microsoft\Edge\User Data\Local State";
            ShowProfileInfo(ProfilePath, "Edge");
        }
    }
}
