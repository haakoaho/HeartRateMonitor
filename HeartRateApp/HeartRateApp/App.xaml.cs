using FreshMvvm;
using HeartRateApp.PageModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeartRateApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var page = FreshPageModelResolver.ResolvePageModel<LoginPageModel>();
            var basicNavContainer = new FreshNavigationContainer(page);
            MainPage = basicNavContainer;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
