namespace LeagueSharp.Common
{
    using System;

    /// <summary>
    ///     Specifies key codes and modifiers.
    /// </summary>
    [Flags]
    public enum Keys
    {
        /// <summary>
        ///     The bitmask to extract a key code from a key value.
        /// </summary>
        KeyCode = 65535,

        /// <summary>
        ///     The bitmask to extract modifiers from a key value.
        /// </summary>
        Modifiers = -65536,

        /// <summary>
        ///     No key pressed.
        /// </summary>
        None = 0,

        /// <summary>
        ///     The left mouse button.
        /// </summary>
        LButton = 1,

        /// <summary>
        ///     The right mouse button.
        /// </summary>
        RButton = 2,

        /// <summary>
        ///     The CANCEL key.
        /// </summary>
        Cancel = RButton | LButton,

        /// <summary>
        ///     The middle mouse button (three-button mouse).
        /// </summary>
        MButton = 4,

        /// <summary>
        ///     The first x mouse button (five-button mouse).
        /// </summary>
        XButton1 = MButton | LButton,

        /// <summary>
        ///     The second x mouse button (five-button mouse).
        /// </summary>
        XButton2 = MButton | RButton,

        /// <summary>
        ///     The BACKSPACE key.
        /// </summary>
        Back = 8,

        /// <summary>
        ///     The TAB key.
        /// </summary>
        Tab = Back | LButton,

        /// <summary>
        ///     The LINEFEED key.
        /// </summary>
        LineFeed = Back | RButton,

        /// <summary>
        ///     The CLEAR key.
        /// </summary>
        Clear = Back | MButton,

        /// <summary>
        ///     The RETURN key.
        /// </summary>
        Return = Clear | LButton,

        /// <summary>
        ///     The ENTER key.
        /// </summary>
        Enter = Return,

        /// <summary>
        ///     The SHIFT key.
        /// </summary>
        ShiftKey = 16,

        /// <summary>
        ///     The CTRL key.
        /// </summary>
        ControlKey = ShiftKey | LButton,

        /// <summary>
        ///     The ALT key.
        /// </summary>
        Menu = ShiftKey | RButton,

        /// <summary>
        ///     The PAUSE key.
        /// </summary>
        Pause = Menu | LButton,

        /// <summary>
        ///     The CAPS LOCK key.
        /// </summary>
        Capital = ShiftKey | MButton,

        /// <summary>
        ///     The CAPS LOCK key.
        /// </summary>
        CapsLock = Capital,

        /// <summary>
        ///     The IME Kana mode key.
        /// </summary>
        KanaMode = CapsLock | LButton,

        /// <summary>
        ///     The IME Hanguel mode key. (maintained for compatibility; use HangulMode)
        /// </summary>
        HanguelMode = KanaMode,

        /// <summary>
        ///     The IME Hangul mode key.
        /// </summary>
        HangulMode = HanguelMode,

        /// <summary>
        ///     The IME Junja mode key.
        /// </summary>
        JunjaMode = HangulMode | RButton,

        /// <summary>
        ///     The IME final mode key.
        /// </summary>
        FinalMode = ShiftKey | Back,

        /// <summary>
        ///     The IME Hanja mode key.
        /// </summary>
        HanjaMode = FinalMode | LButton,

        /// <summary>
        ///     The IME Kanji mode key.
        /// </summary>
        KanjiMode = HanjaMode,

        /// <summary>
        ///     The ESC key.
        /// </summary>
        Escape = KanjiMode | RButton,

        /// <summary>
        ///     The IME convert key.
        /// </summary>
        IMEConvert = FinalMode | MButton,

        /// <summary>
        ///     The IME nonconvert key.
        /// </summary>
        IMENonconvert = IMEConvert | LButton,

        /// <summary>
        ///     The IME accept key, replaces IMEAceept.
        /// </summary>
        IMEAccept = IMEConvert | RButton,

        /// <summary>
        ///     The IME accept key.
        /// </summary>
        [Obsolete("Obsolete, use IMEAccept instead.")]
        IMEAceept = IMEAccept,

        /// <summary>
        ///     The IME mode change key.
        /// </summary>
        IMEModeChange = IMEAccept | LButton,

        /// <summary>
        ///     The SPACEBAR key.
        /// </summary>
        Space = 32,

        /// <summary>
        ///     The PAGE UP key.
        /// </summary>
        Prior = Space | LButton,

        /// <summary>
        ///     The PAGE UP key.
        /// </summary>
        PageUp = Prior,

        /// <summary>
        ///     The PAGE DOWN key.
        /// </summary>
        Next = Space | RButton,

