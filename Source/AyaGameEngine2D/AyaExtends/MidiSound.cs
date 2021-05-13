using System;

namespace AyaGameEngine2D.Extends
{
    /// <summary>
    /// 类      名：MidiSound
    /// 功      能：利用Winmm.dll实现Midi音频的播放
    /// 日      期：2015-12-27
    /// 修      改：2015-12-27
    /// 作      者：来自网络,有改动
    /// </summary>
    public class MidiSound
    {
        #region 私有成员
        /// <summary>
        /// Midi输出句柄
        /// </summary>
        private readonly uint _lphMidiOut;

        /// <summary>
        /// 输出音量
        /// </summary>
        private byte _volume;

        /// <summary>
        /// 打开标记
        /// </summary>
        private readonly bool isopen;
        #endregion

        #region 构造 & 析构
        /// <summary>
        /// 构造函数
        /// </summary>
        public MidiSound()
        {
            UInt32 r = Win32.midiOutOpen(out _lphMidiOut, 0, 0, 0, 0);
            isopen = (r == 0);
            _volume = 96;
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~MidiSound()
        {
            if (IsOpen) Win32.midiOutClose(_lphMidiOut);
        }
        #endregion

        #region 功能函数
        /// <summary>
        /// 音量信息（0-127）
        /// </summary>
        public byte Volume
        {
            get { return _volume; }
            set { if (value >= 0 && value <= 127) _volume = value; }
        }

        /// <summary>
        /// 是否已成功打开通道
        /// </summary>
        public bool IsOpen
        {
            get { return isopen; }
        }

        /// <summary>
        /// 直接发送消息
        /// </summary>
        /// <param name="dwMsg">消息内容</param>
        public void PostMessage(uint dwMsg)
        {
            if (IsOpen) Win32.midiOutShortMsg(_lphMidiOut, dwMsg);
        }

        /// <summary>
        /// 按键按下
        /// </summary>
        /// <param name="chn">频道</param>
        /// <param name="vol">音量</param>
        /// <param name="scl">音阶</param>
        public void Down(byte chn, byte vol, byte scl)
        {
            var x = _volume * 65536 + scl * 256 + 144 + chn;
            if (isopen) PostMessage((uint)x);
        }

        /// <summary>
        /// 接键按下
        /// </summary>
        /// <param name="scl">音阶</param>
        public void Down(byte scl)
        {
            Down(0, _volume, scl);
        }

        /// <summary>
        /// 按键按下
        /// </summary>
        /// <param name="chn">频道</param>
        /// <param name="scl">音阶</param>
        public void Down(byte chn, byte scl)
        {
            Down(chn, _volume, scl);
        }

        /// <summary>
        /// 取消发音
        /// </summary>
        /// <param name="chn">频道</param>
        /// <param name="scl">音阶</param>
        public void Up(byte chn, byte scl)
        {
            var x = scl * 256 + 128 + chn;
            if (isopen) PostMessage((uint)x);
        }

        /// <summary>
        /// 取消发音
        /// </summary>
        /// <param name="scl">音阶</param>
        public void Up(byte scl)
        {
            Up(0, scl);
        }

        /// <summary>
        /// 设定乐器音色
        /// </summary>
        /// <param name="timbre">音色</param>
        /// <param name="chn">频道</param>
        public void SetTimbre(byte timbre, byte chn)
        {
            var x = timbre * 256 + 192 + chn;
            if (isopen) PostMessage((uint)x);
        }

        /// <summary>
        /// 设定乐器音色
        /// </summary>
        /// <param name="timbre">音乐</param>
        public void SetTimbre(byte timbre)
        {
            SetTimbre(timbre, 0);
        }
        #endregion
    }

    #region Midi乐器银色代码枚举
    /// <summary>
    /// 类      名：MidiToneType
    /// 功      能：Midi乐器音色代码枚举
    /// 日      期：2015-12-27
    /// 修      改：2015-12-27
    /// 作      者：ls9512
    /// </summary>
    public enum MidiToneType
    {
        // ★ 以下为 Piano（钢琴）

        /// <summary>
        /// 大提琴
        /// </summary>
        Acoustic_Grand_Piano = 0,
        /// <summary>
        /// 亮音钢琴
        /// </summary>
        Bright_Acoustic_Piano = 1,
        /// <summary>
        /// 大电钢琴
        /// </summary>
        Electric_Grand_Piano = 2,
        /// <summary>
        /// 酒吧钢琴
        /// </summary>
        Honky_tonk_Piano = 3,
        /// <summary>
        /// 电钢琴1
        /// </summary>
        Electric_Piano_1 = 4,
        /// <summary>
        /// 电钢琴2
        /// </summary>
        Electric_Piano_2 = 5,
        /// <summary>
        /// 拨弦古钢琴/大键琴
        /// </summary>
        Harpsichord = 6,
        /// <summary>
        /// 击弦古钢琴/电翼琴
        /// </summary>
        Clavinet = 7,

