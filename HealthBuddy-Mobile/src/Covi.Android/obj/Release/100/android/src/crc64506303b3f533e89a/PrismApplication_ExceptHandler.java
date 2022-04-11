package crc64506303b3f533e89a;


public class PrismApplication_ExceptHandler
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		java.lang.Thread.UncaughtExceptionHandler
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_uncaughtException:(Ljava/lang/Thread;Ljava/lang/Throwable;)V:GetUncaughtException_Ljava_lang_Thread_Ljava_lang_Throwable_Handler:Java.Lang.Thread/IUncaughtExceptionHandlerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Prism.PrismApplication+ExceptHandler, Prism.Forms.Extended", PrismApplication_ExceptHandler.class, __md_methods);
	}


	public PrismApplication_ExceptHandler ()
	{
		super ();
		if (getClass () == PrismApplication_ExceptHandler.class)
			mono.android.TypeManager.Activate ("Prism.PrismApplication+ExceptHandler, Prism.Forms.Extended", "", this, new java.lang.Object[] {  });
	}


	public void uncaughtException (java.lang.Thread p0, java.lang.Throwable p1)
	{
		n_uncaughtException (p0, p1);
	}

	private native void n_uncaughtException (java.lang.Thread p0, java.lang.Throwable p1);

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