        /// <summary>
        ///     The PAGE DOWN key.
        /// </summary>
        PageDown = Next,

        /// <summary>
        ///     The END key.
        /// </summary>
        End = PageDown | LButton,

        /// <summary>
        ///     The HOME key.
        /// </summary>
        Home = Space | MButton,

        /// <summary>
        ///     The LEFT ARROW key.
        /// </summary>
        Left = Home | LButton,

        /// <summary>
        ///     The UP ARROW key.
        /// </summary>
        Up = Home | RButton,

        /// <summary>
        ///     The RIGHT ARROW key.
        /// </summary>
        Right = Up | LButton,

        /// <summary>
        ///     The DOWN ARROW key.
        /// </summary>
        Down = Space | Back,

        /// <summary>
        ///     The SELECT key.
        /// </summary>
        Select = Down | LButton,

        /// <summary>
        ///     The PRINT key.
        /// </summary>
        Print = Down | RButton,

        /// <summary>
        ///     The EXECUTE key.
        /// </summary>
        Execute = Print | LButton,

        /// <summary>
        ///     The PRINT key.
        /// </summary>
        Snapshot = Down | MButton,

        /// <summary>
        ///     The PRINT key.
        /// </summary>
        PrintScreen = Snapshot,

        /// <summary>
        ///     The INS key.
        /// </summary>
        Insert = PrintScreen | LButton,

        /// <summary>
        ///     The DEL key.
        /// </summary>
        Delete = PrintScreen | RButton,

        /// <summary>
        ///     The HELP key.
        /// </summary>
        Help = Delete | LButton,

        /// <summary>
        ///     The 0 key on the numeric keypad.
        /// </summary>
        D0 = Space | ShiftKey,

        /// <summary>
        ///     The 1 key on the numeric keypad.
        /// </summary>
        D1 = D0 | LButton,

        /// <summary>
        ///     The 2 key on the numeric keypad.
        /// </summary>
        D2 = D0 | RButton,

        /// <summary>
        ///     The 3 key on the numeric keypad.
        /// </summary>
        D3 = D2 | LButton,

        /// <summary>
        ///     The 4 key on the numeric keypad.
        /// </summary>
        D4 = D0 | MButton,

        /// <summary>
        ///     The 5 key on the numeric keypad.
        /// </summary>
        D5 = D4 | LButton,

        /// <summary>
        ///     The 6 key on the numeric keypad.
        /// </summary>
        D6 = D4 | RButton,

        /// <summary>
        ///     The 7 key on the numeric keypad.
        /// </summary>
        D7 = D6 | LButton,

        /// <summary>
        ///     The 8 key on the numeric keypad.
        /// </summary>
        D8 = D0 | Back,

        /// <summary>
        ///     The 9 key on the numeric keypad.
        /// </summary>
        D9 = D8 | LButton,

        /// <summary>
        ///     The A key.
        /// </summary>
        A = 65,

        /// <summary>
        ///     The B key.
        /// </summary>
        B = 66,

        /// <summary>
        ///     The C key.
        /// </summary>
        C = B | LButton,

        /// <summary>
        ///     The D key.
        /// </summary>
        D = 68,

        /// <summary>
        ///     The E key.
        /// </summary>
        E = D | LButton,

        /// <summary>
        ///     The F key.
        /// </summary>
        F = D | RButton,

        /// <summary>
        ///     The G key.
        /// </summary>
        G = F | LButton,

        /// <summary>
        ///     The H key.
        /// </summary>
        H = 72,

        /// <summary>
        ///     The I key.
        /// </summary>
        I = H | LButton,

        /// <summary>
        ///     The J key.
        /// </summary>
        J = H | RButton,

        /// <summary>
        ///     The K key.
        /// </summary>
        K = J | LButton,

        /// <summary>
        ///     The L key.
        /// </summary>
        L = H | MButton,

        /// <summary>
        ///     The M key.
        /// </summary>
        M = L | LButton,

        /// <summary>
        ///     The N key.
        /// </summary>
        N = L | RButton,

        /// <summary>
        ///     The O key.
        /// </summary>
        O = N | LButton,

        /// <summary>
        ///     The P key.
        /// </summary>
        P = 80,

        /// <summary>
        ///     The Q key.
        /// </summary>
        Q = P | LButton,

        /// <summary>
        ///     The R key.
        /// </summary>
        R = P | RButton,

        /// <summary>
        ///     The S key.
        /// </summary>
        S = R | LButton,

        /// <summary>
        ///     The T key.
        /// </summary>
        T = P | MButton,

        /// <summary>
        ///     The U key.
        /// </summary>
        U = T | LButton,

