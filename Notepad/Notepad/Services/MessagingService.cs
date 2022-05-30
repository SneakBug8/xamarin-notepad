using System;
using Xamarin.Forms;

namespace Notepad.Services
{
    public class MessagingService
    {
        private static readonly object sender = new object();

        public void Send<T>(string message, T parameter)
        {
            MessagingCenter.Send(sender, message, parameter);
        }

        public void Subscribe<T>(object subscriber, string message, Action<T> callback)
        {
            MessagingCenter.Subscribe<object, T>(subscriber, message, (sender, args) => callback(args));
        }

        public void Unsubscribe<T>(object subscriber, string message)
        {
            MessagingCenter.Unsubscribe<object, T>(subscriber, message);
        }
    }
}