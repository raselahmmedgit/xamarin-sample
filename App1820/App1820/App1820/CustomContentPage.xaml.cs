using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1820
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CustomContentPage : ContentPage
    {
        public static readonly BindableProperty FormattedTitleProperty = BindableProperty.Create(nameof(FormattedTitle), typeof(FormattedString), typeof(CustomContentPage), null);

        public FormattedString FormattedTitle
        {
            get { return (FormattedString)GetValue(FormattedTitleProperty); }
            set
            {
                SetValue(FormattedTitleProperty, value);
            }
        }

        public static readonly BindableProperty FormattedSubtitleProperty = BindableProperty.Create(nameof(FormattedSubtitle), typeof(FormattedString), typeof(CustomContentPage), null);

        public FormattedString FormattedSubtitle
        {
            get { return (FormattedString)GetValue(FormattedSubtitleProperty); }
            set
            {
                SetValue(FormattedSubtitleProperty, value);
            }
        }

        public static readonly BindableProperty SubtitleProperty = BindableProperty.Create(nameof(Subtitle), typeof(string), typeof(CustomContentPage), null);


        public string Subtitle
        {
            get { return (string)GetValue(SubtitleProperty); }
            set
            {
                SetValue(SubtitleProperty, value);
            }
        }


        public CustomContentPage()
        {
            CustomNavigationPage.SetTitlePosition(this, CustomNavigationPage.TitleAlignment.Start);
            CustomNavigationPage.SetTitleFont(this, Font.SystemFontOfSize(20));
            CustomNavigationPage.SetSubtitleFont(this, Font.SystemFontOfSize(16));
            CustomNavigationPage.SetGradientColors(this, new Tuple<Color, Color>(Color.FromHex("#073f5d"), Color.FromHex("#073f5d")));
            InitializeComponent();
        }
    }
}