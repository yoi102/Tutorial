using System.ComponentModel;

namespace DataBinding
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private int myVar;

        public MainWindowViewModel()
        {
            //この処理を気にしないでください、
            //ロジックの意味：BackgroundThreadでMyPropertyの値を0.5ｓ毎1を増やします。
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(500);
                    MyProperty++;
                }
            });
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public int MyProperty
        {
            get { return myVar; }
            set
            {
                myVar = value;
                //senderをnullにしたら、UIの数字が更新しない、なぜ？　　
                //=>　原因：WPFがsenderから名前がMyPropertyというプロパティの値を取得して、BindingのControlに渡すんのです。senderがnullと、プロパティの値を取得出ません
                //PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(MyProperty)));

                //正常系：
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MyProperty)));

                //senderはあっているのに、UIの数字が更新しない、なぜ？　　
                //senderは名前がMMPPというプロパティがないから。
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MMPP"));
            }
        }

        public string TestProperty => "Test:";
    }




    //自分で考えてみて欲しいもの：
    //0.5ｓ毎でMyPropertyを増やしていますが、
    //なぜ、Windowの数字の変化が時に早いたり、遅いたりになってるでしょうか？
    //0.5ｓ毎で更新していないように見えるでしょうか？
    //Hint：Dispatcher
}