package mono.com.microsoft.windowsazure.messaging.notificationhubs;


public class NotificationListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.microsoft.windowsazure.messaging.notificationhubs.NotificationListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onPushNotificationReceived:(Landroid/content/Context;Lcom/microsoft/windowsazure/messaging/notificationhubs/NotificationMessage;)V:GetOnPushNotificationReceived_Landroid_content_Context_Lcom_microsoft_windowsazure_messaging_notificationhubs_NotificationMessage_Handler:WindowsAzure.Messaging.NotificationHubs.INotificationListenerInvoker, Xamarin.Azure.NotificationHubs.Android\n" +
			"";
		mono.android.Runtime.register ("WindowsAzure.Messaging.NotificationHubs.INotificationListenerImplementor, Xamarin.Azure.NotificationHubs.Android", NotificationListenerImplementor.class, __md_methods);
	}


	public NotificationListenerImplementor ()
	{
		super ();
		if (getClass () == NotificationListenerImplementor.class)
			mono.android.TypeManager.Activate ("WindowsAzure.Messaging.NotificationHubs.INotificationListenerImplementor, Xamarin.Azure.NotificationHubs.Android", "", this, new java.lang.Object[] {  });
	}


	public void onPushNotificationReceived (android.content.Context p0, com.microsoft.windowsazure.messaging.notificationhubs.NotificationMessage p1)
	{
		n_onPushNotificationReceived (p0, p1);
	}

	private native void n_onPushNotificationReceived (android.content.Context p0, com.microsoft.windowsazure.messaging.notificationhubs.NotificationMessage p1);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
