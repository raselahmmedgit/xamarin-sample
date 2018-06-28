package md53da296b4b9f7aa6e187dc28cf9fd28f3;


public class PNIIDService
	extends com.google.firebase.iid.FirebaseInstanceIdService
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onTokenRefresh:()V:GetOnTokenRefreshHandler\n" +
			"";
		mono.android.Runtime.register ("Plugin.PushNotification.PNIIDService, Plugin.PushNotification", PNIIDService.class, __md_methods);
	}


	public PNIIDService ()
	{
		super ();
		if (getClass () == PNIIDService.class)
			mono.android.TypeManager.Activate ("Plugin.PushNotification.PNIIDService, Plugin.PushNotification", "", this, new java.lang.Object[] {  });
	}


	public void onTokenRefresh ()
	{
		n_onTokenRefresh ();
	}

	private native void n_onTokenRefresh ();

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
