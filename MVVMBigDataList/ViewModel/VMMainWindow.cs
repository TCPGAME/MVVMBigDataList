using MVVMBigDataList.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MVVMBigDataList.ViewModel
{
    public class VMMainWindow:VMBase
    {
        private ObservableCollection<Info> workDatas = new ObservableCollection<Info>();
        public ObservableCollection<Info> WorkDatas 
        { 
            get => workDatas;
            set 
            {
                workDatas = value;
                RaisePropertyChanged("WorkDatas");
            }
        }

        public ICommand CmdAddMillionData { get; set; }
        public ICommand CmdAddOneData { get; set; }
        public ICommand CmdThreadAddData { get; set; }

        private bool ThreadRun = false;
        public VMMainWindow() 
        {
            CmdAddMillionData = new RelayCommand<ItemsControl>(AddMillionData);
            CmdAddOneData = new RelayCommand<ItemsControl>(AddOneData);
            CmdThreadAddData = new RelayCommand<ItemsControl>(ThreadAddData);
        }

        private void AddMillionData(ItemsControl dg)
        {
            ObservableCollection<Info> temp = new ObservableCollection<Info>(WorkDatas);
            int currentSize = temp.Count();
            for (int i = currentSize; i < currentSize + 1000000;i++) 
            {
                var info = new Info();
                info.Number = i;
                info.Time = DateTime.Now;
                info.Message = "Message" + i;
                temp.Add(info);
            }
            WorkDatas = temp;
            ScrollViewer s = dg.Template.FindName("ScrollViewer", dg) as ScrollViewer;
            s.ScrollToEnd();
        }

        private void AddOneData(ItemsControl dg)
        {
            int currentSize = WorkDatas.Count();
            var info = new Info();
            info.Number = currentSize;
            info.Time = DateTime.Now;
            info.Message = "Message" + currentSize;
            WorkDatas.Add(info);
            ScrollViewer s= dg.Template.FindName("ScrollViewer" ,dg) as ScrollViewer;
            s.ScrollToEnd();
        }

        private async void ThreadAddData(ItemsControl dg)
        {
            if (ThreadRun) 
            {
                ThreadRun = false;
                return;
            }
            ThreadRun = true;
            while (ThreadRun) 
            {
                AddOneData(dg);
                await Task.Delay(1);
            }
        }
    }
}
