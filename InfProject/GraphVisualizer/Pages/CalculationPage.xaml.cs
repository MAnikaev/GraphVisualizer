using Contract;
using System.Reflection;

namespace GraphVisualizer;

public partial class CalculationPage : ContentPage
{
	public CalculationPage()
	{
		InitializeComponent();
	}

	public async void ChooseLibFileAndGoToResultPage(object sender, EventArgs e)
	{
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.WinUI, new[] { ".dll" } },
                    })
            });

            var assembly = Assembly.LoadFrom(result.FullPath);
            if (assembly != null)
            {
                var implementation = assembly.GetTypes()
                                     .FirstOrDefault(t => typeof(IGraphSolver).IsAssignableFrom(t) && !t.IsInterface);
                if(implementation == null)
                {
                    await DisplayAlert("Некорректная сборка", "Выберите другой файл", "ОК");
                    return;
                }
                DataFromUser.Solver = Activator.CreateInstance(implementation) as IGraphSolver;
                await Navigation.PushAsync(new ResultPage());
            }
        }
        catch
        {
            await DisplayAlert("Ошибка", "Выберите файл", "ОК");
            return;
        }

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