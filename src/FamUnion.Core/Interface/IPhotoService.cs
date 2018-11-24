using System.IO;

namespace FamUnion.Core.Interface
{
    public interface IPhotoService
    {
        string SavePhoto(File photo);
        byte[] GetPhoto(string photoLocation);
    }
}
