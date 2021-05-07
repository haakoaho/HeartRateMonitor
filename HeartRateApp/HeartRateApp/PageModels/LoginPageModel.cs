using FreshMvvm;
using HeartRateApp.Backend;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Windows.Input;
using System.Threading.Tasks;

namespace HeartRateApp.PageModels
{
    class LoginPageModel : FreshBasePageModel
    {
        public static long UserId { get; private set; }
        public string Username { get; set; }

        public Command LogIn
        {
            get
            {
                return new Command(async () =>
                {
                    UserId = await APIModule.GetUserIdAsync(Username);
                    await CoreMethods.PushPageModel<MainPageModel>();
                });
            }
        }


        public Command Register
        {
            get
            {
                return new Command(async () =>
                {
                    await APIModule.CreateUserAsync(Username);
                    await Task.Delay(2000); //waiting for server to create user
                    LogIn.Execute(true);
                });
            }

        }
    }
}
