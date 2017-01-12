using System;
namespace WODTasticMobile
{
    public class UIChangedEventArgs : EventArgs
    {
        public UIChangedEventArgs(string val = "", string mess = "")
        {
            ModuleName = val;
            Message = mess;
        }

        public readonly string ModuleName;
        public readonly string Message;
    }

    public class UIChangedEvent
    {
        public event UIChangeHandler Change;

        public delegate void UIChangeHandler(object s, UIChangedEventArgs ea);

        protected void OnChange(object s, UIChangedEventArgs e)
        {
            if (Change != null)
                Change(s, e);
        }

        public void BroadcastIt(string message, string message2)
        {
            if (!string.IsNullOrEmpty(message))
            {
                var info = new UIChangedEventArgs(message, message2);
                OnChange(this, info);
            }
        }

        public void BroadcastIt(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                var info = new UIChangedEventArgs(message);
                OnChange(this, info);
            }
        }
    }
}

