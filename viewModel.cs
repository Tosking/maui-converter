using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.InteropServices.JavaScript;
using Newtonsoft.Json;
using PCLStorage;
using Android.Util;

namespace Converter;

public class viewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    private Model _model = new Model();
    
    private ObservableCollection<Currency> currencyList = new ObservableCollection<Currency>();
    public ObservableCollection<Currency> CurrencyList
    {
        get => currencyList;
        set
        {
            if (currencyList != value)
            {
                currencyList = value;
                OnPropertyChanged(nameof(CurrencyList));
            }
        }
    }
    private void CopyData(string first,string last)
    {
        if (!string.IsNullOrWhiteSpace(first))
            MainCurrency = CurrencyList.FirstOrDefault(value => value.CharCode == first);
        if (!string.IsNullOrWhiteSpace(last))
            SecondCurrency = CurrencyList.FirstOrDefault(value => value.CharCode == last);
    }

    /*private async Task SaveData(int data)
    {
        IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
        Log.Error("Save", "Done");
        if (data == 0)
        {
            IFile file = await rootFolder.CreateFileAsync("maincur.txt",
                CreationCollisionOption.OpenIfExists);
            await file.WriteAllTextAsync($"{MainCurrency.CharCode} {EntryMain}");
        }
        else
        {
            IFile file = await rootFolder.CreateFileAsync("secondcur.txt",
                CreationCollisionOption.OpenIfExists);
            await file.WriteAllTextAsync($"{MainCurrency.CharCode} {Result}");
        }
    }

    private async Task<string[]> OpenData(int data)
    {
        IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
        Log.Error("Open", "Done");
        if (data == 0)
        {
            IFile file = await rootFolder.CreateFileAsync("maincur.txt",
                CreationCollisionOption.OpenIfExists);
            string str = await file.ReadAllTextAsync();
            return str.Split(' ');
        }
        else
        {
            IFile file = await rootFolder.CreateFileAsync("secondcur.txt",
                CreationCollisionOption.OpenIfExists);
            string str = await file.ReadAllTextAsync();
            return str.Split(' ');
        }
    }*/
    
    
    
    private Dictionary<DateTime, ObservableCollection<Currency>> _buffer = 
        new Dictionary<DateTime, ObservableCollection<Currency>>();

    private DateTime currentDate = DateTime.Now;

    public DateTime CurrentDate
    {
        get { return currentDate; }
        set
        {
            currentDate = value;
            OnPropertyChanged(nameof(CurrentDate));
            GetList();
        }
    }

    public viewModel()
    {
        GetList();
        /*if(OpenData(0).ToString()?.Split(' ').Length > 1)
            EntryMain = OpenData(0).Result[1];
        if(OpenData(0).ToString()?.Split(' ').Length > 1)
            Result = OpenData(1).Result[1];*/
    }
    
    private string _entryMain;
    public string EntryMain { get=>_entryMain; 
        set 
        { 
            if(_entryMain !=value)
            {
                _entryMain = value;
                Convert();
                //SaveData(0);
                OnPropertyChanged(nameof(EntryMain));
                    
            }
        } 
    }
    private string _result;
    public string Result
    {
        get => _result;
        set
        {
            if (_result != value)
            {
                _result = value;
                Convert();
                //SaveData(1);
                OnPropertyChanged(nameof(Result));
            }
        }
    }

    private string _errorText;
    public string ErrorText
    {
        get => _errorText;
        set 
        {
            if(_errorText !=value)
            {
                _errorText = value;
                OnPropertyChanged(nameof(ErrorText));
            }
        }
            
    }
    

    public async void GetList()
    {
        Data json = null;
        if (_buffer.Count > 0)
        {
            if (_buffer.ContainsKey(CurrentDate))
            {
                CurrencyList.Clear();
                CurrencyList = _buffer[currentDate];
                CopyData(MainCurrency?.CharCode, SecondCurrency?.CharCode);
                return;
            }
        }
        try
        {
            json = await _model.GetAsync(currentDate);
        }
        catch {}
        if (json?.Valute == null)
        {
            ErrorText = "No data for this date";
            return;
        };
        ErrorText = $"{json.Date:yyyy/MM/dd}";
        CurrencyList.Clear();
        foreach (var i in json.Valute.Values)
            currencyList.Add(i);
        if (_buffer.ContainsKey(currentDate))
            _buffer[currentDate] = new ObservableCollection<Currency>(CurrencyList.Select(value => value));
        else
            _buffer.Add(currentDate, new ObservableCollection<Currency>(CurrencyList.Select(value => value)));

        //CopyData(OpenData(0).Result[0], OpenData(1).Result[0]);

    }

    private void Convert()
    {
        int result;
        if (MainCurrency == null || SecondCurrency == null)
            return;
        if (!int.TryParse(_entryMain, out result))
            return;
        
        Result = $"{(MainCurrency.Value / MainCurrency.Nominal) * result / (SecondCurrency.Value / SecondCurrency.Nominal)}";
    }
    

    private Currency maincurrency;
    private Currency secondcurrency;
    public Currency MainCurrency
    {
        get => maincurrency;
        set
        {
            if (maincurrency != value)
            {
                maincurrency = value;
                Convert();
                //SaveData(0);
                OnPropertyChanged(nameof(MainCurrency));
            }
        }
    }
    public Currency SecondCurrency
    {
        get => secondcurrency;
        set
        {
            if (secondcurrency != value)
            {
                secondcurrency = value;
                Convert();
                //SaveData(1);
                OnPropertyChanged(nameof(SecondCurrency));
            }
        }
    }

    public void OnPropertyChanged(string prop)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

}