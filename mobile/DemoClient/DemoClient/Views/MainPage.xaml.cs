using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DemoClient.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DemoClient
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();


        }

        public void listView_Refreshing(object sender,EventArgs eventArgs)
        {
            LoadUsers();
            listView.IsRefreshing = false;
        }
        private void LoadUsers()
        {
            var service = new DbService();

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                var users = await service.getUsers();
                listView.ItemsSource = users;
            });
        }
    }
}

