using System.Threading.Tasks;
using Prism.AppModel;
using Prism.Mvvm;
using Prism.Navigation;

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
    }
}