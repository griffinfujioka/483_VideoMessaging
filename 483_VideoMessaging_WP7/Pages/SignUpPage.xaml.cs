using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Xml;
using System.Xml.Serialization;
using _483_VideoMessaging_WP7.Models;
using System.IO;
using System.Text;

namespace _483_VideoMessaging_WP7.Pages
{
    public partial class SignUpPage : PhoneApplicationPage
    {
        public SignUpPage()
        {
            InitializeComponent();
        }

        private void submitBtn_Click(object sender, RoutedEventArgs e)
        {
            var email = emailTxtBox.Text;
            var username = usernameTxtBox.Text;
            var password = passwordTxtBox.Password;
            var confirm_password = confirmPasswordTxtBox.Password;

            var newProfileRequest = new NewProfileRequest(email, username, password, confirm_password); 
            
            // Do some XML serialization 
            XmlSerializer serializer = new XmlSerializer(typeof(NewProfileRequest));
            StringBuilder outputString = new StringBuilder();
            XmlWriter xmlWriter = XmlWriter.Create(outputString);
            serializer.Serialize(xmlWriter, newProfileRequest); 


            MessageBox.Show("Welcome to the 483 Video Messaging app, " + username + "!");

            var backstack = NavigationService.BackStack;

            NavigationService.RemoveBackEntry();
            //NavigationService.RemoveBackEntry();

            backstack = NavigationService.BackStack;

            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));

            NavigationService.RemoveBackEntry(); 
            backstack = NavigationService.BackStack;

            

        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack(); 
        }
    }
}