using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SprayingSystem.Utility
{
    /// <summary>
    /// Interaction logic for IntUpDownCounter.xaml
    /// </summary>
    public partial class IntUpDownCounter : UserControl
    {
        private int _startValue = 10;

        public int MinValue { get; set; }

        public int MaxValue { get; set; }

        public int StartValue
        {
            get { return _startValue;}
            set
            {
                _startValue = value;
                NUDTextBox = value.ToString();
            }
        }

        public IntUpDownCounter()
        {
            InitializeComponent();

            //DataContext = this;
            NUDTextBox = StartValue.ToString();
        }

        #region NUDTextBox

        public static DependencyProperty NUDTextBoxProperty;

        public string NUDTextBox
        {
            get { return (string)GetValue(NUDTextBoxProperty); }
            set { SetValue(NUDTextBoxProperty, value);}
        }

        #endregion

        #region Private Stuff

        private void NUDButtonUP_Click(object sender, RoutedEventArgs e)
        {
            int number;
            if (NUDTextBox != "") 
                number = Convert.ToInt32(NUDTextBox);
            else 
                number = 0;
            if (number < MaxValue)
                NUDTextBox = Convert.ToString(number + 1);
            else
                NUDTextBox = Convert.ToString(MinValue);
        }

        private void NUDButtonDown_Click(object sender, RoutedEventArgs e)
        {
            int number;
            if (NUDTextBox != "") 
                number = Convert.ToInt32(NUDTextBox);
            else 
                number = Int32.MinValue+1;
            if (number > MinValue)
                NUDTextBox = Convert.ToString(number - 1);
            else
                NUDTextBox = Convert.ToString(MaxValue);
        }

        /// <summary>
        /// TODO: Learn this code.
        /// </summary>
        private void NUDTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                NUDButtonUP.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonUP, new object[] { true });
            }

            if (e.Key == Key.Down)
            {
                NUDButtonDown.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonDown, new object[] { true });
            }
        }

        /// <summary>
        /// TODO: Learn this code.
        /// </summary>
        private void NUDTextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonUP, new object[] { false });

            if (e.Key == Key.Down)
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonDown, new object[] { false });
        }

        private void NUDTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int number = 0;
            if (NUDTextBox != "")
                if (!int.TryParse(NUDTextBox, out number)) 
                    NUDTextBox = StartValue.ToString();

            // Bring the number to be within range.
            if (number > MaxValue) 
                NUDTextBox = MaxValue.ToString();
            if (number < MinValue) 
                NUDTextBox = MinValue.ToString();

            // NUDTextBox.SelectionStart = NUDTextBox.Length;
        }

        #endregion

        static IntUpDownCounter()
        {
            NUDTextBoxProperty = DependencyProperty.Register("NUDTextBox", typeof(string),
                typeof(IntUpDownCounter));
        }
    }
}
