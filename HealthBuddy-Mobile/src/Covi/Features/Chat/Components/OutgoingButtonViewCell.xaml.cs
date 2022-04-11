using System;
using Xamarin.Forms;

namespace Covi.Features.Chat.Components
{
    public partial class OutgoingButtonViewCell : ViewCell
    {
        public OutgoingButtonViewCell()
        {
            InitializeComponent();
        }

        private void OnActionSendCommand(object sender, EventArgs e)
        {
            try
            {
                Console.WriteLine("OutgoingButtonViewCell - OnActionSendCommand");

                var chatViewModel = (this.Parent.Parent.BindingContext as ChatViewModel);

                Button btnActionSendCommand = (Button)sender;
                if (btnActionSendCommand != null)
                {
                    chatViewModel.RapidProMessageId = btnActionSendCommand.ClassId.Trim().ToString();
                    chatViewModel.ActionInputText = btnActionSendCommand.Text.Trim().ToString();
                }
                chatViewModel.OnActionSendCommand.Execute(null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OutgoingButtonViewCell - OnActionSendCommand: Exception - {ex.Message.ToString()}");
            }
        }
    }
}
