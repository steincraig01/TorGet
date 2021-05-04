package crc641d0f40866223f90f;


public class IconToggleButton
	extends android.widget.ToggleButton
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_setText:(Ljava/lang/CharSequence;Landroid/widget/TextView$BufferType;)V:GetSetText_Ljava_lang_CharSequence_Landroid_widget_TextView_BufferType_Handler\n" +
			"n_onAttachedToWindow:()V:GetOnAttachedToWindowHandler\n" +
			"n_onDetachedFromWindow:()V:GetOnDetachedFromWindowHandler\n" +
			"";
		mono.android.Runtime.register ("JoanZapata.XamarinIconify.Widget.IconToggleButton, xamarin-iconify", IconToggleButton.class, __md_methods);
	}


	public IconToggleButton (android.content.Context p0)
	{
		super (p0);
		if (getClass () == IconToggleButton.class)
			mono.android.TypeManager.Activate ("JoanZapata.XamarinIconify.Widget.IconToggleButton, xamarin-iconify", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public IconToggleButton (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == IconToggleButton.class)
			mono.android.TypeManager.Activate ("JoanZapata.XamarinIconify.Widget.IconToggleButton, xamarin-iconify", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public IconToggleButton (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == IconToggleButton.class)
			mono.android.TypeManager.Activate ("JoanZapata.XamarinIconify.Widget.IconToggleButton, xamarin-iconify", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public void setText (java.lang.CharSequence p0, android.widget.TextView.BufferType p1)
	{
		n_setText (p0, p1);
	}

	private native void n_setText (java.lang.CharSequence p0, android.widget.TextView.BufferType p1);


	public void onAttachedToWindow ()
	{
		n_onAttachedToWindow ();
	}

	private native void n_onAttachedToWindow ();


	public void onDetachedFromWindow ()
	{
		n_onDetachedFromWindow ();
	}

	private native void n_onDetachedFromWindow ();

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