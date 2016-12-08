using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace BeamgunApp.Models
{
    public class KeyConverter
    {
        public string Convert(IEnumerable<Keys> inputKeys)
        {
            var stringBuilder = new StringBuilder();
            foreach (var key in inputKeys)
            {
                stringBuilder.Append(Convert(key));
            }
            return stringBuilder.ToString();
        }

        public string Convert(Keys key)
        {
            switch (key)
            {
                case Keys.Return:
                    return "\n";
                case Keys.Space:
                    return " ";
                case Keys.KeyCode:
                    break;
                case Keys.Modifiers:
                    break;
                case Keys.None:
                    break;
                case Keys.LButton:
                    return "<LEFT>";
                case Keys.RButton:
                    return "<RIGHT>";
                case Keys.Cancel:
                    return "<CANCEL>";
                case Keys.MButton:
                    break;
                case Keys.XButton1:
                    break;
                case Keys.XButton2:
                    break;
                case Keys.Back:
                    return "<BACK>";
                case Keys.Tab:
                    return "\t";
                case Keys.LineFeed:
                    return "\r";
                case Keys.Clear:
                case Keys.ShiftKey:
                    return "<SHFT>";
                case Keys.ControlKey:
                    return "<CTRL>";
                case Keys.Menu:
                    break;
                case Keys.Pause:
                    return "<PAUSE>";
                case Keys.Capital:
                    break;
                case Keys.KanaMode:
                    break;
                case Keys.JunjaMode:
                    break;
                case Keys.FinalMode:
                    break;
                case Keys.HanjaMode:
                    break;
                case Keys.Escape:
                    return "<ESC>";
                case Keys.IMEConvert:
                    break;
                case Keys.IMENonconvert:
                    break;
                case Keys.IMEAccept:
                    break;
                case Keys.IMEModeChange:
                    break;
                case Keys.Prior:
                    break;
                case Keys.Next:
                    break;
                case Keys.End:
                    break;
                case Keys.Home:
                    return "<HOME>";
                case Keys.Left:
                    return "<LEFT>";
                case Keys.Up:
                    return "<UP>";
                case Keys.Right:
                    return "<RIGHT>";
                case Keys.Down:
                    return "<DOWN>";
                case Keys.Select:
                case Keys.Print:
                    return "<PRT>";
                case Keys.Execute:
                    break;
                case Keys.Snapshot:
                    break;
                case Keys.Insert:
                    return "<INS>";
                case Keys.Delete:
                    return "<DEL>";
                case Keys.Help:
                    break;
                case Keys.D0:
                    return "0";
                case Keys.D1:
                    return "1";
                case Keys.D2:
                    return "2";
                case Keys.D3:
                    return "3";
                case Keys.D4:
                    return "4";
                case Keys.D5:
                    return "5";
                case Keys.D6:
                    return "6";
                case Keys.D7:
                    return "7";
                case Keys.D8:
                    return "8";
                case Keys.D9:
                    return "9";
                case Keys.A:
                    return "a";
                case Keys.B:
                    return "b";
                case Keys.C:
                    return "c";
                case Keys.D:
                    return "d";
                case Keys.E:
                    return "e";
                case Keys.F:
                    return "f";
                case Keys.G:
                    return "g";
                case Keys.H:
                    return "h";
                case Keys.I:
                    return "i";
                case Keys.J:
                    return "j";
                case Keys.K:
                    return "k";
                case Keys.L:
                    return "l";
                case Keys.M:
                    return "m";
                case Keys.N:
                    return "n";
                case Keys.O:
                    return "o";
                case Keys.P:
                    return "p";
                case Keys.Q:
                    return "q";
                case Keys.R:
                    return "r";
                case Keys.S:
                    return "s";
                case Keys.T:
                    return "t";
                case Keys.U:
                    return "u";
                case Keys.V:
                    return "v";
                case Keys.W:
                    return "w";
                case Keys.X:
                    return "x";
                case Keys.Y:
                    return "y";
                case Keys.Z:
                    return "z";
                case Keys.LWin:
                    return "<WIN>";
                case Keys.RWin:
                    return "<WIN>";
                case Keys.Apps:
                    break;
                case Keys.Sleep:
                    break;
                case Keys.NumPad0:
                    return "0";
                case Keys.NumPad1:
                    return "1";
                case Keys.NumPad2:
                    return "2";
                case Keys.NumPad3:
                    return "3";
                case Keys.NumPad4:
                    return "4";
                case Keys.NumPad5:
                    return "5";
                case Keys.NumPad6:
                    return "6";
                case Keys.NumPad7:
                    return "7";
                case Keys.NumPad8:
                    return "8";
                case Keys.NumPad9:
                    return "9";
                case Keys.Multiply:
                    return "*";
                case Keys.Add:
                    return "+";
                case Keys.Separator:
                    return ",";
                case Keys.Subtract:
                    return "-";
                case Keys.Decimal:
                    return ".";
                case Keys.Divide:
                    return "/";
                case Keys.F1:
                    return "<F1>";
                case Keys.F2:
                    return "<F2>";
                case Keys.F3:
                    return "<F3>";
                case Keys.F4:
                    return "<F4>";
                case Keys.F5:
                    return "<F5>";
                case Keys.F6:
                    return "<F6>";
                case Keys.F7:
                    return "<F7>";
                case Keys.F8:
                    return "<F8>";
                case Keys.F9:
                    return "<F9>";
                case Keys.F10:
                    return "<F10>";
                case Keys.F11:
                    return "<F11>";
                case Keys.F12:
                    return "<F12>";
                case Keys.F13:
                    return "<F13>";
                case Keys.F14:
                    return "<F14>";
                case Keys.F15:
                    return "<F15>";
                case Keys.F16:
                    return "<F16>";
                case Keys.F17:
                    return "<F17>";
                case Keys.F18:
                    return "<F18>";
                case Keys.F19:
                    return "<F19>";
                case Keys.F20:
                    return "<F20>";
                case Keys.F21:
                    return "<F21>";
                case Keys.F22:
                    return "<F22>";
                case Keys.F23:
                    return "<F23>";
                case Keys.F24:
                    return "<F24>";
                case Keys.NumLock:
                    return "<NUM>";
                case Keys.Scroll:
                    return "<SCR>";
                case Keys.LShiftKey:
                    return "<SHIFT>";
                case Keys.RShiftKey:
                    return "<SHIFT>";
                case Keys.LControlKey:
                    return "<CTRL>";
                case Keys.RControlKey:
                    return "<CTRL>";
                case Keys.LMenu:
                    return "<MENU>";
                case Keys.RMenu:
                    return "<MENU>";
                case Keys.BrowserBack:
                    break;
                case Keys.BrowserForward:
                    break;
                case Keys.BrowserRefresh:
                    break;
                case Keys.BrowserStop:
                    break;
                case Keys.BrowserSearch:
                    break;
                case Keys.BrowserFavorites:
                    break;
                case Keys.BrowserHome:
                    break;
                case Keys.VolumeMute:
                    break;
                case Keys.VolumeDown:
                    break;
                case Keys.VolumeUp:
                    break;
                case Keys.MediaNextTrack:
                    break;
                case Keys.MediaPreviousTrack:
                    break;
                case Keys.MediaStop:
                    break;
                case Keys.MediaPlayPause:
                    break;
                case Keys.LaunchMail:
                    break;
                case Keys.SelectMedia:
                    break;
                case Keys.LaunchApplication1:
                    break;
                case Keys.LaunchApplication2:
                    break;
                case Keys.OemSemicolon:
                    return ";";
                case Keys.Oemplus:
                    return "+";
                case Keys.Oemcomma:
                    return ",";
                case Keys.OemMinus:
                    return "-";
                case Keys.OemPeriod:
                    return ".";
                case Keys.OemQuestion:
                    return "?";
                case Keys.Oemtilde:
                    return "~";
                case Keys.OemOpenBrackets:
                    return "[";
                case Keys.OemPipe:
                    return "|";
                case Keys.OemCloseBrackets:
                    return "]";
                case Keys.OemQuotes:
                    return "\'";
                case Keys.Oem8:
                    break;
                case Keys.OemBackslash:
                    return "/";
                case Keys.ProcessKey:
                    break;
                case Keys.Packet:
                    break;
                case Keys.Attn:
                    break;
                case Keys.Crsel:
                    break;
                case Keys.Exsel:
                    break;
                case Keys.EraseEof:
                    break;
                case Keys.Play:
                    break;
                case Keys.Zoom:
                    break;
                case Keys.NoName:
                    break;
                case Keys.Pa1:
                    break;
                case Keys.OemClear:
                    break;
                case Keys.Shift:
                    return "<SHIFT>";
                case Keys.Control:
                    return "<CTRL>";
                case Keys.Alt:
                    return "<ALT>";
                default:
                    return $"<UNK: {key}>";
            }
            return "";
        }
    }
}
