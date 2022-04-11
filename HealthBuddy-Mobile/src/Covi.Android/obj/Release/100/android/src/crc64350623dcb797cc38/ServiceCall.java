package crc64350623dcb797cc38;


public class ServiceCall
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.microsoft.appcenter.http.ServiceCall
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_cancel:()V:GetCancelHandler:Com.Microsoft.Appcenter.Http.IServiceCallInvoker, Microsoft.AppCenter.Android.Bindings\n" +
			"";
		mono.android.Runtime.register ("Microsoft.AppCenter.ServiceCall, Microsoft.AppCenter", ServiceCall.class, __md_methods);
	}


	public ServiceCall ()
	{
		super ();
		if (getClass () == ServiceCall.class)
			mono.android.TypeManager.Activate ("Microsoft.AppCenter.ServiceCall, Microsoft.AppCenter", "", this, new java.lang.Object[] {  });
	}


	public void cancel ()
	{
		n_cancel ();
	}

	private native void n_cancel ();

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
