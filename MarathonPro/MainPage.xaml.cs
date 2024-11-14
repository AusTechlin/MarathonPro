using Newtonsoft.Json;
using MarathonPro.Models;

namespace MarathonPro;

public partial class MainPage : ContentPage
{
    public RaceCollection raceObject;
    
    public MainPage()
    {
        InitializeComponent();
        FillPicker();
    }

    public void FillPicker()
    {
        // set new http client
        var client = new HttpClient();
        // set base address for client
        client.BaseAddress = new Uri("https://joewetzel.com/fvtc/marathon/");
        // collect the results of races
        var response = client.GetAsync("races").Result;
        // Collect the json from the response uri
        var wsJson = response.Content.ReadAsStringAsync().Result;
        
        // deserialize the objects in the json form wsJson
        raceObject = JsonConvert.DeserializeObject<RaceCollection>(wsJson);

        // bind races to the picker items
        RacePicker.ItemsSource = raceObject.races;
    }

    private void RacePicker_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedRace = ((Picker)sender).SelectedIndex;
        var RaceId = raceObject.races[selectedRace].id;
        
        // set new http client
        var client = new HttpClient();
        // set base address for client
        client.BaseAddress = new Uri("https://joewetzel.com/fvtc/marathon/");
        // collect the results of results
        var response = client.GetAsync("results/" + RaceId).Result;
        // Collect the json from the response uri
        var wsJson = response.Content.ReadAsStringAsync().Result;

        var ResultsObject = JsonConvert.DeserializeObject<ResultCollection>(wsJson);

        var CellTemplate = new DataTemplate(typeof(TextCell));
        CellTemplate.SetBinding(TextCell.TextProperty, "name");
        CellTemplate.SetBinding(TextCell.DetailProperty, "detail");
        ResultListView.ItemTemplate = CellTemplate;

        ResultListView.ItemsSource = ResultsObject.results;
    }
}