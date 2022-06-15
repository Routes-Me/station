using System;
namespace RoutesStation.Interface.Upload
{
	public interface IUploudRep
	{
		Task<string> AddImageProfile(IFormFile image, string Folder, string UserName);
		Task<bool> DeleteFileProfile(string Folder, string UserName);
	}
}

