package crc64350623dcb797cc38;


public class AndroidHttpClientAdapter
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.microsoft.appcenter.http.HttpClient,
		java.io.Closeable
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_callAsync:(Ljava/lang/String;Ljava/lang/String;Ljava/util/Map;Lcom/microsoft/appcenter/http/HttpClient$CallTemplate;Lcom/microsoft/appcenter/http/ServiceCallback;)Lcom/microsoft/appcenter/http/ServiceCall;:GetCallAsync_Ljava_lang_String_Ljava_lang_String_Ljava_util_Map_Lcom_microsoft_appcenter_http_HttpClient_CallTemplate_Lcom_microsoft_appcenter_http_ServiceCallback_Handler:Com.Microsoft.Appcenter.Http.IAndroidHttpClientInvoker, Microsoft.AppCenter.Android.Bindings\n" +
			"n_reopen:()V:GetReopenHandler:Com.Microsoft.Appcenter.Http.IAndroidHttpClientInvoker, Microsoft.AppCenter.Android.Bindings\n" +
			"n_close:()V:GetCloseHandler:Java.IO.ICloseableInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Microsoft.AppCenter.AndroidHttpClientAdapter, Microsoft.AppCenter", AndroidHttpClientAdapter.class, __md_methods);
	}


	public AndroidHttpClientAdapter ()
	{
		super ();
		if (getClass () == AndroidHttpClientAdapter.class)
			mono.android.TypeManager.Activate ("Microsoft.AppCenter.AndroidHttpClientAdapter, Microsoft.AppCenter", "", this, new java.lang.Object[] {  });
	}


	public com.microsoft.appcenter.http.ServiceCall callAsync (java.lang.String p0, java.lang.String p1, java.util.Map p2, com.microsoft.appcenter.http.HttpClient.CallTemplate p3, com.microsoft.appcenter.http.ServiceCallback p4)
	{
		return n_callAsync (p0, p1, p2, p3, p4);
	}

	private native com.microsoft.appcenter.http.ServiceCall n_callAsync (java.lang.String p0, java.lang.String p1, java.util.Map p2, com.microsoft.appcenter.http.HttpClient.CallTemplate p3, com.microsoft.appcenter.http.ServiceCallback p4);


	public void reopen ()
	{
		n_reopen ();
	}

	private native void n_reopen ();


	public void close ()
	{
		n_close ();
	}

	private native void n_close ();

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
