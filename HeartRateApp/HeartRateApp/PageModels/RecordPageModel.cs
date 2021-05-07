using System;
using System.Collections.Generic;
using System.Text;
using FreshMvvm;
using HeartRateApp.Models;
using System.Windows.Input;
using Xamarin.Forms;
using HeartRateApp.Backend;
using Acr.UserDialogs;

namespace HeartRateApp.PageModels
{
    class RecordPageModel : FreshBasePageModel
    {
        public string DisplayText { get; set; }
        public bool SystolicWarning { get; set; } = false;
        public bool ArterisWarning { get; set; } = false;
        private HeartRateModel heartRateModel;
        public override void Init(object initData)
        {
            base.Init(initData);
            heartRateModel = (HeartRateModel)initData;
            DisplayText = "Systolic Pressure: " + heartRateModel.SystolicPressure + "\n" + "Arteris Pressure: " + heartRateModel.ArterisPressure;
            SystolicWarning = heartRateModel.SystolicPressure >= 150;
            ArterisWarning = heartRateModel.ArterisPressure >= 70;
        }

        private Command deleteRecord;

        public Command DeleteRecord
        {
            get
            {
                if (deleteRecord == null)
                {
                    deleteRecord = new Command(PerformDeleteRecord);
                }

                return deleteRecord;
            }
        }

        private void PerformDeleteRecord()
        {
            _=APIModule.DeleteRecord(heartRateModel.RecordId);
            CoreMethods.PopPageModel();
            UserDialogs.Instance.Toast("Record will be deleted on next refresh");
        }
    }
}
