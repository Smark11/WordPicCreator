using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Common.IsolatedStoreage;

namespace WordPicCreator
{
    public partial class SettingsPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        public SettingsPage()
        {
            InitializeComponent();
            DataContext = this;

            LoadFonts();
            LoadColors();
            LoadFontSizes();

            ChooseSelectedItems();
        }

        private void ChooseSelectedItems()
        {
            //1.  Color
            string colorToFind = IS.GetSettingStringValue(ISKeys.FONTCOLOR);
            foreach (var row in WpColors)
            {
                if (row.ColorName.ToUpper().Equals(colorToFind.ToUpper()))
                {
                    SelectedColor = row;
                }
            }

            //2.  Font Size
            string fontSizeToFind = IS.GetSettingStringValue(ISKeys.FONTSIZE);
            foreach (var row in AvailFontSizes)
            {
                if (row.FontSizeName.ToUpper().Equals(fontSizeToFind.ToUpper()))
                {
                    SelectedFontSize = row;
                }
            }

            //3.  Font
            string fontToFind = IS.GetSettingStringValue(ISKeys.FONT);
            foreach (var row in Fonts)
            {
                if (row.FontName.ToUpper().Equals(fontToFind.ToUpper()))
                {
                    SelectedFont = row;
                }
            }

            //4.  Bold
            if (IS.GetSetting(ISKeys.BOLD) != null)
            {
                Bold = (bool)IS.GetSetting(ISKeys.BOLD);
            }

            //5.  Italic
            if (IS.GetSetting(ISKeys.ITALIC) != null)
            {
                Italic = (bool)IS.GetSetting(ISKeys.ITALIC);
            }
        }

        private void SaveSettings()
        {
            //1. Color
            IS.SaveSetting(ISKeys.FONTCOLOR, SelectedColor.ColorName);

            //2. Font Size
            IS.SaveSetting(ISKeys.FONTSIZE, SelectedFontSize.FontSize);

            //3.  Font
            IS.SaveSetting(ISKeys.FONT, SelectedFont.FontName);

            //4.  Bold
            IS.SaveSetting(ISKeys.BOLD, Bold);

            //5.  Italic
            IS.SaveSetting(ISKeys.ITALIC, Italic);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SaveSettings();
            base.OnNavigatedFrom(e);
        }

        private void LoadFontSizes()
        {
            AvailFontSizes = new ObservableCollection<WordPicFontSizes>();

            AvailFontSizes.Add(new WordPicFontSizes(20, "20"));
            AvailFontSizes.Add(new WordPicFontSizes(21, "21"));

            AvailFontSizes.Add(new WordPicFontSizes(22, "22"));
            AvailFontSizes.Add(new WordPicFontSizes(23, "23"));
            AvailFontSizes.Add(new WordPicFontSizes(24, "24"));
            AvailFontSizes.Add(new WordPicFontSizes(25, "25"));
            AvailFontSizes.Add(new WordPicFontSizes(26, "26"));
            AvailFontSizes.Add(new WordPicFontSizes(27, "27"));
            AvailFontSizes.Add(new WordPicFontSizes(28, "28"));
            AvailFontSizes.Add(new WordPicFontSizes(29, "29"));
            AvailFontSizes.Add(new WordPicFontSizes(30, "30"));

            AvailFontSizes.Add(new WordPicFontSizes(31, "31"));
            AvailFontSizes.Add(new WordPicFontSizes(32, "32"));
            AvailFontSizes.Add(new WordPicFontSizes(33, "33"));
            AvailFontSizes.Add(new WordPicFontSizes(34, "34"));
            AvailFontSizes.Add(new WordPicFontSizes(35, "35"));
            AvailFontSizes.Add(new WordPicFontSizes(36, "36"));
            AvailFontSizes.Add(new WordPicFontSizes(37, "37"));
            AvailFontSizes.Add(new WordPicFontSizes(38, "38"));
            AvailFontSizes.Add(new WordPicFontSizes(39, "39"));

            AvailFontSizes.Add(new WordPicFontSizes(40, "40"));
            AvailFontSizes.Add(new WordPicFontSizes(41, "41"));
            AvailFontSizes.Add(new WordPicFontSizes(42, "42"));
            AvailFontSizes.Add(new WordPicFontSizes(43, "43"));
            AvailFontSizes.Add(new WordPicFontSizes(44, "44"));
            AvailFontSizes.Add(new WordPicFontSizes(45, "45"));
            AvailFontSizes.Add(new WordPicFontSizes(46, "46"));
            AvailFontSizes.Add(new WordPicFontSizes(47, "47"));
            AvailFontSizes.Add(new WordPicFontSizes(48, "48"));

            AvailFontSizes.Add(new WordPicFontSizes(49, "49"));
            AvailFontSizes.Add(new WordPicFontSizes(50, "50"));
            AvailFontSizes.Add(new WordPicFontSizes(51, "51"));
            AvailFontSizes.Add(new WordPicFontSizes(52, "52"));
            AvailFontSizes.Add(new WordPicFontSizes(53, "53"));
            AvailFontSizes.Add(new WordPicFontSizes(54, "54"));
            AvailFontSizes.Add(new WordPicFontSizes(55, "55"));
            AvailFontSizes.Add(new WordPicFontSizes(56, "56"));
            AvailFontSizes.Add(new WordPicFontSizes(57, "57"));

            AvailFontSizes.Add(new WordPicFontSizes(58, "58"));
            AvailFontSizes.Add(new WordPicFontSizes(59, "59"));
            AvailFontSizes.Add(new WordPicFontSizes(60, "60"));
            AvailFontSizes.Add(new WordPicFontSizes(61, "61"));
            AvailFontSizes.Add(new WordPicFontSizes(62, "62"));
            AvailFontSizes.Add(new WordPicFontSizes(63, "63"));
            AvailFontSizes.Add(new WordPicFontSizes(64, "64"));
            AvailFontSizes.Add(new WordPicFontSizes(65, "65"));
        }

