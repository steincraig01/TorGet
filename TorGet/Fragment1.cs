//using Android.App;

//using Android.Support.V4.App;
//using Android.Support.V7.Widget;
//using Android.Views;
//using Android.Widget;
//using Java.Lang;

//public class TorDetailDialog : Android.Support.V4.App.DialogFragment
//{
//    override public View OnCreateView(LayoutInflater inflater, ViewGroup container,
//                             Bundle savedInstanceState)
//{
//    Bundle args = getArguments();
//    String title = args.getString("title");
//    View v = inflater.inflate(R.layout.action_bar_dialog, container, false);
//    TextView tv = (TextView)v.findViewById(R.id.text);
//    tv.setText("This is an instance of ActionBarDialog");
//    Toolbar toolbar = (Toolbar)v.findViewById(R.id.my_toolbar);
//    toolbar.setOnMenuItemClickListener(new Toolbar.OnMenuItemClickListener() {
//            @Override
//            public boolean onMenuItemClick(MenuItem item)
//    {
//        // Handle the menu item
//        return true;
//    }
//});
//toolbar.inflateMenu(R.menu.menu_main);
//toolbar.setTitle(title);
//return v;
//    }
//    @Override
//    public Dialog onCreateDialog(Bundle savedInstanceState)
//{
//    Dialog dialog = super.onCreateDialog(savedInstanceState);
//    // request a window without the title
//    dialog.getWindow().requestFeature(Window.FEATURE_NO_TITLE);
//    return dialog;
//}
//}