
using PharmacyApp.Helpers;
using PharmacyApp.Services.ApiServices;
using PharmacyApp.Services.Common;
using PharmacyApp.ViewModels.ForPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace PharmacyApp.ViewModels.ForWindows
{
    public class MainAppWindowVM : BaseWindowVM
    {
        protected RestService restService;
        protected BasePageVM _currentPageVM;
        protected List<BasePageVM> _pageVMs;
        protected PagesRegistrator PagesRegistrator { get; set; }

        public MainAppWindowVM()
        {
            restService = new RestService();
        }

        protected RelayCommand exit;
        public RelayCommand Exit
        {
            get 
            {
                return exit ??
                    (exit = new RelayCommand(obj =>
                    {
                        restService.Exit();
                        WindowNavigation.Instance.OpenAndHideWindow(this, new LoginWindowVM());
                    }));
            }
        }

        protected RelayCommand _changePageCommand;

        public RelayCommand ChangePageCommand
        {
            get
            {
                return _changePageCommand ??
                    ( _changePageCommand = new Services.Common.RelayCommand(obj =>
                {
                    ChangePageVM((BasePageVM)obj);
                }));
            }
        }

  

        public List<BasePageVM> PageVMs
        {
            get
            {
                if(_pageVMs == null)
                    _pageVMs = new List<BasePageVM>();
                return _pageVMs;
            }
        }

        public BasePageVM CurentPageVM
        {
            get => _currentPageVM;
            set
            {
                if (_currentPageVM != value)
                {
                    _currentPageVM = value;
                    OnPropertyChanged("CurentPageVM");
                }
            }
        }

        protected void ChangePageVM(BasePageVM pageVM)
        {
            if (!PageVMs.Contains(pageVM))
                PageVMs.Add(pageVM);

            CurentPageVM = PageVMs
                .FirstOrDefault(vm => vm == pageVM);
        }
    }
}
