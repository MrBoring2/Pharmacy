using Microsoft.Toolkit.Mvvm.Input;
using PharmacyApp.Helpers;
using PharmacyApp.Services.Common;
using PharmacyApp.ViewModels.ForPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PharmacyApp.ViewModels.ForWindows
{
    public class MainAppWindowVM : BaseWindowVM
    {
        private BasePageVM _currentPageVM;
        private List<BasePageVM> _pageVMs;
        private PagesRegistrator PagesRegistrator { get; set; }

        public MainAppWindowVM(Roles role)
        {
            PagesRegistrator = new PagesRegistrator();
            PagesRegistrator.RegisterPagesForRole(role);
            PageVMs.AddRange(PagesRegistrator.RolePages);
            
            CurentPageVM = PageVMs[0];
        }
        private Services.Common.RelayCommand _changePageCommand;

        public Services.Common.RelayCommand ChangePageCommand
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

        private void ChangePageVM(BasePageVM pageVM)
        {
            if (!PageVMs.Contains(pageVM))
                PageVMs.Add(pageVM);

            CurentPageVM = PageVMs
                .FirstOrDefault(vm => vm == pageVM);
        }
    }
}