        // ★ 以下为 Chromatic Percussion（固定音高敲击乐器）	

        /// <summary>
        /// 钢片琴
        /// </summary>
        Celesta = 8,
        /// <summary>
        /// 钟琴
        /// </summary>
        Glockenspiel = 9,
        /// <summary>
        /// 八音盒
        /// </summary>
        Musical_Box = 10,
        /// <summary>
        /// 颤音琴
        /// </summary>
        Vibraphone = 11,
        /// <summary>
        /// 马林巴琴
        /// </summary>
        Marimba = 12,
        /// <summary>
        /// 木琴
        /// </summary>
        Xylophone = 13,
        /// <summary>
        /// 管钟
        /// </summary>
        Tubular_Bell = 14,
        /// <summary>
        /// 洋琴
        /// </summary>
        Dulcimer = 15,

        // ★ 以下为 Organ（风琴）

        /// <summary>
        /// 音栓风琴
        /// </summary>
        Drawbar_Organ = 16,
        /// <summary>
        /// 敲击风琴
        /// </summary>
        Percussive_Organ = 17,
        /// <summary>
        /// 摇滚风琴
        /// </summary>
        Rock_Organ = 18,
        /// <summary>
        /// 教堂管风琴
        /// </summary>
        Church_Organ = 19,
        /// <summary>
        /// 簧风琴
        /// </summary>
        Reed_Organ = 20,
        /// <summary>
        /// 手风琴
        /// </summary>
        Accordion = 21,
        /// <summary>
        /// 口琴
        /// </summary>
        Harmonica = 22,
        /// <summary>
        /// 探戈手风琴
        /// </summary>
        Tango_Accordion = 23,

        // ★ 以下为 Guitar（吉他）	

        /// <summary>
        /// 木吉他（尼龙弦）
        /// </summary>
        Acoustic_Guitar_Nylon = 24,
        /// <summary>
        /// 木吉他（钢弦）
        /// </summary>
        Acoustic_Guitar_Steel = 25,
        /// <summary>
        /// 电吉他（爵士）
        /// </summary>
        Electric_Guitar_Jazz = 26,
        /// <summary>
        /// 电吉他（清音）
        /// </summary>
        Electric_Guitar_Clean = 27,
        /// <summary>
        /// 电吉他（闷音）
        /// </summary>
        Electric_Guitar_Muted = 28,
        /// <summary>
        /// 电吉他（驱动音效）
        /// </summary>
        Overdriven_Guitar = 29,
        /// <summary>
        /// 电吉他（失真音效
        /// </summary>
        Distortion_Guitar = 30,
        /// <summary>
        /// 吉他泛音
        /// </summary>
        Guitar_Harmonics = 31,

        // ★ 以下为 Bass（贝斯）

        /// <summary>
        /// 贝斯
        /// </summary>
        Acoustic_Bass = 32,
        /// <summary>
        /// 电贝斯（指奏）
        /// </summary>
        Electric_Bass_Finger = 33,
        /// <summary>
        /// 电贝斯（拨奏）
        /// </summary>
        Electric_Bass_Pick = 34,
        /// <summary>
        /// 无品贝司
        /// </summary>
        Fretless_Bass = 35,
        /// <summary>
        /// 击弦贝司1
        /// </summary>
        Slap_Bass_1 = 36,
        /// <summary>
        /// 击弦贝司2
        /// </summary>
        Slap_Bass_2 = 37,
        /// <summary>
        /// 合成贝司1
        /// </summary>
        Synth_Bass_1 = 38,
        /// <summary>
        /// 合成贝司2
        /// </summary>
        Synth_Bass_2 = 39,

        // ★ Strings（弦乐器）

        /// <summary>
        /// 小提琴
        /// </summary>
        Violin = 40,
        /// <summary>
        /// 中提琴
        /// </summary>
        Viola = 41,
        /// <summary>
        /// 大提琴
        /// </summary>
        Cello = 42,
        /// <summary>
        /// 低音大提琴
        /// </summary>
        Contrabass = 43,
        /// <summary>
        /// 颤弓弦乐
        /// </summary>
        Tremolo_Strings = 44,
        /// <summary>
        /// 弹拨弦乐
        /// </summary>
        Pizzicato_Strings = 45,
        /// <summary>
        /// 竖琴
        /// </summary>
        Orchestral_Harp = 46,
        /// <summary>
        /// 定音鼓
        /// </summary>
        Timpani = 47,

