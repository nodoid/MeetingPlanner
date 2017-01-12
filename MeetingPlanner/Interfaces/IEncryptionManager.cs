using System.Collections.Generic;

namespace DeskBooking
{
    interface IEncryptionManager
    {
        IEnumerable<ActiveDirectoryUser> DecryptUsers(IEnumerable<Encryption> encryptions);

        Encryption EncryptUsers(IEnumerable<ActiveDirectoryUser> users);

        bool Compare(byte[] array1, byte[] array2);

        bool IsEncryptionValid(Encryption encryption);
    }
}