        /// <summary>
        ///     The V key.
        /// </summary>
        V = T | RButton,

        /// <summary>
        ///     The W key.
        /// </summary>
        W = V | LButton,

        /// <summary>
        ///     The X key.
        /// </summary>
        X = P | Back,

        /// <summary>
        ///     The Y key.
        /// </summary>
        Y = X | LButton,

        /// <summary>
        ///     The Z key.
        /// </summary>
        Z = X | RButton,

        /// <summary>
        ///     The left Windows logo key (Microsoft Natural Keyboard).
        /// </summary>
        LWin = Z | LButton,

        /// <summary>
        ///     The right Windows logo key (Microsoft Natural Keyboard).
        /// </summary>
        RWin = X | MButton,

        /// <summary>
        ///     The application key (Microsoft Natural Keyboard).
        /// </summary>
        Apps = RWin | LButton,

        /// <summary>
        ///     The computer sleep key.
        /// </summary>
        Sleep = Apps | RButton,

        /// <summary>
        ///     The 0 key on the numeric keypad.
        /// </summary>
        NumPad0 = 96,

        /// <summary>
        ///     The 1 key on the numeric keypad.
        /// </summary>
        NumPad1 = NumPad0 | LButton,

        /// <summary>
        ///     The 2 key on the numeric keypad.
        /// </summary>
        NumPad2 = NumPad0 | RButton,

        /// <summary>
        ///     The 3 key on the numeric keypad.
        /// </summary>
        NumPad3 = NumPad2 | LButton,

        /// <summary>
        ///     The 4 key on the numeric keypad.
        /// </summary>
        NumPad4 = NumPad0 | MButton,

        /// <summary>
        ///     The 5 key on the numeric keypad.
        /// </summary>
        NumPad5 = NumPad4 | LButton,

        /// <summary>
        ///     The 6 key on the numeric keypad.
        /// </summary>
        NumPad6 = NumPad4 | RButton,

        /// <summary>
        ///     The 7 key on the numeric keypad.
        /// </summary>
        NumPad7 = NumPad6 | LButton,

        /// <summary>
        ///     The 8 key on the numeric keypad.
        /// </summary>
        NumPad8 = NumPad0 | Back,

        /// <summary>
        ///     The 9 key on the numeric keypad.
        /// </summary>
        NumPad9 = NumPad8 | LButton,

        /// <summary>
        ///     The multiply key.
        /// </summary>
        Multiply = NumPad8 | RButton,

        /// <summary>
        ///     The add key.
        /// </summary>
        Add = Multiply | LButton,

        /// <summary>
        ///     The separator key.
        /// </summary>
        Separator = NumPad8 | MButton,

        /// <summary>
        ///     The subtract key.
        /// </summary>
        Subtract = Separator | LButton,

        /// <summary>
        ///     The decimal key.
        /// </summary>
        Decimal = Separator | RButton,

        /// <summary>
        ///     The divide key.
        /// </summary>
        Divide = Decimal | LButton,

        /// <summary>
        ///     The F1 key.
        /// </summary>
        F1 = NumPad0 | ShiftKey,

        /// <summary>
        ///     The F2 key.
        /// </summary>
        F2 = F1 | LButton,

        /// <summary>
        ///     The F3 key.
        /// </summary>
        F3 = F1 | RButton,

        /// <summary>
        ///     The F4 key.
        /// </summary>
        F4 = F3 | LButton,

        /// <summary>
        ///     The F5 key.
        /// </summary>
        F5 = F1 | MButton,

        /// <summary>
        ///     The F6 key.
        /// </summary>
        F6 = F5 | LButton,

        /// <summary>
        ///     The F7 key.
        /// </summary>
        F7 = F5 | RButton,

        /// <summary>
        ///     The F8 key.
        /// </summary>
        F8 = F7 | LButton,

        /// <summary>
        ///     The F9 key.
        /// </summary>
        F9 = F1 | Back,

        /// <summary>
        ///     The F10 key.
        /// </summary>
        F10 = F9 | LButton,

        /// <summary>
        ///     The F11 key.
        /// </summary>
        F11 = F9 | RButton,

        /// <summary>
        ///     The F12 key.
        /// </summary>
        F12 = F11 | LButton,

        /// <summary>
        ///     The F13 key.
        /// </summary>
        F13 = F9 | MButton,

        /// <summary>
        ///     The F14 key.
        /// </summary>
        F14 = F13 | LButton,

        /// <summary>
        ///     The F15 key.
        /// </summary>
        F15 = F13 | RButton,

        /// <summary>
        ///     The F16 key.
        /// </summary>
        F16 = F15 | LButton,

