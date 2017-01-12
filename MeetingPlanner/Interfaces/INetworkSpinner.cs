using System;
namespace WODTasticMobile
{
    public interface INetworkSpinner
    {
        void Spinner(bool on, string title, string message);

        void ChangeMessage(string newMessage);
    }
}

