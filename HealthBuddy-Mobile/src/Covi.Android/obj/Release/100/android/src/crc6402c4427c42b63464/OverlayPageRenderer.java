package crc6402c4427c42b63464;


public class OverlayPageRenderer
	extends crc643f46942d9dd1fff9.PageRenderer
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Covi.Droid.CustomRenderers.OverlayPageRenderer, Covi.Android", OverlayPageRenderer.class, __md_methods);
	}


	public OverlayPageRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == OverlayPageRenderer.class)
			mono.android.TypeManager.Activate ("Covi.Droid.CustomRenderers.OverlayPageRenderer, Covi.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public OverlayPageRenderer (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == OverlayPageRenderer.class)
			mono.android.TypeManager.Activate ("Covi.Droid.CustomRenderers.OverlayPageRenderer, Covi.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public OverlayPageRenderer (android.content.Context p0)
	{
		super (p0);
		if (getClass () == OverlayPageRenderer.class)
			mono.android.TypeManager.Activate ("Covi.Droid.CustomRenderers.OverlayPageRenderer, Covi.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}

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
