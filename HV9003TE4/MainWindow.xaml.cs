using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using SCEEC.MI.High_Precision;
using SCEEC.Numerics;
using System.IO;

namespace HV9003TE4
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public enum HeadOrTail { Head, Tail }
        public bool IsAble = false;
        public MainWindow()
        {
            InitializeComponent();
            (this.DataContext as MainWindowModel).StartRecCom();
            (this.DataContext as MainWindowModel).SetFre(50);
            // (this.DataContext as MainWindowModel).BoomFive();
            (this.DataContext as MainWindowModel).T1.IsBackground = true;
            (this.DataContext as MainWindowModel).T1.Start();
            (this.DataContext as MainWindowModel).OutTestResult += MainWindow_OutTestResult;
            // (this.DataContext as MainWindowModel).ReadMult();
        }
        private float[] GetTwoNum(int mainNum, int secNum, int ChennelNum)
        {
            // Testresult //mainNum*112 +secNum*14//
            try
            {
                float value1 = Testresult[mainNum * 112 + secNum * 14 + ChennelNum - 1];
                float value2 = Testresult[mainNum * 112 + secNum * 14 + ChennelNum];
                return new float[] { value1, value2 };
            }
            catch
            {
            }
            return null;
        }

        private void MainWindow_OutTestResult(byte[] data)
        {
            ViewSources vs = new ViewSources(data);
            this.Dispatcher.BeginInvoke((Action)delegate ()
            {
                if (Com1.SelectedIndex == 0)
                {
                    DL.Text = NumericsConverter.Value2Text(vs.TestIn, 6, -13, "", SCEEC.Numerics.Quantities.QuantityName.Current);
                    XW.Text = vs.TestPh1.ToString("F6");
                    float[] tpd = GetTwoNum(Com1.SelectedIndex, Com2.SelectedIndex, (int)vs.TestRn);
                    if (tpd != null)
                    {
                        if ((((int)vs.TestRn).ToString() != LC.Text) && CurrentTextBox.Text.Length < 1)
                        {
                            CurrentTextBox.Text = tpd[0].ToString();
                            Tant.Text = tpd[1].ToString();
                        }
                    }
                    LC.Text = vs.TestRn.ToString();

                }
                if (Com1.SelectedIndex == 3)
                {
                    DL.Text = NumericsConverter.Value2Text(vs.TestIx1, 6, -13, "", SCEEC.Numerics.Quantities.QuantityName.Current);
                    XW.Text = vs.TestPh2.ToString("F6");
                    float[] tpd = GetTwoNum(Com1.SelectedIndex, Com2.SelectedIndex, (int)vs.TestRx1);
                    if (tpd != null)
                    {
                        if ((((int)vs.TestRx1).ToString() != LC.Text) && CurrentTextBox.Text.Length < 1)
                        {
                            CurrentTextBox.Text = tpd[0].ToString();
                            Tant.Text = tpd[1].ToString();
                        }
                    }
                    LC.Text = vs.TestRx1.ToString();

                }
                if (Com1.SelectedIndex == 4)
                {
                    DL.Text = NumericsConverter.Value2Text(vs.TestIx2, 6, -13, "", SCEEC.Numerics.Quantities.QuantityName.Current);
                    XW.Text = vs.TestPh3.ToString("F6");
                    float[] tpd = GetTwoNum(Com1.SelectedIndex, Com2.SelectedIndex, (int)vs.TestRx2);
                    if (tpd != null)
                    {
                        if ((((int)vs.TestRx2).ToString() != LC.Text) && CurrentTextBox.Text.Length < 1)
                        {
                            CurrentTextBox.Text = tpd[0].ToString();
                            Tant.Text = tpd[1].ToString();
                        }

                    }
                    LC.Text = vs.TestRx2.ToString();

                }
                if (Com1.SelectedIndex == 5)
                {
                    DL.Text = NumericsConverter.Value2Text(vs.TestIx3, 6, -13, "", SCEEC.Numerics.Quantities.QuantityName.Current);
                    XW.Text = vs.TestPh4.ToString("F6");
                    float[] tpd = GetTwoNum(Com1.SelectedIndex, Com2.SelectedIndex, (int)vs.TestRx3);
                    if (tpd != null)
                    {
                        if ((((int)vs.TestRx3).ToString() != LC.Text) && CurrentTextBox.Text.Length < 1)
                        {
                            CurrentTextBox.Text = tpd[0].ToString();
                            Tant.Text = tpd[1].ToString();
                        }
                    }
                    LC.Text = vs.TestRx3.ToString();

                }
                if (Com1.SelectedIndex == 6)
                {
                    DL.Text = NumericsConverter.Value2Text(vs.TestIx4, 6, -13, "", SCEEC.Numerics.Quantities.QuantityName.Current);
                    XW.Text = vs.TestPh4.ToString("F6");
                    float[] tpd = GetTwoNum(Com1.SelectedIndex, Com2.SelectedIndex, (int)vs.TestRx4);
                    if (tpd != null)
                    {
                        if ((((int)vs.TestRx4).ToString() != LC.Text) && CurrentTextBox.Text.Length < 1)
                        {
                            CurrentTextBox.Text = tpd[0].ToString();
                            Tant.Text = tpd[1].ToString();
                        }
                    }
                    LC.Text = vs.TestRx4.ToString();

                }
                if (Com1.SelectedIndex == 1 || Com1.SelectedIndex == 2 || Com1.SelectedIndex == 7)
                {
                    LC.Text = "";
                    DL.Text = "";
                    XW.Text = "";
                    CurrentTextBox.Text = "";
                    Tant.Text = "";
                }
            });

        }

        public float[] Testresult;
        public List<Message> Result = new List<Message>();
        public struct Message
        {
            public int ChannelNum { get; set; }
            public string Value1 { get; set; }
            public string Value2 { get; set; }
        }

        private void DownVolate()
        {
            (this.DataContext as MainWindowModel).DownVolate();
        }

        private void Power(object sender, RoutedEventArgs e)
        {
            NeedVolate = 0;
            if (PowerState.IsChecked != false)
            {
                (this.DataContext as MainWindowModel).StartPower();
            }
            else
                (this.DataContext as MainWindowModel).ClosePower();

            #region enable
            if (PowerState.IsChecked == false)
            {
                b1.IsEnabled = false;
                b2.IsEnabled = false;
                b3.IsEnabled = false;
                b4.IsEnabled = false;
                b5.IsEnabled = false;
                b6.IsEnabled = false;
                b7.IsEnabled = false;
                b8.IsEnabled = false;
            }
            else
            {
                b1.IsEnabled = true;
                b2.IsEnabled = true;
                b3.IsEnabled = true;
                b4.IsEnabled = true;
                b5.IsEnabled = true;
                b6.IsEnabled = true;
                b7.IsEnabled = true;
                b8.IsEnabled = true;
            }
            #endregion
        }
        public float NeedFre { get; set; } = 50;
        private void AddFre(float addbum)
        {
            NeedFre += addbum;
            (this.DataContext as MainWindowModel).SetFre(NeedFre);
        }
        private void MulFre(float addbum)
        {

            //  var tempdata = NumericsConverter.Text2Value((this.DataContext as MainWindowModel).HVVoltage.ToString());
            // var tempdata = NumericsConverter.Text2Value("100kV");
            if (NeedFre >= addbum)
            {
                NeedFre -= addbum;
                (this.DataContext as MainWindowModel).SetFre(NeedFre);
            }
            else
            {
                NeedVolate = 0;
                (this.DataContext as MainWindowModel).SetFre(NeedFre);

            }

        }
        private void Mul_01__Fre_Click(object sender, RoutedEventArgs e)
        {
            MulFre(0.1f);
        }

        private void Mul_1_Fre_Click(object sender, RoutedEventArgs e)
        {
            MulFre(1f);


        }

        private void Mul_10_Fre_Click(object sender, RoutedEventArgs e)
        {
            MulFre(10f);
        }

        private void Mul_50_Fre_Click(object sender, RoutedEventArgs e)
        {
            MulFre(50f);
        }

        private void Add_01__Fre_Click(object sender, RoutedEventArgs e)
        {
            AddFre(0.1f);
        }

        private void Add_1_Fre_Click(object sender, RoutedEventArgs e)
        {
            AddFre(1f);
        }

        private void Add_10_Fre_Click(object sender, RoutedEventArgs e)
        {
            AddFre(10f);
        }

        private void Add_50_Fre_Click(object sender, RoutedEventArgs e)
        {
            AddFre(50f);
        }

        private void GetPra_Click(object sender, RoutedEventArgs e)
        {
            //  (this.DataContext as MainWindowModel).T1.Suspend();
            (this.DataContext as MainWindowModel).T1.Abort();
            Testresult = (this.DataContext as MainWindowModel).ReadMult();
            // (this.DataContext as MainWindowModel).T1.Resume();
            //  (this.DataContext as MainWindowModel).T1.Resume();
        }

        private void ConfirePra_Click(object sender, RoutedEventArgs e)
        {

        }



        //  public List<Message> ComContent { get; set; }
        private void GetOneNum(float[] res, int select, int needNum)
        {
            //  ComContent = new List<Message>();
            //  ComContent.Clear();
            try
            {
                if (Result != null)
                {
                    for (int i = 0; i < needNum / 2; i++)
                    {
                        Message temp = new Message();
                        temp.ChannelNum = i + 1;
                        temp.Value1 = res[select + i * 2].ToString();
                        temp.Value2 = res[select + i * 2 + 1].ToString();
                        Result.Add(temp);
                        ResultDataGrid.ItemsSource = null;
                        ResultDataGrid.ItemsSource = Result;
                        ResultDataGrid.AutoGenerateColumns = false;//禁止自动添加列
                        ResultDataGrid.CanUserAddRows = false;//禁止自动添加
                                                              // ComContent = Result;
                                                              //Exten_selectChanged(null, null);
                                                              //Channel_selectChanged(null, null);
                    }
                }
            }
            catch
            {

            }

        }

        private void Exten_selectChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Result.Clear();
                GetOneNum(Testresult, ExtensionCom.SelectedIndex * 112, 14);

            }
            catch
            {

                throw new Exception("未导入数据（TestResult）");
            }
        }

        private void Channel_selectChanged(object sender, SelectionChangedEventArgs e)
        {
            Result.Clear();
            GetOneNum(Testresult, ChannelCom.SelectedIndex * 112, 14);

        }

        private void SendLargeData_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as MainWindowModel).SendLargeData(Testresult);
        }

        private void Confire_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Tant.Text == "")
                {
                    Tant.Text = "0";
                }
                (this.DataContext as MainWindowModel).SendPra((byte)(Com1.SelectedIndex + 1), (byte)(Com2.SelectedIndex + 1),
              (byte)(Convert.ToInt32(LC.Text)), (float)Convert.ToDouble(CurrentTextBox.Text), (float)Convert.ToDouble(Tant.Text));

                // Thread.Sleep(1000);
                //GetPra_Click(null, null);
                //  Testresult = (this.DataContext as MainWindowModel).ReadMult();
            }
            catch
            {
                throw new Exception("输入参数有误");
            }


        }
        private void Open_click(object sender, RoutedEventArgs e)
        {
            string path = "C:\\Users\\My\\Desktop\\HV9003TE4调试数据\\HV9003TE4\\LocalFiles\\TextData.txt";
            if (File.Exists(path))
            {
                string tpddata = StaticSources.WriteDataToFile.ReadFile(path);
                byte[] bt = Encoding.ASCII.GetBytes(tpddata);
                float[] Needdata = new float[bt.Length / 4];//BitConverter.ToSingle(bt, 4);
                for (int i = 0; i < bt.Length/4; i++)
                {
                    Needdata[i]= BitConverter.ToSingle(bt, i*4);
                }

            }

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string path = "C:\\Users\\My\\Desktop\\HV9003TE4调试数据\\HV9003TE4\\LocalFiles\\TextData.txt";
            List<byte> ct = new List<byte>();
            if (Testresult != null)
            {
                if (!File.Exists(path))
                {
                    StaticSources.WriteDataToFile.FileBaseDeel(path, StaticSources.MyFileMode.Create);
                }
                else
                {
                    StaticSources.WriteDataToFile.FileBaseDeel(path, StaticSources.MyFileMode.Clear);
                }

                foreach (var a in Testresult)
                {
                    byte[] data = BitConverter.GetBytes(a);
                    ct.AddRange(data);
                }
                string datat = Encoding.ASCII.GetString(ct.ToArray());
                StaticSources.WriteDataToFile.WriteFile(path, datat);
            }
        }


        private void Con1_SelectChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                LC.Text = "";
                DL.Text = "";
                XW.Text = "";
                CurrentTextBox.Text = "";
                Tant.Text = "";
            }
            catch
            {

            }

        }

        private void Con2_SelectChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                LC.Text = "";
                DL.Text = "";
                XW.Text = "";
                CurrentTextBox.Text = "";
                Tant.Text = "";
            }
            catch
            {

            }

        }
        public float NeedVolate { get; set; } = 0;

        private void AddVolate(float addbum)
        {

            NeedVolate += addbum;
            (this.DataContext as MainWindowModel).SetBaseVolate(NeedVolate);
            Thread.Sleep(300);
            (this.DataContext as MainWindowModel).UpVolate();

        }
        private void MulVolate(float addbum)
        {

            //  var tempdata = NumericsConverter.Text2Value((this.DataContext as MainWindowModel).HVVoltage.ToString());
            // var tempdata = NumericsConverter.Text2Value("100kV");
            if (NeedVolate >= addbum)
            {
                NeedVolate -= addbum;
                (this.DataContext as MainWindowModel).SetBaseVolate(NeedVolate);
                Thread.Sleep(300);
                (this.DataContext as MainWindowModel).UpVolate();
            }
            else
            {
                NeedVolate = 0;
                (this.DataContext as MainWindowModel).SetBaseVolate(NeedVolate);
                Thread.Sleep(300);
                (this.DataContext as MainWindowModel).UpVolate();
            }

        }

        private void Add_10K__Val_Click(object sender, RoutedEventArgs e)
        {
            AddVolate(10000);
        }

        private void Add_1K__Val_Click(object sender, RoutedEventArgs e)
        {
            AddVolate(1000);
        }

        private void Add_100__Val_Click(object sender, RoutedEventArgs e)
        {
            AddVolate(300);
        }

        private void Add_10__Val_Click(object sender, RoutedEventArgs e)
        {
            //Task.Factory.
            AddVolate(100);

        }

        private void Mul_10K_Vol_CLick(object sender, RoutedEventArgs e)
        {
            MulVolate(10000);
        }

        private void Mul_1K_Vol_CLick(object sender, RoutedEventArgs e)
        {
            MulVolate(1000);
        }

        private void Mul_100_Vol_CLick(object sender, RoutedEventArgs e)
        {
            MulVolate(300);
        }

        private void Mul_10_Vol_CLick(object sender, RoutedEventArgs e)
        {
            MulVolate(100);
        }

        private void Alarm_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Down_Click(object sender, RoutedEventArgs e)
        {
            NeedVolate = 0;
            DownVolate();
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DownVolate();
        }


    }
}