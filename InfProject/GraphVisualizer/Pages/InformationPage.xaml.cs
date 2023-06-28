namespace GraphVisualizer;

public partial class InformationPage : ContentPage
{
	public InformationPage()
	{
		InitializeComponent();
	}
    public void SwitchTheme(object sender, EventArgs e)
    {
        var mergedDictionaries = Application.Current.Resources.MergedDictionaries;
        if (mergedDictionaries != null && DataFromUser.IsDarkTheme)
        {
            mergedDictionaries.Clear();
            mergedDictionaries.Add(new LightTheme());
            DataFromUser.IsDarkTheme = false;
        }
        else
        {
            mergedDictionaries.Clear();
            mergedDictionaries.Add(new DarkTheme());
            DataFromUser.IsDarkTheme = true;
        }
    }
}