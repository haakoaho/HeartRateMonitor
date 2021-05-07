using FreshMvvm;
using HeartRateApp.Backend;
using HeartRateApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Windows.Input;
using Acr.UserDialogs;

namespace HeartRateApp.PageModels
{
    class MainPageModel : FreshBasePageModel
    {
        public new event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ObservableCollection<HeartRateModel> Records { get; set; } = new ObservableCollection<HeartRateModel>();
        private HeartRateModel _selectedRecord;
        public HeartRateModel SelectedRecord
        {
            get
            {
                return _selectedRecord;
            }
            set
            {
                _selectedRecord = value;
                RecordSelected.Execute(value);
            }
        }
        public override void Init(object initData)
        {
            base.Init(initData);
            Task.Run(FetchData);
        }

        private async Task FetchData()
        {
            Records.Clear();
            List<HeartRateModel> recordList = await APIModule.GetRecords(LoginPageModel.UserId);
            foreach (HeartRateModel model in recordList)
            {
                Records.Add(model);
            }
        }

        public Command<HeartRateModel> RecordSelected
        {

            get
            {
                return new Command<HeartRateModel>(async (record) =>
                    {

                        await CoreMethods.PushPageModel<RecordPageModel>(record);
                    });

            }
        }


        public Command AddRecord
        {
            get
            {
                return new Command<HeartRateModel>(async (record) =>
                {

                    UserDialogs.Instance.ShowLoading("Measuring blood pressure");
                    CreateHeartRateRequestModel createHeartRateRequestModel = new CreateHeartRateRequestModel();
                    createHeartRateRequestModel.UserId = LoginPageModel.UserId;
                    Random random = new Random();
                    createHeartRateRequestModel.SystolicPressure = random.Next(120, 160);
                    createHeartRateRequestModel.ArterisPressure = random.Next(40, 80);
                    _ = APIModule.CreateRecord(createHeartRateRequestModel);
                    _ = Task.Run(FetchData);
                    await Task.Delay(2000); //the actual operation will take some extra time
                    UserDialogs.Instance.HideLoading();
                    UserDialogs.Instance.Toast("Record saved on server");
                });
            }
        }
    }
}