        // ★ 以下为 Ensemble（合奏）

        /// <summary>
        /// 弦乐合奏1
        /// </summary>
        String_Ensemble_1 = 48,
        /// <summary>
        /// 弦乐合奏2
        /// </summary>
        String_Ensemble_2 = 49,
        /// <summary>
        /// 合成弦乐1
        /// </summary>
        Synth_Strings_1 = 50,
        /// <summary>
        /// 合成弦乐2
        /// </summary>
        Synth_Strings_2 = 51,
        /// <summary>
        /// 人声“啊”
        /// </summary>
        Voice_Aahs = 52,
        /// <summary>
        /// 人声“喔”
        /// </summary>
        Voice_Oohs = 53,
        /// <summary>
        /// 合成人声
        /// </summary>
        Synth_Voice = 54,
        /// <summary>
        /// 交响打击乐
        /// </summary>
        Orchestra_Hit = 55,

        // ★ 以下为 Brass（铜管 乐器）

        /// <summary>
        /// 小号
        /// </summary>
        Trumpet = 56,
        /// <summary>
        /// 长号
        /// </summary>
        Trombone = 57,
        /// <summary>
        /// 大号
        /// </summary>
        Tuba = 58,
        /// <summary>
        /// 闷音小号
        /// </summary>
        Muted_Trumpet = 59,
        /// <summary>
        /// 法国号/圆号
        /// </summary>
        French_Born = 60,
        /// <summary>
        /// 铜管乐
        /// </summary>
        Brass_Section = 61,
        /// <summary>
        /// 合成铜管1
        /// </summary>
        Synth_Brass_1 = 62,
        /// <summary>
        /// 合成铜管2
        /// </summary>
        Synth_Brass_2 = 63,

        // ★ 以下为 Reed（簧乐 器）

        /// <summary>
        /// 高音萨克斯
        /// </summary>
        Soprano_Sax = 64,
        /// <summary>
        /// 中音萨克斯
        /// </summary>
        Alto_Sax = 65,
        /// <summary>
        /// 次中音萨克斯
        /// </summary>
        Tenor_Sax = 66,
        /// <summary>
        /// 上低音萨克斯
        /// </summary>
        Baritone_Sax = 67,
        /// <summary>
        /// 双簧管
        /// </summary>
        Oboe = 68,
        /// <summary>
        /// 英国管
        /// </summary>
        English_Horn = 69,
        /// <summary>
        /// 低音管（巴颂管）
        /// </summary>
        Bassoon = 70,
        /// <summary>
        /// 单簧管（黑管、竖笛）
        /// </summary>
        Clarinet = 71,

        // ★ 以下为 Pipe（吹管 乐器）

        /// <summary>
        /// 短笛
        /// </summary>
        Piccolo = 72,
        /// <summary>
        /// 长笛
        /// </summary>
        Flute = 73,
        /// <summary>
        /// 竖笛
        /// </summary>
        Recorder = 74,
        /// <summary>
        /// 排笛
        /// </summary>
        Pan_Flute = 75,
        /// <summary>
        /// 瓶笛
        /// </summary>
        Blown_Bottle = 76,
        /// <summary>
        /// 尺八
        /// </summary>
        Shakuhachi = 77,
        /// <summary>
        /// 哨子
        /// </summary>
        Whistle = 78,
        /// <summary>
        /// 陶笛
        /// </summary>
        Ocarina = 79,

        // ★ 以下为 Synth Lead（合成音主旋律）

        /// <summary>
        /// 合成主音1方波
        /// </summary>
        Lead_1_Square = 80,
        /// <summary>
        /// 合成主音2锯齿波
        /// </summary>
        Lead_2_Sawtooth = 81,
        /// <summary>
        /// 合成主音3汽笛风琴
        /// </summary>
        Lead_3_Calliope = 82,
        /// <summary>
        /// 合成主音4吹管
        /// </summary>
        Lead_4_Chiff = 83,
        /// <summary>
        /// 合成主音5电吉他
        /// </summary>
        Lead_5_Charang = 84,
        /// <summary>
        /// 合成主音6人声
        /// </summary>
        Lead_6_Voice = 85,
        /// <summary>
        /// 合成主音7五度
        /// </summary>
        Lead_7_Fifths = 86,
        /// <summary>
        /// 合成主音8贝斯吉他合奏
        /// </summary>
        Lead_8_Bass_Lead = 87,