        /// <summary>
        ///     The F17 key.
        /// </summary>
        F17 = 128,

        /// <summary>
        ///     The F18 key.
        /// </summary>
        F18 = F17 | LButton,

        /// <summary>
        ///     The F19 key.
        /// </summary>
        F19 = F17 | RButton,

        /// <summary>
        ///     The F20 key.
        /// </summary>
        F20 = F19 | LButton,

        /// <summary>
        ///     The F21 key.
        /// </summary>
        F21 = F17 | MButton,

        /// <summary>
        ///     The F22 key.
        /// </summary>
        F22 = F21 | LButton,

        /// <summary>
        ///     The F23 key.
        /// </summary>
        F23 = F21 | RButton,

        /// <summary>
        ///     The F24 key.
        /// </summary>
        F24 = F23 | LButton,

        /// <summary>
        ///     The NUM LOCK key.
        /// </summary>
        NumLock = F17 | ShiftKey,

        /// <summary>
        ///     The SCROLL LOCK key.
        /// </summary>
        Scroll = NumLock | LButton,

        /// <summary>
        ///     The left SHIFT key.
        /// </summary>
        LShiftKey = F17 | Space,

        /// <summary>
        ///     The right SHIFT key.
        /// </summary>
        RShiftKey = LShiftKey | LButton,

        /// <summary>
        ///     The left CTRL key.
        /// </summary>
        LControlKey = LShiftKey | RButton,

        /// <summary>
        ///     The right CTRL key.
        /// </summary>
        RControlKey = LControlKey | LButton,

        /// <summary>
        ///     The left ALT key.
        /// </summary>
        LMenu = LShiftKey | MButton,

        /// <summary>
        ///     The right ALT key.
        /// </summary>
        RMenu = LMenu | LButton,

        /// <summary>
        ///     The browser back key (Windows 2000 or later).
        /// </summary>
        BrowserBack = LMenu | RButton,

        /// <summary>
        ///     The browser forward key (Windows 2000 or later).
        /// </summary>
        BrowserForward = BrowserBack | LButton,

        /// <summary>
        ///     The browser refresh key (Windows 2000 or later).
        /// </summary>
        BrowserRefresh = LShiftKey | Back,

        /// <summary>
        ///     The browser stop key (Windows 2000 or later).
        /// </summary>
        BrowserStop = BrowserRefresh | LButton,

        /// <summary>
        ///     The browser search key (Windows 2000 or later).
        /// </summary>
        BrowserSearch = BrowserRefresh | RButton,

        /// <summary>
        ///     The browser favorites key (Windows 2000 or later).
        /// </summary>
        BrowserFavorites = BrowserSearch | LButton,

        /// <summary>
        ///     The browser home key (Windows 2000 or later).
        /// </summary>
        BrowserHome = BrowserRefresh | MButton,

        /// <summary>
        ///     The volume mute key (Windows 2000 or later).
        /// </summary>
        VolumeMute = BrowserHome | LButton,

        /// <summary>
        ///     The volume down key (Windows 2000 or later).
        /// </summary>
        VolumeDown = BrowserHome | RButton,

        /// <summary>
        ///     The volume up key (Windows 2000 or later).
        /// </summary>
        VolumeUp = VolumeDown | LButton,

        /// <summary>
        ///     The media next track key (Windows 2000 or later).
        /// </summary>
        MediaNextTrack = LShiftKey | ShiftKey,

        /// <summary>
        ///     The media previous track key (Windows 2000 or later).
        /// </summary>
        MediaPreviousTrack = MediaNextTrack | LButton,

        /// <summary>
        ///     The media Stop key (Windows 2000 or later).
        /// </summary>
        MediaStop = MediaNextTrack | RButton,

        /// <summary>
        ///     The media play pause key (Windows 2000 or later).
        /// </summary>
        MediaPlayPause = MediaStop | LButton,

        /// <summary>
        ///     The launch mail key (Windows 2000 or later).
        /// </summary>
        LaunchMail = MediaNextTrack | MButton,

        /// <summary>
        ///     The select media key (Windows 2000 or later).
        /// </summary>
        SelectMedia = LaunchMail | LButton,

        /// <summary>
        ///     The start application one key (Windows 2000 or later).
        /// </summary>
        LaunchApplication1 = LaunchMail | RButton,

        /// <summary>
        ///     The start application two key (Windows 2000 or later).
        /// </summary>
        LaunchApplication2 = LaunchApplication1 | LButton,

        /// <summary>
        ///     The OEM Semicolon key on a US standard keyboard (Windows 2000 or later).
        /// </summary>
        OemSemicolon = MediaStop | Back,

