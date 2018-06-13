using Xamarin.Forms;

namespace App1710.ApiHelper
{
    public class TokenContainer : ITokenContainer
    {
        private const string ApiCurrentTokenKey = "ApiToken";

        public object ApiCurrentToken
        {
            get
            {
                return Application.Current.Properties.ContainsKey(ApiCurrentTokenKey) ? Application.Current.Properties[ApiCurrentTokenKey] : null;
            }
            set
            {
                if (Application.Current.Properties != null)
                {
                    if (!Application.Current.Properties.ContainsKey(ApiCurrentTokenKey)) {
                        Application.Current.Properties[ApiCurrentTokenKey] = value;
                        Application.Current.SavePropertiesAsync();
                    }
                }
            }
        }

        public bool IsApiCurrentToken()
        {
            return this.ApiCurrentToken == null ? false : true;
        }

        public void ClearApiCurrentToken()
        {
            Application.Current.Properties.Clear();
            Application.Current.SavePropertiesAsync();
        }
    }
}
