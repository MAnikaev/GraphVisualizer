using Contract;
using Microsoft.Maui.Controls.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace GraphVisualizer;

public partial class ResultPage : ContentPage
{
	public ResultPage()
	{
		InitializeComponent();
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Начать вычисления
        await Task.Run(() =>
        {
            DataFromUser.SolvedGraph = DataFromUser.Solver.SolveGraph(DataFromUser.Graph);
        });
    }
    private void SwitchTheme(object sender, EventArgs e)
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

    private async void RestartApp(object sender, EventArgs e)
    {
        DataFromUser.Graph = null;
        DataFromUser.Solver = null;
        DataFromUser.SolvedGraph = null;
        await Navigation.PushAsync(new MainPage());
    }

    private void RefreshButtonClicked(object sender, EventArgs e)
    {
        if(DataFromUser.SolvedGraph != null)
        {
            Congratulations.Text = "Результат обработки";
            var graphImage = new GraphicsView() 
            {
                IsVisible = true,
                Drawable = new ResultGraphDrawer(),
                HeightRequest = 550,
                WidthRequest = 550,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            RefreshButton.IsVisible = false;
            TotalLayout.Add(graphImage);
            RestartButton.IsVisible = true;
            FilePathField.IsVisible = true;
            SubmitButton.IsVisible = true;
        }
        else
        {
            Congratulations.Text = "Все еще ожидание...";
        }
    }

    private async void SubmitResult(object sender, EventArgs e)
    {
        try
        {
            var path = FilePathField.Text + "MST.csv";

            using (var sw = new StreamWriter(path))
            {
                sw.WriteLine(GetCsvStringWithMatrix(DataFromUser.SolvedGraph));
            }

            FilePathField.IsVisible = false;
            SubmitButton.IsVisible = false;
            DoneLabel.IsVisible = true;
        }
        catch
        {
            await DisplayAlert("Неверный путь", "Выберите другой путь", "ОК");
        }
    }

    private string GetCsvStringWithMatrix(int[,] matrix)
    {
        var csv = string.Empty;

        for(int i = 0; i < matrix.GetLength(0); i++)
        {
            for(int j = 0;  j < matrix.GetLength(1); j++)
            {
                csv += matrix[i, j] + ",";
            }
            csv += "\n";
        }

        return csv;
    }
}