        private void LoadColors()
        {
            WpColors = new ObservableCollection<WordPicColors>();
            WpColors.Add(new WordPicColors(new SolidColorBrush(Colors.Black), "Black"));
            WpColors.Add(new WordPicColors(new SolidColorBrush(Colors.Blue), "Blue"));
            WpColors.Add(new WordPicColors(new SolidColorBrush(Colors.Brown), "Brown"));
            WpColors.Add(new WordPicColors(new SolidColorBrush(Colors.Cyan), "Cyan"));

            WpColors.Add(new WordPicColors(new SolidColorBrush(Colors.DarkGray), "Dark Gray"));
            WpColors.Add(new WordPicColors(new SolidColorBrush(Colors.Gray), "Gray"));
            WpColors.Add(new WordPicColors(new SolidColorBrush(Colors.Green), "Green"));
            WpColors.Add(new WordPicColors(new SolidColorBrush(Colors.LightGray), "Light Gray"));

            WpColors.Add(new WordPicColors(new SolidColorBrush(Colors.Magenta), "Magenta"));
            WpColors.Add(new WordPicColors(new SolidColorBrush(Colors.Orange), "Orange"));
            WpColors.Add(new WordPicColors(new SolidColorBrush(Colors.Purple), "Purple"));
            WpColors.Add(new WordPicColors(new SolidColorBrush(Colors.Red), "Red"));

            WpColors.Add(new WordPicColors(new SolidColorBrush(Colors.White), "White"));
            WpColors.Add(new WordPicColors(new SolidColorBrush(Colors.Yellow), "Yellow"));
        }

        private void LoadFonts()
        {
            Fonts = new ObservableCollection<WordPicFont>();
            Fonts.Add(new WordPicFont("Arial"));
            Fonts.Add(new WordPicFont("Arial Black"));
            Fonts.Add(new WordPicFont("Arial Bold"));
            Fonts.Add(new WordPicFont("Arial Italic"));
            Fonts.Add(new WordPicFont("Calibri"));
            Fonts.Add(new WordPicFont("Calibri Bold"));
            Fonts.Add(new WordPicFont("Calibri Italic"));
            Fonts.Add(new WordPicFont("Comic Sans MS"));
            Fonts.Add(new WordPicFont("Comic Sans MS Bold"));
            Fonts.Add(new WordPicFont("Courier New"));
            Fonts.Add(new WordPicFont("Courier New Bold"));
            Fonts.Add(new WordPicFont("Courier New Italic"));
            Fonts.Add(new WordPicFont("Georgia"));
            Fonts.Add(new WordPicFont("Georgia Bold"));
            Fonts.Add(new WordPicFont("Georgia Italic"));
            Fonts.Add(new WordPicFont("Lucida Sans Unicode"));
            Fonts.Add(new WordPicFont("Malgun Gothic"));
            Fonts.Add(new WordPicFont("Meiryo UI"));
            Fonts.Add(new WordPicFont("Microsoft YaHei"));
            Fonts.Add(new WordPicFont("Segoe UI"));
            Fonts.Add(new WordPicFont("Segoe WP"));
            Fonts.Add(new WordPicFont("Segoe WP Black"));
            Fonts.Add(new WordPicFont("Segoe WP Bold"));
            Fonts.Add(new WordPicFont("Segoe WP Light"));
            Fonts.Add(new WordPicFont("Segoe WP Semibold"));
            Fonts.Add(new WordPicFont("Segoe WP SemiLight"));
            Fonts.Add(new WordPicFont("Tahoma"));
            Fonts.Add(new WordPicFont("Tahoma Bold"));
            Fonts.Add(new WordPicFont("Times New Roman"));
            Fonts.Add(new WordPicFont("Times New Roman Bold"));
            Fonts.Add(new WordPicFont("Times New Roman Italic"));
            Fonts.Add(new WordPicFont("Trebuchet MS"));
            Fonts.Add(new WordPicFont("Trebuchet MS Bold"));
            Fonts.Add(new WordPicFont("Veranda"));
            Fonts.Add(new WordPicFont("Veranda Bold"));
            Fonts.Add(new WordPicFont("Veranda Italic"));
            Fonts.Add(new WordPicFont("Webdings"));
            Fonts.Add(new WordPicFont("Wingdings"));
        }

