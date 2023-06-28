using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace GraphVisualizer
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        public async void NavigateToInfomationPage(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new InformationPage());
        }
        public void Exit(object sender, EventArgs e)
        {
            App.Current.Quit();
        }
        public async void ChooseFileAndGoToCalculatePage(object sender, EventArgs e)
        {
            try
            {
                FileResult result = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.WinUI, new[] { ".csv" } },
                    })
                });

                if (result == null)
                    return;

                var content = await File.ReadAllTextAsync(result.FullPath);
                var graph = GetMatrixWithCsvString(content.Substring(0, content.Length - 2));

                if (graph == null) 
                { 
                    await DisplayAlert("Некорректный граф", "Выберите другой файл", "ОК"); 
                    return;
                }

                if (IsGraphCorrect(graph))
                {
                    DataFromUser.Graph = graph;
                    await Navigation.PushAsync(new CalculationPage());
                }
                else
                    await DisplayAlert("Некорректный граф", "Некорректный граф", "ОК");
            }
            catch
            {
                await DisplayAlert("Неверные данные", "Ошибка выбора файла", "ОК");
                return;
            }
        }
        private int[,] GetMatrixWithCsvString(string csvString)
        {
            try
            {
                string[] rows = csvString.Split("\r\n");
                int rowsCount = rows.Length;
                int colsCount = rows[0].Split(',').Length;
                int[,] matrix = new int[rowsCount, colsCount];

                for (int i = 0; i < rowsCount; i++)
                {
                    string[] cols = rows[i].Split(',');
                    for (int j = 0; j < colsCount; j++)
                    {
                        matrix[i, j] = int.Parse(cols[j]);
                    }
                } 
                return matrix;
            }
            catch
            {
                return null;
            }
        }
        public static bool IsGraphCorrect(int[,] graph)
        {
            try
            {
                if (graph.GetLength(0) != graph.GetLength(1)) { return false; }

                for (int i = 0; i < graph.GetLength(0); i++)
                    for (int j = 0; j < graph.GetLength(1); j++)
                        if ((i == j && graph[i, j] != 0) || graph[i, j] != graph[j, i]) { return false; }

                if (graph == null) { return false; }

                return true;
            }
            catch
            {
                return false;
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
}