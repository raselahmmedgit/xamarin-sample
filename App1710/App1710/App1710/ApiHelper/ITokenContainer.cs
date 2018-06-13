namespace App1710.ApiHelper
{
    public interface ITokenContainer
    {
        object ApiCurrentToken { get; set; }

        bool IsApiCurrentToken();

        void ClearApiCurrentToken();
    }
}