        /// <summary>
        ///     The OEM 1 key.
        /// </summary>
        Oem1 = OemSemicolon,

        /// <summary>
        ///     The OEM plus key on any country/region keyboard (Windows 2000 or later).
        /// </summary>
        Oemplus = Oem1 | LButton,

        /// <summary>
        ///     The OEM comma key on any country/region keyboard (Windows 2000 or later).
        /// </summary>
        Oemcomma = LaunchMail | Back,

        /// <summary>
        ///     The OEM minus key on any country/region keyboard (Windows 2000 or later).
        /// </summary>
        OemMinus = Oemcomma | LButton,

        /// <summary>
        ///     The OEM period key on any country/region keyboard (Windows 2000 or later).
        /// </summary>
        OemPeriod = Oemcomma | RButton,

        /// <summary>
        ///     The OEM question mark key on a US standard keyboard (Windows 2000 or later).
        /// </summary>
        OemQuestion = OemPeriod | LButton,

        /// <summary>
        ///     The OEM 2 key.
        /// </summary>
        Oem2 = OemQuestion,

        /// <summary>
        ///     The OEM tilde key on a US standard keyboard (Windows 2000 or later).
        /// </summary>
        Oemtilde = 192,

        /// <summary>
        ///     The OEM 3 key.
        /// </summary>
        Oem3 = Oemtilde,

        /// <summary>
        ///     The OEM open bracket key on a US standard keyboard (Windows 2000 or later).
        /// </summary>
        OemOpenBrackets = Oem3 | Escape,

        /// <summary>
        ///     The OEM 4 key.
        /// </summary>
        Oem4 = OemOpenBrackets,

        /// <summary>
        ///     The OEM pipe key on a US standard keyboard (Windows 2000 or later).
        /// </summary>
        OemPipe = Oem3 | IMEConvert,

        /// <summary>
        ///     The OEM 5 key.
        /// </summary>
        Oem5 = OemPipe,

        /// <summary>
        ///     The OEM close bracket key on a US standard keyboard (Windows 2000 or later).
        /// </summary>
        OemCloseBrackets = Oem5 | LButton,

        /// <summary>
        ///     The OEM 6 key.
        /// </summary>
        Oem6 = OemCloseBrackets,

        /// <summary>
        ///     The OEM singled/double quote key on a US standard keyboard (Windows 2000 or later).
        /// </summary>
        OemQuotes = Oem5 | RButton,

        /// <summary>
        ///     The OEM 7 key.
        /// </summary>
        Oem7 = OemQuotes,

        /// <summary>
        ///     The OEM 8 key.
        /// </summary>
        Oem8 = Oem7 | LButton,

        /// <summary>
        ///     The OEM angle bracket or backslash key on the RT 102 key keyboard (Windows 2000 or later).
        /// </summary>
        OemBackslash = Oem3 | PageDown,

        /// <summary>
        ///     The OEM 102 key.
        /// </summary>
        Oem102 = OemBackslash,

        /// <summary>
        ///     The PROCESS KEY key.
        /// </summary>
        ProcessKey = Oem3 | Left,

        /// <summary>
        ///     Used to pass Unicode characters as if they were keystrokes. The Packet key value is the low word of a 32-bit
        ///     virtual-key value used for non-keyboard input methods.
        /// </summary>
        Packet = ProcessKey | RButton,

        /// <summary>
        ///     The ATTN key.
        /// </summary>
        Attn = Oem102 | CapsLock,

        /// <summary>
        ///     The CRSEL key.
        /// </summary>
        Crsel = Attn | LButton,

        /// <summary>
        ///     The EXSEL key.
        /// </summary>
        Exsel = Oem3 | D8,

        /// <summary>
        ///     The ERASE EOF key.
        /// </summary>
        EraseEof = Exsel | LButton,

        /// <summary>
        ///     The PLAY key.
        /// </summary>
        Play = Exsel | RButton,

        /// <summary>
        ///     The ZOOM key.
        /// </summary>
        Zoom = Play | LButton,

        /// <summary>
        ///     A constant reserved for future use.
        /// </summary>
        NoName = Exsel | MButton,

        /// <summary>
        ///     The PA1 key.
        /// </summary>
        Pa1 = NoName | LButton,

        /// <summary>
        ///     The CLEAR key.
        /// </summary>
        OemClear = NoName | RButton,

        /// <summary>
        ///     The SHIFT modifier key.
        /// </summary>
        Shift = 65536,

        /// <summary>
        ///     The CTRL modifier key.
        /// </summary>
        Control = 131072,

        /// <summary>
        ///     The ALT modifier key.
        /// </summary>
        Alt = 262144,
    }
}
