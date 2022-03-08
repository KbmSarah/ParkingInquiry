using System;
using DevExpress.Mvvm;

namespace ParkingInquiry.ViewModels
{
    public class MainViewModel : ViewModelBase, IDisposable
    {
        private bool _isDisposed =false;
        public MainViewModel()
        {
            SalesInquiryViewModel = new ParkingSalesStatisticInquiryViewModel();
        }

        private ParkingSalesStatisticInquiryViewModel _salesInquiryViewModel;

        public ParkingSalesStatisticInquiryViewModel SalesInquiryViewModel
        {
            get { return _salesInquiryViewModel; }
            set { SetValue( ref _salesInquiryViewModel, value); }
        }

        public void Dispose()
        {
            if(!_isDisposed)
            {
                _isDisposed = true;
                SalesInquiryViewModel?.Dispose();
                SalesInquiryViewModel = null;
            }
        }

        ~MainViewModel()
        {
            Dispose();
        }
    }
}