        // ★ 以下为 Synth Pad（合成音和弦衬底）

        /// <summary>
        /// 合成柔音1新时代
        /// </summary>
        Pad_1_New_Age = 88,
        /// <summary>
        /// 合成柔音2暖音
        /// </summary>
        Pad_2_Warm = 89,
        /// <summary>
        /// 合成柔音3多重合音
        /// </summary>
        Pad_3_Polysynth = 90,
        /// <summary>
        /// 合成柔音4人声合唱
        /// </summary>
        Pad_4_Choir = 91,
        /// <summary>
        /// 合成柔音5玻璃
        /// </summary>
        Pad_5_Bowed = 92,
        /// <summary>
        /// 合成柔音6金属
        /// </summary>
        Pad_6_Metallic = 93,
        /// <summary>
        /// 合成柔音7光环
        /// </summary>
        Pad_7_Halo = 94,
        /// <summary>
        /// 合成柔音8扫弦
        /// </summary>
        Pad_8_Sweep = 95,

        // ★ 以下为 Synth Effects（合成音效果）

        /// <summary>
        /// 合成特效1雨
        /// </summary>
        FX_1_Rain = 96,
        /// <summary>
        /// 合成特效2音轨
        /// </summary>
        FX_2_Soundtrack = 97,
        /// <summary>
        /// 合成特效3水晶
        /// </summary>
        FX_3_Crystal = 98,
        /// <summary>
        /// 合成特效4气氛
        /// </summary>
        FX_4_Atmosphere = 99,
        /// <summary>
        /// 合成特效5明亮
        /// </summary>
        FX_5_Brightness = 100,
        /// <summary>
        /// 合成特效6魅影
        /// </summary>
        FX_6_Goblins = 101,
        /// <summary>
        /// 合成特效7回声
        /// </summary>
        FX_7_Echoes = 102,
        /// <summary>
        /// 合成特效8科幻
        /// </summary>
        FX_8_Sci_Fi = 103,

        // ★ 以下为 Ethnic（民族 乐器）

        /// <summary>
        /// 西塔琴
        /// </summary>
        Sitar = 104,
        /// <summary>
        /// 五弦琴（斑鸠琴）
        /// </summary>
        Banjo = 105,
        /// <summary>
        /// 三味线
        /// </summary>
        Shamisen = 106,
        /// <summary>
        /// 十三弦琴（古筝）
        /// </summary>
        Koto = 107,
        /// <summary>
        /// 卡林巴铁片琴
        /// </summary>
        Kalimba = 108,
        /// <summary>
        /// 苏格兰风笛
        /// </summary>
        Bagpipe = 109,
        /// <summary>
        /// 古提琴
        /// </summary>
        Fiddle = 110,
        /// <summary>
        /// 兽笛，发声机制类似唢呐
        /// </summary>
        Shanai = 111,

        // ★ 以下为 Percussive（打击 乐器）	

        /// <summary>
        /// 叮当铃
        /// </summary>
        Tinkle_Bell = 112,
        /// <summary>
        /// 阿哥哥鼓
        /// </summary>
        Agogo = 113,
        /// <summary>
        /// 钢鼓
        /// </summary>
        Steel_Drums = 114,
        /// <summary>
        /// 木鱼
        /// </summary>
        Woodblock = 115,
        /// <summary>
        /// 太鼓
        /// </summary>
        Taiko_Drum = 116,
        /// <summary>
        /// 定音筒鼓
        /// </summary>
        Melodic_Tom = 117,
        /// <summary>
        /// 合成鼓
        /// </summary>
        Synth_Drum = 118,
        /// <summary>
        /// 逆转钹声
        /// </summary>
        Reverse_Cymbal = 119,

        // ★ 以下为 Sound effects（特殊 音效）

        /// <summary>
        /// 吉他滑弦杂音
        /// </summary>
        Guitar_Fret_Noise = 120,
        /// <summary>
        /// 呼吸杂音
        /// </summary>
        Breath_Noise = 121,
        /// <summary>
        /// 海岸
        /// </summary>
        Seashore = 122,
        /// <summary>
        /// 鸟鸣
        /// </summary>
        Bird_Tweet = 123,
        /// <summary>
        /// 电话铃声
        /// </summary>
        Telephone_Ring = 124,
        /// <summary>
        /// 直升机声
        /// </summary>
        Helicopter = 125,
        /// <summary>
        /// 鼓掌声
        /// </summary>
        Applause = 126,
        /// <summary>
        /// 枪声
        /// </summary>
        Gunshot = 127
    } 
    #endregion
}
