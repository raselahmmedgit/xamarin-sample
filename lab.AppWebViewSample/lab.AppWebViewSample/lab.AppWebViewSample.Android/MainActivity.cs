using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Webkit;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Threading.Tasks;

namespace lab.AppWebViewSample.Droid
{
	[Activity (Label = "lab.AppWebViewSample.Android", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		int count = 1;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

            //load url by WebView tools
            WebView webView = FindViewById<WebView>(Resource.Id.webViewOnTrackHealth);
            webView.SetWebViewClient(new WebViewClient());
            webView.LoadUrl(AppConstants.WebViewLoadUrl);
            webView.Settings.JavaScriptEnabled = true;
            webView.Settings.BuiltInZoomControls = true;
            webView.Settings.SetSupportZoom(true);
            webView.ScrollBarStyle = ScrollbarStyles.OutsideOverlay;
            webView.ScrollbarFadingEnabled = true;

            ////open default browser
            //WebBrowser.OpenPage(this, AppConstants.WebViewLoadUrl);

            var data = GetStudentList();

            //var data2 = await GetStudents();
        }

        #region Api Call

        public async Task<List<Student>> GetStudents()
        {
            try
            {
                StudentManager studentManager = new StudentManager();
                var data = await studentManager.GetStudentListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private List<Student> GetStudentList()
        {
            try
            {
                List<Student> studentList = new List<Student>();

                string url = "http://localhost:47641/api/default";

                var request = HttpWebRequest.Create(url);
                request.ContentType = "application/json";
                request.Method = "GET";

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                    }

                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        var content = reader.ReadToEnd();
                        if (string.IsNullOrWhiteSpace(content))
                        {
                            Console.Out.WriteLine("Response contained empty body...");
                        }
                        else
                        {
                            Console.Out.WriteLine("Response Body: \r\n {0}", content);
                        }
                    }
                }

                return studentList;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        #endregion
    }
}


