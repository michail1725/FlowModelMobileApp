using System.IO;
using System.Threading.Tasks;

public interface ISave
{
    Task Save(string filename, string contentType, MemoryStream stream);
}
