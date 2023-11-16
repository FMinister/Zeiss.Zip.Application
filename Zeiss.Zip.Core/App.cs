using MvvmCross.ViewModels;
using Zeiss.Zip.Core.ViewModels;

namespace Zeiss.Zip.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            RegisterAppStart<ZipFileExplorerViewModel>();
        }
    }
}
