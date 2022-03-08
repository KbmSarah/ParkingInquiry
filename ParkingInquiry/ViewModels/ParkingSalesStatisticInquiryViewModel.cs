using System;
using System.Windows.Input;
using DevExpress.Mvvm;
using Newtonsoft.Json.Linq;
using ParkingInquiry.Common;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using ParkingInquiry.Enums;
using DevExpress.Xpf.Charts;

namespace ParkingInquiry.ViewModels
{
    public class ParkingSalesStatisticInquiryViewModel : ViewModelBase, IDisposable
    {

        #region Members
        private ObservableCollection<DataSeries> _data;
        private ICommand _searchButtonClicked;
        private ICommand _backButtonClicked;
        private bool _isVisibleBackButton = false;
        private DataPoint _selectedMonth;
        private string _chartTitle;
        private bool _isDisposed = false;
        #endregion

        #region Properties
        public ObservableCollection<DataSeries> Data
        {
            get { return _data; }
            set { SetValue(ref _data, value); }
        }
        public ICommand SearchButtonClicked
        {
            get
            {
                if (_searchButtonClicked == null)
                    _searchButtonClicked = new DelegateCommand(SearchButtonClickedCallBack);

                return _searchButtonClicked;
            }
        }

        private void SearchButtonClickedCallBack()
        {
            IsVisibleBackButton = false;
            InitiateChart();
            InquiryParkingSales();
        }

        public ICommand BackButtonClicked
        {
            get
            {
                if (_backButtonClicked == null)
                    _backButtonClicked = new DelegateCommand<XYDiagram2D>(BackButtonClickedCallBack);

                return _backButtonClicked;
            }
        }
        public bool IsVisibleBackButton
        {
            get { return _isVisibleBackButton; }
            set { SetValue(ref _isVisibleBackButton, value); }
        }
        public DataPoint SelectedMonth
        {
            get { return _selectedMonth; }
            set
            {
                SetValue(ref _selectedMonth, value);
                
                IsVisibleBackButton = true;

                if(value.SalesType == SalesType.month)
                {
                    InitiateChart();
                    InquiryParkingSales(SalesType.day);
                }
            }
        }
        public string ChartTitle
        {
            get { return _chartTitle; }
            set { SetValue(ref _chartTitle,value); }
        }
        #endregion

        #region Constructors
        public ParkingSalesStatisticInquiryViewModel()
        {
            string address = "172.16.32.198";
            int port = 5000;
            ChartTitle = "월별 매출통계";

            ParkingCommands.SessionStart(address, port);

            InitiateChart();
        }
        #endregion

        #region Methods
        private void BackButtonClickedCallBack(XYDiagram2D graph)
        {
            IsVisibleBackButton = false;
            InitiateChart();
            InquiryParkingSales();
        }

        /// <summary>
        /// Initiate Chart Control
        /// </summary>
        private void InitiateChart()
        {
            if (Data != null)
            {
                foreach (var item in Data)
                    item.Values.Clear();

                Data.Clear();
            }

            Data = new ObservableCollection<DataSeries>
            {
                new DataSeries()
                {
                    Name = ParkingSalesType.Normal,
                    Values = new ObservableCollection<DataPoint> { }
                },

                new DataSeries()
                {
                    Name = ParkingSalesType.Season,
                    Values = new ObservableCollection<DataPoint> { }
                }
            };
        }

        /// <summary>
        /// Rest API Request
        /// </summary>
        /// <param name="salesType">month or day</param>
        private void InquiryParkingSales(SalesType salesType = SalesType.month)
        {
            ChartTitle = "월별 매출통계";

            ParkingRequest parameter = new ParkingRequest()
            {
                dt_from = "20211201000000",
                dt_to = "20220221235959",
                stt_type = salesType.ToString(),
                page = 1,
                row_count = 12,
                park_id = 0
            };

            ParkingReceive result = ParkingCommands.RequestSales(parameter);
            if (result == null || result.result_msg != "success")
            {
                Console.WriteLine("API request 실패");
                return;
            }

            foreach (var item in result.sale_list)
            {
                double sumNormal = item.normal.cash + item.normal.credit + item.normal.trns + item.normal.etc;
                double sumCommuter = item.commuter.cash + item.commuter.credit + item.commuter.etc;

                string tempYear = item.date.Substring(0, 4);
                string tempMonth = item.date.Substring(4, 2);
                string tempDay = "1";

                if (salesType == SalesType.day)
                {
                    ChartTitle = "일별 매출통계";
                    tempDay = item.date.Substring(6, 2);
                }

                DateTime tempTime = new DateTime(int.Parse(tempYear), int.Parse(tempMonth), int.Parse(tempDay));

                var normalGraph = Data.FirstOrDefault(i => i.Name == ParkingSalesType.Normal);
                var seasonGraph = Data.FirstOrDefault(i => i.Name == ParkingSalesType.Season);

                normalGraph.Values.Add(new DataPoint(tempTime, sumNormal, salesType));
                seasonGraph.Values.Add(new DataPoint(tempTime, sumCommuter, salesType));
            }
        }

        public void Dispose()
        {
            if(!_isDisposed)
            {
                _isDisposed = true;

                foreach (var item in Data)
                {
                    item.Dispose();
                }

                Data?.Clear();
                Data = null;

                SelectedMonth?.Dispose();
                SelectedMonth = null;
            }
        }
        #endregion

        public class DataSeries : ViewModelBase, IDisposable
        {
            #region Members
            private bool _isDisposed = false;
            private ObservableCollection<DataPoint> _values;
            #endregion

            #region Properties
            public ParkingSalesType Name { get; set; }
            public ObservableCollection<DataPoint> Values
            {
                get { return _values; }
                set { SetValue(ref _values, value); }
            }
            #endregion

            #region  Methods
            public void Dispose()
            {

                if (!_isDisposed)
                {
                    _isDisposed = true;

                    foreach (var item in Values)
                    {
                        item.Dispose();
                    }

                    Values?.Clear();
                    Values = null;
                }
            }
            #endregion

        }
        public class DataPoint : ViewModelBase,IDisposable
        {
            #region Members
            private DateTime _argumnet;
            private string _subArgument;
            private double _value;
            private bool _isDisposed = false;
            #endregion

            #region Properties
            public DateTime Argument
            {
                get { return _argumnet; }
                set { SetValue(ref _argumnet, value); }
            }
            public string SubArgument
            {
                get { return _subArgument; }
                set { SetValue(ref _subArgument, value); }
            }
            public double Value
            {
                get { return _value; }
                set { SetValue(ref _value, value); }
            }

            private SalesType _salesType;

            public SalesType SalesType
            {
                get { return _salesType; }
                set { _salesType = value; }
            }


            #endregion

            #region Constructors
            public DataPoint(DateTime argument, double value, SalesType salesType)
            {
                Argument = argument;
                Value = value;
                SalesType = salesType;

                switch (SalesType)
                {
                    case SalesType.month:
                        SubArgument = argument.Month.ToString() + "월";
                        break;
                    case SalesType.day:
                        SubArgument = argument.Day.ToString() + "일";
                        break;
                    default:
                        break;
                }
            }
            #endregion

            #region Methods
            public void Dispose()
            {
                if (!_isDisposed)
                {
                    _isDisposed = true;
                }
            }
            #endregion
        }
    }
}