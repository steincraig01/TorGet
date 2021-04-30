package crc64ed83947e49f54016;


public class Border
	extends android.graphics.drawable.GradientDrawable
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Syncfusion.Android.Buttons.Border, Syncfusion.Buttons.Android", Border.class, __md_methods);
	}


	public Border ()
	{
		super ();
		if (getClass () == Border.class)
			mono.android.TypeManager.Activate ("Syncfusion.Android.Buttons.Border, Syncfusion.Buttons.Android", "", this, new java.lang.Object[] {  });
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
