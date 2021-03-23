using Prism.AppModel;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.ViewModels
{
    public class BaseViewModel : BindableBase, INavigationAware, IDestructible, IInitialize, IApplicationLifecycleAware
    {
        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        public void Destroy()
        {
        }

        public virtual void OnResume()
        {
        }

        public virtual void OnSleep()
        {
        }

        public virtual void Initialize(INavigationParameters parameters)
        {
        }

        protected string ImageSourceToBase64(ImageSource source)
        {
            StreamImageSource streamImageSource = (StreamImageSource)source;
            System.Threading.CancellationToken cancellationToken = System.Threading.CancellationToken.None;
            Task<Stream> task = streamImageSource.Stream(cancellationToken);
            Stream stream = task.Result;

            byte[] b;
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                b = ms.ToArray();
            }

            return Convert.ToBase64String(b);
        }
    }
}