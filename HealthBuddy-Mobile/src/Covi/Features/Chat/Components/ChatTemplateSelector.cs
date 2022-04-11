using Xamarin.Forms;

namespace Covi.Features.Chat.Components
{
    public class ChatDataTemplateSelector : DataTemplateSelector
    {
        DataTemplate incomingDataTemplate;
        DataTemplate outgoingDataTemplate;
        DataTemplate outgoingButtonTemplate;

        public ChatDataTemplateSelector()
        {
            this.incomingDataTemplate = new DataTemplate(typeof(IncomingViewCell));
            this.outgoingDataTemplate = new DataTemplate(typeof(OutgoingViewCell));
            this.outgoingButtonTemplate = new DataTemplate(typeof(OutgoingButtonViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, Xamarin.Forms.BindableObject container)
        {
            var messageVm = item as RapidProMessage;
            if (messageVm == null)
            {
                return null;
            }

            if (messageVm.User == MessageUserEnum.UserHealthBuddy.ToDescriptionAttr())
            {
                if (messageVm.MessageAction == true)
                {
                    return outgoingButtonTemplate;
                }
                else
                {
                    return outgoingDataTemplate;
                }
            }
            else if (messageVm.User == MessageUserEnum.UserLogin.ToDescriptionAttr())
            {
                return incomingDataTemplate;
            }
            else if (messageVm.User == MessageUserEnum.UserAnonymous.ToDescriptionAttr())
            {
                return incomingDataTemplate;
            }
            else
            {
                if (messageVm.MessageAction == true)
                {
                    return outgoingButtonTemplate;
                }
                else
                {
                    return outgoingDataTemplate;
                }
            }
        }

    }
}