        private bool _bold;
        public bool Bold
        {
            get { return _bold; }
            set { _bold = value; RaisePropertyChanged("Bold"); }
        }

        private bool  _italic;
        public bool  Italic
        {
            get { return _italic; }
            set { _italic = value; RaisePropertyChanged("Italic"); }
        }
        

        private ObservableCollection<WordPicFontSizes> _availFontSizes;
        public ObservableCollection<WordPicFontSizes> AvailFontSizes
        {
            get { return _availFontSizes; }
            set { _availFontSizes = value; RaisePropertyChanged("AvailFontSizes"); }
        }

        private WordPicFontSizes _selectedFontSize;
        public WordPicFontSizes SelectedFontSize
        {
            get { return _selectedFontSize; }
            set { _selectedFontSize = value; RaisePropertyChanged("SelectedFontSize"); }
        }
        
        private ObservableCollection<WordPicFont> _fonts;
        public ObservableCollection<WordPicFont> Fonts
        {
            get { return _fonts; }
            set { _fonts = value; RaisePropertyChanged("Fonts"); }
        }

        private WordPicFont _selectedFont;
        public WordPicFont SelectedFont
        {
            get { return _selectedFont; }
            set { _selectedFont = value; RaisePropertyChanged("SelectedFont"); }
        }
        

        private ObservableCollection<WordPicColors> _wpColors;
        public ObservableCollection<WordPicColors> WpColors
        {
            get { return _wpColors; }
            set { _wpColors = value; RaisePropertyChanged("WpColors"); }
        }

        private WordPicColors _selectedColor;
        public WordPicColors SelectedColor
        {
            get { return _selectedColor; }
            set { _selectedColor = value; RaisePropertyChanged("SelectedColor"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string p)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(p));
            }
        }
    }

    public class WordPicFontSizes : INotifyPropertyChanged
    {
        public WordPicFontSizes(int fontSize, string fontSizeName)
        {
            FontSize = fontSize;
            FontSizeName = fontSizeName;
        }

        private int _fontSize;
        public int FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; RaisePropertyChanged("FontSize"); }
        }
        

        private string _fontSizeName;
        public string FontSizeName
        {
            get { return _fontSizeName; }
            set { _fontSizeName = value; RaisePropertyChanged("FontSizeName"); }
        }
        

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string p)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(p));
            }
        }
    }

    public class WordPicColors : INotifyPropertyChanged
    {
        public WordPicColors(SolidColorBrush brush, string colorName)
        {
            ColorToBindTo = brush;
            ColorName = colorName;
        }

        private string _colorName;
        public string ColorName
        {
            get { return _colorName; }
            set { _colorName = value; RaisePropertyChanged("ColorName"); }
        }
        

        private SolidColorBrush _colorToBindTo;
        public SolidColorBrush ColorToBindTo
        {
            get { return _colorToBindTo; }
            set { _colorToBindTo = value; RaisePropertyChanged("ColorToBindTo"); }
        }
        

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string p)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(p));
            }
        }
    }

    public class WordPicFont : INotifyPropertyChanged
    {
        public WordPicFont(string font)
        {
            FontName = font;
            FontToBindTo = new FontFamily(font);
        }

        private string _fontName;
        public string FontName
        {
            get { return _fontName; }
            set { _fontName = value; RaisePropertyChanged("FontName"); }
        }

        private FontFamily _fontToBindTo;
        public FontFamily FontToBindTo
        {
            get { return _fontToBindTo; }
            set { _fontToBindTo = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string p)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(p));
            }
        }
    }
}